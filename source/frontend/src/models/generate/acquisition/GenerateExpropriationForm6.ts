import {
  Api_GenerateExpropriationFormBase,
  IApiGenerateExpropriationFormBaseInput,
} from './GenerateExpropriationFormBase';

export type IApiGenerateExpropriationForm6Input = IApiGenerateExpropriationFormBaseInput;

export class Api_GenerateExpropriationForm6 extends Api_GenerateExpropriationFormBase {
  constructor({
    file,
    interestHolders = [],
    expropriationAuthority,
    impactedProperties = [],
  }: IApiGenerateExpropriationForm6Input) {
    super({
      file,
      interestHolders,
      expropriationAuthority,
      impactedProperties,
    });
  }
}
