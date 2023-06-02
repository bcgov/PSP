import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_Take extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  description: string;
  areaUnitTypeCode: string;
  isSurplus: boolean;
  isLandAct: boolean;
  isStatutoryRightOfWay: boolean;
  isNewRightOfWay: boolean;
  isLicenseToConstruct: boolean;
  licenseToConstructArea: number | null;
  ltcEndDt: string | null;
  newRightOfWayArea: number | null;
  landActArea: number | null;
  landActEndDt: string | null;
  landActTypeCode: Api_TypeCode<string> | null;
  statutoryRightOfWayArea: number | null;
  surplusArea: number | null;
  propertyAcquisitionFileId: number | null;
  takeSiteContamTypeCode: string | null;
  takeTypeCode: string | null;
  takeStatusTypeCode: string | null;
}
