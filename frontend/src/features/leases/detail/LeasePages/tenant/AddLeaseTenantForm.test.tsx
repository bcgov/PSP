import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { IContactSearchResult } from 'interfaces';
import { noop } from 'lodash';
import React from 'react';
import {
  getAllByRole as getAllByRoleBase,
  mockKeycloak,
  renderAsync,
  RenderOptions,
  within,
} from 'utils/test-utils';

import AddLeaseTenantForm, { IAddLeaseTenantFormProps } from './AddLeaseTenantForm';

// mock auth library
jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

xdescribe('AddLeaseTenantForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IAddLeaseTenantFormProps> & {
        initialValues?: any;
        selectedTenants?: IContactSearchResult[];
        onCancel?: () => void;
        setSelectedTenants?: (tenants: IContactSearchResult[]) => void;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <AddLeaseTenantForm
          selectedTenants={renderOptions.selectedTenants ?? []}
          setSelectedTenants={renderOptions.setSelectedTenants ?? noop}
          onCancel={renderOptions.onCancel ?? noop}
          onSubmit={noop as any}
          formikRef={{} as any}
        />
      </Formik>,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      findFirstRow: () => {
        const rows = component.getAllByRole('row');
        return rows && rows.length > 1 ? rows[1] : null;
      },
      findFirstRowTableTwo: () => {
        const rows = within(component.getByTestId('selected-items')).getAllByRole('row');
        return rows && rows.length > 1 ? rows[1] : null;
      },
      findCell: (row: HTMLElement, index: number) => {
        const columns = getAllByRoleBase(row, 'cell');
        return columns && columns.length > index ? columns[index] : null;
      },
      component,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
    mockKeycloak({ claims: [Claims.CONTACT_VIEW] });
  });
  it('renders as expected', async () => {
    mockAxios.onGet().reply(200, []);
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('the cancel button triggers the cancel action', async () => {
    const cancel = jest.fn();
    mockAxios.onGet().reply(200, { items: [] });
    const {
      component: { getByText },
    } = await setup({ onCancel: cancel });

    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);
    expect(cancel).toHaveBeenCalled();
  });

  it('items from the contact list view can be selected', async () => {
    const setSelectedTenants = jest.fn();
    mockAxios.onGet().reply(200, {
      items: sampleContactResponse,
    });
    const {
      component: { findByTestId },
    } = await setup({ setSelectedTenants: setSelectedTenants });

    const checkBox = await findByTestId('selectrow-O5');
    userEvent.click(checkBox);
    expect(setSelectedTenants).toHaveBeenCalledWith([sampleContactResponse[1]]);
  });

  it('items from the contact list view can be added', async () => {
    mockAxios.onGet().reply(200, {
      items: sampleContactResponse,
    });
    const {
      component: { getByText, findAllByTitle },
      findFirstRowTableTwo,
      findCell,
    } = await setup({
      initialValues: { tenants: [] } as any,
      selectedTenants: [sampleContactResponse[0] as any],
    });

    const addButton = getByText('Add selected tenants');
    userEvent.click(addButton);

    await findAllByTitle('Click to remove');
    const dataRow = findFirstRowTableTwo() as HTMLElement;
    expect(dataRow).not.toBeNull();
    expect(findCell(dataRow, 3)?.textContent).toBe('Bob Billy Smith');
    expect(findCell(dataRow, 4)?.textContent).toBe('Smith');
    expect(findCell(dataRow, 5)?.textContent).toBe('Bob');
    const saveButton = getByText('Save');
    expect(saveButton).not.toBeDisabled();
  });

  it('items from the contact list cannot be duplicated', async () => {
    mockAxios.onGet().reply(200, {
      items: sampleContactResponse,
    });
    const {
      component: { getByText, getByTestId, findAllByTitle },
    } = await setup({
      initialValues: { tenants: sampleContactResponse } as any,
      selectedTenants: [sampleContactResponse[0] as any],
    });
    const addButton = getByText('Add selected tenants');
    userEvent.click(addButton);

    await findAllByTitle('Click to remove');
    const dataRows = within(getByTestId('selected-items')).getAllByRole('row');
    expect(dataRows).not.toBeNull();
    expect(dataRows).toHaveLength(3);
  });

  it('items can be removed', async () => {
    mockAxios.onAny().reply(200, {
      items: sampleContactResponse,
    });
    const {
      findFirstRowTableTwo,
      findCell,
      component: { findAllByTitle, getByText },
    } = await setup({
      initialValues: { tenants: sampleContactResponse } as any,
      selectedTenants: [sampleContactResponse[0] as any],
    });

    const addButton = getByText('Add selected tenants');
    userEvent.click(addButton);

    await findAllByTitle('Click to remove');
    let dataRow = findFirstRowTableTwo() as HTMLElement;
    expect(dataRow).not.toBeNull();
    const deleteCell = findCell(dataRow, 0);
    deleteCell && userEvent.click(deleteCell);

    dataRow = findFirstRowTableTwo() as HTMLElement;
    expect(findCell(dataRow, 3)?.textContent).toBe('Bob Billy Smith');
    expect(findCell(dataRow, 5)?.textContent).toBe('Bob');
  });
});

const sampleContactResponse = [
  {
    id: 'P2',
    personId: 2,
    organizationId: 5,
    rowVersion: 0,
    summary: 'Bob Billy Smith',
    surname: 'Smith',
    firstName: 'Bob',
    isDisabled: false,
  },
  {
    id: 'O5',
    organizationId: 5,
    rowVersion: 0,
    summary: "Bob's Property Management",
    organizationName: "Bob's Property Management",
    isDisabled: false,
  },
];
