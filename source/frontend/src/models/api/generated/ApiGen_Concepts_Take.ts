/**
 * File autogenerated by TsGenerator.
 * Do not manually modify, changes made to this file will be lost when this file is regenerated.
 */
import { UtcIsoDate } from '@/models/api/UtcIsoDateTime';

import { ApiGen_Base_BaseAudit } from './ApiGen_Base_BaseAudit';
import { ApiGen_Base_CodeType } from './ApiGen_Base_CodeType';
import { ApiGen_Concepts_AcquisitionFile } from './ApiGen_Concepts_AcquisitionFile';

// LINK: @backend/apimodels/Models/Concepts/Take/TakeModel.cs
export interface ApiGen_Concepts_Take extends ApiGen_Base_BaseAudit {
  id: number;
  description: string | null;
  newHighwayDedicationArea: number | null;
  areaUnitTypeCode: ApiGen_Base_CodeType<string> | null;
  isAcquiredForInventory: boolean | null;
  isThereSurplus: boolean | null;
  isNewLicenseToConstruct: boolean | null;
  isNewHighwayDedication: boolean | null;
  isNewLandAct: boolean | null;
  isNewInterestInSrw: boolean | null;
  isLeasePayable: boolean | null;
  licenseToConstructArea: number | null;
  ltcEndDt: UtcIsoDate | null;
  landActEndDt: UtcIsoDate | null;
  completionDt: UtcIsoDate | null;
  srwEndDt: UtcIsoDate | null;
  leasePayableEndDt: UtcIsoDate | null;
  propertyAcquisitionFile: ApiGen_Concepts_AcquisitionFile | null;
  propertyAcquisitionFileId: number;
  statutoryRightOfWayArea: number | null;
  landActArea: number | null;
  surplusArea: number | null;
  leasePayableArea: number | null;
  takeSiteContamTypeCode: ApiGen_Base_CodeType<string> | null;
  takeTypeCode: ApiGen_Base_CodeType<string> | null;
  takeStatusTypeCode: ApiGen_Base_CodeType<string> | null;
  landActTypeCode: ApiGen_Base_CodeType<string> | null;
}
