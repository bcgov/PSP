import { useKeycloak } from '@react-keycloak/web';
import { render } from '@testing-library/react';
import { useFormikContext } from 'formik';
import { mockOrganization } from 'mocks/filterDataMock';

import { PropertyFilterOrganizationOptions } from './PropertyFilterOrganizationOptions';

jest.mock('@react-keycloak/web');
jest.mock('formik');

(useFormikContext as jest.Mock).mockReturnValue({
  values: {
    includeAllProperties: false,
  },
  setFieldValue: jest.fn(),
});

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
    <PropertyFilterOrganizationOptions organizations={[mockOrganization] as any} />,
  );
  expect(asFragment()).toMatchSnapshot();
});

it('disables when appropriate prop is passed', () => {
  const { container } = render(
    <PropertyFilterOrganizationOptions disabled organizations={[mockOrganization] as any} />,
  );
  const input = container.querySelector('input[name="organizations"]');
  expect(input).toHaveAttribute('disabled');
});
