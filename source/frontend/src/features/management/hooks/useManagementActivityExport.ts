import axios from 'axios';
import fileDownload from 'js-file-download';
import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { hideLoading, showLoading } from 'react-redux-loading-bar';

import * as actionTypes from '@/constants/actionTypes';
import { catchAxiosError } from '@/customAxios';
import {
  IPaginateManagementActivities,
  useApiManagementActivities,
} from '@/hooks/pims-api/useApiManagementActivities';
import { logRequest, logSuccess } from '@/store/slices/network/networkSlice';

export const useManagementActivityExport = () => {
  const { exportManagementActivitiesApi } = useApiManagementActivities();

  const dispatch = useDispatch();

  const exportManagementActivities = useCallback(
    async (
      filter: IPaginateManagementActivities,
      outputFormat: 'csv' | 'excel' = 'excel',
      fileName = `pims_management_activities.${outputFormat === 'csv' ? 'csv' : 'xlsx'}`,
      requestId = 'management-activities-report',
    ) => {
      dispatch(logRequest(requestId));
      dispatch(showLoading());
      try {
        const { data, status } = await exportManagementActivitiesApi(filter, outputFormat);
        dispatch(logSuccess({ name: requestId, status }));
        dispatch(hideLoading());
        // trigger file download in client browser
        fileDownload(data, fileName);
      } catch (axiosError) {
        if (axios.isAxiosError(axiosError)) {
          catchAxiosError(axiosError, dispatch, actionTypes.DELETE_PARCEL);
        }
      }
    },
    [dispatch, exportManagementActivitiesApi],
  );

  return { exportManagementActivities };
};
