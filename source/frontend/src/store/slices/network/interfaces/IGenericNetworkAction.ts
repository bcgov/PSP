import { AxiosError } from 'axios';

export interface IGenericNetworkAction {
  isFetching?: boolean;
  name: string;
  type?: string;
  error?: AxiosError;
  status?: number;
  data?: any;
}
