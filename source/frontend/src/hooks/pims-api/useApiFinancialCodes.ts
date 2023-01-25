import { ENVIRONMENT } from 'constants/environment';
import { FinancialCodeTypes } from 'constants/index';
import CustomAxios from 'customAxios';
import { Api_FinancialCode } from 'models/api/FinancialCode';

export const getFinancialCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FinancialCode[]>(`/admin/financial-codes`);

export const postFinancialCode = (codeType: FinancialCodeTypes, financialCode: Api_FinancialCode) =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).post<Api_FinancialCode>(
    `/admin/financial-codes/${codeType}`,
    financialCode,
  );
