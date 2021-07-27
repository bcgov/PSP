import { render } from '@testing-library/react';

import GenericModal from './GenericModal';

it('renders correctly', () => {
  const { asFragment } = render(<GenericModal />);
  expect(asFragment()).toMatchSnapshot();
});
