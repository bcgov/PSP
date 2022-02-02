import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { mockOrganization, mockPerson } from 'mocks/filterDataMock';
import { render, RenderOptions } from 'utils/test-utils';

import Tenant, { ITenantProps } from './Tenant';

const history = createMemoryHistory();

describe('Tenant component', () => {
  const setup = (renderOptions: RenderOptions & ITenantProps & { lease?: IFormLease } = {}) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
        <Tenant nameSpace={renderOptions.nameSpace} />
      </Formik>,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };
  it('renders as expected', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, persons: [mockPerson], organizations: [mockOrganization] },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders one Person Tenant section per person', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, persons: [mockPerson, mockPerson] },
    });
    const { getAllByText } = component;
    const personTenant = getAllByText('Tenant Name:');

    expect(personTenant).toHaveLength(2);
  });

  it('renders one notes section per tenant note', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, tenantNotes: ['note one', 'note two'] },
    });
    const { getAllByText } = component;
    const personTenant = getAllByText('Notes:');

    expect(personTenant).toHaveLength(2);
  });

  it('renders no person information section if there are no persons', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, persons: [] },
    });
    const { queryByText } = component;
    const personTenant = queryByText('Tenant Name:');

    expect(personTenant).toBeNull();
  });

  it('renders one organization tenant section per organization', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, organizations: [mockOrganization, mockOrganization] },
    });
    const { getAllByText } = component;
    const organizationTenant = getAllByText('Tenant organization:');

    expect(organizationTenant).toHaveLength(2);
  });

  it('renders no organization information section if there are no organizations', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, organizations: [] },
    });
    const { queryByText } = component;
    const organizationTenant = queryByText('Tenant organization:');

    expect(organizationTenant).toBeNull();
  });

  it('renders organization phone numbers as expected', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, organizations: [mockOrganization] },
    });
    const { getByLabelText } = component;
    const landline = getByLabelText('Landline:');
    const mobile = getByLabelText('Mobile:');

    expect(landline).toHaveDisplayValue('222-333-4444');
    expect(mobile).toHaveDisplayValue('555-666-7777');
  });

  it('renders person phone numbers as expected', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, persons: [mockPerson] },
    });
    const { getByLabelText } = component;
    const landline = getByLabelText('Landline:');
    const mobile = getByLabelText('Mobile:');

    expect(landline).toHaveDisplayValue('222-333-4444');
    expect(mobile).toHaveDisplayValue('555-666-7777');
  });
});
