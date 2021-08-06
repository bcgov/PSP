import { useKeycloak } from '@react-keycloak/web';
import { render } from '@testing-library/react';
import { IAgencyFilter } from 'interfaces';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import { AgencyFilterBar } from './AgencyFilterBar';

const mockValues: IAgencyFilter = {};
const mockAdd = jest.fn();
const mockChange = jest.fn();

jest.mock('@react-keycloak/web');

(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    userInfo: {
      agencies: [1],
      roles: [],
    },
    subject: 'test',
  },
});

it('renders correctly...', () => {
  const { asFragment } = render(
    <TestCommonWrapper store={{ lookupCode: { lookupCodes: [] } }}>
      <AgencyFilterBar value={mockValues} handleAdd={mockAdd} onChange={mockChange} />
    </TestCommonWrapper>,
  );
  expect(asFragment()).toMatchSnapshot();
});
