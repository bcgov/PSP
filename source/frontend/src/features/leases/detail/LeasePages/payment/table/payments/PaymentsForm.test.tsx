import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { Claims, LeaseTermStatusTypes } from '@/constants';
import { LeaseFormModel } from '@/features/leases/models';
import { act, fillInput, renderAsync, RenderOptions, screen, userEvent } from '@/utils/test-utils';
import { getAllByRole as getAllByRoleBase } from '@/utils/test-utils';

import { defaultFormLeasePayment, defaultFormLeaseTerm, FormLeasePayment } from '../../models';
import PaymentsForm, { IPaymentsFormProps } from './PaymentsForm';

jest.mock('@react-keycloak/web');
const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
export const defaultTestFormLeasePayment: FormLeasePayment = {
  ...defaultFormLeasePayment,
  leasePaymentMethodType: { id: 'Cheque' },
  leasePaymentStatusTypeCode: { id: 'Paid' },
  amountTotal: 1200,
  amountGst: 100,
  amountPreTax: 1100,
  receivedDate: '2020-01-01',
  note: 'note',
  id: 1,
};

const defaultLeaseWithTermsPayments: LeaseFormModel = {
  ...new LeaseFormModel(),
  terms: [
    {
      ...defaultFormLeaseTerm,
      statusTypeCode: { id: LeaseTermStatusTypes.EXERCISED },
      payments: [{ ...defaultTestFormLeasePayment }],
    },
  ],
};

const onEdit = jest.fn();
const onDelete = jest.fn();
const onSave = jest.fn();

describe('PaymentsForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IPaymentsFormProps> & {
        initialValues?: any;
        onCancel?: () => void;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <PaymentsForm
          onEdit={onEdit}
          onDelete={onDelete}
          onSave={onSave}
          nameSpace="terms.0"
          isExercised={renderOptions?.isExercised ?? true}
          isReceivable={renderOptions?.isReceivable ?? true}
          isGstEligible={renderOptions?.isGstEligible ?? true}
        />
      </Formik>,
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
    onEdit.mockReset();
    onSave.mockReset();
    onDelete.mockReset();
  });
  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders with data as expected', async () => {
    const { component } = await setup({
      initialValues: { ...new LeaseFormModel(), terms: [defaultFormLeaseTerm] },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  describe('claim behaviour', () => {
    it('Does not display edit payment if missing claim ', async () => {
      const {
        component: { queryByTitle },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        claims: [Claims.LEASE_DELETE],
      });

      const editButton = await queryByTitle('edit actual');
      expect(editButton).toBeNull();
    });

    it('Does display edit payment if has claim ', async () => {
      const {
        component: { findAllByTitle },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        claims: [Claims.LEASE_EDIT],
      });

      const editButton = await findAllByTitle('edit actual');
      expect(editButton[0]).toBeVisible();
    });

    it('Does not display delete payment if missing claim', async () => {
      const {
        component: { queryByTitle },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        claims: [],
      });

      const deleteButton = await queryByTitle('delete actual');
      expect(deleteButton).toBeNull();
    });

    it('Does display delete payment if has claim ', async () => {
      const {
        component: { findAllByTitle },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        claims: [Claims.LEASE_EDIT],
      });
      const deleteButton = await findAllByTitle('delete actual');
      expect(deleteButton[0]).toBeVisible();
    });
  });

  describe('view only logic', () => {
    it('Displays there are no payments message if there are no payments', async () => {
      const {
        component: { findByText },
      } = await setup({
        initialValues: {
          ...defaultLeaseWithTermsPayments,
          terms: [{ ...defaultFormLeaseTerm }],
        },
      });
      const text = await findByText('There are no recorded payments for this term.');
      expect(text).toBeVisible();
    });

    it('Displays not exercised message if term is not exercised', async () => {
      const {
        component: { findByText },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        isExercised: false,
      });
      const text = await findByText('Term must be exercised to add payments.');
      expect(text).toBeVisible();
    });

    it('Displays proper received columns if received', async () => {
      const {
        component: { findByText },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        isReceivable: true,
      });
      expect(await findByText('Payments Received')).toBeVisible();
      expect(await findByText('Received date')).toBeVisible();
      expect(await findByText('Received payment ($)')).toBeVisible();
      expect(await findByText('Received total ($)')).toBeVisible();
    });

    it('Displays proper sent columns if payable', async () => {
      const {
        component: { findByText },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        isReceivable: false,
      });
      expect(await findByText('Payments Sent')).toBeVisible();
      expect(await findByText('Sent date')).toBeVisible();
      expect(await findByText('Sent payment ($)')).toBeVisible();
      expect(await findByText('Sent total ($)')).toBeVisible();
    });

    it('Does not display GST values or calculations if term is not gst eligible', async () => {
      const { findFooter, findCell, findFooterCell, findFirstRow } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        isGstEligible: false,
      });
      const row = findFirstRow() as HTMLElement;
      const footer = findFooter() as HTMLElement;
      expect(row).not.toBeNull();
      expect(footer).not.toBeNull();
      expect(findCell(row, 3)?.textContent).toBe('-');
      expect(findFooterCell(footer, 3)?.textContent).toBe('-');
    });

    it('Sums expected payment columns', async () => {
      const { findFooter, findCell, findFooterCell, findFirstRow } = await setup({
        initialValues: {
          ...new LeaseFormModel(),
          terms: [
            {
              ...defaultFormLeaseTerm,
              payments: [defaultTestFormLeasePayment, defaultTestFormLeasePayment],
            },
          ],
        },
      });
      const row = findFirstRow() as HTMLElement;
      const footer = findFooter() as HTMLElement;
      expect(row).not.toBeNull();
      expect(footer).not.toBeNull();
      expect(findFooterCell(footer, 1)?.textContent).toBe('(2) payments');
      expect(findCell(row, 2)?.textContent).toBe('$1,100.00');
      expect(findFooterCell(footer, 2)?.textContent).toBe('$2,200.00');
      expect(findCell(row, 3)?.textContent).toBe('$100.00');
      expect(findFooterCell(footer, 3)?.textContent).toBe('$200.00');
      expect(findCell(row, 4)?.textContent).toBe('$1,200.00');
      expect(findFooterCell(footer, 4)?.textContent).toBe('$2,400.00');
    });
  });
  describe('interactive functionality', () => {
    it('Allows user to enter and save a note', async () => {
      const {
        component: { findByTitle, getByText },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        isReceivable: false,
      });
      const notesButton = await findByTitle('notes');
      act(() => {
        userEvent.click(notesButton);
      });
      await act(async () => {
        await fillInput(document.body, 'terms.0.payments.0.note', 'a test note', 'textarea');
      });
      const saveButton = getByText('Save');
      act(() => {
        userEvent.click(saveButton);
      });
      expect(onSave).toHaveBeenCalledWith({ ...defaultTestFormLeasePayment, note: 'a test note' });
    });
    it('Does not update note content if note modal is cancelled', async () => {
      const {
        component: { findByTitle, getByText },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        isReceivable: false,
      });
      const notesButton = await findByTitle('notes');
      act(() => {
        userEvent.click(notesButton);
      });
      await act(async () => {
        await fillInput(document.body, 'terms.0.payments.0.note', 'a test note', 'textarea');
      });
      await screen.findByDisplayValue('a test note');
      const cancelButton = getByText('Cancel');
      act(() => {
        userEvent.click(cancelButton);
        userEvent.click(notesButton);
      });
      //expect that the note content should have returned to the original value.
      const noteText = await screen.findByDisplayValue('note');
      expect(noteText).toBeVisible();
    });

    it('Allows user to trigger an edit action', async () => {
      const {
        component: { findAllByTitle },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        isReceivable: false,
      });
      const editButton = await findAllByTitle('edit actual');
      userEvent.click(editButton[0]);
      expect(onEdit).toHaveBeenCalledWith(defaultTestFormLeasePayment);
    });

    it('Allows user to trigger a delete action', async () => {
      const {
        component: { findAllByTitle },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        isReceivable: false,
        claims: [Claims.LEASE_EDIT],
      });
      const deleteButton = await findAllByTitle('delete actual');
      userEvent.click(deleteButton[0]);
      expect(onDelete).toHaveBeenCalledWith(defaultTestFormLeasePayment);
    });
  });
});
