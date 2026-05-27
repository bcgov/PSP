import queryString from 'query-string';
import React from 'react';

import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_NotificationOutput } from '@/models/api/generated/ApiGen_Concepts_NotificationOutput';

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
        api.get<ApiGen_Base_Page<ApiGen_Concepts_NotificationOutput>>(
          `/notifications/inbox?${queryString.stringify({ page, quantity })}`,
        ),

      getNotificationOutputApi: (outputId: number) =>
        api.get<ApiGen_Concepts_NotificationOutput>(`/notifications/inbox/${outputId}`),

      getUnreadCountApi: () => api.get<number>(`/notifications/inbox/unread-count`),

      updateReadStatusApi: (outputId: number, isRead: boolean) =>
        api.patch<unknown>(`/notifications/inbox/${outputId}/read`, {
          isRead,
        }),

      markAllAsReadApi: () => api.patch<unknown>('/notifications/inbox/read-all'),

      deleteNotificationOutputApi: (outputId: number) =>
        api.delete<boolean>(`/notifications/inbox/${outputId}`),
    }),
    [api],
  );
};
