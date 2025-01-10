import {
  Api_GenerateExpropriationFormBase,
  IApiGenerateExpropriationFormBaseInput,
} from './GenerateExpropriationFormBase';

export interface IApiGenerateExpropriationForm1Input
  extends IApiGenerateExpropriationFormBaseInput {
  landInterest?: string;
  purpose?: string;
  expropriationNoticeServedDate?: string;
}

export class Api_GenerateExpropriationForm1 extends Api_GenerateExpropriationFormBase {
  land_interest: string;
  purpose: string;
  notice_served_date: string;

  constructor({
    file,
    interestHolders = [],
    expropriationAuthority,
    impactedProperties = [],
    landInterest,
    purpose,
    expropriationNoticeServedDate,
  }: IApiGenerateExpropriationForm1Input) {
    super({
      file,
      interestHolders,
      expropriationAuthority,
      impactedProperties,
    });
    this.land_interest = landInterest ?? '';
    this.purpose = purpose ?? '';
    this.notice_served_date = expropriationNoticeServedDate ?? '';
  }
}
