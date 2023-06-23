import moment from 'moment';

import { Api_Agreement } from '@/models/api/Agreement';
import { formatMoney } from '@/utils';

import { Api_GenerateAcquisitionFile } from './acquisition/GenerateAcquisitionFile';
export class Api_GenerateAgreement {
  file: Api_GenerateAcquisitionFile | null;
  current_year: string;
  date: string;
  completion_date: string;
  termination_date: string;
  commencement_date: string;
  status: string;
  deposit_amount: string;
  purchase_price: string;
  survey_plan_number: string;
  no_later_than_days: string;

  constructor(agreement: Api_Agreement | null, generateFile: Api_GenerateAcquisitionFile | null) {
    this.file = generateFile;
    this.current_year = moment().format('YYYY');
    this.date = agreement?.agreementDate
      ? moment(agreement?.agreementDate).format('MMM DD, YYYY') ?? ''
      : '';
    this.status = agreement?.isDraft ? 'DRAFT' : '';
    this.completion_date = agreement?.completionDate
      ? moment(agreement?.completionDate).format('MMM DD, YYYY') ?? ''
      : '';
    this.termination_date = agreement?.terminationDate
      ? moment(agreement?.terminationDate).format('MMM DD, YYYY') ?? ''
      : '';
    this.commencement_date = agreement?.commencementDate
      ? moment(agreement?.commencementDate).format('MMM DD, YYYY') ?? ''
      : '';
    this.deposit_amount = agreement?.depositAmount ? formatMoney(agreement?.depositAmount) : '';
    this.purchase_price = agreement?.purchasePrice ? formatMoney(agreement?.purchasePrice) : '';
    this.survey_plan_number = agreement?.legalSurveyPlanNum ?? '';
    this.no_later_than_days = agreement?.noLaterThanDays?.toString() ?? '';
  }
}
