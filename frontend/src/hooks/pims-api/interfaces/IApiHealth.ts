import { AxiosResponse } from 'axios';
import { IApiVersion, IHealthLive, IHealthReady } from '.';

export interface IApiHealth {
  getVersion: () => Promise<AxiosResponse<IApiVersion>>;
  getLive: () => Promise<AxiosResponse<IHealthLive>>;
  getReady: () => Promise<AxiosResponse<IHealthReady>>;
}
