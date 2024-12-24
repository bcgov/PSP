import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { Claims, LeasePeriodStatusTypes } from '@/constants';
import { LeaseFormModel } from '@/features/leases/models';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { act, fillInput, renderAsync, RenderOptions, screen, userEvent } from '@/utils/test-utils';
import { getAllByRole as getAllByRoleBase } from '@/utils/test-utils';

import { defaultFormLeasePayment, defaultFormLeasePeriod, FormLeasePayment } from '../../models';
import PaymentsView, { IPaymentsViewProps } from './PaymentsView';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
export const defaultTestFormLeasePayment: FormLeasePayment = {
  ...defaultFormLeasePayment,
  leasePaymentMethodType: toTypeCodeNullable('Cheque'),
  leasePaymentStatusTypeCode: toTypeCodeNullable('Paid') ?? undefined,
  amountTotal: 1200,
  amountGst: 100,
  amountPreTax: 1100,
  receivedDate: '2020-01-01',
  note: 'note',
  id: 1,
};

const getDefaultLeaseWithPeriodsPayments = () => ({
  ...new LeaseFormModel(),
  periods: [
    {
      ...defaultFormLeasePeriod,
      statusTypeCode: toTypeCodeNullable(LeasePeriodStatusTypes.EXERCISED),
      payments: [{ ...defaultTestFormLeasePayment }],
    },
  ],
});

const onEdit = vi.fn();
const onDelete = vi.fn();
const onSave = vi.fn();

describe('PaymentsView component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IPaymentsViewProps> & {
        initialValues?: any;
        onCancel?: () => void;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <PaymentsView
        onEdit={onEdit}
        onDelete={onDelete}
        onSave={onSave}
        period={renderOptions.initialValues?.periods?.[0]}
        isExercised={renderOptions?.isExercised ?? true}
        isReceivable={renderOptions?.isReceivable ?? true}
        isGstEligible={renderOptions?.isGstEligible ?? true}
      />,
      {
        ...renderOptions,
        claims: renderOptions.claims ?? [Claims.LEASE_EDIT],
        history,
      },
    );

    return {
      findFirstRow: () => {
        const rows = component.getAllByRole('row');
        return rows && rows.length > 1 ? rows[1] : null;
      },
      findFooter: () => {
        const rows = component.getAllByRole('row');
        return rows && rows.length > 1 ? rows[rows.length - 1] : null;
      },
      findCell: (row: HTMLElement, index: number) => {
        const columns = getAllByRoleBase(row, 'cell');
        return columns && columns.length > index ? columns[index] : null;
      },
      findFooterCell: (row: HTMLElement, index: number) => {
        const columns = getAllByRoleBase(row, 'columnheader');
        return columns && columns.length > index ? columns[index] : null;
      },
      component,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
    onEdit.mockClear();
    onSave.mockClear();
    onDelete.mockClear();
  });
  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders with data as expected', async () => {
    const { component } = await setup({
      initialValues: { ...new LeaseFormModel(), periods: [defaultFormLeasePeriod] },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  describe('claim behaviour', () => {
    it('Does not display edit payment if missing claim ', async () => {
      const {
        component: { queryByTitle },
      } = await setup({
        initialValues: getDefaultLeaseWithPeriodsPayments(),
        claims: [Claims.LEASE_DELETE],
      });

      const editButton = await queryByTitle('edit payment');
      expect(editButton).toBeNull();
    });

    it('Does display edit payment if has claim ', async () => {
      const {
        component: { findAllByTitle },
      } = await setup({
        initialValues: getDefaultLeaseWithPeriodsPayments(),
        claims: [Claims.LEASE_EDIT],
      });

      const editButton = await findAllByTitle('edit payment');
      expect(editButton[0]).toBeVisible();
    });

    it('Does not display delete payment if missing claim', async () => {
      const {
        component: { queryByTitle },
      } = await setup({
        initialValues: getDefaultLeaseWithPeriodsPayments(),
        claims: [],
      });

      const deleteButton = await queryByTitle('delete actual');
      expect(deleteButton).toBeNull();
    });

    it('Does display delete payment if has claim ', async () => {
      const {
        component: { findAllByTitle },
      } = await setup({
        initialValues: getDefaultLeaseWithPeriodsPayments(),
        claims: [Claims.LEASE_EDIT],
      });
      const deleteButton = await findAllByTitle('delete payment');
      expect(deleteButton[0]).toBeVisible();
    });
  });

  describe('view only logic', () => {
    it('Displays there are no payments message if there are no payments', async () => {
      const {
        component: { findByText },
      } = await setup({
        initialValues: {
          ...getDefaultLeaseWithPeriodsPayments(),
          periods: [{ ...defaultFormLeasePeriod }],
        },
      });
      const text = await findByText('There are no recorded payments for this period.');
      expect(text).toBeVisible();
    });

    it('Displays not exercised message if period is not exercised', async () => {
      const {
        component: { findByText },
      } = await setup({
        initialValues: getDefaultLeaseWithPeriodsPayments(),
        isExercised: false,
      });
      const text = await findByText('Period must be exercised to add payments.');
      expect(text).toBeVisible();
    });

    it('Displays proper received columns if received', async () => {
      const {
        component: { findByText },
      } = await setup({
        initialValues: getDefaultLeaseWithPeriodsPayments(),
        isReceivable: true,
      });
      expect(await findByText('Payments')).toBeVisible();
      expect(await findByText('Date')).toBeVisible();
      expect(await findByText('Received payment ($)', { exact: false })).toBeVisible();
      expect(await findByText('Total ($)')).toBeVisible();
    });

    it('Displays proper sent columns if payable', async () => {
      const {
        component: { findByText },
      } = await setup({
        initialValues: getDefaultLeaseWithPeriodsPayments(),
        isReceivable: false,
      });
      expect(await findByText('Payments')).toBeVisible();
      expect(await findByText('Date')).toBeVisible();
      expect(await findByText('Sent payment ($)')).toBeVisible();
      expect(await findByText('Total ($)')).toBeVisible();
    });

    it('Does not display GST values or calculations if period is not gst eligible', async () => {
      const { findFooter, findCell, findFooterCell, findFirstRow } = await setup({
        initialValues: getDefaultLeaseWithPeriodsPayments(),
        isGstEligible: false,
      });
      const row = findFirstRow() as HTMLElement;
      const footer = findFooter() as HTMLElement;
      expect(row).not.toBeNull();
      expect(footer).not.toBeNull();
      expect(findCell(row, 4)?.textContent).toBe('-');
      expect(findFooterCell(footer, 4)?.textContent).toBe('-');
    });

    it('Sums expected payment columns', async () => {
      const { findFooter, findCell, findFooterCell, findFirstRow } = await setup({
        initialValues: {
          ...new LeaseFormModel(),
          periods: [
            {
              ...defaultFormLeasePeriod,
              payments: [defaultTestFormLeasePayment, defaultTestFormLeasePayment],
            },
          ],
        },
      });
      const row = findFirstRow() as HTMLElement;
      const footer = findFooter() as HTMLElement;
      expect(row).not.toBeNull();
      expect(footer).not.toBeNull();
      expect(findFooterCell(footer, 2)?.textContent).toBe('(2) payments');
      expect(findCell(row, 3)?.textContent).toBe('$1,100.00');
      expect(findFooterCell(footer, 3)?.textContent).toBe('$2,200.00');
      expect(findCell(row, 4)?.textContent).toBe('$100.00');
      expect(findFooterCell(footer, 4)?.textContent).toBe('$200.00');
      expect(findCell(row, 5)?.textContent).toBe('$1,200.00');
      expect(findFooterCell(footer, 5)?.textContent).toBe('$2,400.00');
    });
  });
  describe('interactive functionality', () => {
    it('Allows user to enter and save a note', async () => {
      const {
        component: { findByTitle, getByText },
      } = await setup({
        initialValues: getDefaultLeaseWithPeriodsPayments(),
        isReceivable: false,
      });
      const notesButton = await findByTitle('Payment comments');
      await act(async () => {
        userEvent.click(notesButton);
      });
      await act(async () => {
        await fillInput(document.body, 'note', 'a test note', 'textarea');
      });
      const saveButton = getByText('Yes');
      await act(async () => {
        userEvent.click(saveButton);
      });
      expect(onSave).toHaveBeenCalledWith({ ...defaultTestFormLeasePayment, note: 'a test note' });
    });

    it('Allows user to trigger an edit action', async () => {
      const {
        component: { findAllByTitle },
      } = await setup({
        initialValues: getDefaultLeaseWithPeriodsPayments(),
        isReceivable: false,
      });
      const editButton = await findAllByTitle('edit payment');
      await act(async () => userEvent.click(editButton[0]));
      expect(onEdit).toHaveBeenCalledWith(defaultTestFormLeasePayment);
    });

    it('Allows user to trigger a delete action', async () => {
      const {
        component: { findAllByTitle },
      } = await setup({
        initialValues: getDefaultLeaseWithPeriodsPayments(),
        isReceivable: false,
        claims: [Claims.LEASE_EDIT],
      });
      const deleteButton = await findAllByTitle('delete payment');
      await act(async () => userEvent.click(deleteButton[0]));
      expect(onDelete).toHaveBeenCalledWith(defaultTestFormLeasePayment);
    });
  });
});
