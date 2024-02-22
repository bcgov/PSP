import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios from '@/customAxios';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';

export const getFinancialCodesByType = (codeType: ApiGen_Concepts_FinancialCodeTypes) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_FinancialCode[]>(
    `/financial-codes/${codeType}`,
  );

export const getFinancialActivitiesCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_FinancialCode[]>(
    `/financial-codes/financial-activities`,
  );

export const getChartOfAccountsCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_FinancialCode[]>(
    `/financial-codes/chart-accounts`,
  );

export const getResponsibilityCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_FinancialCode[]>(
    `/financial-codes/responsibilities`,
  );

export const getYearlyFinancialCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_FinancialCode[]>(
    `/financial-codes/yearly-financials`,
  );

export const getFinancialCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_FinancialCode[]>(
    `/admin/financial-codes`,
  );

export const getFinancialCode = (codeType: ApiGen_Concepts_FinancialCodeTypes, id: number) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<ApiGen_Concepts_FinancialCode>(
    `/admin/financial-codes/${codeType}/${id}`,
  );

export const postFinancialCode = (
  codeType: ApiGen_Concepts_FinancialCodeTypes,
  financialCode: ApiGen_Concepts_FinancialCode,
) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<ApiGen_Concepts_FinancialCode>(
    `/admin/financial-codes/${codeType}`,
    financialCode,
  );

export const putFinancialCode = (financialCode: ApiGen_Concepts_FinancialCode) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).put<ApiGen_Concepts_FinancialCode>(
    `/admin/financial-codes/${financialCode.type}/${financialCode.id}`,
    financialCode,
  );
