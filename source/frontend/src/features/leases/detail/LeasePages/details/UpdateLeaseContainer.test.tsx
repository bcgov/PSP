import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';
import React, { forwardRef } from 'react';

import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { useLeaseDetail } from '@/features/leases/hooks/useLeaseDetail';
import { getDefaultFormLease } from '@/features/leases/models';
import { getMockApiLease } from '@/mocks/lease.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_CodeTypes_LeaseAccountTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseAccountTypes';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { defaultApiLease, getEmptyLease } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import UpdateLeaseContainer, { UpdateLeaseContainerProps } from './UpdateLeaseContainer';
import { IUpdateLeaseFormProps } from './UpdateLeaseForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

const mockLeaseApi: ReturnType<typeof useLeaseDetail> = {
  lease: getMockApiLease(),
  setLease: noop,
  getCompleteLease: vi.fn().mockResolvedValue(getMockApiLease()),
  refresh: vi.fn(),
  loading: false,
};

vi.mock('@/features/leases/hooks/useLeaseDetail');
vi.mocked(useLeaseDetail).mockReturnValue(mockLeaseApi);

const onEdit = vi.fn();

describe('Update lease container component', () => {
  let viewProps: IUpdateLeaseFormProps;
  const View = forwardRef<FormikProps<any>, IUpdateLeaseFormProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  // render component under test
  const setup = (renderOptions: RenderOptions & Partial<UpdateLeaseContainerProps> = {}) => {
    const component = render(
      <LeaseStateContext.Provider
        value={{ lease: { ...getMockApiLease(), id: 1 }, setLease: noop }}
      >
        <UpdateLeaseContainer View={View} formikRef={React.createRef()} onEdit={onEdit} />
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
    mockAxios.onGet().reply(200, { ...defaultApiLease(), id: 1 });
    mockAxios.resetHistory();
  });

  it('renders as expected', () => {
    const { component } = setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('saves the form with minimal data', async () => {
    setup({});

    mockAxios.onPut().reply(200, { ...getMockApiLease(), id: 1 });
    await act(async () => viewProps.onSubmit({ ...getDefaultFormLease() }));

    expect(JSON.parse(mockAxios.history.put[0].data)).toEqual(expectedLease);
    expect(onEdit).toHaveBeenCalledWith(false);
    expect(useLeaseDetail().refresh).toHaveBeenCalled();
  });

  it('triggers the popup for business rule violation', async () => {
    setup({});

    mockAxios.onPut().reply(400, {
      error: 'Retired property can not be selected',
      type: 'BusinessRuleViolationException',
    });
    await act(async () => viewProps.onSubmit({ ...getDefaultFormLease() }));

    expect(await screen.findByText(/Retired property can not be selected/i)).toBeVisible();
    expect(await screen.findByText(/Close/i)).toBeVisible();
    expect(JSON.parse(mockAxios.history.put[0].data)).toEqual(expectedLease);
  });

  it('refreshes the lease when clicking on the close button after a business rule violation', async () => {
    setup({});

    mockAxios.onPut().reply(400, {
      error: 'This property cannot be deleted because it is part of a subdivision or consolidation',
      type: 'BusinessRuleViolationException',
    });
    await act(async () => viewProps.onSubmit({ ...getDefaultFormLease() }));

    expect(
      await screen.findByText(
        /This property cannot be deleted because it is part of a subdivision or consolidation/i,
      ),
    ).toBeVisible();
    const button = await screen.findByText(/Close/i);
    expect(button).toBeVisible();
    await act(async () => userEvent.click(button));
    expect(mockLeaseApi.getCompleteLease).toHaveBeenCalled();
    expect(JSON.parse(mockAxios.history.put[0].data)).toEqual(expectedLease);
  });

  it('triggers the user-override popup', async () => {
    setup({});

    mockAxios.onPut().reply(409, {
      error: 'test message',
      errorCode: UserOverrideCode.PROPERTY_OF_INTEREST_TO_INVENTORY,
    });
    await act(async () => viewProps.onSubmit({ ...getDefaultFormLease() }));

    expect(await screen.findByText(/test message/i)).toBeVisible();
    expect(await screen.findByText(/Yes/i)).toBeVisible();
    expect(await screen.findByText(/No/i)).toBeVisible();
    expect(JSON.parse(mockAxios.history.put[0].data)).toEqual(expectedLease);
  });

  it('clicking on the save anyways popup saves the form', async () => {
    setup({});

    mockAxios.onPut().reply(409, {
      error: 'test message',
      errorCode: UserOverrideCode.PROPERTY_OF_INTEREST_TO_INVENTORY,
    });
    await act(async () => viewProps.onSubmit({ ...getDefaultFormLease() }));
    const button = await screen.findByText('Yes');
    await act(async () => userEvent.click(button));

    expect(JSON.parse(mockAxios.history.put[1].data)).toEqual(expectedLease);
  });
});

const expectedLease: ApiGen_Concepts_Lease = {
  ...getEmptyLease(),
  startDate: null,
  amount: 0,
  paymentReceivableType: toTypeCodeNullable(ApiGen_CodeTypes_LeaseAccountTypes.RCVBL),
  fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_LeaseStatusTypes.DRAFT),
  type: null,
  region: null,
  programType: null,
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
  expiryDate: null,
  stakeholders: [],
  periods: [],
  consultations: null,
  programName: null,
  renewalCount: 0,
  hasPhysicalFile: false,
  hasDigitalFile: false,
  hasPhysicalLicense: null,
  hasDigitalLicense: null,
  primaryArbitrationCity: null,
  isExpired: false,
  project: null,
  id: 0,
  rowVersion: null,
};
