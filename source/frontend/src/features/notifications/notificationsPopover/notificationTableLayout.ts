import { css } from 'styled-components';

/**
 * Column template shared by the notification table header and every row.
 * Order: unread indicator | file number | notification type | key date | actions.
 * Keep header and rows on the same template so labels line up with their data.
 */
export const NOTIFICATION_GRID_COLUMNS = '2.4rem 1fr 1fr 12rem 6rem';

export const notificationGridLayout = css`
  display: grid;
  grid-template-columns: ${NOTIFICATION_GRID_COLUMNS};
  align-items: center;
  gap: 0.4rem;
  padding: 0.6rem 0.8rem;
`;
