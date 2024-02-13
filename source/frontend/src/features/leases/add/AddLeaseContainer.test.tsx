import { useKeycloak } from '@react-keycloak/web';
import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { Api_Lease } from '@/models/api/Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
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
    const utils = render(<AddLeaseContainer onClose={onClose} />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    return {
      ...utils,
      getCloseButton: () => utils.getByTitle('close'),
    };
  };

  beforeEach(() => {
    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('cancels the form', async () => {
    const { getByTitle, getCloseButton } = await setup({});

    await act(async () => userEvent.click(getCloseButton()));
    await act(async () => userEvent.click(getByTitle('ok-modal')));

    expect(onClose).toBeCalled();
    expect(history.location.pathname).toBe('/');
  });

  it('saves the form with minimal data', async () => {
    const { getByText, container } = await setup({});

    await act(() => selectOptions('statusTypeCode', 'DRAFT'));
    await act(() => selectOptions('paymentReceivableTypeCode', 'RCVBL'));
    await act(() => selectOptions('regionId', '1'));
    await act(() => selectOptions('programTypeCode', 'BCFERRIES'));
    await act(() => selectOptions('leaseTypeCode', 'LIOCCTTLD'));
    await act(() => selectOptions('purposeTypeCode', 'BCFERRIES'));
    await act(() => fillInput(container, 'startDate', '01/01/2020', 'datepicker'));
    await act(() => fillInput(container, 'expiryDate', '01/02/2020', 'datepicker'));
    await act(async () => userEvent.click(getByText(/Save/i)));

    expect(addLease).toBeCalledWith(leaseData, []);
  });

  it('triggers the confirm popup', async () => {
    addLease.mockRejectedValue(
      createAxiosError(409, 'test message', {
        errorCode: UserOverrideCode.ADD_LOCATION_TO_PROPERTY,
      }),
    );

    const { getByText, findByText, container } = await setup({});

    await act(() => selectOptions('statusTypeCode', 'DRAFT'));
    await act(() => selectOptions('paymentReceivableTypeCode', 'RCVBL'));
    await act(() => selectOptions('regionId', '1'));
    await act(() => selectOptions('programTypeCode', 'BCFERRIES'));
    await act(() => selectOptions('leaseTypeCode', 'LIOCCTTLD'));
    await act(() => selectOptions('purposeTypeCode', 'BCFERRIES'));
    await act(() => fillInput(container, 'startDate', '01/01/2020', 'datepicker'));
    await act(() => fillInput(container, 'expiryDate', '01/02/2020', 'datepicker'));
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

    const { getByText, container } = await setup({});

    await act(() => selectOptions('statusTypeCode', 'DRAFT'));
    await act(() => selectOptions('paymentReceivableTypeCode', 'RCVBL'));
    await act(() => selectOptions('regionId', '1'));
    await act(() => selectOptions('programTypeCode', 'BCFERRIES'));
    await act(() => selectOptions('leaseTypeCode', 'LIOCCTTLD'));
    await act(() => selectOptions('purposeTypeCode', 'BCFERRIES'));
    await act(() => fillInput(container, 'startDate', '01/01/2020', 'datepicker'));
    await act(() => fillInput(container, 'expiryDate', '01/02/2020', 'datepicker'));
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

const leaseData: Api_Lease = {
  startDate: '2020-01-01',
  amount: 0,
  paymentReceivableType: { id: 'RCVBL' },
  purposeType: { id: 'BCFERRIES' },
  statusType: { id: 'DRAFT' },
  type: { id: 'LIOCCTTLD' },
  region: { id: 1 },
  programType: { id: 'BCFERRIES' },
  returnNotes: '',
  motiName: '',
  properties: [],
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
  insurances: [],
  consultations: [
    {
      id: 0,
      consultationType: { id: '1STNATION' },
      consultationStatusType: { id: 'UNKNOWN' },
      parentLeaseId: 0,
      otherDescription: null,
    },
    {
      id: 0,
      consultationType: { id: 'STRATRE' },
      consultationStatusType: { id: 'UNKNOWN' },
      parentLeaseId: 0,
      otherDescription: null,
    },
    {
      id: 0,
      consultationType: { id: 'REGPLANG' },
      consultationStatusType: { id: 'UNKNOWN' },
      parentLeaseId: 0,
      otherDescription: null,
    },
    {
      id: 0,
      consultationType: { id: 'REGPRPSVC' },
      consultationStatusType: { id: 'UNKNOWN' },
      parentLeaseId: 0,
      otherDescription: null,
    },
    {
      id: 0,
      consultationType: { id: 'DISTRICT' },
      consultationStatusType: { id: 'UNKNOWN' },
      parentLeaseId: 0,
      otherDescription: null,
    },
    {
      id: 0,
      consultationType: { id: 'HQ' },
      consultationStatusType: { id: 'UNKNOWN' },
      parentLeaseId: 0,
      otherDescription: null,
    },
    {
      id: 0,
      consultationType: { id: 'OTHER' },
      consultationStatusType: { id: 'UNKNOWN' },
      parentLeaseId: 0,
      otherDescription: null,
    },
  ],
};
