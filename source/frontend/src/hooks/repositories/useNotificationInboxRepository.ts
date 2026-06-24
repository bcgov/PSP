import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import {
  useAxiosErrorHandler,
  useAxiosErrorHandlerWithAuthorization,
  useAxiosSuccessHandler,
} from '@/utils/axiosUtils';

import { useApiNotificationInbox } from '../pims-api/useApiNotificationInbox';

/**
 * Hook that interacts with the Notifications User-Inbox API.
 */
export const useNotificationInboxRepository = () => {
  const {
    getUserInboxApi,
    getNotificationOutputApi,
    getUnreadCountApi,
    updateReadStatusApi,
    markAllAsReadApi,
    deleteNotificationOutputApi,
  } = useApiNotificationInbox();

  const getUserInbox = useApiRequestWrapper<typeof getUserInboxApi>({
    requestFunction: useCallback<typeof getUserInboxApi>(
      async (page, quantity) => await getUserInboxApi(page, quantity),
      [getUserInboxApi],
    ),
    requestName: 'GetUserInbox',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load notifications'),
  });

  const getNotificationOutputById = useApiRequestWrapper<typeof getNotificationOutputApi>({
    requestFunction: useCallback<typeof getNotificationOutputApi>(
      async outputId => await getNotificationOutputApi(outputId),
      [getNotificationOutputApi],
    ),
    requestName: 'GetNotificationOutputById',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandlerWithAuthorization('Failed to load notification'),
  });

  const getUnreadCount = useApiRequestWrapper<typeof getUnreadCountApi>({
    requestFunction: useCallback<typeof getUnreadCountApi>(
      async () => await getUnreadCountApi(),
      [getUnreadCountApi],
    ),
    requestName: 'GetUnreadNotificationCount',
    onSuccess: useAxiosSuccessHandler(),
  });

  const updateReadStatus = useApiRequestWrapper<typeof updateReadStatusApi>({
    requestFunction: useCallback<typeof updateReadStatusApi>(
      async (outputId, isRead) => await updateReadStatusApi(outputId, isRead),
      [updateReadStatusApi],
    ),
    requestName: 'UpdateNotificationOutputReadStatus',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to update notification read status'),
  });

  const markAllAsRead = useApiRequestWrapper<typeof markAllAsReadApi>({
    requestFunction: useCallback<typeof markAllAsReadApi>(
      async () => await markAllAsReadApi(),
      [markAllAsReadApi],
    ),
    requestName: 'MarkAllNotificationsAsRead',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to mark all notifications as read'),
  });

  const deleteNotificationOutput = useApiRequestWrapper<typeof deleteNotificationOutputApi>({
    requestFunction: useCallback<typeof deleteNotificationOutputApi>(
      async outputId => await deleteNotificationOutputApi(outputId),
      [deleteNotificationOutputApi],
    ),
    requestName: 'DeleteNotificationOutput',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to delete user notification'),
  });

  return useMemo(
    () => ({
      getUserInbox: getUserInbox,
      getNotificationOutputById: getNotificationOutputById,
      deleteNotificationOutput: deleteNotificationOutput,
      getUnreadCount: getUnreadCount,
      updateReadStatus: updateReadStatus,
      markAllAsRead: markAllAsRead,
    }),
    [
      getUserInbox,
      getNotificationOutputById,
      deleteNotificationOutput,
      getUnreadCount,
      updateReadStatus,
      markAllAsRead,
    ],
  );
};
