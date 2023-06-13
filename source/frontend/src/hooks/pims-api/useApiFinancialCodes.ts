import { ENVIRONMENT } from '@/constants/environment';
import { FinancialCodeTypes } from '@/constants/index';
import CustomAxios from '@/customAxios';
import { Api_FinancialCode } from '@/models/api/FinancialCode';

export const getFinancialCodesByType = (codeType: FinancialCodeTypes) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FinancialCode[]>(
    `/financial-codes/${codeType}`,
  );

export const getFinancialActivitiesCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FinancialCode[]>(
    `/financial-codes/financial-activities`,
  );

export const getChartOfAccountsCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FinancialCode[]>(
    `/financial-codes/chart-accounts`,
  );

export const getResponsibilityCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FinancialCode[]>(
    `/financial-codes/responsibilities`,
  );

export const getYearlyFinancialCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FinancialCode[]>(
    `/financial-codes/yearly-financials`,
  );

export const getFinancialCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FinancialCode[]>(`/admin/financial-codes`);

export const getFinancialCode = (codeType: FinancialCodeTypes, id: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FinancialCode>(
    `/admin/financial-codes/${codeType}/${id}`,
  );

export const postFinancialCode = (codeType: FinancialCodeTypes, financialCode: Api_FinancialCode) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<Api_FinancialCode>(
    `/admin/financial-codes/${codeType}`,
    financialCode,
  );

export const putFinancialCode = (financialCode: Api_FinancialCode) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<Api_FinancialCode>(
    `/admin/financial-codes/${financialCode.type}/${financialCode.id}`,
    financialCode,
  );
