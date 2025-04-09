import { Claims } from '@/constants';
import { getEmptyLease } from '@/models/defaultInitializers';
import { render, RenderOptions } from '@/utils/test-utils';

import { toTypeCodeNullable } from '@/utils/formUtils';

import { ILeaseDetailViewProps } from './LeaseDetailView';
import { LeaseTeamView } from './LeaseTeamView';
import { getMockPerson } from '@/mocks/contacts.mock';
import { getMockOrganization } from '@/mocks/organization.mock';

describe('LeaseTeamView component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<ILeaseDetailViewProps> } = {},
  ) => {
    const utils = render(<LeaseTeamView lease={renderOptions?.props?.lease ?? getEmptyLease()} />, {
      ...renderOptions,
      claims: renderOptions?.claims ?? [Claims.LEASE_VIEW],
    });

    return { ...utils };
  };

  it('renders empty lease team', () => {
    const { asFragment } = setup({
      props: {
        lease: {
          ...getEmptyLease(),
          leaseTeam: [],
        },
      },
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders a team person', () => {
    const { getByText } = setup({
      props: {
        lease: {
          ...getEmptyLease(),
          leaseTeam: [
            {
              person: getMockPerson({ firstName: 'first', surname: 'last', id: 1 }),
              id: 1,
              leaseId: 1,
              personId: 1,
              organization: null,
              organizationId: null,
              teamProfileType: { ...toTypeCodeNullable('profile'), description: 'profile' },
              teamProfileTypeCode: 'profile',
              rowVersion: 1,
              primaryContact: null,
              primaryContactId: null,
            },
          ],
        },
      },
    });
    expect(getByText('profile', { exact: false })).toBeVisible();
    expect(getByText('first last')).toBeVisible();
  });

  it('renders a team organization', () => {
    const { getByText } = setup({
      props: {
        lease: {
          ...getEmptyLease(),
          leaseTeam: [
            {
              person: null,
              id: 1,
              leaseId: 1,
              personId: 1,
              organization: getMockOrganization({ id: 1, name: 'test org' }),
              organizationId: null,
              teamProfileType: { ...toTypeCodeNullable('profile'), description: 'profile' },
              teamProfileTypeCode: 'profile',
              rowVersion: 1,
              primaryContact: null,
              primaryContactId: null,
            },
          ],
        },
      },
    });
    expect(getByText('profile', { exact: false })).toBeVisible();
    expect(getByText('test org')).toBeVisible();
  });

  it('renders a team organization with a primary contact', () => {
    const { getByText } = setup({
      props: {
        lease: {
          ...getEmptyLease(),
          leaseTeam: [
            {
              person: null,
              id: 1,
              leaseId: 1,
              personId: null,
              organization: getMockOrganization({ id: 1, name: 'test org' }),
              organizationId: 1,
              teamProfileType: { ...toTypeCodeNullable('profile'), description: 'profile' },
              teamProfileTypeCode: 'profile',
              rowVersion: 1,
              primaryContact: getMockPerson({ id: 1, firstName: 'primary', surname: 'contact' }),
              primaryContactId: 1,
            },
          ],
        },
      },
    });
    expect(getByText('profile', { exact: false })).toBeVisible();
    expect(getByText('test org')).toBeVisible();
    expect(getByText('primary contact')).toBeVisible();
  });
});
