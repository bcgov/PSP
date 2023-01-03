import { LeaseContextProvider } from 'features/leases/context/LeaseContext';
import { createMemoryHistory } from 'history';
import { defaultLease, ILease } from 'interfaces';
import { mockApiPerson, mockOrganization } from 'mocks/filterDataMock';
import { getMockLease } from 'mocks/mockLease';
import { render, RenderOptions } from 'utils/test-utils';

import Tenant, { ITenantProps } from './ViewTenantForm';

const history = createMemoryHistory();

describe('Tenant component', () => {
  const setup = (renderOptions: RenderOptions & ITenantProps & { lease?: ILease } = {}) => {
    // render component under test
    const component = render(
      <LeaseContextProvider initialLease={renderOptions.lease ? renderOptions.lease : defaultLease}>
        <Tenant nameSpace={renderOptions.nameSpace} />
      </LeaseContextProvider>,
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
      lease: { ...defaultLease, persons: [mockApiPerson], organizations: [mockOrganization] },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders one Person Tenant section per person', () => {
    const { component } = setup({
      lease: {
        ...defaultLease,
        tenants: [
          { leaseId: 1, personId: mockApiPerson.id, note: 'person note' },
          { leaseId: 1, organizationId: mockOrganization.id, note: 'organization id' },
        ],
      },
    });
    const { getAllByText } = component;
    const tenantSection = getAllByText('Tenant');

    expect(tenantSection).toHaveLength(1);
  });

  // notes are disabled
  it.skip('renders one notes section per tenant note', () => {
    const { component } = setup({
      lease: {
        ...defaultLease,
        persons: [mockApiPerson, mockOrganization],
        tenants: [
          { leaseId: 1, personId: mockApiPerson.id, note: 'person note' },
          { leaseId: 1, organizationId: mockOrganization.id, note: 'organization id' },
        ],
      },
    });
    const { getAllByText } = component;
    const notes = getAllByText('Notes');

    expect(notes).toHaveLength(2);
  });

  it('renders representative section', () => {
    const { component } = setup({
      lease: { ...defaultLease, persons: [] },
    });
    const { getAllByText } = component;
    const repSection = getAllByText('Representative');

    expect(repSection).toHaveLength(1);
  });

  it('renders property manager section', () => {
    const { component } = setup({
      lease: { ...defaultLease, persons: [] },
    });
    const { getAllByText } = component;
    const propSection = getAllByText('Property Manager');

    expect(propSection).toHaveLength(1);
  });
  it('renders unknown section', () => {
    const { component } = setup({
      lease: { ...defaultLease, persons: [] },
    });
    const { getAllByText } = component;
    const unknownSection = getAllByText('Unknown');

    expect(unknownSection).toHaveLength(1);
  });

  it('renders summary successfully', () => {
    const mockLeaseWithTenants = getMockLease();
    const { component } = setup({
      lease: mockLeaseWithTenants,
    });
    const { getByText } = component;
    const summary = getByText('French Mouse Property Management');
    expect(summary).toBeVisible();
  });

  it('renders primary contact successfully', () => {
    const mockLeaseWithTenants = getMockLease();
    const { component } = setup({
      lease: mockLeaseWithTenants,
    });
    const { getByText } = component;
    const primaryContact = getByText('Bob Billy Smith');
    expect(primaryContact).toBeVisible();
  });
});
