import { ApiGen_Concepts_LeaseTenant } from '@/models/api/generated/ApiGen_Concepts_LeaseTenant';
import { isValidId } from '@/utils';
import { formatNames } from '@/utils/personUtils';

/**
 * return all of the person tenant names and organization tenant names of this lease
 * @param lease
 */
export const getAllNames = (tenants: ApiGen_Concepts_LeaseTenant[]): string[] => {
  const persons = tenants?.filter(t => isValidId(t.personId)).map(p => p.person) ?? [];
  const organizations = tenants?.filter(t => !isValidId(t.personId)).map(p => p.organization) ?? [];
  const allNames =
    persons
      ?.map<string>(p => formatNames([p?.firstName, p?.middleNames, p?.surname]))
      .concat(organizations?.map(o => o?.name ?? '')) ?? [];
  return allNames;
};

export const getSuggestedFee = (isPublicBenefit: boolean, isFinancialGain: boolean): string => {
  if (isPublicBenefit == null || isFinancialGain == null) return 'Unknown';
  else if (isPublicBenefit && isFinancialGain) return 'Licence Administration Fee (LAF) *';
  else if (isPublicBenefit && !isFinancialGain) return 'Fair Market Value (FMV)';
  else if (!isPublicBenefit && isFinancialGain) return '$1 - Nominal';
  else return '$1 / Fair Market Value / Licence Administration Fee';
};
