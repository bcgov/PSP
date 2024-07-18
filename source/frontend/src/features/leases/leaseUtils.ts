import moment from 'moment';

import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
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

export const getCalculatedExpiry = (
  lease: ApiGen_Concepts_Lease,
  renewals: ApiGen_Concepts_LeaseRenewal[],
): string => {
  const excercisedRenewalDates = renewals.filter(r => r.isExercised).flatMap(fr => fr.expiryDt);

  let calculatedExpiry: string | null = lease?.expiryDate ?? '';
  for (let i = 0; i < excercisedRenewalDates.length; i++) {
    if (moment(excercisedRenewalDates[i]).isAfter(calculatedExpiry)) {
      calculatedExpiry = excercisedRenewalDates[i];
    }
  }
  return calculatedExpiry;
};

export const getSuggestedFee = (isPublicBenefit: boolean, isFinancialGain: boolean): string => {
  if (isPublicBenefit == null || isFinancialGain == null) return 'Unknown';
  else if (isPublicBenefit && isFinancialGain) return 'Licence Administration Fee (LAF) *';
  else if (isPublicBenefit && !isFinancialGain) return '$1 - Nominal';
  else if (!isPublicBenefit && isFinancialGain)
    return 'Fair Market Value (FMV) - (Licence Administration Fee Minimum)';
  else return '$1 / Fair Market Value / Licence Administration Fee';
};
