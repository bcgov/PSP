import {
  Api_GenerateExpropriationFormBase,
  IApiGenerateExpropriationFormBaseInput,
} from './GenerateExpropriationFormBase';

export type IApiGenerateExpropriationForm5Input = IApiGenerateExpropriationFormBaseInput;

export class Api_GenerateExpropriationForm5 extends Api_GenerateExpropriationFormBase {
  constructor({
    file,
    interestHolders = [],
    expropriationAuthority,
    impactedProperties = [],
  }: IApiGenerateExpropriationForm5Input) {
    super({
      file,
      interestHolders,
      expropriationAuthority,
      impactedProperties,
    });
  }
}
