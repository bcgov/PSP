import { render } from '@testing-library/react';
import { useFormikContext } from 'formik';

import { PropertyFilterOptions } from './PropertyFilterOptions';

jest.mock('formik');

(useFormikContext as jest.Mock).mockReturnValue({
  values: {
    includeAllProperties: false,
  },
  setFieldValue: jest.fn(),
});
it('renders correctly...', () => {
  const { asFragment } = render(<PropertyFilterOptions />);
  expect(asFragment()).toMatchSnapshot();
});

it('disabled prop works as intended...', () => {
  const { container } = render(<PropertyFilterOptions disabled />);
  const searchBy = container.querySelector('select[name="searchBy"]');
  expect(searchBy).toHaveAttribute('disabled');
});
