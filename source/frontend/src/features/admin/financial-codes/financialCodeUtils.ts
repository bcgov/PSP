import { SelectOption } from '@/components/common/form';
import { FinancialCodeTypes } from '@/constants/financialCodeTypes';

export function formatFinancialCodeType(value: FinancialCodeTypes): string {
  switch (value) {
    case FinancialCodeTypes.BusinessFunction:
      return 'Business function';
    case FinancialCodeTypes.CostType:
      return 'Cost types';
    case FinancialCodeTypes.WorkActivity:
      return 'Work activity';
    case FinancialCodeTypes.ChartOfAccounts:
      return 'Chart of accounts';
    case FinancialCodeTypes.FinancialActivity:
      return 'Financial activity';
    case FinancialCodeTypes.Responsibility:
      return 'Responsibility';
    case FinancialCodeTypes.YearlyFinancial:
      return 'Yearly financial';
    default:
      return 'Unknown';
  }
}

/**
 * Converts the FinancialCodeTypes enum to SELECT options for display in the UI
 */
export function formatAsSelectOptions(): SelectOption[] {
  return Object.keys(FinancialCodeTypes).map<SelectOption>(key => ({
    value: key,
    label: formatFinancialCodeType(FinancialCodeTypes[key as FinancialCodeTypes]),
  }));
}
