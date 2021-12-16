import ITypeCode from './ITypeCode';

export interface IInsurance {
  id: number;
  insuranceType: ITypeCode<string>;
  otherInsuranceType: string;
  coverageDescription: string;
  coverageLimit: number;
  expiryDate: string;
  isInsuranceInPlace: boolean;
}
