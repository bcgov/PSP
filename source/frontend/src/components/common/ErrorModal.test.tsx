import { render } from '@/utils/test-utils';

import ErrorModal from './ErrorModal';

describe('Error modal tests...', () => {
  it('renders correctly', () => {
    const { container } = render(<ErrorModal error={{ message: 'test' }} />);
    expect(container).toMatchSnapshot();
  });
});
