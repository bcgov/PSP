import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_H120Category } from '@/models/api/generated/ApiGen_Concepts_H120Category';
import { formatMoney } from '@/utils';

export class Api_GenerateCompensationFinancialSummary {
  pretax_total: string;
  file_pretax_total: string;
  h120_category_name: string;

  constructor(
    h120Category: ApiGen_Concepts_H120Category | null,
    h120Financials: ApiGen_Concepts_CompensationFinancial[],
    finalFileFinancials: ApiGen_Concepts_CompensationFinancial[],
  ) {
    this.h120_category_name = h120Category?.description ?? '';
    this.pretax_total = formatMoney(
      h120Financials
        .filter(f => f.financialActivityCode?.id === h120Category?.financialActivityId)
        .reduce((acc, curr) => acc + (curr?.pretaxAmount ?? 0), 0),
    );
    this.file_pretax_total = formatMoney(
      finalFileFinancials
        .filter(f => f.financialActivityCodeId === h120Category?.financialActivityId)
        .reduce((acc, curr) => acc + (curr?.pretaxAmount ?? 0), 0),
    );
  }
}
