import React from 'react';

import { ApiGen_Concepts_Notification } from '@/models/api/generated/ApiGen_Concepts_Notification';
import { Api_NotificationSearchCriteria } from '@/models/api/NotificationSearchCriteria';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the notification endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiNotifications = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getUserNotificationsApi: () => api.get<ApiGen_Concepts_Notification[]>(`/notifications/user`),

      searchNotificationsApi: (criteria: Api_NotificationSearchCriteria) =>
        api.post<ApiGen_Concepts_Notification[]>(`/notifications/search`, criteria),

      getNotificationByIdApi: (notificationId: number) =>
        api.get<ApiGen_Concepts_Notification>(`/notifications/${notificationId}`),

      postNotificationApi: (notification: ApiGen_Concepts_Notification) =>
        api.post<ApiGen_Concepts_Notification>(`/notifications`, notification),

      putNotificationApi: (notification: ApiGen_Concepts_Notification) =>
        api.put<ApiGen_Concepts_Notification>(
          `/notifications/${notification.notificationId}`,
          notification,
        ),

      deleteNotificationApi: (notificationId: number) =>
        api.delete<boolean>(`/notifications/${notificationId}`),
    }),
    [api],
  );
};
