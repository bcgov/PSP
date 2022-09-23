import { apiLeaseToFormLease } from 'features/leases/leaseUtils';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { mockApiPerson, mockOrganization } from 'mocks/filterDataMock';
import { getMockLease } from 'mocks/mockLease';
import { getMockOrganization } from 'mocks/mockOrganization';
import { render, RenderOptions } from 'utils/test-utils';

import Tenant, { FormTenant, ITenantProps } from './Tenant';

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
      lease: { ...defaultFormLease, persons: [mockApiPerson], organizations: [mockOrganization] },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders one Person Tenant section per person', () => {
    const { component } = setup({
      lease: {
        ...defaultFormLease,
        tenants: [
          new FormTenant({
            lessorType: { id: 'PER' },
            person: { ...mockApiPerson },
            leaseId: 1,
          }),
          new FormTenant({
            lessorType: { id: 'PER' },
            person: { ...mockApiPerson },
            leaseId: 1,
          }),
        ],
      },
    });
    const { getAllByText } = component;
    const personTenant = getAllByText('Tenant Name:');

    expect(personTenant).toHaveLength(2);
  });

  it('renders one notes section per tenant note', () => {
    const { component } = setup({
      lease: {
        ...defaultFormLease,
        persons: [mockApiPerson, mockOrganization],
        tenants: [
          { personId: mockApiPerson.id, note: 'person note' },
          { organizationId: mockOrganization.id, note: 'organization id' },
        ],
      },
    });
    const { getAllByText } = component;
    const notes = getAllByText('Notes');

    expect(notes).toHaveLength(2);
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
      lease: {
        ...defaultFormLease,
        tenants: [
          new FormTenant({
            organization: { ...getMockOrganization() },
            leaseId: 1,
          }),
          new FormTenant({
            organization: { ...getMockOrganization() },
            leaseId: 1,
          }),
        ],
      },
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
      lease: {
        ...defaultFormLease,
        tenants: [
          new FormTenant({
            organization: { ...getMockOrganization() },
            leaseId: 1,
          }),
        ],
      },
    });
    const { getByLabelText } = component;
    const landline = getByLabelText('Landline:');
    const mobile = getByLabelText('Mobile:');

    expect(landline).toHaveDisplayValue('222-333-4444');
    expect(mobile).toHaveDisplayValue('555-666-7777');
  });

  it('renders person phone numbers as expected', () => {
    const { component } = setup({
      lease: {
        ...defaultFormLease,
        tenants: [new FormTenant({ person: mockApiPerson, leaseId: 1 })],
      },
    });
    const { getByLabelText } = component;
    const landline = getByLabelText('Landline:');
    const mobile = getByLabelText('Mobile:');

    expect(landline).toHaveDisplayValue('222-333-4444');
    expect(mobile).toHaveDisplayValue('555-666-7777');
  });

  it('renders primary contact successfully', () => {
    const mockLeaseWithTenants = getMockLease();
    const { component } = setup({
      lease: apiLeaseToFormLease(mockLeaseWithTenants),
    });
    const { getByText } = component;
    const primaryContact = getByText('Bob Billy Smith');
    expect(primaryContact).toBeVisible();
  });

  it('renders primary contact even if not part of organization persons', () => {
    const mockLeaseWithTenants = getMockLease();
    if (mockLeaseWithTenants.tenants[0].primaryContact?.id) {
      mockLeaseWithTenants.tenants[0].primaryContact.id = 5;
    }
    mockLeaseWithTenants.tenants[0].primaryContactId = 5;
    const { component } = setup({
      lease: apiLeaseToFormLease(mockLeaseWithTenants),
    });
    const { getByText } = component;
    const primaryContact = getByText('Bob Billy Smith');
    expect(primaryContact).toBeVisible();
  });

  it('renders updated primary contact if primaryContact id does not match', () => {
    const mockLeaseWithTenants = getMockLease();
    mockLeaseWithTenants.tenants[0].primaryContactId = 4; // this is the id of the other person associated to this organization tenant.
    const { component } = setup({
      lease: apiLeaseToFormLease(mockLeaseWithTenants),
    });
    const { getByText } = component;
    const primaryContact = getByText('Minnie Nacho Cheese Mouse');
    expect(primaryContact).toBeVisible();
  });
});
