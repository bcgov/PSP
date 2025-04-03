import {
  Api_GenerateExpropriationFormBase,
  IApiGenerateExpropriationFormBaseInput,
} from './GenerateExpropriationFormBase';

export interface IApiGenerateExpropriationForm9Input
  extends IApiGenerateExpropriationFormBaseInput {
  registeredPlanNumbers?: string;
  expropriationVestingDate?: string;
}

export class Api_GenerateExpropriationForm9 extends Api_GenerateExpropriationFormBase {
  registered_plan_numbers: string;
  vesting_date: string;

  constructor({
    file,
    interestHolders = [],
    expropriationAuthority,
    impactedProperties = [],
    registeredPlanNumbers,
    expropriationVestingDate,
  }: IApiGenerateExpropriationForm9Input) {
    super({
      file,
      interestHolders,
      expropriationAuthority,
      impactedProperties,
    });
    this.registered_plan_numbers = registeredPlanNumbers ?? '';
    this.vesting_date = expropriationVestingDate ?? '';
  }
}
