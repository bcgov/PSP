import { AreaUnitTypes } from 'constants/areaUnitTypes';
import { Api_Take } from 'models/api/Take';
import { convertArea } from 'utils/convertUtils';
/* eslint-disable no-template-curly-in-string */
import { stringToNull } from 'utils/formUtils';
import * as Yup from 'yup';

export const TakesYupSchema = Yup.object().shape({
  takes: Yup.array().of(
    Yup.object().shape({
      description: Yup.string().max(4000, 'Description must be at most ${max} characters'),
      takeTypeCode: Yup.string().required('Take type is required'),
      takeStatusTypeCode: Yup.string().required('Take status type is required.'),
      isSurplusSeverance: Yup.bool().required('Surplus severance flag required'),
      isNewRightOfWay: Yup.bool().required('Surplus severance flag required'),
      isSection16: Yup.bool().required('Section 16 flag required'),
      isStatutoryRightOfWay: Yup.bool().required('Statutory right of way flag required'),
      isLicenseToConstruct: Yup.bool().required('License to construct flag required'),
      surplusSeveranceArea: Yup.number().when('isSurplusSeverance', {
        is: (isSurplusSeverance: boolean) => isSurplusSeverance,
        then: Yup.number()
          .required('Required when flag is true')
          .moreThan(0, 'Must be greater than 0 when flag is true'),
      }),
      licenseToConstructArea: Yup.number().when('isLicenseToConstruct', {
        is: (isLicenseToConstruct: boolean) => isLicenseToConstruct,
        then: Yup.number()
          .required('Required when flag is true')
          .moreThan(0, 'Must be greater than 0 when flag is true'),
      }),
      newRightOfWayArea: Yup.number().when('isNewRightOfWay', {
        is: (isNewRightOfWay: boolean) => isNewRightOfWay,
        then: Yup.number()
          .required('Rrequired when flag is true')
          .moreThan(0, 'Must be greater than 0 when flag is true'),
      }),
      section16Area: Yup.number().when('isSection16', {
        is: (isSection16: boolean) => isSection16,
        then: Yup.number()
          .required('Required when flag is true')
          .moreThan(0, 'Must be greater than 0 when flag is true'),
      }),
      statutoryRightOfWayArea: Yup.number().when('isStatutoryRightOfWay', {
        is: (isStatutoryRightOfWay: boolean) => isStatutoryRightOfWay,
        then: Yup.number()
          .required('Required when flag is true')
          .moreThan(0, 'Must be greater than 0 when flag is true'),
      }),
    }),
  ),
});

export class TakeModel {
  id?: number;
  description: string;
  isSurplusSeverance: 'false' | 'true';
  isNewRightOfWay: 'false' | 'true';
  isSection16: 'false' | 'true';
  isStatutoryRightOfWay: 'false' | 'true';
  isLicenseToConstruct: 'false' | 'true';
  ltcEndDt: string;
  licenseToConstructArea: number;
  licenseToConstructAreaUnitTypeCode: string;
  newRightOfWayArea: number;
  newRightOfWayAreaUnitTypeCode: string;
  section16Area: number;
  section16AreaUnitTypeCode: string;
  statutoryRightOfWayArea: number;
  statutoryRightOfWayAreaUnitTypeCode: string;
  surplusSeveranceArea: number;
  surplusSeveranceAreaUnitTypeCode: string;
  section16EndDt: string;
  propertyAcquisitionFileId: number | null;
  takeSiteContamTypeCode: string | null;
  takeTypeCode: string | null;
  takeStatusTypeCode: string | null;
  rowVersion?: number;

  constructor(base: Api_Take) {
    this.id = base.id;
    this.rowVersion = base.rowVersion;
    this.description = base.description;
    this.isSurplusSeverance = base.isSurplusSeverance ? 'true' : 'false';
    this.isNewRightOfWay = base.isNewRightOfWay ? 'true' : 'false';
    this.isSection16 = base.isSection16 ? 'true' : 'false';
    this.isLicenseToConstruct = base.isLicenseToConstruct ? 'true' : 'false';
    this.isStatutoryRightOfWay = base.isStatutoryRightOfWay ? 'true' : 'false';
    this.licenseToConstructArea = base.licenseToConstructArea ?? 0;
    this.licenseToConstructAreaUnitTypeCode =
      base.areaUnitTypeCode ?? AreaUnitTypes.SquareMeters.toString();
    this.newRightOfWayArea = base.newRightOfWayArea ?? 0;
    this.newRightOfWayAreaUnitTypeCode =
      base.areaUnitTypeCode ?? AreaUnitTypes.SquareMeters.toString();
    this.section16Area = base.section16Area ?? 0;
    this.section16AreaUnitTypeCode = base.areaUnitTypeCode ?? AreaUnitTypes.SquareMeters.toString();
    this.surplusSeveranceArea = base.surplusSeveranceArea ?? 0;
    this.surplusSeveranceAreaUnitTypeCode =
      base.areaUnitTypeCode ?? AreaUnitTypes.SquareMeters.toString();
    this.statutoryRightOfWayArea = base.statutoryRightOfWayArea ?? 0;
    this.statutoryRightOfWayAreaUnitTypeCode =
      base.areaUnitTypeCode ?? AreaUnitTypes.SquareMeters.toString();
    this.takeTypeCode = base.takeTypeCode;
    this.takeStatusTypeCode = base.takeStatusTypeCode;
    this.takeSiteContamTypeCode = base.takeSiteContamTypeCode;
    this.propertyAcquisitionFileId = base.propertyAcquisitionFileId;
    this.section16EndDt = base.section16EndDt ?? '';
    this.ltcEndDt = base.ltcEndDt ?? '';
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
      section16Area:
        convertArea(
          parseFloat(this.section16Area.toString()),
          this.section16AreaUnitTypeCode,
          AreaUnitTypes.SquareMeters.toString(),
        ) || null,
      surplusSeveranceArea:
        convertArea(
          parseFloat(this.surplusSeveranceArea.toString()),
          this.surplusSeveranceAreaUnitTypeCode,
          AreaUnitTypes.SquareMeters.toString(),
        ) || null,
      statutoryRightOfWayArea:
        convertArea(
          parseFloat(this.statutoryRightOfWayArea.toString()),
          this.statutoryRightOfWayAreaUnitTypeCode,
          AreaUnitTypes.SquareMeters.toString(),
        ) || null,
      ltcEndDt: stringToNull(this.ltcEndDt),
      section16EndDt: stringToNull(this.section16EndDt),
      isSurplusSeverance: this.isSurplusSeverance === 'true',
      isNewRightOfWay: this.isNewRightOfWay === 'true',
      isSection16: this.isSection16 === 'true',
      isLicenseToConstruct: this.isLicenseToConstruct === 'true',
      isStatutoryRightOfWay: this.isStatutoryRightOfWay === 'true',
    };
  }
}
