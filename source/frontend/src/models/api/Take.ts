import { Api_AcquisitionFile } from './AcquisitionFile';
import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

// LINK @backend/api/Models/Concepts/Take/TakeModel.cs
export interface Api_Take extends Api_ConcurrentVersion, Api_AuditFields {
  id: number;
  description: string;
  newHighwayDedicationArea: number | null;
  areaUnitTypeCode: Api_TypeCode<string> | null;
  isAcquiredForInventory: boolean | null;
  isThereSurplus: boolean | null;
  isNewLicenseToConstruct: boolean | null;
  isNewHighwayDedication: boolean | null;
  isNewLandAct: boolean | null;
  isNewInterestInSrw: boolean | null;
  licenseToConstructArea: number | null;
  ltcEndDt: string | null;
  landActArea: number | null;
  landActEndDt: string | null;
  propertyAcquisitionFile: Api_AcquisitionFile | null;
  propertyAcquisitionFileId: number;
  statutoryRightOfWayArea: number | null;
  srwEndDt: string | null;
  surplusArea: number | null;
  takeSiteContamTypeCode: Api_TypeCode<string> | null;
  takeTypeCode: Api_TypeCode<string> | null;
  takeStatusTypeCode: Api_TypeCode<string> | null;
  landActTypeCode: Api_TypeCode<string> | null;
}
