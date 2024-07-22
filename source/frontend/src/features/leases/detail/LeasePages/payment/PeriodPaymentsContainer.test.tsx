import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import noop from 'lodash/noop';

import { Claims } from '@/constants';
import { LeasePeriodStatusTypes } from '@/constants/leaseStatusTypes';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { useLeasePeriodRepository } from '@/hooks/repositories/useLeasePeriodRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { defaultApiLease } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes/lookupCodesSlice';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { act, fillInput, renderAsync, RenderOptions, screen, userEvent } from '@/utils/test-utils';
import { defaultFormLeasePeriod, FormLeasePeriod } from './models';
import { Mock } from 'vitest';
import { ApiGen_CodeTypes_LeaseAccountTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseAccountTypes';
import PeriodPaymentsView, { IPeriodPaymentsViewProps } from './table/periods/PaymentPeriodsView';
import { createRef } from 'react';
import PeriodPaymentsContainer from './PeriodPaymentsContainer';
import { defaultTestFormLeasePayment } from './table/payments/PaymentsView.test';
import { createMemoryHistory } from 'history';

const defaultRepositoryResponse = {
  execute: vi.fn(),
  response: {} as any,
  error: undefined,
  status: undefined,
  loading: false,
};

const getLeasePeriods = vi.fn();
const mockGetLeasePeriods = {
  execute: getLeasePeriods,
  response: [
    FormLeasePeriod.toApi({ ...defaultFormLeasePeriod, expiryDate: '2020-01-01', payments: [] }),
  ],
  error: undefined,
  status: undefined,
  loading: false,
};

const defaultLeaseWithPeriodsPayments: LeaseFormModel = {
  ...new LeaseFormModel(),
  periods: [
    {
      ...defaultFormLeasePeriod,
      expiryDate: '2020-01-01',
      startDate: '2020-01-01',
      statusTypeCode: toTypeCodeNullable(LeasePeriodStatusTypes.EXERCISED),
      payments: [{ ...defaultTestFormLeasePayment }],
    },
  ],
};

vi.mock('@/hooks/repositories/useUserInfoRepository');
vi.mock('@/hooks/repositories/useLeasePeriodRepository');
vi.mocked(useLeasePeriodRepository).mockReturnValue({
  getLeasePeriods: mockGetLeasePeriods,
  updateLeasePeriod: { ...defaultRepositoryResponse },
  addLeasePeriod: { ...defaultRepositoryResponse },
  deleteLeasePeriod: { ...defaultRepositoryResponse },
});

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const setLease = vi.fn();
const onSuccessMock = vi.fn();

let viewProps!: IPeriodPaymentsViewProps;
const TermsView = (props: IPeriodPaymentsViewProps) => {
  viewProps = props;
  return (
    <Formik innerRef={props.formikRef} onSubmit={noop} initialValues={{ value: 0 }}>
      {({ values }) => <>{values.value}</>}
    </Formik>
  );
};

describe('PeriodsPaymentsContainer component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<LeasePageProps<IPeriodPaymentsViewProps>> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <LeaseStateContext.Provider
        value={{
          lease: {
            ...defaultApiLease(),
            ...renderOptions.initialValues,
            id: 1,
            startDate: '2020-01-01',
          },
          setLease,
        }}
      >
        <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
          <PeriodPaymentsContainer
            formikRef={createRef()}
            isEditing={false}
            onSuccess={onSuccessMock}
            componentView={PeriodPaymentsView}
          />
        </Formik>
      </LeaseStateContext.Provider>,
      {
        ...renderOptions,
        history,
        store: storeState,
      },
    );

    return {
      component,
      periodStartDateInput: () => document.querySelector('#datepicker-startDate') as HTMLElement,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
  });

  it('renders as expected', async () => {
    const { component } = await setup({ claims: [Claims.LEASE_EDIT] });
    await act(async () => {});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders with data as expected', async () => {
    const { component } = await setup({
      claims: [Claims.LEASE_EDIT],
      initialValues: { ...defaultApiLease(), periods: [defaultFormLeasePeriod] },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  describe('period logic tests', () => {
    it('when adding a new initial period, the start date is set to the start date of the lease', async () => {
      const temp = mockGetLeasePeriods.response;
      try {
        mockGetLeasePeriods.response = [];
        const {
          component: { getAllByText },
          periodStartDateInput,
        } = await setup({ claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD] });
        mockAxios.onPost().reply(200, { id: 1 });

        const addButton = getAllByText('Add a Period')[0];
        await act(async () => {
          userEvent.click(addButton);
        });

        expect(periodStartDateInput()).toHaveValue('Jan 01, 2020');
      } finally {
        mockGetLeasePeriods.response = temp;
      }
    });

    it('when adding a new sequential period, the start date is set to one day after the previous period expiry date', async () => {
      const {
        component: { getAllByText },
        periodStartDateInput,
      } = await setup({ claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD] });

      const addButton = getAllByText('Add a Period')[0];
      await act(async () => {
        userEvent.click(addButton);
      });

      expect(periodStartDateInput()).toHaveValue('Jan 02, 2020');
    });

    it('when adding a new seqquential period, the start date is set to empty when previous period is flexible and no expiry date', async () => {
      const temp = mockGetLeasePeriods.response;
      try {
        mockGetLeasePeriods.response = [
          FormLeasePeriod.toApi({
            ...defaultFormLeasePeriod,
            isFlexible: 'true',
            expiryDate: null,
            payments: [],
          }),
        ];
        const {
          component: { getAllByText },
          periodStartDateInput,
        } = await setup({ claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD] });

        const addButton = getAllByText('Add a Period')[0];
        await act(async () => {
          userEvent.click(addButton);
        });

        expect(periodStartDateInput()).toHaveValue('');
      } finally {
        mockGetLeasePeriods.response = temp;
      }
    });

    it(`doesn't make any request when cancelling the period modal`, async () => {
      const {
        component: { getAllByText, getByText },
      } = await setup({ claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD] });

      const addButton = getAllByText('Add a Period')[0];
      await act(async () => {
        userEvent.click(addButton);
      });

      const cancelButton = getByText('No');
      await act(async () => userEvent.click(cancelButton));

      expect(useLeasePeriodRepository().addLeasePeriod.execute).not.toHaveBeenCalled();
      expect(onSuccessMock).not.toHaveBeenCalled();
    });

    it('makes a post request when adding a new period', async () => {
      (useLeasePeriodRepository().updateLeasePeriod.execute as Mock).mockResolvedValue(
        FormLeasePeriod.toApi({ ...defaultFormLeasePeriod, id: 1 }),
      );
      mockGetLeasePeriods.execute.mockResolvedValue(
        (mockGetLeasePeriods.response = [
          FormLeasePeriod.toApi({ ...defaultFormLeasePeriod, payments: [], id: 1 }),
        ]),
      );
      const {
        component: { getAllByText, getByText },
      } = await setup({ claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD] });
      mockAxios.onPost().reply(200, { id: 1 });

      const addButton = getAllByText('Add a Period')[0];
      await act(async () => {
        userEvent.click(addButton);
      });

      await fillInput(document.body, 'startDate', '2020-01-01', 'datepicker');
      const saveButton = getByText('Yes');
      await act(async () => userEvent.click(saveButton));
    });

    it('makes a put request when updating a period', async () => {
      (useLeasePeriodRepository().updateLeasePeriod.execute as Mock).mockResolvedValue(
        FormLeasePeriod.toApi({ ...defaultFormLeasePeriod, id: 1 }),
      );
      mockGetLeasePeriods.execute.mockResolvedValue(
        (mockGetLeasePeriods.response = [
          FormLeasePeriod.toApi({ ...defaultFormLeasePeriod, payments: [], id: 1 }),
        ]),
      );
      const {
        component: { findAllByTitle, getByText },
      } = await setup({
        initialValues: { ...defaultApiLease(), periods: [{ ...defaultFormLeasePeriod, id: 1 }] },
        claims: [Claims.LEASE_EDIT],
      });
      mockAxios.onPut().reply(200, { id: 1 });

      const editButton = (await findAllByTitle('edit period'))[0];
      await act(async () => {
        userEvent.click(editButton);
      });

      await fillInput(document.body, 'startDate', '2020-01-01', 'datepicker');
      const saveButton = getByText('Yes');
      await act(async () => userEvent.click(saveButton));
    });

    it('deleting a period with payments is not possible', async () => {
      const {
        component: { queryByTitle },
      } = await setup({
        initialValues: {
          ...defaultApiLease(),
          periods: [{ ...defaultFormLeasePeriod, id: 1, payments: [{ id: 1 }] }],
        },
        claims: [Claims.LEASE_DELETE],
      });

      const deleteButton = queryByTitle('delete period');
      expect(deleteButton).toBeNull();
    });

    it('displays a warning when deleting the initial period when there are other periods', async () => {
      mockGetLeasePeriods.execute.mockResolvedValue(
        (mockGetLeasePeriods.response = [
          FormLeasePeriod.toApi({ ...defaultFormLeasePeriod, payments: [], id: 1 }),
          FormLeasePeriod.toApi({ ...defaultFormLeasePeriod, payments: [], id: 1 }),
        ]),
      );
      const {
        component: { findAllByTitle, getByText },
      } = await setup({
        initialValues: {
          ...defaultApiLease(),
          periods: [
            { ...defaultFormLeasePeriod, id: 1 },
            { ...defaultFormLeasePeriod, id: 1 },
          ],
        },
        claims: [Claims.LEASE_EDIT],
      });

      const deleteButton = (await findAllByTitle('delete period'))[0];
      await act(async () => userEvent.click(deleteButton));
      const warning = getByText('You must delete all renewals before deleting the initial period.');
      expect(warning).toBeVisible();
    });

    it('asks for confirmation when deleting a period', async () => {
      mockGetLeasePeriods.execute.mockResolvedValue(
        (mockGetLeasePeriods.response = [
          FormLeasePeriod.toApi({ ...defaultFormLeasePeriod, payments: [], id: 1 }),
        ]),
      );
      const {
        component: { findAllByTitle },
      } = await setup({
        initialValues: {
          ...defaultApiLease(),
          periods: [{ ...defaultFormLeasePeriod, id: 1 }],
        },
        claims: [Claims.LEASE_EDIT],
      });

      const deleteButton = (await findAllByTitle('delete period'))[0];
      await act(async () => userEvent.click(deleteButton));
      const warning = await screen.findByText(
        'You are about to delete a period. Do you wish to continue?',
      );
      expect(warning).toBeVisible();
    });

    it('makes a delete request when delete confirmed', async () => {
      mockGetLeasePeriods.execute.mockResolvedValue(
        (mockGetLeasePeriods.response = [
          FormLeasePeriod.toApi({ ...defaultFormLeasePeriod, payments: [], id: 1 }),
        ]),
      );
      const {
        component: { findAllByTitle, getByText },
      } = await setup({
        initialValues: {
          ...defaultApiLease(),
          periods: [{ ...defaultFormLeasePeriod, id: 1 }],
        },
        claims: [Claims.LEASE_EDIT],
      });
      mockAxios.onDelete().reply(200, { id: 1 });

      const deleteButton = (await findAllByTitle('delete period'))[0];
      await act(async () => userEvent.click(deleteButton));
      const warning = getByText('You are about to delete a period. Do you wish to continue?');
      expect(warning).toBeVisible();
      const continueButton = getByText('Continue');
      await act(async () => userEvent.click(continueButton));

      expect(useLeasePeriodRepository().deleteLeasePeriod.execute).toHaveBeenCalled();
    });

    it.each([
      [ApiGen_CodeTypes_LeaseAccountTypes.RCVBL, true],
      [ApiGen_CodeTypes_LeaseAccountTypes.PYBLBCTFA, false],
      [ApiGen_CodeTypes_LeaseAccountTypes.PYBLMOTI, false],
    ])(
      'when adding a new period, the GST field is defaulted based on lease type - %s',
      async (leaseType: string, gstDefault: boolean) => {
        const {
          component: { getAllByText, getByLabelText },
        } = await setup({
          initialValues: {
            ...defaultApiLease(),
            paymentReceivableType: { id: leaseType },
          },
          claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD],
        });

        const addButton = getAllByText('Add a Period')[0];
        await act(async () => {
          userEvent.click(addButton);
        });

        if (gstDefault) {
          // For Receivable Leases: Yes
          expect(getByLabelText('Y')).toBeChecked();
        } else {
          // For Payable leases: No
          expect(getByLabelText('N')).toBeChecked();
        }
      },
    );
  });

  describe('payments logic tests', () => {
    mockGetLeasePeriods.execute.mockResolvedValue(
      defaultLeaseWithPeriodsPayments.periods.map(t => FormLeasePeriod.toApi(t)),
    );

    it('makes a post request when adding a new payment', async () => {
      mockGetLeasePeriods.execute.mockResolvedValue(
        (mockGetLeasePeriods.response = defaultLeaseWithPeriodsPayments.periods.map(t =>
          FormLeasePeriod.toApi(t),
        )),
      );
      const {
        component: { findByText, getByText, findByTestId },
      } = await setup({
        claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD],
        initialValues: defaultLeaseWithPeriodsPayments,
      });
      mockAxios.onPost().reply(200, { id: 1 });

      const expander = await findByTestId('table-row-expander-');
      await act(async () => userEvent.click(expander));

      const addButton = await findByText('Add a Payment');
      await act(async () => {
        userEvent.click(addButton);
      });

      await fillInput(document.body, 'receivedDate', '2020-01-01', 'datepicker');
      await fillInput(document.body, 'amountTotal', '1200');
      const saveButton = getByText('Save payment');
      await act(async () => userEvent.click(saveButton));
      expect(mockAxios.history.post.length).toBe(1);
    });

    it(`doesn't make any request when payment modal is cancelled`, async () => {
      mockGetLeasePeriods.execute.mockResolvedValue(
        (mockGetLeasePeriods.response = defaultLeaseWithPeriodsPayments.periods.map(t =>
          FormLeasePeriod.toApi(t),
        )),
      );
      const {
        component: { findByText, getByText, findByTestId },
      } = await setup({
        claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD],
        initialValues: defaultLeaseWithPeriodsPayments,
      });
      mockAxios.onPost().reply(200, { id: 1 });

      const expander = await findByTestId('table-row-expander-');
      await act(async () => userEvent.click(expander));
      const addButton = await findByText('Add a Payment');
      await act(async () => {
        userEvent.click(addButton);
      });

      await fillInput(document.body, 'receivedDate', '2020-01-01', 'datepicker');
      const cancelButton = getByText('Cancel');
      await act(async () => userEvent.click(cancelButton));
      expect(mockAxios.history.post.length).toBe(0);
    });

    it('makes a put request when updating a payment', async () => {
      mockGetLeasePeriods.execute.mockResolvedValue(
        (mockGetLeasePeriods.response = defaultLeaseWithPeriodsPayments.periods.map(t =>
          FormLeasePeriod.toApi(t),
        )),
      );
      const {
        component: { findAllByTitle, findByTestId, getByText, findByText },
      } = await setup({
        initialValues: defaultLeaseWithPeriodsPayments,
        claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD],
      });
      mockAxios.onPut().reply(200, { id: 1 });

      const expander = await findByTestId('table-row-expander-');
      await act(async () => userEvent.click(expander));
      const editButton = await findAllByTitle('edit actual');
      await act(async () => {
        userEvent.click(editButton[0]);
      });

      await fillInput(document.body, 'receivedDate', '2020-01-01', 'datepicker');
      await fillInput(document.body, 'amountTotal', '1200');
      const saveButton = getByText('Save payment');
      await act(async () => userEvent.click(saveButton));
      expect(mockAxios.history.put.length).toBe(1);
    });

    it('asks for confirmation when deleting a payment', async () => {
      mockGetLeasePeriods.execute.mockResolvedValue(
        (mockGetLeasePeriods.response = defaultLeaseWithPeriodsPayments.periods.map(t =>
          FormLeasePeriod.toApi(t),
        )),
      );
      const {
        component: { findAllByTitle, getByText, findByText, getByTestId },
      } = await setup({
        initialValues: defaultLeaseWithPeriodsPayments,
        claims: [Claims.LEASE_EDIT],
      });

      await findByText('- Jan 1, 2020', { exact: false });
      const expander = getByTestId('table-row-expander-');
      await act(async () => userEvent.click(expander));

      const deleteButton = (await findAllByTitle('delete actual'))[0];
      await act(async () => userEvent.click(deleteButton));
      const warning = getByText('You are about to delete a payment. Do you wish to continue?');
      expect(warning).toBeVisible();
    });

    it('makes a delete request when delete payment confirmed', async () => {
      mockGetLeasePeriods.execute.mockResolvedValue(
        (mockGetLeasePeriods.response = defaultLeaseWithPeriodsPayments.periods.map(t =>
          FormLeasePeriod.toApi(t),
        )),
      );
      const {
        component: { findAllByTitle, getByText, findByText, getByTestId },
      } = await setup({
        initialValues: defaultLeaseWithPeriodsPayments,
        claims: [Claims.LEASE_EDIT],
      });
      mockAxios.onDelete().reply(200, { id: 1 });
      await findByText('- Jan 1, 2020', { exact: false });
      const expander = getByTestId('table-row-expander-');
      await act(async () => userEvent.click(expander));

      const deleteButton = (await findAllByTitle('delete actual'))[0];
      await act(async () => userEvent.click(deleteButton));
      const warning = getByText('You are about to delete a payment. Do you wish to continue?');
      expect(warning).toBeVisible();
      const continueButton = getByText('Continue');
      await act(async () => userEvent.click(continueButton));
      expect(mockAxios.history.delete.length).toBe(1);
    });
  });
});
