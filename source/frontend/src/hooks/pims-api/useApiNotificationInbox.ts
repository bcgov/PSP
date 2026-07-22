import queryString from 'query-string';
import React from 'react';

import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_NotificationInboxItem } from '@/models/api/generated/ApiGen_Concepts_NotificationInboxItem';

import useAxiosApi from './useApi';

/**
 * PIMS API wrapper to centralize all AJAX requests to the notification inbox endpoints.
 * @returns Object containing functions to make requests to the PIMS API.
 */
export const useApiNotificationInbox = () => {
  const api = useAxiosApi();

  return React.useMemo(
    () => ({
      getUserInboxApi: (page = 1, quantity = 10) =>
        api.get<ApiGen_Base_Page<ApiGen_Concepts_NotificationInboxItem>>(
          `/notifications/inbox?${queryString.stringify({ page, quantity })}`,
        ),

      getNotificationOutputApi: (outputId: number) =>
        api.get<ApiGen_Concepts_NotificationInboxItem>(`/notifications/inbox/${outputId}`),

      getUnreadCountApi: () =>
        api.get<{ unreadCount: number }>(`/notifications/inbox/unread-count`),

      updateReadStatusApi: (outputId: number, isRead: boolean) =>
        api.patch<void>(`/notifications/inbox/${outputId}/read`, isRead, {
          headers: { 'Content-Type': 'application/json' },
        }),

      markAllAsReadApi: () => api.patch<void>('/notifications/inbox/read-all'),

      deleteNotificationOutputApi: (outputId: number) =>
        api.delete<boolean>(`/notifications/inbox/${outputId}`),
    }),
    [api],
  );
};
