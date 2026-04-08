import { useCallback, useMemo } from 'react';

import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import {
  useAxiosErrorHandler,
  useAxiosErrorHandlerWithAuthorization,
  useAxiosSuccessHandler,
} from '@/utils/axiosUtils';

import { useApiNotifications } from '../pims-api/useApiNotifications';

/**
 * Hook that interacts with the Notifications API.
 */
export const useNotificationRepository = () => {
  const {
    getUserNotificationsApi,
    getNotificationByIdApi,
    postNotificationApi,
    putNotificationApi,
    deleteNotificationApi,
  } = useApiNotifications();

  const addNotification = useApiRequestWrapper<typeof postNotificationApi>({
    requestFunction: useCallback<typeof postNotificationApi>(
      async notification => await postNotificationApi(notification),
      [postNotificationApi],
    ),
    requestName: 'AddNotification',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

  const updateNotification = useApiRequestWrapper<typeof putNotificationApi>({
    requestFunction: useCallback<typeof putNotificationApi>(
      async notification => await putNotificationApi(notification),
      [putNotificationApi],
    ),
    requestName: 'UpdateNotification',
    onSuccess: useAxiosSuccessHandler(),
    throwError: true,
  });

  const deleteNotification = useApiRequestWrapper<typeof deleteNotificationApi>({
    requestFunction: useCallback<typeof deleteNotificationApi>(
      async notificationId => await deleteNotificationApi(notificationId),
      [deleteNotificationApi],
    ),
    requestName: 'DeleteNotification',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to delete notification'),
  });

  const getUserNotifications = useApiRequestWrapper<typeof getUserNotificationsApi>({
    requestFunction: useCallback<typeof getUserNotificationsApi>(
      async () => await getUserNotificationsApi(),
      [getUserNotificationsApi],
    ),
    requestName: 'GetUserNotifications',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandler('Failed to load user notifications'),
  });

  const getNotificationById = useApiRequestWrapper<typeof getNotificationByIdApi>({
    requestFunction: useCallback<typeof getNotificationByIdApi>(
      async notificationId => await getNotificationByIdApi(notificationId),
      [getNotificationByIdApi],
    ),
    requestName: 'GetNotificationById',
    onSuccess: useAxiosSuccessHandler(),
    onError: useAxiosErrorHandlerWithAuthorization('Failed to load notification'),
  });

  return useMemo(
    () => ({
      addNotification: addNotification,
      updateNotification: updateNotification,
      deleteNotification: deleteNotification,
      getUserNotifications: getUserNotifications,
      getNotificationById: getNotificationById,
    }),
    [
      addNotification,
      updateNotification,
      deleteNotification,
      getUserNotifications,
      getNotificationById,
    ],
  );
};
