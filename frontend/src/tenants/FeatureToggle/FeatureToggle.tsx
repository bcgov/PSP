import React from 'react';
import { useTenant } from 'tenants';

/**
 * Feature Toggle properties.
 */
interface Props {
  tenant: string;
  hide?: boolean;
}

/**
 * The FeatureToggle component provides a way to hide or show UI features based on the current tenant.
 * @param param0 FeatureToggle component properties.
 * @returns If the current tenant matches the `tenant` property it will render the children; otherwise returns `null`.
 */
const FeatureToggle: React.FC<Props> = ({ tenant, hide = false, children }) => {
  const tenantConfig = useTenant();
  let hasFeature = tenantConfig.id === tenant;
  if (!!hide) {
    hasFeature = !hasFeature;
  }
  // Support render props
  const render = children;
  if (typeof render === 'function') return render(hasFeature);
  if (!hasFeature) return null;
  return render;
};

export default FeatureToggle;
