import ITypeCode from '@/interfaces/ITypeCode';

import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_Insurance extends Api_ConcurrentVersion {
  id: number | null;
  leaseId: number;
  insuranceType: ITypeCode<string>;
  otherInsuranceType: string | null;
  coverageDescription: string | null;
  coverageLimit: number | null;
  expiryDate: string | null;
  isInsuranceInPlace: boolean | null;
}
