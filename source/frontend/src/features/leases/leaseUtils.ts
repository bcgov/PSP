import { Api_LeaseTenant } from '@/models/api/LeaseTenant';
import { formatNames } from '@/utils/personUtils';

/**
 * return all of the person tenant names and organization tenant names of this lease
 * @param lease
 */
export const getAllNames = (tenants: Api_LeaseTenant[]): string[] => {
  const persons = tenants?.filter(t => t.personId !== null).map(p => p.person) ?? [];
  const organizations = tenants?.filter(t => t.personId === null).map(p => p.organization) ?? [];
  const allNames =
    persons
      ?.map<string>(p => formatNames([p?.firstName, p?.middleNames, p?.surname]))
      .concat(organizations?.map(o => o?.name ?? '')) ?? [];
  return allNames;
};
