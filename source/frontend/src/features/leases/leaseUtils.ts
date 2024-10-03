import moment from 'moment';

import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { isValidId } from '@/utils';
import { formatNames } from '@/utils/personUtils';

/**
 * return all of the person tenant names and organization tenant names of this lease
 * @param lease
 */
export const getAllNames = (tenants: ApiGen_Concepts_LeaseStakeholder[]): string[] => {
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
  const exercisedRenewalDates = renewals.filter(r => r.isExercised).flatMap(fr => fr.expiryDt);

  let calculatedExpiry: string | null = lease?.expiryDate ?? '';
  for (let i = 0; i < exercisedRenewalDates.length; i++) {
    if (moment(exercisedRenewalDates[i]).isAfter(calculatedExpiry)) {
      calculatedExpiry = exercisedRenewalDates[i];
    }
  }
  return calculatedExpiry;
};

export enum SuggestedFeeCode {
  LAF = 'Licence Administration Fee (LAF) *',
  FMV = 'Fair Market Value (FMV) - (Licence Administration Fee Minimum)',
  ANY = '$1 / Fair Market Value / Licence Administration Fee',
  NOMINAL = '$1 - Nominal',
  UNKNOWN = 'Unknown',
}

export const getSuggestedFee = (
  isPublicBenefit: boolean,
  isFinancialGain: boolean,
): SuggestedFeeCode => {
  if (isPublicBenefit == null || isFinancialGain == null) return SuggestedFeeCode.UNKNOWN;
  else if (isPublicBenefit && isFinancialGain) return SuggestedFeeCode.LAF;
  else if (isPublicBenefit && !isFinancialGain) return SuggestedFeeCode.NOMINAL;
  else if (!isPublicBenefit && isFinancialGain) return SuggestedFeeCode.FMV;
  else return SuggestedFeeCode.ANY;
};
