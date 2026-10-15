import { render, screen } from '@/utils/test-utils';

import NotificationHeader from './NotificationHeader';

describe('NotificationHeader', () => {
  it('renders all column headings', () => {
    render(<NotificationHeader />);

    expect(screen.getByText('File number')).toBeVisible();
    expect(screen.getByText('Notification type')).toBeVisible();
    expect(screen.getByText('Key date')).toBeVisible();
    expect(screen.getByText('Actions')).toBeVisible();
  });
});
