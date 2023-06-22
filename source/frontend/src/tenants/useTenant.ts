import React from 'react';

import { ITenantConfig2 } from '@/hooks/pims-api/interfaces/ITenantConfig';

import { TenantContext } from '.';

/**
 * Provides the tenant context state.
 * Requires TenantProvider to be in a parent component to enable 'useTenant()'.
 * @returns ITenantConfig object from context.
 */
export const useTenant = (): ITenantConfig2 => {
  var { tenant } = React.useContext(TenantContext);

  return tenant;
};
