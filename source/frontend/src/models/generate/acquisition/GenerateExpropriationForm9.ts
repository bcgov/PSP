import {
  Api_GenerateExpropriationFormBase,
  IApiGenerateExpropriationFormBaseInput,
} from './GenerateExpropriationFormBase';

export interface IApiGenerateExpropriationForm9Input
  extends IApiGenerateExpropriationFormBaseInput {
  registeredPlanNumbers?: string;
}

export class Api_GenerateExpropriationForm9 extends Api_GenerateExpropriationFormBase {
  registered_plan_numbers: string;

  constructor({
    file,
    interestHolders = [],
    expropriationAuthority,
    impactedProperties = [],
    registeredPlanNumbers,
  }: IApiGenerateExpropriationForm9Input) {
    super({
      file,
      interestHolders,
      expropriationAuthority,
      impactedProperties,
    });
    this.registered_plan_numbers = registeredPlanNumbers ?? '';
  }
}
