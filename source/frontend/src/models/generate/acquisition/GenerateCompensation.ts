import { chain } from 'lodash';
import moment from 'moment';

import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_H120Category } from '@/models/api/generated/ApiGen_Concepts_H120Category';
import { ApiGen_Concepts_InterestHolderProperty } from '@/models/api/generated/ApiGen_Concepts_InterestHolderProperty';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
import { formatMoney } from '@/utils';

import { Api_GenerateCompReqFileLease } from '../CompensationRequisition/GenerateCompReqFileLease';
import { ICompensationRequisitionFile } from '../CompensationRequisition/ICompensationRequisitionFile';
import { Api_GenerateProject } from '../GenerateProject';
import { Api_GenerateAcquisitionFile } from './GenerateAcquisitionFile';
import { Api_GenerateCompensationFinancial } from './GenerateCompensationFinancial';
import { Api_GenerateCompensationFinancialSummary } from './GenerateCompensationFinancialSummary';
import { Api_GenerateCompensationPayee } from './GenerateCompensationPayee';
import { Api_GenerateH120InterestHolder } from './GenerateH120InterestHolder';
import { Api_GenerateH120Property } from './GenerateH120Property';

export class Api_GenerateCompensation {
  file: ICompensationRequisitionFile | null;
  status: string;
  generated_date: string;
  requisition_number: string;
  special_instructions: string;
  detailed_remarks: string;
  financial_activities: Api_GenerateCompensationFinancial[];
  summary_financial_activities: Api_GenerateCompensationFinancialSummary[];
  file_financial_pretax_total: string;
  financial_pretax_total: string;
  yearly_financial: string;
  service_line: string;
  responsibility_center: string;
  client: string;
  payee: Api_GenerateCompensationPayee;
  alternate_project: Api_GenerateProject | null;
  properties: Api_GenerateH120Property[];

  constructor(
    compensation: ApiGen_Concepts_CompensationRequisition | null,
    compReqProperties: ApiGen_Concepts_AcquisitionFileProperty[] | ApiGen_Concepts_PropertyLease[],
    file: ICompensationRequisitionFile | null,
    h120Categories: ApiGen_Concepts_H120Category[],
    finalFileFinancials: ApiGen_Concepts_CompensationFinancial[],
    client = '',
  ) {
    const allInterestHoldersPropertes = (
      file as Api_GenerateAcquisitionFile
    )?.interestHolders?.flatMap(ih => ih?.interestHolderProperties ?? []);

    if (file instanceof Api_GenerateAcquisitionFile) {
      this.properties =
        compReqProperties.map(cp => {
          const matchingInterestHolderProperties =
            allInterestHoldersPropertes.filter(
              ihp =>
                ihp.acquisitionFilePropertyId === cp?.id &&
                ihp.propertyInterestTypes?.some(pit => pit.id !== 'NIP'),
            ) ?? [];

          const interestHoldersForProperty = matchingInterestHolderProperties.flatMap(
            (mihp: ApiGen_Concepts_InterestHolderProperty) =>
              mihp.propertyInterestTypes?.map(
                pit =>
                  new Api_GenerateH120InterestHolder(
                    file.interestHolders.find(
                      ih => ih.interestHolderId === mihp.interestHolderId,
                    ) ?? null,
                    mihp,
                    pit,
                  ),
              ) || [],
          );

          return new Api_GenerateH120Property(cp?.property, interestHoldersForProperty);
        }) ?? [];
    } else if (file instanceof Api_GenerateCompReqFileLease) {
      this.properties = compReqProperties?.map(fp => {
        return new Api_GenerateH120Property(fp.property, []);
      });
    }

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
      .filter(summary => summary.file_pretax_total !== '$0.00' || summary.pretax_total !== '$0.00')
      .value();

    this.file_financial_pretax_total = formatMoney(
      finalFileFinancials?.reduce((acc, curr) => acc + (curr?.pretaxAmount ?? 0), 0) ?? 0,
    );
    this.financial_pretax_total = formatMoney(
      compensation?.financials?.reduce((acc, curr) => acc + (curr?.pretaxAmount ?? 0), 0) ?? 0,
    );
    this.yearly_financial = compensation?.yearlyFinancial?.code ?? '';
    this.service_line = compensation?.chartOfAccounts?.code ?? '';
    this.responsibility_center = compensation?.responsibility?.code ?? '';
    this.client = client;
    this.payee = new Api_GenerateCompensationPayee(compensation, compensation?.financials ?? []);
    this.alternate_project = new Api_GenerateProject(compensation?.alternateProject ?? null);
  }
}
