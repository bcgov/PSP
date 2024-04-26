import * as Yup from 'yup';

import { AreaUnitTypes } from '@/constants/areaUnitTypes';
import { ApiGen_CodeTypes_LandActTypes } from '@/models/api/generated/ApiGen_CodeTypes_LandActTypes';
import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { convertArea } from '@/utils/convertUtils';
import { fromTypeCodeNullable, stringToNull, toTypeCodeNullable } from '@/utils/formUtils';

import { ApiGen_CodeTypes_AcquisitionTakeStatusTypes } from './../../../../../../models/api/generated/ApiGen_CodeTypes_AcquisitionTakeStatusTypes';

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
      landActEndDt: Yup.string().when(['isNewLandAct', 'landActTypeCode'], {
        is: (isNewLandAct: boolean, landActTypeCode: string) =>
          isNewLandAct &&
          ![
            ApiGen_CodeTypes_LandActTypes.TRANSFER_OF_ADMIN_AND_CONTROL.toString(),
            ApiGen_CodeTypes_LandActTypes.CROWN_GRANT.toString(),
          ].includes(landActTypeCode),
        then: Yup.string().required('End Date is required'),
      }),
      landActTypeCode: Yup.string().when('isNewLandAct', {
        is: (isNewLandAct: boolean) => isNewLandAct,
        then: Yup.string().required('Land Act is required'),
      }),
      completionDt: Yup.string()
        .nullable()
        .when('takeStatusTypeCode', {
          is: (takeStatusTypeCode: string) =>
            takeStatusTypeCode === ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE,
          then: Yup.string().nullable().required('A completed take must have a completion date.'),
        }),
    }),
  ),
});

export class TakeModel {
  id?: number;
  completionDt: string | null;
  description: string;
  isThereSurplus: 'false' | 'true';
  isNewHighwayDedication: 'false' | 'true';
  isNewLandAct: 'false' | 'true';
  isNewInterestInSrw: 'false' | 'true';
  isNewLicenseToConstruct: 'false' | 'true';
  isLeasePayable: 'false' | 'true';
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
  leasePayableEndDt: string;
  leasePayableArea: number;
  leasePayableAreaUnitTypeCode: string;
  rowVersion?: number;
  appCreateTimestamp: UtcIsoDateTime | null;

  constructor(base: ApiGen_Concepts_Take) {
    this.id = base.id;
    this.rowVersion = base.rowVersion ?? undefined;
    this.description = base.description ?? '';
    this.isThereSurplus = base.isThereSurplus ? 'true' : 'false';
    this.isNewHighwayDedication = base.isNewHighwayDedication ? 'true' : 'false';
    this.isNewLandAct = base.isNewLandAct ? 'true' : 'false';
    this.isNewLicenseToConstruct = base.isNewLicenseToConstruct ? 'true' : 'false';
    this.isNewInterestInSrw = base.isNewInterestInSrw ? 'true' : 'false';
    this.isLeasePayable = base.isLeasePayable ? 'true' : 'false';
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
    this.leasePayableAreaUnitTypeCode =
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
    this.leasePayableEndDt = base.leasePayableEndDt ?? '';
    this.leasePayableArea = base.leasePayableArea ?? 0;
    this.landActDescription = base.landActTypeCode?.description ?? '';
    this.landActTypeCode = base.landActTypeCode?.id ?? '';

    this.isAcquiredForInventory = base.isAcquiredForInventory ? 'true' : 'false';
    this.newHighwayDedicationArea = base.newHighwayDedicationArea ?? 0;
    this.newHighwayDedicationAreaUnitTypeCode =
      fromTypeCodeNullable(base.areaUnitTypeCode) ?? AreaUnitTypes.SquareMeters.toString();
    this.completionDt = base.completionDt;
    this.appCreateTimestamp = base.appCreateTimestamp ?? null;
  }

  toApi(): ApiGen_Concepts_Take {
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
      srwEndDt: stringToNull(this.srwEndDt),
      ltcEndDt: stringToNull(this.ltcEndDt),
      landActEndDt: stringToNull(this.landActEndDt),
      landActTypeCode: toTypeCodeNullable(this.landActTypeCode),
      isThereSurplus: this.isThereSurplus === 'true',
      isNewLandAct: this.isNewLandAct === 'true',
      isNewLicenseToConstruct: this.isNewLicenseToConstruct === 'true',
      isNewInterestInSrw: this.isNewInterestInSrw === 'true',
      isLeasePayable: this.isLeasePayable === 'true',
      leasePayableArea:
        convertArea(
          parseFloat(this.leasePayableArea.toString()),
          this.leasePayableAreaUnitTypeCode,
          AreaUnitTypes.SquareMeters.toString(),
        ) || null,
      leasePayableEndDt: stringToNull(this.leasePayableEndDt),
      ...getEmptyBaseAudit(this.rowVersion),
      completionDt: stringToNull(this.completionDt),
    };
  }
}
