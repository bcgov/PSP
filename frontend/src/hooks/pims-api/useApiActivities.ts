import { ENVIRONMENT } from 'constants/environment';
import { FileTypes } from 'constants/fileTypes';
import CustomAxios from 'customAxios';
import { Api_Activity, Api_ActivityTemplate, Api_FileActivity } from 'models/api/Activity';

export const getActivity = (activityId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_Activity>(`/activities/${activityId}`);
export const getFileActivities = (fileType: FileTypes, fileId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_Activity[]>(
    `/activities/${fileType}/${fileId}`,
  );
export const getActivityTemplates = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_ActivityTemplate[]>(`/activities/templates`);
export const postActivity = (activity: Api_Activity) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<Api_Activity>(`/activities`, activity);
export const postFileActivity = (fileType: FileTypes, activity: Api_FileActivity) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<Api_Activity>(
    `/activities/${fileType}`,
    activity,
  );
export const putActivity = (activity: Api_Activity) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_Activity>(
    `/activities/${activity.id ?? 0}`,
    activity,
  );
export const putActivityProperties = (fileType: FileTypes, activity: Api_Activity) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_Activity>(
    `/activities/${fileType}/activity/${activity.id ?? 0}/properties`,
    activity,
  );
export const deleteActivity = (activityId: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).delete<boolean>(`/activities/${activityId}`);
