import React from 'react';

import { TenantContext } from '.';
import { ITenantConfig } from './ITenantConfig';

/**
 * Provides the tenant context state.
 * Requires TenantProvider to be in a parent component to enable 'useTenant()'.
 * @returns ITenantConfig object from context.
 */
export const useTenant = (): ITenantConfig => {
  var { tenant } = React.useContext(TenantContext);

  return tenant;
};
