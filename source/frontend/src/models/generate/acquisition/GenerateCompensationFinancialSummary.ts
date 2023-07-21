import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_H120Category } from '@/models/api/H120Category';
import { formatMoney } from '@/utils';

export class Api_GenerateCompensationFinancialSummary {
  total: string;
  file_total: string;
  h120_category_name: string;

  constructor(
    h120Category: Api_H120Category | null,
    h120Financials: Api_CompensationFinancial[],
    finalFileFinancials: Api_CompensationFinancial[],
  ) {
    this.h120_category_name = h120Category?.description ?? '';
    this.total = formatMoney(
      h120Financials
        .filter(f => f.financialActivityCode?.id === h120Category?.financialActivityId)
        .reduce((acc, curr) => acc + (curr?.totalAmount ?? 0), 0),
    );
    this.file_total = formatMoney(
      finalFileFinancials
        .filter(f => f.financialActivityCodeId === h120Category?.financialActivityId)
        .reduce((acc, curr) => acc + (curr?.totalAmount ?? 0), 0),
    );
  }
}
