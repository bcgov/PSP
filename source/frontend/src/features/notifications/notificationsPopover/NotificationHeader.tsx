import { FC } from 'react';
import styled from 'styled-components';

import { notificationGridLayout } from './notificationTableLayout';

export const NotificationHeader: FC = () => {
  return (
    <HeaderRow data-testid="notification-table-header">
      <HeaderCell aria-hidden="true" />
      <HeaderCell>File number</HeaderCell>
      <HeaderCell>Notification type</HeaderCell>
      <DateHeaderCell>Key date</DateHeaderCell>
      <ActionsHeaderCell>Actions</ActionsHeaderCell>
    </HeaderRow>
  );
};

const HeaderRow = styled.div`
  ${notificationGridLayout}
  min-height: 3.2rem;
  font-weight: 700;
  color: ${props => props.theme.css.pimsGrey80 ?? '#606060'};
  border-bottom: 1px solid ${props => (props.theme.css.pimsGrey80 ?? '#606060') + '40'};
`;

const HeaderCell = styled.div`
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
`;

const DateHeaderCell = styled(HeaderCell)`
  text-align: left;
  margin-right: 1rem;
`;

const ActionsHeaderCell = styled(HeaderCell)`
  text-align: center;
`;

export default NotificationHeader;
