import * as Yup from 'yup';

import { AreaUnitTypes } from '@/constants/areaUnitTypes';
import { Api_Take } from '@/models/api/Take';
import { convertArea } from '@/utils/convertUtils';
import { fromTypeCodeNullable, stringToUndefined, toTypeCodeNullable } from '@/utils/formUtils';

/* eslint-disable no-template-curly-in-string */
export const TakesYupSchema = Yup.object().shape({
  takes: Yup.array().of(
    Yup.object().shape({
      description: Yup.string().max(4000, 'Description must be at most ${max} characters'),
      takeTypeCode: Yup.string().required('Take type is required').nullable(),
      takeStatusTypeCode: Yup.string().required('Take status type is required.'),
      isThereSurplus: Yup.bool().required('Surplus flag required'),
      isNewHighwayDedication: Yup.bool().required('New highway dedication flag required'),
      isNewLandAct: Yup.bool().required('Section 16 flag required'),
      isNewInterestInSrw: Yup.bool().required('Statutory right of way (SRW) flag required'),
      isNewLicenseToConstruct: Yup.bool().required('License to construct flag required'),
      ltcEndDt: Yup.string().when('isNewLicenseToConstruct', {
        is: (isNewLicenseToConstruct: boolean) => isNewLicenseToConstruct,
        then: Yup.string().required('End Date is required'),
      }),
      landActEndDt: Yup.string().when('isNewLandAct', {
        is: (isNewLandAct: boolean) => isNewLandAct,
        then: Yup.string().required('End Date is required'),
      }),
      landActTypeCode: Yup.string().when('isNewLandAct', {
        is: (isNewLandAct: boolean) => isNewLandAct,
        then: Yup.string().required('Land Act is required'),
      }),
    }),
  ),
});

export class TakeModel {
  id?: number;
  description: string;
  isThereSurplus: 'false' | 'true';
  isNewHighwayDedication: 'false' | 'true';
  isNewLandAct: 'false' | 'true';
  isNewInterestInSrw: 'false' | 'true';
  isNewLicenseToConstruct: 'false' | 'true';
  ltcEndDt: string;
  licenseToConstructArea: number;
  licenseToConstructAreaUnitTypeCode: string;
  landActArea: number;
  landActAreaUnitTypeCode: string;
  landActEndDt: string;
  landActDescription: string | null;
  landActTypeCode: string | null;
  statutoryRightOfWayArea: number;
  statutoryRightOfWayAreaUnitTypeCode: string;
  srwEndDt: string;
  surplusArea: number;
  surplusAreaUnitTypeCode: string;
  propertyAcquisitionFileId: number | null;
  takeSiteContamTypeCode: string | null = 'UNK';
  takeTypeCode: string | null;
  takeStatusTypeCode: string | null;
  isAcquiredForInventory: 'false' | 'true';
  newHighwayDedicationArea: number;
  newHighwayDedicationAreaUnitTypeCode: string;
  rowVersion?: number;

  constructor(base: Api_Take) {
    this.id = base.id;
    this.rowVersion = base.rowVersion;
    this.description = base.description;
    this.isThereSurplus = base.isThereSurplus ? 'true' : 'false';
    this.isNewHighwayDedication = base.isNewHighwayDedication ? 'true' : 'false';
    this.isNewLandAct = base.isNewLandAct ? 'true' : 'false';
    this.isNewLicenseToConstruct = base.isNewLicenseToConstruct ? 'true' : 'false';
    this.isNewInterestInSrw = base.isNewInterestInSrw ? 'true' : 'false';
    this.licenseToConstructArea = base.licenseToConstructArea ?? 0;
    this.licenseToConstructAreaUnitTypeCode =
      fromTypeCodeNullable(base.areaUnitTypeCode) ?? AreaUnitTypes.SquareMeters.toString();
    this.landActArea = base.landActArea ?? 0;
    this.landActAreaUnitTypeCode =
      fromTypeCodeNullable(base.areaUnitTypeCode) ?? AreaUnitTypes.SquareMeters.toString();
    this.surplusArea = base.surplusArea ?? 0;
    this.surplusAreaUnitTypeCode =
      fromTypeCodeNullable(base.areaUnitTypeCode) ?? AreaUnitTypes.SquareMeters.toString();
    this.statutoryRightOfWayArea = base.statutoryRightOfWayArea ?? 0;
    this.statutoryRightOfWayAreaUnitTypeCode =
      fromTypeCodeNullable(base.areaUnitTypeCode) ?? AreaUnitTypes.SquareMeters.toString();
    this.takeTypeCode = fromTypeCodeNullable(base.takeTypeCode);
    this.takeStatusTypeCode = fromTypeCodeNullable(base.takeStatusTypeCode);
    this.takeSiteContamTypeCode = base.takeSiteContamTypeCode
      ? fromTypeCodeNullable(base.takeSiteContamTypeCode)
      : 'UNK';
    this.propertyAcquisitionFileId = base.propertyAcquisitionFileId;
    this.landActEndDt = base.landActEndDt ?? '';
    this.ltcEndDt = base.ltcEndDt ?? '';
    this.srwEndDt = base.srwEndDt ?? '';
    this.landActDescription = base.landActTypeCode?.description ?? '';
    this.landActTypeCode = base.landActTypeCode?.id ?? '';

    this.isAcquiredForInventory = base.isAcquiredForInventory ? 'true' : 'false';
    this.newHighwayDedicationArea = base.newHighwayDedicationArea ?? 0;
    this.newHighwayDedicationAreaUnitTypeCode =
      fromTypeCodeNullable(base.areaUnitTypeCode) ?? AreaUnitTypes.SquareMeters.toString();
  }

  toApi(): Api_Take {
    return {
      id: this.id || 0,
      propertyAcquisitionFileId: this.propertyAcquisitionFileId || 0,
      propertyAcquisitionFile: null,
      description: this.description,
      isNewHighwayDedication: this.isNewHighwayDedication === 'true',
      newHighwayDedicationArea:
        convertArea(
          parseFloat(this.newHighwayDedicationArea.toString()),
          this.newHighwayDedicationAreaUnitTypeCode,
          AreaUnitTypes.SquareMeters.toString(),
        ) || null,
      isAcquiredForInventory: this.isAcquiredForInventory === 'true',
      takeSiteContamTypeCode: toTypeCodeNullable(this.takeSiteContamTypeCode),
      areaUnitTypeCode: toTypeCodeNullable(AreaUnitTypes.SquareMeters.toString()),
      takeTypeCode: toTypeCodeNullable(this.takeTypeCode),
      takeStatusTypeCode: toTypeCodeNullable(this.takeStatusTypeCode),
      licenseToConstructArea:
        convertArea(
          parseFloat(this.licenseToConstructArea.toString()),
          this.licenseToConstructAreaUnitTypeCode,
          AreaUnitTypes.SquareMeters.toString(),
        ) || null,
      landActArea:
        convertArea(
          parseFloat(this.landActArea.toString()),
          this.landActAreaUnitTypeCode,
          AreaUnitTypes.SquareMeters.toString(),
        ) || null,
      surplusArea:
        convertArea(
          parseFloat(this.surplusArea.toString()),
          this.surplusAreaUnitTypeCode,
          AreaUnitTypes.SquareMeters.toString(),
        ) || null,
      statutoryRightOfWayArea:
        convertArea(
          parseFloat(this.statutoryRightOfWayArea.toString()),
          this.statutoryRightOfWayAreaUnitTypeCode,
          AreaUnitTypes.SquareMeters.toString(),
        ) || null,
      srwEndDt: stringToUndefined(this.srwEndDt),
      ltcEndDt: stringToUndefined(this.ltcEndDt),
      landActEndDt: stringToUndefined(this.landActEndDt),
      landActTypeCode: toTypeCodeNullable(this.landActTypeCode),
      isThereSurplus: this.isThereSurplus === 'true',
      isNewLandAct: this.isNewLandAct === 'true',
      isNewLicenseToConstruct: this.isNewLicenseToConstruct === 'true',
      isNewInterestInSrw: this.isNewInterestInSrw === 'true',
      rowVersion: this.rowVersion,
    };
  }
}
