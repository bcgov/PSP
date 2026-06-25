import { FC, useCallback, useEffect } from 'react';

import { useNotificationInboxRepository } from '@/hooks/repositories/useNotificationInboxRepository';

import NotificationInboxContainer from './NotificationInboxContainer';
import NotificationInboxView from './NotificationInboxView';
import NotificationsBell from './NotificationsBell';

/**
 * Container that owns the unread-count badge for the notifications bell. Fetches the count on
 * mount so the badge is accurate as soon as the app loads, and exposes a refresh handle that
 * the inbox invokes after mutations (mark read/unread, delete). The popover body is supplied
 * as a render prop so the inbox can close the popover when navigating to a file.
 */
export const NotificationBellContainer: FC = () => {
  const {
    getUnreadCount: { execute: getUnreadCount, response },
  } = useNotificationInboxRepository();

  const refreshUnreadCount = useCallback(async () => {
    await getUnreadCount();
  }, [getUnreadCount]);

  useEffect(() => {
    refreshUnreadCount();
  }, [refreshUnreadCount]);

  const unreadCount = response?.unreadCount ?? 0;

  return (
    <NotificationsBell
      unreadCount={unreadCount}
      popoverContent={closePopover => (
        <NotificationInboxContainer
          View={NotificationInboxView}
          onRequestClose={closePopover}
          onInboxChanged={refreshUnreadCount}
        />
      )}
    />
  );
};

export default NotificationBellContainer;
