/**
 * API Tenant configuration.
 */
export interface ITenantConfig {
  // Unique code to identify the tenant.
  code: string;
  // The name of the tenant.
  name: string;
  // The tenant description.
  description: string;
  // Tenant configuration settings.
  settings: {
    // The email to use for the help desk.
    helpDeskEmail: string;
  };
}

export default ITenantConfig;
