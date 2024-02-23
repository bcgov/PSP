import { SelectOption } from '@/components/common/form';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';

export function formatFinancialCodeType(value: ApiGen_Concepts_FinancialCodeTypes): string {
  switch (value) {
    case ApiGen_Concepts_FinancialCodeTypes.BusinessFunction:
      return 'Business function';
    case ApiGen_Concepts_FinancialCodeTypes.CostType:
      return 'Cost types';
    case ApiGen_Concepts_FinancialCodeTypes.WorkActivity:
      return 'Work activity';
    case ApiGen_Concepts_FinancialCodeTypes.ChartOfAccounts:
      return 'Chart of accounts';
    case ApiGen_Concepts_FinancialCodeTypes.FinancialActivity:
      return 'Financial activity';
    case ApiGen_Concepts_FinancialCodeTypes.Responsibility:
      return 'Responsibility';
    case ApiGen_Concepts_FinancialCodeTypes.YearlyFinancial:
      return 'Yearly financial';
    default:
      return 'Unknown';
  }
}

/**
 * Converts the FinancialCodeTypes enum to SELECT options for display in the UI
 */
export function formatAsSelectOptions(): SelectOption[] {
  return Object.keys(ApiGen_Concepts_FinancialCodeTypes).map<SelectOption>(key => ({
    value: key,
    label: formatFinancialCodeType(
      ApiGen_Concepts_FinancialCodeTypes[key as ApiGen_Concepts_FinancialCodeTypes],
    ),
  }));
}
