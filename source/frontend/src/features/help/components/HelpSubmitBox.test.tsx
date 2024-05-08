import { act, render, screen } from '@/utils/test-utils';

import HelpSubmitBox from './HelpSubmitBox';

describe('HelpSubmitBox component', () => {
  const setup = () =>
    render(
      <HelpSubmitBox
        user="Test User"
        email="test@test.com"
        setMailto={vi.fn()}
        page="Landing Page"
      />,
    );

  it('renders properly', async () => {
    const { asFragment } = setup();
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays appropriate fields', async () => {
    setup();
    await act(async () => {});
    const name = screen.getByText('Name:');
    const email = screen.getByText('Email:');
    const desc = screen.getByText('Description:');
    expect(name).toBeInTheDocument();
    expect(email).toBeInTheDocument();
    expect(desc).toBeInTheDocument();
  });
});
