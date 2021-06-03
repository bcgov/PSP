import React from 'react';
import { useTenant } from 'tenants';

/**
 * FeatureVisible properties.
 */
interface Props {
  tenant: string;
}

/**
 * Component for showing content for specific tenants.
 * @param param0 Component properties.
 * @returns If the current tenant matches the `tenant` property it will render the content; otherwise returns `null`.
 */
const FeatureVisible: React.FC<Props> = ({ tenant, children }) => {
  const tenantConfig = useTenant();
  let visible = tenantConfig.id === tenant;

  // Support render props
  if (typeof children === 'function') {
    return children(visible);
  }

  return visible ? children : null;
};

export default FeatureVisible;
