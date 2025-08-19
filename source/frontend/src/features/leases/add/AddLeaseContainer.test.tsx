import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { getMockPolygon } from '@/mocks/geometries.mock';
import { getMockApiLease } from '@/mocks/lease.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { ApiGen_CodeTypes_LeaseAccountTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseAccountTypes';
import { ApiGen_CodeTypes_LeasePurposeTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePurposeTypes';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_RegionUser } from '@/models/api/generated/ApiGen_Concepts_RegionUser';
import { ApiGen_Concepts_User } from '@/models/api/generated/ApiGen_Concepts_User';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { getEmptyBaseAudit, getEmptyLease } from '@/models/defaultInitializers';
import { emptyRegion } from '@/models/layers/motRegionalBoundary';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCodeNullable } from '@/utils/formUtils';
import {
  act,
  createAxiosError,
  fillInput,
  render,
  RenderOptions,
  screen,
  selectOptions,
} from '@/utils/test-utils';

import { useAddLease } from '../hooks/useAddLease';
import AddLeaseContainer, { IAddLeaseContainerProps } from './AddLeaseContainer';
import AddLeaseForm from './AddLeaseForm';

const retrieveUserInfo = vi.fn();
vi.mock('@/hooks/repositories/useUserInfoRepository');
vi.mocked(useUserInfoRepository).mockReturnValue({
  retrieveUserInfo,
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: {
    userRegions: [
      {
        id: 1,
        userId: 5,
        regionCode: 1,
      } as ApiGen_Concepts_RegionUser,
      {
        id: 2,
        userId: 5,
        regionCode: 2,
      } as ApiGen_Concepts_RegionUser,
    ],
  } as ApiGen_Concepts_User,
});

const addLease = vi.fn();
vi.mock('../hooks/useAddLease');
vi.mocked(useAddLease).mockReturnValue({
  addLease: {
    execute: addLease,
    error: undefined,
    loading: false,
    response: undefined,
    status: 200,
  },
});

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children as Function;
    }),
  };
});

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onClose = vi.fn();
const onSuccess = vi.fn();

describe('AddLeaseContainer component', () => {
  // render component under test
  const setup = async (renderOptions: RenderOptions & Partial<IAddLeaseContainerProps> = {}) => {
    const utils = render(
      <SideBarContextProvider>
        <AddLeaseContainer onClose={onClose} onSuccess={onSuccess} View={AddLeaseForm} />
      </SideBarContextProvider>,
      {
        ...renderOptions,
        store: storeState,
        useMockAuthentication: true,
        mockMapMachine: renderOptions.mockMapMachine,
        history,
      },
    );

    // wait for the component to finish loading
    await act(async () => {});

    return {
      ...utils,
      getCloseButton: () => utils.getByTitle('close'),
      getPurposeMultiSelect: () =>
        utils.container.querySelector(`#multiselect-purposes_input`) as HTMLElement,
    };
  };

  beforeEach(() => {
    vi.clearAllMocks();
    addLease.mockResolvedValue(getMockApiLease());
  });

  it('renders as expected', async () => {
    const { asFragment, findByText } = await setup({});
    await findByText(/MOTT contact/i);
    expect(asFragment()).toMatchSnapshot();
  });

  it('cancels the form', async () => {
    const { getCloseButton } = await setup({});

    await act(async () => selectOptions('regionId', '1'));
    await act(async () => userEvent.click(getCloseButton()));

    expect(onClose).toHaveBeenCalled();
  });

  it('requires confirmation when navigating away', async () => {
    const { getByTitle } = await setup({});

    await act(async () => selectOptions('regionId', '1'));

    await act(async () => history.push('/'));

    await act(async () => userEvent.click(getByTitle('ok-modal')));
    expect(history.location.pathname).toBe('/');
  });

  it('saves the form with minimal data', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      processCreation: vi.fn(),
      refreshMapProperties: vi.fn(),
    };

    const { getByText, getPurposeMultiSelect, container } = await setup({
      mockMapMachine: testMockMachine,
    });

    await act(async () => selectOptions('statusTypeCode', ApiGen_CodeTypes_LeaseStatusTypes.DRAFT));
    await act(async () =>
      selectOptions('paymentReceivableTypeCode', ApiGen_CodeTypes_LeaseAccountTypes.RCVBL),
    );
    await act(async () => selectOptions('regionId', '1'));
    await act(async () => selectOptions('programTypeCode', 'COMMBLDG'));
    await act(async () => selectOptions('leaseTypeCode', 'LOOBCTFA'));
    await act(async () => {
      fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    });
    await act(async () => {
      fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    });

    const multiSelectPurposes = getPurposeMultiSelect();
    expect(multiSelectPurposes).not.toBeNull();

    await act(async () => {
      userEvent.click(multiSelectPurposes);
    });

    await act(async () => {
      userEvent.type(multiSelectPurposes, 'BC Ferries');
    });
    await act(async () => {
      userEvent.click(multiSelectPurposes);
    });
    await act(async () => {
      const firstOption = container.querySelector(`div ul li.option`);
      userEvent.click(firstOption);
    });

    await act(async () => userEvent.click(getByText(/Save/i)));

    expect(addLease).toHaveBeenCalledWith(leaseData, []);
    expect(testMockMachine.processCreation).toHaveBeenCalled();
    expect(onSuccess).toHaveBeenCalled();
  });

  it('triggers the confirm popup', async () => {
    addLease.mockRejectedValue(
      createAxiosError(409, 'test message', {
        errorCode: UserOverrideCode.ADD_LOCATION_TO_PROPERTY,
      }),
    );

    const { getByText, findByText, getPurposeMultiSelect, container } = await setup({});

    await act(async () => selectOptions('statusTypeCode', ApiGen_CodeTypes_LeaseStatusTypes.DRAFT));
    await act(async () =>
      selectOptions('paymentReceivableTypeCode', ApiGen_CodeTypes_LeaseAccountTypes.RCVBL),
    );
    await act(async () => selectOptions('regionId', '1'));
    await act(async () => selectOptions('programTypeCode', 'COMMBLDG'));
    await act(async () => selectOptions('leaseTypeCode', 'LOOBCTFA'));
    await act(async () => {
      fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    });
    await act(async () => {
      fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    });

    const multiSelectPurposes = getPurposeMultiSelect();
    expect(multiSelectPurposes).not.toBeNull();

    await act(async () => {
      userEvent.click(multiSelectPurposes);
    });

    await act(async () => {
      userEvent.type(multiSelectPurposes, 'BC Ferries');
      userEvent.click(multiSelectPurposes);

      const firstOption = container.querySelector(`div ul li.option`);
      userEvent.click(firstOption);
    });

    await act(async () => userEvent.click(getByText(/Save/i)));

    expect(await findByText('test message')).toBeVisible();
  });

  it('clicking on the save anyways popup saves the form', async () => {
    // simulate api error
    addLease.mockRejectedValue(
      createAxiosError(409, 'test message', {
        errorCode: UserOverrideCode.ADD_LOCATION_TO_PROPERTY,
      }),
    );

    const { getByText, getPurposeMultiSelect, getByTestId, container } = await setup({});

    await act(async () => selectOptions('statusTypeCode', ApiGen_CodeTypes_LeaseStatusTypes.DRAFT));
    await act(async () =>
      selectOptions('paymentReceivableTypeCode', ApiGen_CodeTypes_LeaseAccountTypes.RCVBL),
    );
    await act(async () => selectOptions('regionId', '1'));
    await act(async () => selectOptions('programTypeCode', 'COMMBLDG'));
    await act(async () => selectOptions('leaseTypeCode', 'LOOBCTFA'));
    await act(async () => {
      fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    });
    await act(async () => {
      fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    });

    const multiSelectPurposes = getPurposeMultiSelect();
    expect(multiSelectPurposes).not.toBeNull();

    await act(async () => {
      userEvent.click(multiSelectPurposes);
    });

    await act(async () => {
      userEvent.type(multiSelectPurposes, 'BC Ferries');
      userEvent.click(multiSelectPurposes);

      const firstOption = container.querySelector(`div ul li.option`);
      userEvent.click(firstOption);
    });

    await act(async () => userEvent.click(getByText(/Save/i)));

    expect(addLease).toHaveBeenCalledWith(leaseData, []);

    const popup = await screen.findByText(/test message/i);
    expect(popup).toBeVisible();

    // simulate api success
    addLease.mockResolvedValue({ ...leaseData, id: 1 });

    await act(async () => {
      const okButton = getByTestId('ok-modal-button');
      userEvent.click(okButton);
    });

    expect(addLease).toHaveBeenCalledWith(leaseData, []);
    expect(onSuccess).toHaveBeenCalledWith(1);
  });

  it('should pre-populate the region if a property is selected', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      selectedFeatures: [
        {
          location: { lng: -120.69195885, lat: 50.25163372 },
          fileLocation: null,
          pimsFeature: null,
          parcelFeature: null,
          regionFeature: {
            type: 'Feature',
            properties: { ...emptyRegion, REGION_NUMBER: 1, REGION_NAME: 'South Coast Region' },
            geometry: getMockPolygon(),
          },
          districtFeature: null,
          selectingComponentId: null,
          municipalityFeature: null,
        },
      ],
    };

    const { findByDisplayValue } = await setup({ mockMapMachine: testMockMachine });
    const text = await findByDisplayValue(/South Coast Region/i);
    expect(text).toBeVisible();
  });

  it('resets the "draft" markers when the file is opened', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
    };
    await setup({ mockMapMachine: testMockMachine });
    expect(testMockMachine.setFilePropertyLocations).toHaveBeenCalledWith([]);
  });
});

const leaseData: ApiGen_Concepts_Lease = {
  ...getEmptyLease(),
  rowVersion: null,
  startDate: '2020-01-01',
  amount: 0,
  paymentReceivableType: toTypeCodeNullable(ApiGen_CodeTypes_LeaseAccountTypes.RCVBL),
  fileStatusTypeCode: toTypeCodeNullable('DRAFT'),
  type: toTypeCodeNullable('LOOBCTFA'),
  region: toTypeCodeNullable(1),
  programType: toTypeCodeNullable('COMMBLDG'),
  returnNotes: '',
  motiName: '',
  fileProperties: [],
  isResidential: false,
  isCommercialBuilding: false,
  isOtherImprovement: false,
  responsibilityType: null,
  initiatorType: null,
  otherType: null,
  otherProgramType: null,
  tfaFileNumber: null,
  responsibilityEffectiveDate: null,
  psFileNo: null,
  note: null,
  lFileNo: null,
  description: null,
  documentationReference: null,
  expiryDate: '2020-01-02',
  stakeholders: [],
  periods: [],
  consultations: null,
  leasePurposes: [
    {
      id: 0,
      leaseId: 0,
      leasePurposeTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_LeasePurposeTypes.BCFERRIES),
      purposeOtherDescription: null,
      ...getEmptyBaseAudit(),
    },
  ],
  leaseTeam: [],
};
