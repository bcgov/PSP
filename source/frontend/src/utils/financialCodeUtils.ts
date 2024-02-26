import orderBy from 'lodash/orderBy';
import moment from 'moment';

import { SelectOption } from '@/components/common/form';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';

import { exists } from './utils';

export function toDropDownOptions(
  values: ApiGen_Concepts_FinancialCode[],
  includeExpired = false,
): SelectOption[] {
  return orderBy(values, ['displayOrder'], ['asc'])
    .filter(c => includeExpired || !isExpiredCode(c))
    .map<SelectOption>(c => {
      return {
        label: `${c.code} - ${c.description}`,
        value: c.id ?? 0,
      };
    });
}

export function isExpiredCode(value: ApiGen_Concepts_FinancialCode): boolean {
  const now = moment();

  if (exists(value.effectiveDate) && moment(value.effectiveDate).isAfter(now)) {
    return true;
  }

  if (exists(value.expiryDate) && moment(value.expiryDate).isBefore(now, 'day')) {
    return true;
  }

  // no expiry date means the code never expires
  return false;
}
