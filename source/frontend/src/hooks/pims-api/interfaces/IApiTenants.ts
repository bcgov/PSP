import { AxiosResponse } from 'axios';

import { ITenantConfig } from './ITenantConfig';

export interface IApiTenants {
  // Get the tenant configuration settings.
  getSettings: () => Promise<AxiosResponse<ITenantConfig>>;
}

export default IApiTenants;
