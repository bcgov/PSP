import ITypeCode from './ITypeCode';
export interface ILeaseTerm {
  id: number;
  leaseId: number;
  statusTypeCode?: ITypeCode<string>;
  startDate?: string;
  expiryDate?: string;
  renewalDate?: string;
}
