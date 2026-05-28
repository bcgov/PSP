import { render } from '@/utils/test-utils';

import ErrorModal from './ErrorModal';

describe('Error modal tests...', () => {
  it('renders correctly', () => {
    const { container } = render(
      <ErrorModal error={new Error('test')} resetErrorBoundary={null} />,
    );
    expect(container).toMatchSnapshot();
  });
});
