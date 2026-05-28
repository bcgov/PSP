import {
  Api_GenerateExpropriationFormBase,
  IApiGenerateExpropriationFormBaseInput,
} from './GenerateExpropriationFormBase';

export type IApiGenerateExpropriationForm4Input = IApiGenerateExpropriationFormBaseInput;

export class Api_GenerateExpropriationForm4 extends Api_GenerateExpropriationFormBase {
  constructor({
    file,
    interestHolders = [],
    expropriationAuthority,
    impactedProperties = [],
  }: IApiGenerateExpropriationForm4Input) {
    super({
      file,
      interestHolders,
      expropriationAuthority,
      impactedProperties,
    });
  }
}
