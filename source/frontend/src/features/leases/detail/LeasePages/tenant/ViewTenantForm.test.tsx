import { createMemoryHistory } from 'history';

import { LeaseContextProvider } from '@/features/leases/context/LeaseContext';
import { mockApiOrganization, mockApiPerson, mockOrganization } from '@/mocks/filterData.mock';
import { getEmptyLeaseTenant, getMockApiLease } from '@/mocks/lease.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { defaultApiLease } from '@/models/defaultInitializers';
import { render, RenderOptions } from '@/utils/test-utils';

import { FormTenant } from './models';
import { ITenantProps, ViewTenantForm } from './ViewTenantForm';

const history = createMemoryHistory();

describe('Tenant component', () => {
  const setup = (
    renderOptions: RenderOptions & ITenantProps & { lease?: ApiGen_Concepts_Lease } = {
      tenants: [],
      isPayableLease: false,
    },
  ) => {
    // render component under test
    const component = render(
      <LeaseContextProvider
        initialLease={renderOptions.lease ? renderOptions.lease : defaultApiLease()}
      >
        <ViewTenantForm
          nameSpace={renderOptions.nameSpace}
          tenants={renderOptions.tenants ?? []}
          isPayableLease={renderOptions.isPayableLease ?? false}
        />
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
      lease: {
        ...defaultApiLease(),
        stakeholders: [
          { ...getEmptyLeaseTenant(), leaseId: 1, person: mockApiPerson },
          { ...getEmptyLeaseTenant(), leaseId: 1, organization: mockApiOrganization },
        ],
      },
      tenants: [],
      isPayableLease: false,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders one Person Tenant section per person', () => {
    const { component } = setup({
      lease: {
        ...defaultApiLease(),
      },
      tenants: [
        { leaseId: 1, personId: mockApiPerson.id, note: 'person note' },
        {
          leaseId: 1,
          organizationId: mockOrganization.id,
          note: 'organization id',
        },
      ],
      isPayableLease: false,
    });
    const { getAllByText } = component;
    const tenantSection = getAllByText('Tenant');

    expect(tenantSection).toHaveLength(1);
  });

  // notes are disabled
  it.skip('renders one notes section per tenant note', () => {
    const { component } = setup({
      lease: {
        ...defaultApiLease(),
        stakeholders: [
          { ...getEmptyLeaseTenant(), leaseId: 1, person: mockApiPerson },
          { ...getEmptyLeaseTenant(), leaseId: 1, organization: mockApiOrganization },
        ],
      },
      tenants: [
        { leaseId: 1, personId: mockApiPerson.id, note: 'person note' },
        { leaseId: 1, organizationId: mockOrganization.id, note: 'organization id' },
      ],
      isPayableLease: false,
    });
    const { getAllByText } = component;
    const notes = getAllByText('Notes');

    expect(notes).toHaveLength(2);
  });

  it('renders assignee section', () => {
    const { component } = setup({
      lease: { ...defaultApiLease(), stakeholders: [] },
      tenants: [],
      isPayableLease: false,
    });
    const { getAllByText } = component;
    const asgnSection = getAllByText('Assignee');

    expect(asgnSection).toHaveLength(1);
  });

  it('renders representative section', () => {
    const { component } = setup({
      lease: { ...defaultApiLease(), stakeholders: [] },
      tenants: [],
      isPayableLease: false,
    });
    const { getAllByText } = component;
    const repSection = getAllByText('Representative');

    expect(repSection).toHaveLength(1);
  });

  it('renders property manager section', () => {
    const { component } = setup({
      lease: { ...defaultApiLease(), stakeholders: [] },
      tenants: [],
      isPayableLease: false,
    });
    const { getAllByText } = component;
    const propSection = getAllByText('Property Manager');

    expect(propSection).toHaveLength(1);
  });

  it('renders unknown section', () => {
    const { component } = setup({
      lease: { ...defaultApiLease(), stakeholders: [] },
      tenants: [],
      isPayableLease: false,
    });
    const { getAllByText } = component;
    const unknownSection = getAllByText('Unknown');

    expect(unknownSection).toHaveLength(1);
  });

  it('renders owner section', () => {
    const { component } = setup({
      lease: { ...defaultApiLease(), stakeholders: [] },
      tenants: [],
      isPayableLease: true,
    });
    const { getAllByText } = component;
    const unknownSection = getAllByText('Owner');

    expect(unknownSection).toHaveLength(1);
  });

  it('renders owner representative section', () => {
    const { component } = setup({
      lease: { ...defaultApiLease(), stakeholders: [] },
      tenants: [],
      isPayableLease: true,
    });
    const { getAllByText } = component;
    const unknownSection = getAllByText('Owner Representative');

    expect(unknownSection).toHaveLength(1);
  });

  it('renders summary successfully', () => {
    const mockLeaseWithTenants = getMockApiLease();
    const { component } = setup({
      lease: mockLeaseWithTenants,
      tenants: mockLeaseWithTenants?.stakeholders?.map(t => new FormTenant(t)) ?? [],
      isPayableLease: false,
    });
    const { getByText } = component;
    const summary = getByText('French Mouse Property Management');
    expect(summary).toBeVisible();
  });

  it('renders primary contact successfully', () => {
    const mockLeaseWithTenants = getMockApiLease();
    const { component } = setup({
      lease: mockLeaseWithTenants,
      tenants: mockLeaseWithTenants?.stakeholders?.map(t => new FormTenant(t)) ?? [],
      isPayableLease: false,
    });
    const { getByText } = component;
    const primaryContact = getByText('Bob Billy Smith');
    expect(primaryContact).toBeVisible();
  });
});
