import { FC, useMemo } from 'react';
import styled from 'styled-components';

import MoreOptionsMenu, { MenuOption } from '@/components/common/MoreOptionsMenu';
import { ApiGen_Concepts_NotificationInboxItem } from '@/models/api/generated/ApiGen_Concepts_NotificationInboxItem';
import { prettyFormatDate } from '@/utils';

export interface INotificationRowProps {
  notification: ApiGen_Concepts_NotificationInboxItem;
  onSelect: (notification: ApiGen_Concepts_NotificationInboxItem) => void;
  onToggleRead: (notification: ApiGen_Concepts_NotificationInboxItem) => void;
  onDelete: (notification: ApiGen_Concepts_NotificationInboxItem) => void;
}

export const NotificationRow: FC<INotificationRowProps> = ({
  notification,
  onSelect,
  onToggleRead,
  onDelete,
}) => {
  const fileLabel = notification?.subject ?? '';
  const typeLabel = notification?.notificationType?.description ?? '';
  const trackedDate = notification?.trackedDate ?? null;
  const unread = !notification.isRead;

  const menuOptions: MenuOption[] = useMemo(
    () => [
      {
        label: 'Open file',
        onClick: () => onSelect(notification),
      },
      {
        label: unread ? 'Mark as read' : 'Mark as unread',
        onClick: () => onToggleRead(notification),
      },
      {
        label: 'Delete',
        separator: true,
        onClick: () => onDelete(notification),
      },
    ],
    [notification, onDelete, onSelect, onToggleRead, unread],
  );

  return (
    <Row
      role="button"
      tabIndex={0}
      onClick={event => {
        event.preventDefault();
        event.stopPropagation();
        onSelect(notification);
      }}
      onKeyDown={event => {
        if (event.key === 'Enter' || event.key === ' ') {
          event.preventDefault();
          event.stopPropagation();
          onSelect(notification);
        }
      }}
      aria-label={`${typeLabel} notification for ${fileLabel}`}
    >
      <DotCell>{unread && <UnreadDot aria-label="Unread notification" />}</DotCell>
      <FileCell>{fileLabel}</FileCell>
      <TypeCell>{typeLabel}</TypeCell>
      <DateCell>{trackedDate !== null ? prettyFormatDate(trackedDate) : ''}</DateCell>
      <ActionsCell>
        <MoreOptionsMenu options={menuOptions} />
      </ActionsCell>
    </Row>
  );
};

const Row = styled.div`
  display: grid;
  grid-template-columns: 2.4rem 1fr 1fr 10rem 3rem;
  align-items: center;
  gap: 0.8rem;
  padding: 0.8rem 1.2rem;
  cursor: pointer;

  &:hover,
  &:focus {
    background-color: ${props => props.theme.css.pimsBlue10 + '38'};
    outline: none;
  }
`;

const DotCell = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
`;

const UnreadDot = styled.div`
  width: 1rem;
  height: 1rem;
  border-radius: 50%;
  background-color: ${props => props.theme.css.pimsRed100};
  border: none;
  padding: 0;
  cursor: pointer;
`;

const FileCell = styled.div`
  font-weight: 600;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
`;

const TypeCell = styled.div`
  color: ${props => props.theme.css.pimsGrey80 ?? '#555'};
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
`;

const DateCell = styled.div`
  text-align: right;
  color: ${props => props.theme.css.pimsGrey80 ?? '#555'};
  font-variant-numeric: tabular-nums;
`;

const ActionsCell = styled.div`
  display: flex;
  justify-content: flex-end;
  gap: 0.4rem;
`;

export default NotificationRow;
