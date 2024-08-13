import { useKeycloak } from '@react-keycloak/web';
import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { getEmptyBaseAudit, getEmptyLease } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCodeNullable } from '@/utils/formUtils';
import {
  act,
  createAxiosError,
  fillInput,
  renderAsync,
  RenderOptions,
  screen,
  selectOptions,
} from '@/utils/test-utils';

import { useAddLease } from '../hooks/useAddLease';
import AddLeaseContainer, { IAddLeaseContainerProps } from './AddLeaseContainer';
import { ApiGen_Concepts_RegionUser } from '@/models/api/generated/ApiGen_Concepts_RegionUser';
import { ApiGen_Concepts_User } from '@/models/api/generated/ApiGen_Concepts_User';
import { ApiGen_CodeTypes_LeaseAccountTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseAccountTypes';
import { ApiGen_CodeTypes_LeasePurposeTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePurposeTypes';

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

describe('AddLeaseContainer component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<IAddLeaseContainerProps> = {}) => {
    // render component under test
    const utils = await renderAsync(<AddLeaseContainer onClose={onClose} />, {
      ...renderOptions,
      store: storeState,
      useMockAuthentication: true,
      history,
    });

    return {
      ...utils,
      getCloseButton: () => utils.getByTitle('close'),
      getPurposeMultiSelect: () =>
        utils.container.querySelector(`#multiselect-purposes_input`) as HTMLElement,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment, findByText } = await setup({});
    await findByText(/First nation/i);
    expect(asFragment()).toMatchSnapshot();
    await act(async () => {});
  });

  it('cancels the form', async () => {
    const { getByTitle, getCloseButton } = await setup({});

    await act(async () => selectOptions('regionId', '1'));
    await act(async () => userEvent.click(getCloseButton()));
    await act(async () => userEvent.click(getByTitle('ok-modal')));

    expect(onClose).toBeCalled();
    expect(history.location.pathname).toBe('/');
  });

  it('saves the form with minimal data', async () => {
    const { getByText, getPurposeMultiSelect, container } = await setup({});

    await act(async () => selectOptions('statusTypeCode', 'DRAFT'));
    await act(async () =>
      selectOptions('paymentReceivableTypeCode', ApiGen_CodeTypes_LeaseAccountTypes.RCVBL),
    );
    await act(async () => selectOptions('regionId', '1'));
    await act(async () => selectOptions('programTypeCode', 'BCFERRIES'));
    await act(async () => selectOptions('leaseTypeCode', 'LIOCCTTLD'));
    await act(async () => {
      fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    });
    await act(async () => {
      fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    });

    const multiSelectPurposes = getPurposeMultiSelect();
    await act(async () => {
      expect(multiSelectPurposes).not.toBeNull();
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
  });

  it('triggers the confirm popup', async () => {
    addLease.mockRejectedValue(
      createAxiosError(409, 'test message', {
        errorCode: UserOverrideCode.ADD_LOCATION_TO_PROPERTY,
      }),
    );

    const { getByText, findByText, getPurposeMultiSelect, container } = await setup({});

    await act(async () => selectOptions('statusTypeCode', 'DRAFT'));
    await act(async () =>
      selectOptions('paymentReceivableTypeCode', ApiGen_CodeTypes_LeaseAccountTypes.RCVBL),
    );
    await act(async () => selectOptions('regionId', '1'));
    await act(async () => selectOptions('programTypeCode', 'BCFERRIES'));
    await act(async () => selectOptions('leaseTypeCode', 'LIOCCTTLD'));
    await act(async () => {
      fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    });
    await act(async () => {
      fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    });

    const multiSelectPurposes = getPurposeMultiSelect();
    await act(async () => {
      expect(multiSelectPurposes).not.toBeNull();
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

    const { getByText, getPurposeMultiSelect, container } = await setup({});

    await act(async () => selectOptions('statusTypeCode', 'DRAFT'));
    await act(async () =>
      selectOptions('paymentReceivableTypeCode', ApiGen_CodeTypes_LeaseAccountTypes.RCVBL),
    );
    await act(async () => selectOptions('regionId', '1'));
    await act(async () => selectOptions('programTypeCode', 'BCFERRIES'));
    await act(async () => selectOptions('leaseTypeCode', 'LIOCCTTLD'));
    await act(async () => {
      fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    });
    await act(async () => {
      fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    });

    const multiSelectPurposes = getPurposeMultiSelect();
    await act(async () => {
      expect(multiSelectPurposes).not.toBeNull();
      userEvent.click(multiSelectPurposes);
    });

    await act(async () => {
      userEvent.type(multiSelectPurposes, 'BC Ferries');
      userEvent.click(multiSelectPurposes);

      const firstOption = container.querySelector(`div ul li.option`);
      userEvent.click(firstOption);
    });

    await act(async () => userEvent.click(getByText(/Save/i)));

    expect(addLease).toBeCalledWith(leaseData, []);

    const popup = await screen.findByText(/test message/i);
    expect(popup).toBeVisible();

    // simulate api success
    addLease.mockResolvedValue({ ...leaseData, id: 1 });

    await act(async () => {
      userEvent.click((await screen.findAllByText('Yes'))[2]);
    });

    expect(addLease).toBeCalledWith(leaseData, []);
    expect(history.location.pathname).toBe('/mapview/sidebar/lease/1');
  });
});

const leaseData: ApiGen_Concepts_Lease = {
  ...getEmptyLease(),
  rowVersion: null,
  startDate: '2020-01-01',
  amount: 0,
  paymentReceivableType: toTypeCodeNullable(ApiGen_CodeTypes_LeaseAccountTypes.RCVBL),
  fileStatusTypeCode: toTypeCodeNullable('DRAFT'),
  type: toTypeCodeNullable('LIOCCTTLD'),
  region: toTypeCodeNullable(1),
  programType: toTypeCodeNullable('BCFERRIES'),
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
  consultations: [
    {
      id: 0,
      consultationType: toTypeCodeNullable('1STNATION'),
      consultationStatusType: toTypeCodeNullable('UNKNOWN'),
      parentLeaseId: 0,
      otherDescription: null,
      ...getEmptyBaseAudit(),
    },
    {
      id: 0,
      consultationType: toTypeCodeNullable('STRATRE'),
      consultationStatusType: toTypeCodeNullable('UNKNOWN'),
      parentLeaseId: 0,
      otherDescription: null,
      ...getEmptyBaseAudit(),
    },
    {
      id: 0,
      consultationType: toTypeCodeNullable('REGPLANG'),
      consultationStatusType: toTypeCodeNullable('UNKNOWN'),
      parentLeaseId: 0,
      otherDescription: null,
      ...getEmptyBaseAudit(),
    },
    {
      id: 0,
      consultationType: toTypeCodeNullable('REGPRPSVC'),
      consultationStatusType: toTypeCodeNullable('UNKNOWN'),
      parentLeaseId: 0,
      otherDescription: null,
      ...getEmptyBaseAudit(),
    },
    {
      id: 0,
      consultationType: toTypeCodeNullable('DISTRICT'),
      consultationStatusType: toTypeCodeNullable('UNKNOWN'),
      parentLeaseId: 0,
      otherDescription: null,
      ...getEmptyBaseAudit(),
    },
    {
      id: 0,
      consultationType: toTypeCodeNullable('HQ'),
      consultationStatusType: toTypeCodeNullable('UNKNOWN'),
      parentLeaseId: 0,
      otherDescription: null,
      ...getEmptyBaseAudit(),
    },
    {
      id: 0,
      consultationType: toTypeCodeNullable('OTHER'),
      consultationStatusType: toTypeCodeNullable('UNKNOWN'),
      parentLeaseId: 0,
      otherDescription: null,
      ...getEmptyBaseAudit(),
    },
  ],
  leasePurposes: [
    {
      id: 0,
      leaseId: 0,
      leasePurposeTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_LeasePurposeTypes.BCFERRIES),
      purposeOtherDescription: null,
      ...getEmptyBaseAudit(),
    },
  ],
};
