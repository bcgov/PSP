import axios from 'axios';
import noop from 'lodash/noop';
import React from 'react';

import ITenantConfig, { ITenantConfig2 } from '@/hooks/pims-api/interfaces/ITenantConfig';

import { config } from './config';
import defaultTenant from './config/defaultTenant';

export interface ITenantContext {
  // The tenant configuration.
  tenant: ITenantConfig2;
  // Update the tenant configuration.
  setTenant: React.Dispatch<React.SetStateAction<ITenantConfig2>>;
}

/**
 * Tenant context to maintain tenant configuration settings.
 */
export const TenantContext = React.createContext<ITenantContext>({
  tenant: defaultTenant,
  setTenant: noop,
});
export const { Consumer: TenantConsumer } = TenantContext;

/**
 * Provides a context provider for tenant configuration settings.
 * If "VITE_TENANT" environment variable is set, it will use settings from it.
 * Otherwise it will fetch the public "/public/tenants/tenant.json" environment specific configuration file.
 * @param props TenantProvider properties.
 * @returns TenantProvider component.
 */
export const TenantProvider: React.FC<React.PropsWithChildren<unknown>> = props => {
  const [tenant, setTenant] = React.useState(defaultTenant);
  React.useMemo(async () => {
    // If the env var exists use it.
    if (import.meta.env.VITE_TENANT) {
      // If it's a JSON string parse it.
      if (import.meta.env.VITE_TENANT.startsWith('{')) {
        const envTenantConfig = JSON.parse(import.meta.env.VITE_TENANT);
        setTenant({ ...defaultTenant, ...envTenantConfig });
      } else {
        setTenant(
          config[import.meta.env.VITE_TENANT]
            ? { ...defaultTenant, ...config[import.meta.env.VITE_TENANT] }
            : defaultTenant,
        );
      }
    } else {
      // Fetch the configuration file generated for the environment.
      const r = await axios.get<ITenantConfig>('/tenants/tenant.json');
      const fileTenantConfig = await r.data;
      setTenant({ ...defaultTenant, ...fileTenantConfig });
    }
  }, []);
  return (
    <TenantContext.Provider value={{ tenant, setTenant }}>{props.children}</TenantContext.Provider>
  );
};

export default TenantContext;
