import { render } from '@testing-library/react';

import { Legend } from './Legend';

describe('Map Legend View', () => {
  it('renders correctly', () => {
    const { asFragment } = render(<Legend />);
    expect(asFragment()).toMatchSnapshot();
  });
});
