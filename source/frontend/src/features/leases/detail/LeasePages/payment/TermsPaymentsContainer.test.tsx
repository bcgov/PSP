import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims, LeaseTermStatusTypes } from 'constants/index';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { LeasePageProps } from 'features/properties/map/lease/LeaseContainer';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, defaultFormLeaseTerm, defaultLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import React from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import { defaultTestFormLeasePayment } from './table/payments/PaymentsForm.test';
import TermPaymentsContainer from './TermPaymentsContainer';

jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const setLease = jest.fn();

const defaultLeaseWithTermsPayments: IFormLease = {
  ...defaultFormLease,
  terms: [
    {
      ...defaultFormLeaseTerm,
      statusTypeCode: { id: LeaseTermStatusTypes.EXERCISED },
      payments: [{ ...defaultTestFormLeasePayment }],
    },
  ],
};

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
            ...defaultLease,
            ...renderOptions.initialValues,
            id: 1,
            startDate: '2020-01-01',
          },
          setLease,
        }}
      >
        <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
          <TermPaymentsContainer formikRef={React.createRef()} isEditing={false} />
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

    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders with data as expected', async () => {
    const { component } = await setup({
      claims: [Claims.LEASE_EDIT],
      initialValues: { ...defaultFormLease, terms: [defaultFormLeaseTerm] },
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
      act(() => {
        userEvent.click(addButton);
      });

      expect(getByDisplayValue('01/01/2020')).toBeVisible();
    });
    it('makes a post request when adding a new term', async () => {
      const {
        component: { getAllByText, getByText },
      } = await setup({ claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD] });
      mockAxios.onPost().reply(200, { id: 1 });

      const addButton = getAllByText('Add a Term')[0];
      act(() => {
        userEvent.click(addButton);
      });

      await fillInput(document.body, 'startDate', '2020-01-01', 'datepicker');
      const saveButton = getByText('Save term');
      await act(() => userEvent.click(saveButton));
      expect(mockAxios.history.post.length).toBe(1);
      expect(setLease).toHaveBeenCalled();
    });

    it('makes a put request when updating a term', async () => {
      const {
        component: { getAllByTitle, getByText },
      } = await setup({
        initialValues: { ...defaultLease, terms: [{ ...defaultFormLeaseTerm, id: 1 }] },
        claims: [Claims.LEASE_EDIT],
      });
      mockAxios.onPut().reply(200, { id: 1 });

      const editButton = getAllByTitle('edit term')[0];
      act(() => {
        userEvent.click(editButton);
      });

      await fillInput(document.body, 'startDate', '2020-01-01', 'datepicker');
      const saveButton = getByText('Save term');
      await act(() => userEvent.click(saveButton));
      expect(mockAxios.history.put.length).toBe(1);
      expect(setLease).toHaveBeenCalled();
    });

    it('deleting a term with payments is not possible', async () => {
      const {
        component: { queryByTitle },
      } = await setup({
        initialValues: {
          ...defaultLease,
          terms: [{ ...defaultFormLeaseTerm, id: 1, payments: [{ id: 1 }] }],
        },
        claims: [Claims.LEASE_DELETE],
      });

      const deleteButton = queryByTitle('delete term');
      expect(deleteButton).toBeNull();
    });

    it('displays a warning when deleting the initial term when there are other terms', async () => {
      const {
        component: { getAllByTitle, getByText },
      } = await setup({
        initialValues: {
          ...defaultLease,
          terms: [
            { ...defaultFormLeaseTerm, id: 1 },
            { ...defaultFormLeaseTerm, id: 1 },
          ],
        },
        claims: [Claims.LEASE_EDIT],
      });

      const deleteButton = getAllByTitle('delete term')[0];
      await act(() => userEvent.click(deleteButton));
      const warning = getByText('You must delete all renewals before deleting the initial term.');
      expect(warning).toBeVisible();
    });

    it('asks for confirmation when deleting a term', async () => {
      const {
        component: { getAllByTitle, getByText },
      } = await setup({
        initialValues: {
          ...defaultLease,
          terms: [{ ...defaultFormLeaseTerm, id: 1 }],
        },
        claims: [Claims.LEASE_EDIT],
      });

      const deleteButton = getAllByTitle('delete term')[0];
      await act(() => userEvent.click(deleteButton));
      const warning = getByText('You are about to delete a term. Do you wish to continue?');
      expect(warning).toBeVisible();
    });

    it('makes a delete request when delete confirmed', async () => {
      const {
        component: { getAllByTitle, getByText },
      } = await setup({
        initialValues: {
          ...defaultLease,
          terms: [{ ...defaultFormLeaseTerm, id: 1 }],
        },
        claims: [Claims.LEASE_EDIT],
      });
      mockAxios.onDelete().reply(200, { id: 1 });

      const deleteButton = getAllByTitle('delete term')[0];
      await act(() => userEvent.click(deleteButton));
      const warning = getByText('You are about to delete a term. Do you wish to continue?');
      expect(warning).toBeVisible();
      const continueButton = getByText('Continue');
      await act(() => userEvent.click(continueButton));
      expect(mockAxios.history.delete.length).toBe(1);
      expect(setLease).toHaveBeenCalled();
    });
  });
  describe('payments logic tests', () => {
    it('makes a post request when adding a new payment', async () => {
      const {
        component: { findByText, getByText },
      } = await setup({
        claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD],
        initialValues: defaultLeaseWithTermsPayments,
      });
      mockAxios.onPost().reply(200, { id: 1 });

      const addButton = await findByText('Record a Payment');
      await userEvent.click(addButton);

      await fillInput(document.body, 'receivedDate', '2020-01-01', 'datepicker');
      const saveButton = getByText('Save payment');
      await act(() => userEvent.click(saveButton));
      expect(mockAxios.history.post.length).toBe(1);
      expect(setLease).toHaveBeenCalled();
    });

    it('makes a put request when updating a payment', async () => {
      const {
        component: { findAllByTitle, getByText },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        claims: [Claims.LEASE_EDIT, Claims.LEASE_ADD],
      });
      mockAxios.onPut().reply(200, { id: 1 });

      const editButton = await findAllByTitle('edit actual');
      await userEvent.click(editButton[0]);

      await fillInput(document.body, 'startDate', '2020-01-01', 'datepicker');
      const saveButton = getByText('Save payment');
      await act(() => userEvent.click(saveButton));
      expect(mockAxios.history.put.length).toBe(1);
      expect(setLease).toHaveBeenCalled();
    });
    it('asks for confirmation when deleting a payment', async () => {
      const {
        component: { findAllByTitle, getByText },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        claims: [Claims.LEASE_EDIT],
      });

      const deleteButton = (await findAllByTitle('delete actual'))[0];
      await act(() => userEvent.click(deleteButton));
      const warning = getByText('You are about to delete a payment. Do you wish to continue?');
      expect(warning).toBeVisible();
    });

    it('makes a delete request when delete payment confirmed', async () => {
      const {
        component: { findAllByTitle, getByText },
      } = await setup({
        initialValues: defaultLeaseWithTermsPayments,
        claims: [Claims.LEASE_EDIT],
      });
      mockAxios.onDelete().reply(200, { id: 1 });

      const deleteButton = (await findAllByTitle('delete actual'))[0];
      await act(() => userEvent.click(deleteButton));
      const warning = getByText('You are about to delete a payment. Do you wish to continue?');
      expect(warning).toBeVisible();
      const continueButton = getByText('Continue');
      await act(() => userEvent.click(continueButton));
      expect(mockAxios.history.delete.length).toBe(1);
      expect(setLease).toHaveBeenCalled();
    });
  });
});
