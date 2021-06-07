import React from 'react';
import { useTenant } from 'tenants';

/**
 * FeatureHidden properties.
 */
interface Props {
  tenant: string;
}

/**
 * Component for hiding content for specific tenants.
 * @param param0 Component properties.
 * @returns If the current tenant matches the `tenant` property it will *not* render the content at all.
 */
const FeatureHidden: React.FC<Props> = ({ tenant, children }) => {
  const tenantConfig = useTenant();
  let hidden = tenantConfig.id === tenant;

  // Support render props
  if (typeof children === 'function') {
    return children(hidden);
  }

  return hidden ? null : children;
};

export default FeatureHidden;
