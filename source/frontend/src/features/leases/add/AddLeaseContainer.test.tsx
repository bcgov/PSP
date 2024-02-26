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

jest.mock('@react-keycloak/web');
(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    subject: 'test',
    authenticated: true,
    userInfo: {
      roles: [],
    },
  },
});

const retrieveUserInfo = jest.fn();
jest.mock('@/hooks/repositories/useUserInfoRepository');
(useUserInfoRepository as jest.Mock).mockReturnValue({
  retrieveUserInfo,
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: {
    userRegions: [
      {
        id: 1,
        userId: 5,
        regionCode: 1,
      },
      {
        id: 2,
        userId: 5,
        regionCode: 2,
      },
    ],
  },
});

const addLease = jest.fn();
jest.mock('../hooks/useAddLease');
(useAddLease as jest.MockedFunction<typeof useAddLease>).mockReturnValue({
  addLease: {
    execute: addLease,
    error: undefined,
    loading: false,
    response: undefined,
    status: 200,
  },
});

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

jest.mock('@/components/common/mapFSM/MapStateMachineContext');

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onClose = jest.fn();

describe('AddLeaseContainer component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<IAddLeaseContainerProps> = {}) => {
    // render component under test
    const component = await renderAsync(<AddLeaseContainer onClose={onClose} />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    return {
      component,
    };
  };

  beforeEach(() => {
    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { component } = await setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('cancels the form', async () => {
    const {
      component: { getAllByText },
    } = await setup({});
    userEvent.click(getAllByText('Cancel')[0]);
    expect(history.location.pathname).toBe('/');
    expect(onClose).toBeCalled();
  });

  it('saves the form with minimal data', async () => {
    const {
      component: { getByText, container },
    } = await setup({});

    await act(() => selectOptions('statusTypeCode', 'DRAFT'));
    await act(() => selectOptions('paymentReceivableTypeCode', 'RCVBL'));
    await act(() => selectOptions('regionId', '1'));
    await act(() => selectOptions('programTypeCode', 'BCFERRIES'));
    await act(() => selectOptions('leaseTypeCode', 'LIOCCTTLD'));
    await act(() => selectOptions('purposeTypeCode', 'BCFERRIES'));
    await act(() => {
      fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    });
    await act(() => {
      fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
    });
    await act(async () => userEvent.click(getByText(/Save/i)));

    expect(addLease).toBeCalledWith(leaseData, []);
  });

  it('triggers the confirm popup', async () => {
    addLease.mockRejectedValue(
      createAxiosError(409, 'test message', {
        errorCode: UserOverrideCode.ADD_LOCATION_TO_PROPERTY,
      }),
    );

    const {
      component: { getByText, findByText, container },
    } = await setup({});

    await act(() => selectOptions('statusTypeCode', 'DRAFT'));
    await act(() => selectOptions('paymentReceivableTypeCode', 'RCVBL'));
    await act(() => selectOptions('regionId', '1'));
    await act(() => selectOptions('programTypeCode', 'BCFERRIES'));
    await act(() => selectOptions('leaseTypeCode', 'LIOCCTTLD'));
    await act(() => selectOptions('purposeTypeCode', 'BCFERRIES'));
    await act(() => {
      fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    });
    await act(() => {
      fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
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

    const {
      component: { getByText, container },
    } = await setup({});

    await act(() => selectOptions('statusTypeCode', 'DRAFT'));
    await act(() => selectOptions('paymentReceivableTypeCode', 'RCVBL'));
    await act(() => selectOptions('regionId', '1'));
    await act(() => selectOptions('programTypeCode', 'BCFERRIES'));
    await act(() => selectOptions('leaseTypeCode', 'LIOCCTTLD'));
    await act(() => selectOptions('purposeTypeCode', 'BCFERRIES'));
    await act(() => {
      fillInput(container, 'startDate', '01/01/2020', 'datepicker');
    });
    await act(() => {
      fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
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
  rowVersion: 0,
  startDate: '2020-01-01',
  amount: 0,
  paymentReceivableType: toTypeCodeNullable('RCVBL'),
  purposeType: toTypeCodeNullable('BCFERRIES'),
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
  categoryType: null,
  initiatorType: null,
  otherType: null,
  otherCategoryType: null,
  otherProgramType: null,
  otherPurposeType: null,
  tfaFileNumber: null,
  responsibilityEffectiveDate: null,
  psFileNo: null,
  note: null,
  lFileNo: null,
  description: null,
  documentationReference: null,
  expiryDate: '2020-01-02',
  tenants: [],
  terms: [],
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
};
