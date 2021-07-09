import { render } from '@testing-library/react';

import ErrorModal from './ErrorModal';

describe('Error modal tests...', () => {
  it('renders correctly', () => {
    const { container } = render(<ErrorModal error={{ message: 'test' }} />);
    expect(container).toMatchSnapshot();
  });
});
