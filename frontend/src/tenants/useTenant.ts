import React from 'react';

import { TenantContext } from '.';

/**
 * Provides the tenant context state.
 * Requires TenantProvider to be in a parent component to enable 'useTenant()'.
 * @returns ITenantConfig object from context.
 */
export const useTenant = () => {
  var { tenant } = React.useContext(TenantContext);

  return tenant;
};
