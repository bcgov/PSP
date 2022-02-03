import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, defaultFormLeaseTerm, defaultLease } from 'interfaces';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions, waitFor } from 'utils/test-utils';

import { IPaymentsContainerProps, PaymentsContainer } from './PaymentsContainer';

jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const setLease = jest.fn();

describe('PaymentsContainer component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IPaymentsContainerProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <LeaseStateContext.Provider
        value={{ lease: { ...defaultLease, ...renderOptions.initialValues, id: 1 }, setLease }}
      >
        <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
          <PaymentsContainer />
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

  it('makes a post request when adding a new term', async () => {
    const {
      component: { getAllByText, getByText },
    } = await setup({});
    mockAxios.onPost().reply(200, { id: 1 });

    const addButton = getAllByText('Add a Term')[0];
    userEvent.click(addButton);

    await fillInput(document.body, 'startDate', '2020-01-01', 'datepicker');
    const saveButton = getByText('Save term');
    userEvent.click(saveButton);
    await waitFor(() => expect(mockAxios.history.post.length).toBe(1));
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
    userEvent.click(editButton);

    await fillInput(document.body, 'startDate', '2020-01-01', 'datepicker');
    const saveButton = getByText('Save term');
    userEvent.click(saveButton);
    await waitFor(() => expect(mockAxios.history.put.length).toBe(1));
    expect(setLease).toHaveBeenCalled();
  });

  it('deleting a term with payments is not possible', async () => {
    const {
      component: { getAllByTitle },
    } = await setup({
      initialValues: {
        ...defaultLease,
        terms: [{ ...defaultFormLeaseTerm, id: 1, payments: [{ id: 1 }] }],
      },
      claims: [Claims.LEASE_EDIT],
    });

    const deleteButton = getAllByTitle('delete term')[0];
    expect(deleteButton).toBeDisabled();
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
    userEvent.click(deleteButton);
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
    userEvent.click(deleteButton);
    const warning = getByText('You are about to delete a term. Do you wish to continue?');
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
    mockAxios.onDelete().reply(200, { id: 1 });

    const deleteButton = getAllByTitle('delete term')[0];
    userEvent.click(deleteButton);
    const warning = getByText('You are about to delete a term. Do you wish to continue?');
    expect(warning).toBeVisible();
    const continueButton = getByText('Continue');
    userEvent.click(continueButton);
    await waitFor(() => expect(mockAxios.history.delete.length).toBe(1));
    expect(setLease).toHaveBeenCalled();
  });
});
