import { IOrganization, IPerson } from 'interfaces';

import ITypeCode from './ITypeCode';

export interface IInsurance {
  id: number;
  insuranceType: ITypeCode<string>;
  insurerOrganization: IOrganization;
  insurerContact: IPerson;
  motiRiskManagementContact: IPerson;
  bctfaRiskManagementContact: IPerson;
  insurancePayeeType: ITypeCode<string>;
  otherInsuranceType: string;
  coverageDescription: string;
  coverageLimit: number;
  insuredValue: number;
  startDate: string;
  expiryDate: string;
  riskAssessmentCompletedDate?: string;
  insuranceInPlace: boolean;
}
