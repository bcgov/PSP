import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Lease } from './Lease';
import Api_TypeCode from './TypeCode';

export interface Api_PropertyImprovement extends Api_ConcurrentVersion {
  id: number | null;
  leaseId: number | null;
  lease: Api_Lease | null;
  address: string | null;
  structureSize: string | null;
  improvementDescription: string | null;
  propertyImprovementTypeCode: Api_TypeCode<string> | null;
}
