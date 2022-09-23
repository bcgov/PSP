import ITypeCode from './ITypeCode';

export interface IConcurrencyGuard {
  rowVersion: number;
}

export interface IInsurance extends IConcurrencyGuard {
  id: number;
  insuranceType: ITypeCode<string>;
  otherInsuranceType?: string;
  coverageDescription?: string;
  coverageLimit?: number;
  expiryDate?: string;
  isInsuranceInPlace?: boolean;
}
