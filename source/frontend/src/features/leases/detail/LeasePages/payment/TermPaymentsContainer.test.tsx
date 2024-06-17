import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import noop from 'lodash/noop';

import { Claims } from '@/constants';
import { LeaseTermStatusTypes } from '@/constants/leaseStatusTypes';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { useLeaseTermRepository } from '@/hooks/repositories/useLeaseTermRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { defaultApiLease } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes/lookupCodesSlice';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { act, fillInput, renderAsync, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { defaultFormLeaseTerm, FormLeaseTerm } from './models';
import { defaultTestFormLeasePayment } from './table/payments/PaymentsForm.test';
import TermPaymentsContainer from './TermPaymentsContainer';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';
import { ApiGen_CodeTypes_LeaseAccountTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseAccountTypes';
import { Mock } from 'vitest';

const defaultRepositoryResponse = {
  execute: vi.fn(),
  response: {} as any,
  error: undefined,
  status: undefined,
  loading: false,
};

const getLeaseTerms = vi.fn();
const mockGetLeaseTerms = {
  execute: getLeaseTerms,
  response: [
    FormLeaseTerm.toApi({ ...defaultFormLeaseTerm, expiryDate: '2020-01-01', payments: [] }),
  ],
  error: undefined,
  status: undefined,
  loading: false,
};

const defaultLeaseWithTermsPayments: LeaseFormModel = {
  ...new LeaseFormModel(),
  terms: [
    {
      ...defaultFormLeaseTerm,
      expiryDate: '2020-01-01',
      statusTypeCode: toTypeCodeNullable(LeaseTermStatusTypes.EXERCISED),
      payments: [{ ...defaultTestFormLeasePayment }],
    },
  ],
};

vi.mock('@/hooks/repositories/useUserInfoRepository');
vi.mock('@/hooks/repositories/useLeaseTermRepository');
vi.mocked(useLeaseTermRepository).mockReturnValue({
  getLeaseTerms: mockGetLeaseTerms,
  updateLeaseTerm: { ...defaultRepositoryResponse },
  addLeaseTerm: { ...defaultRepositoryResponse },
  deleteLeaseTerm: { ...defaultRepositoryResponse },
});

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const setLease = vi.fn();
const onSuccessMock = vi.fn();

describe('TermsPaymentsContainer component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<LeasePageProps> & {
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
          <TermPaymentsContainer
            formikRef={createRef()}
            isEditing={false}
            onSuccess={onSuccessMock}
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
      initialValues: { ...defaultApiLease(), terms: [defaultFormLeaseTerm] },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  describe('term logic tests', () => {
    it('when adding a new initial term, the start date is set to the start date of the lease', async () => {
      const {
        component: { getAllByText, getByDisplayValue },
      } = await setup({ claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD] });
      mockAxios.onPost().reply(200, { id: 1 });

      const addButton = getAllByText('Add a Term')[0];
      await act(async () => {
        userEvent.click(addButton);
      });

      expect(getByDisplayValue('Jan 01, 2020')).toBeVisible();
    });

    it(`doesn't make any request when cancelling the term modal`, async () => {
      const {
        component: { getAllByText, getByText },
      } = await setup({ claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD] });

      const addButton = getAllByText('Add a Term')[0];
      await act(async () => {
        userEvent.click(addButton);
      });

      const cancelButton = getByText('No');
      await act(async () => userEvent.click(cancelButton));

      expect(useLeaseTermRepository().addLeaseTerm.execute).not.toHaveBeenCalled();
      expect(onSuccessMock).not.toHaveBeenCalled();
    });

    it('makes a post request when adding a new term', async () => {
      (useLeaseTermRepository().updateLeaseTerm.execute as Mock).mockResolvedValue(
        FormLeaseTerm.toApi({ ...defaultFormLeaseTerm, id: 1 }),
      );
      mockGetLeaseTerms.execute.mockResolvedValue(
        (mockGetLeaseTerms.response = [
          FormLeaseTerm.toApi({ ...defaultFormLeaseTerm, payments: [], id: 1 }),
        ]),
      );
      const {
        component: { getAllByText, getByText },
      } = await setup({ claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD] });
      mockAxios.onPost().reply(200, { id: 1 });

      const addButton = getAllByText('Add a Term')[0];
      await act(async () => {
        userEvent.click(addButton);
      });

      await fillInput(document.body, 'startDate', '2020-01-01', 'datepicker');
      const saveButton = getByText('Yes');
      await act(async () => userEvent.click(saveButton));

      expect(useLeaseTermRepository().addLeaseTerm.execute).toHaveBeenCalled();
      expect(onSuccessMock).toHaveBeenCalled();
    });

    it('makes a put request when updating a term', async () => {
      (useLeaseTermRepository().updateLeaseTerm.execute as Mock).mockResolvedValue(
        FormLeaseTerm.toApi({ ...defaultFormLeaseTerm, id: 1 }),
      );
      mockGetLeaseTerms.execute.mockResolvedValue(
        (mockGetLeaseTerms.response = [
          FormLeaseTerm.toApi({ ...defaultFormLeaseTerm, payments: [], id: 1 }),
        ]),
      );
      const {
        component: { findAllByTitle, getByText },
      } = await setup({
        initialValues: { ...defaultApiLease(), terms: [{ ...defaultFormLeaseTerm, id: 1 }] },
        claims: [Claims.LEASE_EDIT],
      });
      mockAxios.onPut().reply(200, { id: 1 });

      const editButton = (await findAllByTitle('edit term'))[0];
      await act(async () => {
        userEvent.click(editButton);
      });

      await fillInput(document.body, 'startDate', '2020-01-01', 'datepicker');
      const saveButton = getByText('Yes');
      await act(async () => userEvent.click(saveButton));

      expect(useLeaseTermRepository().updateLeaseTerm.execute).toHaveBeenCalled();
      expect(onSuccessMock).toHaveBeenCalled();
    });

    it('deleting a term with payments is not possible', async () => {
      const {
        component: { queryByTitle },
      } = await setup({
        initialValues: {
          ...defaultApiLease(),
          terms: [{ ...defaultFormLeaseTerm, id: 1, payments: [{ id: 1 }] }],
        },
        claims: [Claims.LEASE_DELETE],
      });

      const deleteButton = queryByTitle('delete term');
      expect(deleteButton).toBeNull();
    });

    it('displays a warning when deleting the initial term when there are other terms', async () => {
      mockGetLeaseTerms.execute.mockResolvedValue(
        (mockGetLeaseTerms.response = [
          FormLeaseTerm.toApi({ ...defaultFormLeaseTerm, payments: [], id: 1 }),
          FormLeaseTerm.toApi({ ...defaultFormLeaseTerm, payments: [], id: 1 }),
        ]),
      );
      const {
        component: { findAllByTitle, getByText },
      } = await setup({
        initialValues: {
          ...defaultApiLease(),
          terms: [
            { ...defaultFormLeaseTerm, id: 1 },
            { ...defaultFormLeaseTerm, id: 1 },
          ],
        },
        claims: [Claims.LEASE_EDIT],
      });

      const deleteButton = (await findAllByTitle('delete term'))[0];
      await act(async () => userEvent.click(deleteButton));
      const warning = getByText('You must delete all renewals before deleting the initial term.');
      expect(warning).toBeVisible();
    });

    it('asks for confirmation when deleting a term', async () => {
      mockGetLeaseTerms.execute.mockResolvedValue(
        (mockGetLeaseTerms.response = [
          FormLeaseTerm.toApi({ ...defaultFormLeaseTerm, payments: [], id: 1 }),
        ]),
      );
      const {
        component: { findAllByTitle },
      } = await setup({
        initialValues: {
          ...defaultApiLease(),
          terms: [{ ...defaultFormLeaseTerm, id: 1 }],
        },
        claims: [Claims.LEASE_EDIT],
      });

      const deleteButton = (await findAllByTitle('delete term'))[0];
      await act(async () => userEvent.click(deleteButton));
      const warning = await screen.findByText(
        'You are about to delete a term. Do you wish to continue?',
      );
      expect(warning).toBeVisible();
    });

    it('makes a delete request when delete confirmed', async () => {
      mockGetLeaseTerms.execute.mockResolvedValue(
        (mockGetLeaseTerms.response = [
          FormLeaseTerm.toApi({ ...defaultFormLeaseTerm, payments: [], id: 1 }),
        ]),
      );
      const {
        component: { findAllByTitle, getByText },
      } = await setup({
        initialValues: {
          ...defaultApiLease(),
          terms: [{ ...defaultFormLeaseTerm, id: 1 }],
        },
        claims: [Claims.LEASE_EDIT],
      });
      mockAxios.onDelete().reply(200, { id: 1 });

      const deleteButton = (await findAllByTitle('delete term'))[0];
      await act(async () => userEvent.click(deleteButton));
      const warning = getByText('You are about to delete a term. Do you wish to continue?');
      expect(warning).toBeVisible();
      const continueButton = getByText('Continue');
      await act(async () => userEvent.click(continueButton));

      expect(useLeaseTermRepository().deleteLeaseTerm.execute).toHaveBeenCalled();
    });

    it.each([
      [ApiGen_CodeTypes_LeaseAccountTypes.RCVBL, true],
      [ApiGen_CodeTypes_LeaseAccountTypes.PYBLBCTFA, false],
      [ApiGen_CodeTypes_LeaseAccountTypes.PYBLMOTI, false],
    ])(
      'when adding a new term, the GST field is defaulted based on lease type - %s',
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

        const addButton = getAllByText('Add a Term')[0];
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
    mockGetLeaseTerms.execute.mockResolvedValue(
      defaultLeaseWithTermsPayments.terms.map(t => FormLeaseTerm.toApi(t)),
    );

    it('makes a post request when adding a new payment', async () => {
      mockGetLeaseTerms.execute.mockResolvedValue(
        (mockGetLeaseTerms.response = defaultLeaseWithTermsPayments.terms.map(t =>
          FormLeaseTerm.toApi(t),
        )),
      );
      const {
        component: { findByText, getByText, findByTestId },
      } = await setup({
        claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD],
        initialValues: defaultLeaseWithTermsPayments,
      });
      mockAxios.onPost().reply(200, { id: 1 });

      const expander = await findByTestId('table-row-expander-');
      await act(async () => userEvent.click(expander));
      const addButton = await findByText('Record a Payment');
      await act(async () => {
        userEvent.click(addButton);
      });

      await fillInput(document.body, 'receivedDate', '2020-01-01', 'datepicker');
      const saveButton = getByText('Save payment');
      await act(async () => userEvent.click(saveButton));
      expect(mockAxios.history.post.length).toBe(1);
    });

    it(`doesn't make any request when payment modal is cancelled`, async () => {
      mockGetLeaseTerms.execute.mockResolvedValue(
        (mockGetLeaseTerms.response = defaultLeaseWithTermsPayments.terms.map(t =>
          FormLeaseTerm.toApi(t),
        )),
      );
      const {
        component: { findByText, getByText, findByTestId },
      } = await setup({
        claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD],
        initialValues: defaultLeaseWithTermsPayments,
      });
      mockAxios.onPost().reply(200, { id: 1 });

      const expander = await findByTestId('table-row-expander-');
      await act(async () => userEvent.click(expander));
      const addButton = await findByText('Record a Payment');
      await act(async () => {
        userEvent.click(addButton);
      });

      await fillInput(document.body, 'receivedDate', '2020-01-01', 'datepicker');
      const cancelButton = getByText('Cancel');
      await act(async () => userEvent.click(cancelButton));
      expect(mockAxios.history.post.length).toBe(0);
    });

    it('makes a put request when updating a payment', async () => {
      mockGetLeaseTerms.execute.mockResolvedValue(
        (mockGetLeaseTerms.response = defaultLeaseWithTermsPayments.terms.map(t =>
          FormLeaseTerm.toApi(t),
        )),
      );
      const {
        component: { findAllByTitle, findByTestId, getByText, findByText },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD],
      });
      mockAxios.onPut().reply(200, { id: 1 });

      const expander = await findByTestId('table-row-expander-');
      await act(async () => userEvent.click(expander));
      const editButton = await findAllByTitle('edit actual');
      await act(async () => {
        userEvent.click(editButton[0]);
      });

      await fillInput(document.body, 'startDate', '2020-01-01', 'datepicker');
      const saveButton = getByText('Save payment');
      await act(async () => userEvent.click(saveButton));
      expect(mockAxios.history.put.length).toBe(1);
    });

    it('asks for confirmation when deleting a payment', async () => {
      mockGetLeaseTerms.execute.mockResolvedValue(
        (mockGetLeaseTerms.response = defaultLeaseWithTermsPayments.terms.map(t =>
          FormLeaseTerm.toApi(t),
        )),
      );
      const {
        component: { findAllByTitle, getByText, findByText, getByTestId },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
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
      mockGetLeaseTerms.execute.mockResolvedValue(
        (mockGetLeaseTerms.response = defaultLeaseWithTermsPayments.terms.map(t =>
          FormLeaseTerm.toApi(t),
        )),
      );
      const {
        component: { findAllByTitle, getByText, findByText, getByTestId },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
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
