import { useKeycloak } from '@react-keycloak/web';
import { render } from '@testing-library/react';
import { IOrganizationFilter } from 'interfaces';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import { OrganizationFilterBar } from './OrganizationFilterBar';

const mockValues: IOrganizationFilter = {};
const mockAdd = jest.fn();
const mockChange = jest.fn();

jest.mock('@react-keycloak/web');

(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    userInfo: {
      organizations: [1],
      roles: [],
    },
    subject: 'test',
  },
});

it('renders correctly...', () => {
  const { asFragment } = render(
    <TestCommonWrapper store={{ lookupCode: { lookupCodes: [] } }}>
      <OrganizationFilterBar value={mockValues} handleAdd={mockAdd} onChange={mockChange} />
    </TestCommonWrapper>,
  );
  expect(asFragment()).toMatchSnapshot();
});
