import {
  Api_GenerateExpropriationFormBase,
  IApiGenerateExpropriationFormBaseInput,
} from './GenerateExpropriationFormBase';

export type IApiGenerateExpropriationForm7Input = IApiGenerateExpropriationFormBaseInput;

export class Api_GenerateExpropriationForm7 extends Api_GenerateExpropriationFormBase {
  constructor({
    file,
    interestHolders = [],
    expropriationAuthority,
    impactedProperties = [],
  }: IApiGenerateExpropriationForm7Input) {
    super({
      file,
      interestHolders,
      expropriationAuthority,
      impactedProperties,
    });
  }
}
