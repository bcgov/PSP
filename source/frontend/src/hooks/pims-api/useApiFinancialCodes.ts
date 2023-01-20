import { ENVIRONMENT } from 'constants/environment';
import CustomAxios from 'customAxios';
import { Api_FinancialCode } from 'models/api/FinancialCode';

export const getFinancialCodes = () =>
  CustomAxios({ baseURL: ENVIRONMENT.apiUrl }).get<Api_FinancialCode[]>(`/admin/financial-codes`);
