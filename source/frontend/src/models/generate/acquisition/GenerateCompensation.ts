import { chain } from 'lodash';
import moment from 'moment';

import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_H120Category } from '@/models/api/H120Category';
import { formatMoney } from '@/utils';

import { Api_GenerateProject } from '../GenerateProject';
import { Api_GenerateAcquisitionFile } from './GenerateAcquisitionFile';
import { Api_GenerateCompensationFinancial } from './GenerateCompensationFinancial';
import { Api_GenerateCompensationFinancialSummary } from './GenerateCompensationFinancialSummary';
import { Api_GenerateCompensationPayee } from './GenerateCompensationPayee';

export class Api_GenerateCompensation {
  file: Api_GenerateAcquisitionFile | null;
  status: string;
  generated_date: string;
  requisition_number: string;
  special_instructions: string;
  detailed_remarks: string;
  financial_activities: Api_GenerateCompensationFinancial[];
  summary_financial_activities: Api_GenerateCompensationFinancialSummary[];
  file_financial_total: string;
  financial_total: string;
  yearly_financial: string;
  service_line: string;
  responsibility_center: string;
  client: string;
  payee: Api_GenerateCompensationPayee;
  alternate_project: Api_GenerateProject | null;

  constructor(
    compensation: Api_CompensationRequisition | null,
    file: Api_GenerateAcquisitionFile | null,
    h120Categories: Api_H120Category[],
    finalFileFinancials: Api_CompensationFinancial[],
    client: string = '',
  ) {
    this.file = file;
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

    this.file_financial_total = formatMoney(
      finalFileFinancials?.reduce((acc, curr) => acc + (curr?.totalAmount ?? 0), 0) ?? 0,
    );
    this.financial_total = formatMoney(
      compensation?.financials?.reduce((acc, curr) => acc + (curr?.totalAmount ?? 0), 0) ?? 0,
    );
    this.yearly_financial = compensation?.yearlyFinancial?.code ?? '';
    this.service_line = compensation?.chartOfAccounts?.code ?? '';
    this.responsibility_center = compensation?.responsibility?.code ?? '';
    this.client = client;
    this.payee = new Api_GenerateCompensationPayee(compensation, compensation?.financials ?? []);
    this.alternate_project = new Api_GenerateProject(compensation?.alternateProject ?? null);
  }
}
