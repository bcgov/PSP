import { createMemoryHistory } from 'history';

import { LeaseContextProvider } from '@/features/leases/context/LeaseContext';
import { mockApiOrganization, mockApiPerson } from '@/mocks/filterData.mock';
import { getEmptyLeaseStakeholder, getMockApiLease } from '@/mocks/lease.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { defaultApiLease } from '@/models/defaultInitializers';
import { render, RenderOptions } from '@/utils/test-utils';

import { FormStakeholder } from './models';
import { ITenantProps, ViewStakeholderForm } from './ViewStakeholderForm';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { ApiGen_CodeTypes_LeaseStakeholderTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStakeholderTypes';
import { leaseStakeholderTypesList } from './AddLeaseStakeholderForm.test';

const history = createMemoryHistory();

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('Tenant component', () => {
  const setup = (
    renderOptions: RenderOptions &
      ITenantProps & { lease?: ApiGen_Concepts_Lease; isPayableLease: boolean } = {
      stakeholders: [],
      isPayableLease: false,
      leaseStakeholderTypes: leaseStakeholderTypesList,
    },
  ) => {
    // render component under test
    const component = render(
      <LeaseContextProvider
        initialLease={renderOptions.lease ? renderOptions.lease : defaultApiLease()}
      >
        <ViewStakeholderForm
          nameSpace={renderOptions.nameSpace}
          stakeholders={renderOptions.stakeholders ?? []}
          isPayableLease={renderOptions.isPayableLease ?? false}
          leaseStakeholderTypes={renderOptions.leaseStakeholderTypes ?? leaseStakeholderTypesList}
        />
      </LeaseContextProvider>,
      {
        ...renderOptions,
        store: storeState,
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
          { ...getEmptyLeaseStakeholder(), leaseId: 1, person: mockApiPerson },
          { ...getEmptyLeaseStakeholder(), leaseId: 1, organization: mockApiOrganization },
        ],
      },
      stakeholders: [],
      isPayableLease: false,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders one Person Tenant section per person', () => {
    const { component } = setup({
      lease: {
        ...defaultApiLease(),
      },
      stakeholders: [
        { leaseId: 1, personId: mockApiPerson.id, note: 'person note', stakeholderType: 'TEN' },
        {
          leaseId: 1,
          organizationId: mockApiOrganization.id,
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
          { ...getEmptyLeaseStakeholder(), leaseId: 1, person: mockApiPerson },
          { ...getEmptyLeaseStakeholder(), leaseId: 1, organization: mockApiOrganization },
        ],
      },
      stakeholders: [
        { leaseId: 1, personId: mockApiPerson.id, note: 'person note' },
        { leaseId: 1, organizationId: mockApiOrganization.id, note: 'organization id' },
      ],
      isPayableLease: false,
    });
    const { getAllByText } = component;
    const notes = getAllByText('Notes');

    expect(notes).toHaveLength(2);
  });

  it('renders assignee section', () => {
    const { component } = setup({
      lease: {
        ...defaultApiLease(),
        stakeholders: [],
      },
      isPayableLease: false,
      stakeholders: [
        {
          leaseId: 1,
          personId: mockApiPerson.id,
          note: 'person note',
          stakeholderType: ApiGen_CodeTypes_LeaseStakeholderTypes.ASGN.toString(),
        },
      ],
    });
    const { getAllByText } = component;
    const asgnSection = getAllByText('Assignee');

    expect(asgnSection).toHaveLength(1);
  });

  it('renders representative section', () => {
    const { component } = setup({
      lease: { ...defaultApiLease(), stakeholders: [] },
      stakeholders: [
        {
          leaseId: 1,
          personId: mockApiPerson.id,
          note: 'person note',
          stakeholderType: ApiGen_CodeTypes_LeaseStakeholderTypes.REP.toString(),
        },
      ],
      isPayableLease: false,
    });
    const { getAllByText } = component;
    const repSection = getAllByText('Representative');

    expect(repSection).toHaveLength(1);
  });

  it('renders property manager section', () => {
    const { component } = setup({
      lease: { ...defaultApiLease(), stakeholders: [] },
      stakeholders: [
        {
          leaseId: 1,
          personId: mockApiPerson.id,
          note: 'person note',
          stakeholderType: ApiGen_CodeTypes_LeaseStakeholderTypes.PMGR.toString(),
        },
      ],
      isPayableLease: false,
    });
    const { getAllByText } = component;
    const propSection = getAllByText('Property manager');

    expect(propSection).toHaveLength(1);
  });

  it('renders unknown section', () => {
    const { component } = setup({
      lease: { ...defaultApiLease(), stakeholders: [] },
      stakeholders: [
        {
          leaseId: 1,
          personId: mockApiPerson.id,
          note: 'person note',
          stakeholderType: ApiGen_CodeTypes_LeaseStakeholderTypes.UNK.toString(),
        },
      ],
      isPayableLease: false,
    });
    const { getAllByText } = component;
    const unknownSection = getAllByText('Unknown');

    expect(unknownSection).toHaveLength(1);
  });

  it('renders owner section', () => {
    const { component } = setup({
      lease: { ...defaultApiLease(), stakeholders: [] },
      stakeholders: [],
      isPayableLease: true,
    });
    const { getAllByText } = component;
    const unknownSection = getAllByText('Owner');

    expect(unknownSection).toHaveLength(1);
  });

  it('renders owner representative section', () => {
    const { component } = setup({
      lease: { ...defaultApiLease(), stakeholders: [] },
      stakeholders: [],
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
      stakeholders: mockLeaseWithTenants?.stakeholders?.map(t => new FormStakeholder(t)) ?? [],
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
      stakeholders: mockLeaseWithTenants?.stakeholders?.map(t => new FormStakeholder(t)) ?? [],
      isPayableLease: false,
    });
    const { getByText } = component;
    const primaryContact = getByText('Bob Billy Smith');
    expect(primaryContact).toBeVisible();
  });
});
