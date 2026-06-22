import { FC, useMemo } from 'react';
import { FaCircle, FaExternalLinkAlt, FaMinus, FaRegCheckCircle } from 'react-icons/fa';
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
        icon: <FaExternalLinkAlt aria-hidden="true" size={12} className="mr-1" />,
        onClick: () => onSelect(notification),
      },
      {
        label: unread ? 'Mark as read' : 'Mark as unread',
        icon: unread ? (
          <FaRegCheckCircle aria-hidden="true" size={12} className="mr-1" />
        ) : (
          <FaCircle aria-hidden="true" size={12} className="mr-1" />
        ),
        onClick: () => onToggleRead(notification),
      },
      {
        label: 'Dismiss',
        icon: <FaMinus aria-hidden="true" size={12} className="mr-1" />,
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
      <ActionsCell
        onClick={event => event.stopPropagation()}
        onKeyDown={event => event.stopPropagation()}
      >
        <MoreOptionsMenu options={menuOptions} />
      </ActionsCell>
    </Row>
  );
};

const Row = styled.div`
  display: grid;
  grid-template-columns: 2.4rem 1fr 1fr 12rem 3rem;
  align-items: center;
  gap: 0.4rem;
  padding: 0.6rem 0.8rem;
  min-height: 4rem;
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
  margin-right: 1rem;
`;

const ActionsCell = styled.div`
  display: flex;
  justify-content: flex-center;
  align-items: center;
  padding: 0.5rem;
  width: 3rem;
  height: 3rem;

  &:hover {
    background-color: ${props => props.theme.css.pimsBlue10 + '95'};
  }
`;

export default NotificationRow;
