import { useKeycloak } from '@react-keycloak/web';
import { render } from '@testing-library/react';
import { useFormikContext } from 'formik';
import { AGENCIES } from 'mocks/filterDataMock';

import { PropertyFilterAgencyOptions } from './PropertyFilterAgencyOptions';

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
      agencies: [1],
      roles: [],
    },
    subject: 'test',
  },
});

it('renders correctly...', () => {
  const { asFragment } = render(<PropertyFilterAgencyOptions agencies={AGENCIES as any} />);
  expect(asFragment()).toMatchSnapshot();
});

it('disables when appropriate prop is passed', () => {
  const { container } = render(<PropertyFilterAgencyOptions disabled agencies={AGENCIES as any} />);
  const input = container.querySelector('input[name="agencies"]');
  expect(input).toHaveAttribute('disabled');
});
