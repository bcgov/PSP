import { AreaUnitTypes } from '@/constants';
import { convertArea, exists, firstValidOrNull, isValidString, pidFormatter } from '@/utils';

import { ApiGen_Concepts_Geometry } from '../api/generated/ApiGen_Concepts_Geometry';
import { ApiGen_Concepts_Property } from '../api/generated/ApiGen_Concepts_Property';
import { PMBC_FullyAttributed_Feature_Properties } from '../layers/parcelMapBC';
import { Api_GenerateAddress } from './GenerateAddress';
export class Api_GenerateProperty {
  location: ApiGen_Concepts_Geometry | null;
  pid: string;
  legal_desc: string;
  region: string;
  address: Api_GenerateAddress | null;
  location_of_land: string;
  district: string;
  electoral_dist: string;
  area_sqm: number;
  plan_number: string;

  constructor(
    pimsProperty: ApiGen_Concepts_Property | null | undefined,
    fullyAttributedProperty?: PMBC_FullyAttributed_Feature_Properties | null,
  ) {
    this.location = exists(pimsProperty?.location) ? pimsProperty.location : null;
    this.pid = pidFormatter(pimsProperty?.pid?.toString()) ?? '';
    this.legal_desc =
      firstValidOrNull(
        [fullyAttributedProperty?.LEGAL_DESCRIPTION, pimsProperty?.landLegalDescription],
        isValidString,
      ) ?? '';
    this.address = exists(pimsProperty?.address)
      ? new Api_GenerateAddress(pimsProperty.address)
      : null;
    this.region = pimsProperty?.region?.description ?? '';
    this.district = pimsProperty?.district?.description ?? '';
    this.electoral_dist = '';
    this.location_of_land = pimsProperty?.generalLocation ?? '';
    this.plan_number =
      firstValidOrNull(
        [fullyAttributedProperty?.PLAN_NUMBER, pimsProperty?.planNumber],
        isValidString,
      ) ?? '';

    const fa_area_sqm = fullyAttributedProperty?.FEATURE_AREA_SQM;
    const pims_area_sqm = convertArea(
      pimsProperty?.landArea || 0,
      pimsProperty?.areaUnit?.id || '',
      AreaUnitTypes.SquareMeters,
    );

    this.area_sqm = firstValidOrNull([fa_area_sqm, pims_area_sqm], exists) ?? 0;
  }
}
