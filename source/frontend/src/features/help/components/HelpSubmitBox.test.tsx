import { act, render, screen, waitFor } from '@testing-library/react';

import { TicketTypes } from '../constants/HelpText';
import HelpSubmitBox from './HelpSubmitBox';

const renderHelpBox = (ticketType: TicketTypes) =>
  render(
    <HelpSubmitBox
      activeTicketType={ticketType}
      user="Test User"
      email="test@test.com"
      setActiveTicketType={jest.fn(x => x)}
      setMailto={jest.fn()}
      page="Landing Page"
    />,
  );

describe('Help Box tests', () => {
  it('renders properly', async () => {
    await act(async () => {
      const { asFragment } = renderHelpBox(TicketTypes.QUESTION);
      const fragment = await waitFor(() => asFragment());

      expect(fragment).toMatchSnapshot();
    });
  });

  it('contains question field and option when question selected', async () => {
    await act(async () => {
      renderHelpBox(TicketTypes.QUESTION);
    });

    const question = await waitFor(() => screen.getAllByText('Question'));
    expect(question).toHaveLength(2);
  });

  it('displays appropriate fields when switching to bug type', async () => {
    await act(async () => {
      renderHelpBox(TicketTypes.BUG);
    });
    const reproduce = await waitFor(() => screen.getByText('Steps to Reproduce'));
    const expected = await waitFor(() => screen.getByText('Expected Result'));
    const result = await waitFor(() => screen.getByText('Actual Result'));
    expect(reproduce).toBeInTheDocument();
    expect(expected).toBeInTheDocument();
    expect(result).toBeInTheDocument();
  });

  it('displays appropriate fields when switching to feature request ticket type', async () => {
    await act(async () => {
      renderHelpBox(TicketTypes.FEATURE_REQUEST);
    });
    const desc = await waitFor(() => screen.getByText('Description'));
    expect(desc).toBeInTheDocument();
  });
});
