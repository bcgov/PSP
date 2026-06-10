import { FC, MouseEvent, useMemo } from 'react';
import styled from 'styled-components';

import variables from '@/assets/scss/_variables.module.scss';
import MoreOptionsMenu, { MenuOption } from '@/components/common/MoreOptionsMenu';
import { ApiGen_Concepts_NotificationOutput } from '@/models/api/generated/ApiGen_Concepts_NotificationOutput';
import { prettyFormatDate } from '@/utils';

import {
  getNotificationFileLabel,
  getNotificationTypeLabel,
  getParentNotification,
  isUnread,
} from './notificationLinks';

export interface INotificationRowProps {
  notification: ApiGen_Concepts_NotificationOutput;
  onSelect: (notification: ApiGen_Concepts_NotificationOutput) => void;
  onToggleRead: (notification: ApiGen_Concepts_NotificationOutput) => void;
}

export const NotificationRow: FC<INotificationRowProps> = ({
  notification,
  onSelect,
  onToggleRead,
}) => {
  const handleDotClick = (event: MouseEvent<HTMLButtonElement>) => {
    event.stopPropagation();
    onToggleRead(notification);
  };

  const fileLabel = getNotificationFileLabel(notification) ?? '—';
  const typeLabel = getNotificationTypeLabel(notification);
  const triggerDate = getParentNotification(notification)?.notificationTriggerDate ?? null;
  const unread = isUnread(notification);

  const menuOptions: MenuOption[] = useMemo(() => {
    const options: MenuOption[] = [];
    if (unread) {
      options.push({
        label: 'Mark as read',
        onClick: () => onToggleRead(notification),
      });
    } else {
      options.push({
        label: 'Mark as unread',
        onClick: () => onToggleRead(notification),
      });
    }
    return options;
  }, [notification, onToggleRead, unread]);

  return (
    <Row
      role="button"
      tabIndex={0}
      onClick={() => onSelect(notification)}
      onKeyDown={event => {
        if (event.key === 'Enter' || event.key === ' ') {
          event.preventDefault();
          onSelect(notification);
        }
      }}
      aria-label={`${typeLabel} notification for ${fileLabel}`}
    >
      <DotCell>{unread && <UnreadDot aria-label="Unread notification" />}</DotCell>
      <FileCell>{fileLabel}</FileCell>
      <TypeCell>{typeLabel}</TypeCell>
      <DateCell>{triggerDate !== null ? prettyFormatDate(triggerDate) : ''}</DateCell>
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
  border-bottom: 1px solid ${variables.borderOutlineColor ?? '#e0e0e0'};

  &:hover,
  &:focus {
    background-color: ${variables.pimsBlue10};
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
  background-color: ${variables.pimsRed100};
  border: none;
  padding: 0;
  cursor: pointer;
`;

const ReadDot = styled.button`
  width: 1rem;
  height: 1rem;
  border-radius: 50%;
  background-color: transparent;
  border: 1px solid ${variables.pimsGrey80 ?? '#999'};
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
  color: ${variables.pimsGrey80 ?? '#555'};
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
`;

const DateCell = styled.div`
  text-align: right;
  color: ${variables.pimsGrey80 ?? '#555'};
  font-variant-numeric: tabular-nums;
`;

const ActionsCell = styled.div`
  display: flex;
  justify-content: flex-end;
  gap: 0.4rem;
`;

export default NotificationRow;
