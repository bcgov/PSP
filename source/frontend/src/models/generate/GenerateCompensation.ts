import { chain } from 'lodash';
import { Api_CompensationFinancial } from 'models/api/CompensationFinancial';
import { Api_CompensationRequisition } from 'models/api/CompensationRequisition';
import { Api_H120Category } from 'models/api/H120Category';
import moment from 'moment';
import { formatMoney } from 'utils';

import { Api_GenerateCompensationFinancial } from './GenerateCompensationFinancial';
import { Api_GenerateCompensationFinancialSummary } from './GenerateCompensationFinancialSummary';
import { Api_GenerateFile } from './GenerateFile';

export class Api_GenerateCompensation {
  file: Api_GenerateFile | null;
  status: string;
  generated_date: string;
  requisition_number: string;
  special_instructions: string;
  detailed_remarks: string;
  financial_activities: Api_GenerateCompensationFinancial[];
  summary_financial_activities: Api_GenerateCompensationFinancialSummary[];
  file_financial_total: string;
  financial_total: string;

  constructor(
    compensation: Api_CompensationRequisition | null,
    generateFile: Api_GenerateFile | null,
    h120Categories: Api_H120Category[],
    finalFileFinancials: Api_CompensationFinancial[],
  ) {
    this.file = generateFile;
    this.generated_date = moment().format('MMM DD, YYYY') ?? '';
    this.requisition_number = compensation?.id?.toString() ?? '';
    this.special_instructions = compensation?.specialInstruction ?? '';
    this.detailed_remarks = compensation?.detailedRemarks ?? '';
    this.status = compensation?.isDraft ? 'Draft' : 'Final';
    this.financial_activities =
      compensation?.financials?.map(
        financial => new Api_GenerateCompensationFinancial(financial),
      ) ?? [];
    this.summary_financial_activities = chain(h120Categories)
      .map(
        category =>
          new Api_GenerateCompensationFinancialSummary(
            category,
            compensation?.financials ?? [],
            finalFileFinancials,
          ),
      )
      .filter(summary => summary.file_total !== '$0.00' || summary.total !== '$0.00')
      .value();

    const otherFileFinancials = finalFileFinancials?.filter(
      f => !compensation?.financials?.find(cf => cf?.id === f?.id),
    );
    this.file_financial_total = formatMoney(
      otherFileFinancials?.reduce((acc, curr) => acc + (curr?.totalAmount ?? 0), 0) ?? 0,
    );
    this.financial_total = formatMoney(
      compensation?.financials?.reduce((acc, curr) => acc + (curr?.totalAmount ?? 0), 0) ?? 0,
    );
  }
}
