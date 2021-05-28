import { ITenantConfig } from 'tenants/ITenantConfig';

import { config as CITZ } from './CITZ';
import { config as MOTI } from './MOTI';
export * from './default';

/**
 * Pre-configured tenant settings.
 */
export const config: Record<string, ITenantConfig> = {
  CITZ,
  MOTI,
};
