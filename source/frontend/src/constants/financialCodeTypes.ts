export enum FinancialCodeTypes {
  BusinessFunction = 'BusinessFunction',
  CostTypes = 'CostTypes',
  WorkActivity = 'WorkActivity',
  ChartOfAccounts = 'ChartOfAccounts',
  FinancialActivity = 'FinancialActivity',
  Responsibility = 'Responsibility',
  YearlyFinancial = 'YearlyFinancial',
}

export function formatFinancialCodeType(value: FinancialCodeTypes): string {
  switch (value) {
    case FinancialCodeTypes.BusinessFunction:
      return 'Business function';
    case FinancialCodeTypes.CostTypes:
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
