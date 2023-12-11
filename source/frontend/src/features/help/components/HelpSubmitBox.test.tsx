import { act, render, screen, waitFor } from '@testing-library/react';

import HelpSubmitBox from './HelpSubmitBox';

const renderHelpBox = () =>
  render(
    <HelpSubmitBox
      user="Test User"
      email="test@test.com"
      setMailto={jest.fn()}
      page="Landing Page"
    />,
  );

describe('Help Box tests', () => {
  it('renders properly', async () => {
    await act(async () => {
      const { asFragment } = renderHelpBox();
      const fragment = await waitFor(() => asFragment());

      expect(fragment).toMatchSnapshot();
    });
  });

  it('displays appropriate fields', async () => {
    await act(async () => {
      renderHelpBox();
    });
    const desc = await waitFor(() => screen.getByText('Description:'));
    expect(desc).toBeInTheDocument();
  });
});
