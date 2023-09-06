import * as Yup from 'yup';

import { AreaUnitTypes } from '@/constants/areaUnitTypes';
import { Api_Take } from '@/models/api/Take';
import { convertArea } from '@/utils/convertUtils';
/* eslint-disable no-template-curly-in-string */
import { stringToUndefined, toTypeCode } from '@/utils/formUtils';

export const TakesYupSchema = Yup.object().shape({
  takes: Yup.array().of(
    Yup.object().shape({
      description: Yup.string().max(4000, 'Description must be at most ${max} characters'),
      takeTypeCode: Yup.string().required('Take type is required'),
      takeStatusTypeCode: Yup.string().required('Take status type is required.'),
      isSurplus: Yup.bool().required('Surplus flag required'),
      isNewRightOfWay: Yup.bool().required('Surplus flag required'),
      isLandAct: Yup.bool().required('Section 16 flag required'),
      isStatutoryRightOfWay: Yup.bool().required('Statutory right of way flag required'),
      isLicenseToConstruct: Yup.bool().required('License to construct flag required'),
      ltcEndDt: Yup.string().when('isLicenseToConstruct', {
        is: (isLicenseToConstruct: boolean) => isLicenseToConstruct,
        then: Yup.string().required('End Date is required'),
      }),
      landActEndDt: Yup.string().when('isLandAct', {
        is: (isLandAct: boolean) => isLandAct,
        then: Yup.string().required('End Date is required'),
      }),
      landActTypeCode: Yup.string().when('isLandAct', {
        is: (isLandAct: boolean) => isLandAct,
        then: Yup.string().required('Land Act is required'),
      }),
    }),
  ),
});

export class TakeModel {
  id?: number;
  description: string;
  isSurplus: 'false' | 'true';
  isNewRightOfWay: 'false' | 'true';
  isLandAct: 'false' | 'true';
  isStatutoryRightOfWay: 'false' | 'true';
  isLicenseToConstruct: 'false' | 'true';
  ltcEndDt: string;
  licenseToConstructArea: number;
  licenseToConstructAreaUnitTypeCode: string;
  newRightOfWayArea: number;
  newRightOfWayAreaUnitTypeCode: string;
  landActArea: number;
  landActAreaUnitTypeCode: string;
  landActEndDt: string;
  landActDescription: string | null;
  landActTypeCode: string | null;
  statutoryRightOfWayArea: number;
  statutoryRightOfWayAreaUnitTypeCode: string;
  surplusArea: number;
  surplusAreaUnitTypeCode: string;
  propertyAcquisitionFileId: number | null;
  takeSiteContamTypeCode: string | null = 'UNK';
  takeTypeCode: string | null;
  takeStatusTypeCode: string | null;
  rowVersion?: number;

  constructor(base: Api_Take) {
    this.id = base.id;
    this.rowVersion = base.rowVersion;
    this.description = base.description;
    this.isSurplus = base.isSurplus ? 'true' : 'false';
    this.isNewRightOfWay = base.isNewRightOfWay ? 'true' : 'false';
    this.isLandAct = base.isLandAct ? 'true' : 'false';
    this.isLicenseToConstruct = base.isLicenseToConstruct ? 'true' : 'false';
    this.isStatutoryRightOfWay = base.isStatutoryRightOfWay ? 'true' : 'false';
    this.licenseToConstructArea = base.licenseToConstructArea ?? 0;
    this.licenseToConstructAreaUnitTypeCode =
      base.areaUnitTypeCode ?? AreaUnitTypes.SquareMeters.toString();
    this.newRightOfWayArea = base.newRightOfWayArea ?? 0;
    this.newRightOfWayAreaUnitTypeCode =
      base.areaUnitTypeCode ?? AreaUnitTypes.SquareMeters.toString();
    this.landActArea = base.landActArea ?? 0;
    this.landActAreaUnitTypeCode = base.areaUnitTypeCode ?? AreaUnitTypes.SquareMeters.toString();
    this.surplusArea = base.surplusArea ?? 0;
    this.surplusAreaUnitTypeCode = base.areaUnitTypeCode ?? AreaUnitTypes.SquareMeters.toString();
    this.statutoryRightOfWayArea = base.statutoryRightOfWayArea ?? 0;
    this.statutoryRightOfWayAreaUnitTypeCode =
      base.areaUnitTypeCode ?? AreaUnitTypes.SquareMeters.toString();
    this.takeTypeCode = base.takeTypeCode;
    this.takeStatusTypeCode = base.takeStatusTypeCode;
    this.takeSiteContamTypeCode = base.takeSiteContamTypeCode ? base.takeSiteContamTypeCode : 'UNK';
    this.propertyAcquisitionFileId = base.propertyAcquisitionFileId;
    this.landActEndDt = base.landActEndDt ?? '';
    this.ltcEndDt = base.ltcEndDt ?? '';
    this.landActDescription = base.landActTypeCode?.description ?? '';
    this.landActTypeCode = base.landActTypeCode?.id ?? '';
  }

  toApi(): Api_Take {
    return {
      ...this,
      takeSiteContamTypeCode: this.takeSiteContamTypeCode || null,
      areaUnitTypeCode: AreaUnitTypes.SquareMeters.toString(),
      takeTypeCode: this.takeTypeCode || null,
      takeStatusTypeCode: this.takeStatusTypeCode || null,
      licenseToConstructArea:
        convertArea(
          parseFloat(this.licenseToConstructArea.toString()),
          this.licenseToConstructAreaUnitTypeCode,
          AreaUnitTypes.SquareMeters.toString(),
        ) || null,
      newRightOfWayArea:
        convertArea(
          parseFloat(this.newRightOfWayArea.toString()),
          this.newRightOfWayAreaUnitTypeCode,
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
      ltcEndDt: stringToUndefined(this.ltcEndDt),
      landActEndDt: stringToUndefined(this.landActEndDt),
      landActTypeCode: toTypeCode(this.landActTypeCode),
      isSurplus: this.isSurplus === 'true',
      isNewRightOfWay: this.isNewRightOfWay === 'true',
      isLandAct: this.isLandAct === 'true',
      isLicenseToConstruct: this.isLicenseToConstruct === 'true',
      isStatutoryRightOfWay: this.isStatutoryRightOfWay === 'true',
    };
  }
}
