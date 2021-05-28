import { AxiosResponse } from 'axios';

import { IApiVersion, IHealthLive, IHealthReady } from '.';

export interface IApiHealth {
  // Get the api version information.
  getVersion: () => Promise<AxiosResponse<IApiVersion>>;
  // Get the status of the api to determine if it has a live connection to its datasource.
  getLive: () => Promise<AxiosResponse<IHealthLive>>;
  // Get the status of the api to determine if it is ready.
  getReady: () => Promise<AxiosResponse<IHealthReady>>;
}
