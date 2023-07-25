import { RenderOptions } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import React, { forwardRef } from 'react';
import { act } from 'react-test-renderer';

import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { useLeaseDetail } from '@/features/leases/hooks/useLeaseDetail';
import { getDefaultFormLease } from '@/features/leases/models';
import { getMockApiLease } from '@/mocks/lease.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_Lease, defaultApiLease } from '@/models/api/Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { renderAsync, screen } from '@/utils/test-utils';

import UpdateLeaseContainer, { UpdateLeaseContainerProps } from './UpdateLeaseContainer';
import { IUpdateLeaseFormProps } from './UpdateLeaseForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);
jest.mock('@react-keycloak/web');

jest.mock('@/features/leases/hooks/useLeaseDetail');
(useLeaseDetail as jest.MockedFunction<typeof useLeaseDetail>).mockReturnValue({
  lease: getMockApiLease(),
  setLease: noop,
  getCompleteLease: jest.fn().mockResolvedValue(getMockApiLease()),
  refresh: noop as any,
  loading: false,
});

describe('Update lease container component', () => {
  let viewProps: IUpdateLeaseFormProps;
  const View = forwardRef<FormikProps<any>, IUpdateLeaseFormProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const setup = async (renderOptions: RenderOptions & Partial<UpdateLeaseContainerProps> = {}) => {
    // render component under test
    const component = await renderAsync(
      <LeaseStateContext.Provider
        value={{ lease: { ...getMockApiLease(), id: 1 }, setLease: noop }}
      >
        <UpdateLeaseContainer View={View} formikRef={React.createRef()} onEdit={noop} />
      </LeaseStateContext.Provider>,
      {
        ...renderOptions,
        store: storeState,
        history,
        claims: [],
      },
    );

    return {
      component,
    };
  };

  beforeEach(() => {
    mockAxios.onGet().reply(200, { id: 1, ...defaultApiLease });
    mockAxios.resetHistory();
  });
  it('renders as expected', async () => {
    const { component } = await setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('saves the form with minimal data', async () => {
    await setup({});

    mockAxios.onPut().reply(200, {});
    await act(async () =>
      viewProps.onSubmit({ ...getDefaultFormLease(), purposeTypeCode: 'BCFERRIES' }),
    );

    expect(JSON.parse(mockAxios.history.put[0].data)).toEqual(leaseData);
  });

  it('triggers the confirm popup', async () => {
    await setup({});

    mockAxios.onPut().reply(409, { error: 'test message' });
    await act(async () =>
      viewProps.onSubmit({ ...getDefaultFormLease(), purposeTypeCode: 'BCFERRIES' }),
    );

    expect(JSON.parse(mockAxios.history.put[0].data)).toEqual(leaseData);
  });

  it('clicking on the save anyways popup saves the form', async () => {
    await setup({});

    mockAxios.onPut().reply(409, {
      error: 'test message',
      errorCode: UserOverrideCode.PROPERTY_OF_INTEREST_TO_INVENTORY,
    });
    await act(async () =>
      viewProps.onSubmit({ ...getDefaultFormLease(), purposeTypeCode: 'BCFERRIES' }),
    );
    const button = await screen.findByText('Acknowledge & Continue');
    await act(async () => userEvent.click(button));

    expect(JSON.parse(mockAxios.history.put[1].data)).toEqual(leaseData);
  });
});

const leaseData: Api_Lease = {
  startDate: '',
  amount: 0,
  paymentReceivableType: { id: 'RCVBL' },
  purposeType: { id: 'BCFERRIES' },
  statusType: { id: 'DRAFT' },
  type: null,
  region: null,
  programType: null,
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
  expiryDate: null,
  tenants: [],
  terms: [],
  insurances: [],
  consultations: [],
};
