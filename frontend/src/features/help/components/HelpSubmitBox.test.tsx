import { render, waitFor } from '@testing-library/react';

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

describe('Help Box tests..', () => {
  it('renders properly', async () => {
    const { asFragment } = renderHelpBox(TicketTypes.QUESTION);
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('contains question field and option when question selected', async () => {
    const { getAllByText } = renderHelpBox(TicketTypes.QUESTION);
    const question = await waitFor(() => getAllByText('Question'));
    expect(question).toHaveLength(2);
  });

  it('displays appropriate fields when switching to bug type', async () => {
    const { getByText } = renderHelpBox(TicketTypes.BUG);
    const reproduce = await waitFor(() => getByText('Steps to Reproduce'));
    const expected = await waitFor(() => getByText('Expected Result'));
    const result = await waitFor(() => getByText('Actual Result'));
    expect(reproduce).toBeInTheDocument();
    expect(expected).toBeInTheDocument();
    expect(result).toBeInTheDocument();
  });

  it('displays appropriate fields when switching to feature request ticket type', async () => {
    const { getByText } = renderHelpBox(TicketTypes.FEATURE_REQUEST);
    const desc = await waitFor(() => getByText('Description'));
    expect(desc).toBeInTheDocument();
  });
});
