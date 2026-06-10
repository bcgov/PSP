import { FC, useCallback, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { useNotificationInboxRepository } from '@/hooks/repositories/useNotificationInboxRepository';
import { ApiGen_Concepts_NotificationOutput } from '@/models/api/generated/ApiGen_Concepts_NotificationOutput';
import { exists } from '@/utils';

import { isUnread } from './notificationLinks';

export interface INotificationInboxContainerProps {
  show?: boolean;
}

const PAGE_SIZE = 10;

export const NotificationInboxContainer: FC<INotificationInboxContainerProps> = ({ show }) => {
  const history = useHistory();
  const [items, setItems] = useState<ApiGen_Concepts_NotificationOutput[]>([]);
  const [page, setPage] = useState(0);
  const [total, setTotal] = useState(0);

  const {
    getUserInbox: { execute: getUserInbox, loading },
    getUnreadCount: { execute: getUnreadCount, response: unreadCount },
    updateReadStatus: { execute: updateReadStatus },
  } = useNotificationInboxRepository();

  // Loads the next page from the server and appends it to the existing list.
  // Passing `reset` clears the list first: used when the popover opens to give a fresh view.
  const fetchPage = useCallback(
    async (nextPage: number, reset: boolean) => {
      const result = await getUserInbox(nextPage, PAGE_SIZE);
      if (!exists(result)) {
        return;
      }
      const newItems = result.items ?? [];
      setItems(prev => (reset ? newItems : [...prev, ...newItems]));
      setTotal(result.total ?? 0);
      setPage(nextPage);
    },
    [getUserInbox],
  );

  useEffect(() => {
    if (show) {
      fetchPage(1, true);
      getUnreadCount();
    }
  }, [show, fetchPage, getUnreadCount]);

  const handleLoadMore = useCallback(() => {
    fetchPage(page + 1, false);
  }, [fetchPage, page]);

  const handleSelect = useCallback(
    async (notification: ApiGen_Concepts_NotificationOutput) => {
      setShow(false);
      if (isUnread(notification)) {
        await updateReadStatus(notification.id, true);
      }
      const target = getNotificationDeepLink(notification);
      if (target !== null) {
        history.push(target);
      }
    },
    [history, updateReadStatus],
  );

  const handleToggleRead = useCallback(
    async (notification: ApiGen_Concepts_NotificationOutput) => {
      const nextIsRead = isUnread(notification);
      const updated = await updateReadStatus(notification.id, nextIsRead);
      if (exists(updated)) {
        setItems(prev =>
          prev.map(item =>
            item.id === notification.id
              ? { ...item, notificationReadDt: updated.notificationReadDt }
              : item,
          ),
        );
        await getUnreadCount();
      }
    },
    [getUnreadCount, updateReadStatus],
  );

  const hasMore = items.length < total;

  return <div>Notification Inbox</div>;
};
