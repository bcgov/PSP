import { FC, useCallback, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { useApiTakes } from '@/hooks/pims-api/useApiTakes';
import { useNotificationInboxRepository } from '@/hooks/repositories/useNotificationInboxRepository';
import { ApiGen_Concepts_NotificationInboxItem } from '@/models/api/generated/ApiGen_Concepts_NotificationInboxItem';
import { exists, isValidId } from '@/utils';
import {
  getNotificationDeepLink,
  getParentNotification,
  isTakeNotification,
} from '@/utils/notificationUtils';

import { INotificationInboxViewProps } from './NotificationInboxView';

export interface INotificationInboxContainerProps {
  /** The presentational component responsible for rendering the inbox list. */
  View: FC<INotificationInboxViewProps>;

  /** Called when the inbox wants the surrounding popover to close (e.g. after navigation). */
  onRequestClose?: () => void;

  /**
   * Called after any change that affects the unread count (mark read/unread, delete) so the
   * owner (the bell container) can refresh the badge.
   */
  onInboxChanged?: () => void;
}

const PAGE_SIZE = 10;

/**
 * Container for the notifications inbox. Owns all data fetching and mutations for the popover
 * body, and delegates rendering to the supplied `View`. Because the popover only mounts this
 * container while open, the first page is fetched on mount — every open shows a fresh list.
 */
export const NotificationInboxContainer: FC<INotificationInboxContainerProps> = ({
  View,
  onRequestClose,
  onInboxChanged,
}) => {
  const history = useHistory();
  const { getTakesByAcqFileId } = useApiTakes();
  const [items, setItems] = useState<ApiGen_Concepts_NotificationInboxItem[]>([]);
  const [page, setPage] = useState(0);
  const [total, setTotal] = useState(0);

  const {
    getUserInbox: { execute: getUserInbox, loading },
    updateReadStatus: { execute: updateReadStatus },
    deleteNotificationOutput: { execute: deleteNotificationOutput },
  } = useNotificationInboxRepository();

  // Loads the next page from the server and appends it to the existing list.
  // Passing `reset` clears the list first: used on mount to give a fresh view.
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
    fetchPage(1, true);
  }, [fetchPage]);

  const handleLoadMore = useCallback(() => {
    fetchPage(page + 1, false);
  }, [fetchPage, page]);

  const getPropertyAcquisitionFileId = useCallback(
    async (notification: ApiGen_Concepts_NotificationInboxItem): Promise<number | undefined> => {
      const parent = getParentNotification(notification);
      if (!exists(parent) || !isValidId(parent.takeId) || !isValidId(parent.acquisitionFileId)) {
        return undefined;
      }

      const { data: takes } = await getTakesByAcqFileId(parent.acquisitionFileId);
      return takes.find(take => take.id === parent.takeId)?.propertyAcquisitionFileId;
    },
    [getTakesByAcqFileId],
  );

  const handleSelect = useCallback(
    async (notification: ApiGen_Concepts_NotificationInboxItem) => {
      try {
        onRequestClose?.();
        await updateReadStatus(notification.id, true);
        const filePropertyId = isTakeNotification(notification)
          ? await getPropertyAcquisitionFileId(notification)
          : undefined;
        const target = getNotificationDeepLink(notification, filePropertyId);
        if (target !== null) {
          history.push(target);
        }
      } finally {
        onInboxChanged?.();
      }
    },
    [getPropertyAcquisitionFileId, history, onInboxChanged, onRequestClose, updateReadStatus],
  );

  const handleToggleRead = useCallback(
    async (notification: ApiGen_Concepts_NotificationInboxItem) => {
      try {
        const nextReadStatus = !notification.isRead;
        await updateReadStatus(notification.id, nextReadStatus);
        // The endpoint returns 204, so reflect the new state optimistically.
        setItems(prev =>
          prev.map(item => {
            if (item.id === notification.id) {
              item.isRead = nextReadStatus;
            }
            return item;
          }),
        );
      } finally {
        onInboxChanged?.();
      }
    },
    [onInboxChanged, updateReadStatus],
  );

  const handleDelete = useCallback(
    async (notification: ApiGen_Concepts_NotificationInboxItem) => {
      try {
        await deleteNotificationOutput(notification.id);
        setItems(prev => prev.filter(item => item.id !== notification.id));
        setTotal(prev => Math.max(prev - 1, 0));
      } finally {
        onInboxChanged?.();
      }
    },
    [deleteNotificationOutput, onInboxChanged],
  );

  const hasMore = items.length < total;

  return (
    <View
      items={items}
      hasMore={hasMore}
      isLoading={loading}
      onLoadMore={handleLoadMore}
      onSelect={handleSelect}
      onToggleRead={handleToggleRead}
      onDelete={handleDelete}
    />
  );
};

export default NotificationInboxContainer;
