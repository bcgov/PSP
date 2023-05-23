import { SelectOption } from 'components/common/form';
import orderBy from 'lodash/orderBy';
import { Api_FinancialCode } from 'models/api/FinancialCode';
import moment from 'moment';

export function toDropDownOptions(
  values: Api_FinancialCode[],
  includeExpired = false,
): SelectOption[] {
  return orderBy(values, ['displayOrder'], ['asc'])
    .filter(c => includeExpired || !isExpiredCode(c))
    .map<SelectOption>(c => {
      return {
        label: c.description!,
        value: c.id!,
      };
    });
}

export function isExpiredCode(value: Api_FinancialCode): boolean {
  const now = moment();

  if (value.effectiveDate !== undefined && moment(value.effectiveDate).isAfter(now)) {
    return true;
  }

  if (value.expiryDate !== undefined && moment(value.expiryDate).isBefore(now, 'day')) {
    return true;
  }

  // no expiry date means the code never expires
  return false;
}
