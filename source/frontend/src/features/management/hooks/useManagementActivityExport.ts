import { AxiosResponse } from 'axios';
import { useCallback } from 'react';

import { useApiManagementActivities } from '@/hooks/pims-api/useApiManagementActivities';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { Api_ManagementActivityFilter } from '@/models/api/ManagementActivityFilter';
import { useAxiosErrorHandler } from '@/utils';

export const useManagementActivityExport = () => {
  const {
    generateManagementActivitiesOverviewReportApi,
    generateManagementActivitiesInvoiceReportApi,
  } = useApiManagementActivities();

  const generateManagementActivitiesOverviewReport = useApiRequestWrapper<
    (filter: Api_ManagementActivityFilter) => Promise<AxiosResponse<AxiosResponse<Blob, any>>>
  >({
    requestFunction: useCallback(
      async (filter: Api_ManagementActivityFilter) =>
        await generateManagementActivitiesOverviewReportApi(filter),
      [generateManagementActivitiesOverviewReportApi],
    ) as unknown as (
      filter: Api_ManagementActivityFilter,
    ) => Promise<AxiosResponse<AxiosResponse<Blob, any>>>,
    requestName: 'GenerateManagementActivitiesOverviewReport',
    rawResponse: true,
    onError: useAxiosErrorHandler('Failed to load Management Activities Overview Report'),
  });

  const generateManagementActivitiesInvoiceReport = useApiRequestWrapper<
    (filter: Api_ManagementActivityFilter) => Promise<AxiosResponse<AxiosResponse<Blob, any>>>
  >({
    requestFunction: useCallback(
      async (filter: Api_ManagementActivityFilter) =>
        await generateManagementActivitiesInvoiceReportApi(filter),
      [generateManagementActivitiesInvoiceReportApi],
    ) as unknown as (
      filter: Api_ManagementActivityFilter,
    ) => Promise<AxiosResponse<AxiosResponse<Blob, any>>>,
    requestName: 'GenerateManagementActivitiesInvoiceReport',
    rawResponse: true,
    onError: useAxiosErrorHandler('Failed to load Management Activities Invoice Report'),
  });

  return { generateManagementActivitiesOverviewReport, generateManagementActivitiesInvoiceReport };
};
