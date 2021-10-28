import React from 'react';

import { config, defaultTenant, ITenantConfig } from '.';

export interface ITenantContext {
  // The tenant configuration.
  tenant: ITenantConfig;
  // Update the tenant configuration.
  setTenant: React.Dispatch<React.SetStateAction<ITenantConfig>>;
}

/**
 * Tenant context to maintain tenant configuration settings.
 */
export const TenantContext = React.createContext<ITenantContext>({
  tenant: defaultTenant,
  setTenant: () => {},
});
export const { Consumer: TenantConsumer } = TenantContext;

/**
 * Provides a context provider for tenant configuration settings.
 * If "REACT_APP_TENANT" environment variable is set, it will use settings from it.
 * Otherwise it will fetch the public "/public/tenants/tenant.json" environment specific configuration file.
 * @param props TenantProvider properties.
 * @returns TenantProvider component.
 */
export const TenantProvider: React.FC = props => {
  const [tenant, setTenant] = React.useState(defaultTenant);
  React.useMemo(async () => {
    // If the env var exists use it.
    if (process.env.REACT_APP_TENANT) {
      // If it's a JSON string parse it.
      if (process.env.REACT_APP_TENANT.startsWith('{')) {
        const envTenantConfig = JSON.parse(process.env.REACT_APP_TENANT);
        setTenant({ ...defaultTenant, ...envTenantConfig });
      } else {
        setTenant(
          config[process.env.REACT_APP_TENANT]
            ? { ...defaultTenant, ...config[process.env.REACT_APP_TENANT] }
            : defaultTenant,
        );
      }
    } else {
      // Fetch the configuration file generated for the environment.
      const r = await fetch(`${process.env.PUBLIC_URL}/tenants/tenant.json`);
      const fileTenantConfig = await r.json();
      setTenant({ ...defaultTenant, ...fileTenantConfig });
    }
  }, []);
  return (
    <TenantContext.Provider value={{ tenant, setTenant }}>{props.children}</TenantContext.Provider>
  );
};

export default TenantContext;
