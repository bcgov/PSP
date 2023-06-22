import { useTenant } from '@/tenants';

/**
 * Provides a way to apply the correct favicon based on the current tenant.
 * @returns Hook to update the favicon with the configured tenant icon.
 */
export const useFavicon = () => {
  const tenant = useTenant();

  const favicon = document.getElementById('favicon') as HTMLLinkElement;
  if (favicon) favicon.href = tenant.logo.favicon;

  return favicon;
};
