import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_Take extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  description: string;
  areaUnitTypeCode: string;
  isSurplusSeverance: boolean;
  isSection16: boolean;
  isStatutoryRightOfWay: boolean;
  isNewRightOfWay: boolean;
  isLicenseToConstruct: boolean;
  licenseToConstructArea: number | null;
  ltcEndDt: string | null;
  newRightOfWayArea: number | null;
  section16Area: number | null;
  section16EndDt: string | null;
  srwEndDt: string | null;
  statutoryRightOfWayArea: number | null;
  surplusSeveranceArea: number | null;
  propertyAcquisitionFileId: number | null;
  takeSiteContamTypeCode: string | null;
  takeTypeCode: string | null;
  takeStatusTypeCode: string | null;
}
