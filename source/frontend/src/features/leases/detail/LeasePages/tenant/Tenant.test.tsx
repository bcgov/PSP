import { apiLeaseToFormLease } from 'features/leases/leaseUtils';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { mockApiPerson, mockOrganization } from 'mocks/filterDataMock';
import { getMockLease } from 'mocks/mockLease';
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
            tenantTypeCode: { id: 'TEN' },
            leaseId: 1,
          }),
          new FormTenant({
            lessorType: { id: 'PER' },
            tenantTypeCode: { id: 'REP' },
            person: { ...mockApiPerson },
            leaseId: 1,
          }),
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

  it('renders representative section', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, persons: [] },
    });
    const { getAllByText } = component;
    const repSection = getAllByText('Representative');

    expect(repSection).toHaveLength(1);
  });

  it('renders property manager section', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, persons: [] },
    });
    const { getAllByText } = component;
    const propSection = getAllByText('Property Manager');

    expect(propSection).toHaveLength(1);
  });
  it('renders unknown section', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, persons: [] },
    });
    const { getAllByText } = component;
    const unknownSection = getAllByText('Unknown');

    expect(unknownSection).toHaveLength(1);
  });

  it('renders summary successfully', () => {
    const mockLeaseWithTenants = getMockLease();
    const { component } = setup({
      lease: apiLeaseToFormLease(mockLeaseWithTenants),
    });
    const { getByText } = component;
    const summary = getByText('French Mouse Property Management');
    expect(summary).toBeVisible();
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
});
