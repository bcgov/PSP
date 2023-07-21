import { useKeycloak } from '@react-keycloak/web';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { Api_Lease } from '@/models/api/Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions, screen, waitFor } from '@/utils/test-utils';

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
const mockAxios = new MockAdapter(axios);

describe('AddLeaseContainer component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<IAddLeaseContainerProps> = {}) => {
    // render component under test
    const component = await renderAsync(<AddLeaseContainer onClose={noop} />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    return {
      component,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
    mockAxios.resetHandlers();

    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);
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
  });

  it('saves the form with minimal data', async () => {
    const {
      component: { getByText, container },
    } = await setup({});
    await act(async () => {
      await fillInput(container, 'statusTypeCode', 'DRAFT', 'select');
      await fillInput(container, 'paymentReceivableTypeCode', 'RCVBL', 'select');
      await fillInput(container, 'startDate', '01/01/2020', 'datepicker');
      await fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
      await fillInput(container, 'regionId', '1', 'select');
      await fillInput(container, 'programTypeCode', 'BCFERRIES', 'select');
      await fillInput(container, 'leaseTypeCode', 'LICONSTRC', 'select');
      await fillInput(container, 'purposeTypeCode', 'BCFERRIES', 'select');
    });

    /*
    await act(async () => {
      userEvent.click(getByText('Remove'));
    });
    */

    mockAxios.onPost().reply(200, {});
    await act(async () => {
      userEvent.click(getByText(/Save/i));
    });
    await waitFor(() => {
      expect(JSON.parse(mockAxios.history.post[0].data)).toEqual(leaseData);
    });

    expect(JSON.parse(mockAxios.history.post[0].data)).toEqual(leaseData);
  });

  it('triggers the confirm popup', async () => {
    const {
      component: { getByText, findByText, container },
    } = await setup({});
    await act(async () => {
      await fillInput(container, 'statusTypeCode', 'DRAFT', 'select');
      await fillInput(container, 'paymentReceivableTypeCode', 'RCVBL', 'select');
      await fillInput(container, 'startDate', '01/01/2020', 'datepicker');
      await fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
      await fillInput(container, 'regionId', '1', 'select');
      await fillInput(container, 'programTypeCode', 'BCFERRIES', 'select');
      await fillInput(container, 'leaseTypeCode', 'LICONSTRC', 'select');
      await fillInput(container, 'purposeTypeCode', 'BCFERRIES', 'select');
    });

    mockAxios
      .onPost()
      .reply(409, { error: 'test message', errorCode: UserOverrideCode.ADD_LOCATION_TO_PROPERTY });
    act(() => userEvent.click(getByText(/Save/i)));
    expect(await findByText('test message')).toBeVisible();
  });

  it('clicking on the save anyways popup saves the form', async () => {
    const {
      component: { getByText, container },
    } = await setup({});

    await act(async () => {
      await fillInput(container, 'statusTypeCode', 'DRAFT', 'select');
      await fillInput(container, 'paymentReceivableTypeCode', 'RCVBL', 'select');
      await fillInput(container, 'startDate', '01/01/2020', 'datepicker');
      await fillInput(container, 'expiryDate', '01/02/2020', 'datepicker');
      await fillInput(container, 'regionId', '1', 'select');
      await fillInput(container, 'programTypeCode', 'BCFERRIES', 'select');
      await fillInput(container, 'leaseTypeCode', 'LICONSTRC', 'select');
      await fillInput(container, 'purposeTypeCode', 'BCFERRIES', 'select');
    });

    mockAxios
      .onPost()
      .reply(409, { error: 'test message', errorCode: UserOverrideCode.ADD_LOCATION_TO_PROPERTY });
    act(() => userEvent.click(getByText(/Save/i)));
    await waitFor(() => {
      expect(JSON.parse(mockAxios.history.post[0].data)).toEqual(leaseData);
    });

    const popup = await screen.findByText(/test message/i);
    expect(popup).toBeVisible();

    await act(async () => {
      userEvent.click(await screen.findByText('Acknowledge & Continue'));
    });

    expect(JSON.parse(mockAxios.history.post[0].data)).toEqual(leaseData);
  });
});
const leaseData: Api_Lease = {
  startDate: '2020-01-01',
  amount: 0,
  paymentReceivableType: { id: 'RCVBL' },
  purposeType: { id: 'BCFERRIES' },
  statusType: { id: 'DRAFT' },
  type: { id: 'LICONSTRC' },
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
