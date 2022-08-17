import { ENVIRONMENT } from 'constants/environment';
import CustomAxios from 'customAxios';
import { Api_Activity } from 'models/api/Activity';

export const getActivity = (activityId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_Activity>(`/activities/${activityId}`);
export const postActivity = (activity: Api_Activity) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<Api_Activity>(`/activities`, activity);
export const putActivity = (activity: Api_Activity) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_Activity>(
    `/activities/${activity.id ?? 0}`,
    activity,
  );
export const deleteActivity = (activityId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(`/activities/${activityId}`);
