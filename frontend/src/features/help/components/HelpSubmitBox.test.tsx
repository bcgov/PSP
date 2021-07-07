import { render } from '@testing-library/react';

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
  it('renders properly', () => {
    const { container } = renderHelpBox(TicketTypes.QUESTION);
    expect(container).toMatchSnapshot();
  });

  it('contains question field and option when question selected', () => {
    const { getAllByText } = renderHelpBox(TicketTypes.QUESTION);
    expect(getAllByText('Question')).toHaveLength(2);
  });

  it('displays appropriate fields when switching to bug type', () => {
    const { getByText } = renderHelpBox(TicketTypes.BUG);
    expect(getByText('Steps to Reproduce')).toBeInTheDocument();
    expect(getByText('Expected Result')).toBeInTheDocument();
    expect(getByText('Actual Result')).toBeInTheDocument();
  });

  it('displays appropriate fields when switching to feature request ticket type', () => {
    const { getByText } = renderHelpBox(TicketTypes.FEATURE_REQUEST);
    expect(getByText('Description')).toBeInTheDocument();
  });
});
