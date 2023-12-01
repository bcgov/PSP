import ITypeCode from '@/interfaces/ITypeCode';

import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { DateOnly } from './DateOnly';

export interface Api_Insurance extends Api_ConcurrentVersion {
  id: number | null;
  leaseId: number;
  insuranceType: ITypeCode<string>;
  otherInsuranceType: string | null;
  coverageDescription: string | null;
  coverageLimit: number | null;
  expiryDate: DateOnly | null;
  isInsuranceInPlace: boolean | null;
}
