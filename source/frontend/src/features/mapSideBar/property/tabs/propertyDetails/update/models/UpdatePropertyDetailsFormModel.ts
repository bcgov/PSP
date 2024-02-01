import { GeoJsonProperties } from 'geojson';
import isEmpty from 'lodash/isEmpty';

import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { ApiGen_Concepts_CodeType } from '@/models/api/generated/ApiGen_Concepts_CodeType';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import {
  booleanToString,
  fromTypeCode,
  stringToBoolean,
  stringToNull,
  toTypeCodeNullable,
} from '@/utils/formUtils';
import { exists } from '@/utils/utils';

import { PropertyAnomalyFormModel, PropertyRoadFormModel, PropertyTenureFormModel } from '.';

export class AddressFormModel {
  id?: number;
  rowVersion?: number;
  streetAddress1?: string;
  streetAddress2?: string;
  streetAddress3?: string;
  municipality?: string;
  postal?: string;
  province?: ApiGen_Concepts_CodeType;
  country?: ApiGen_Concepts_CodeType;

  static fromApi(apiAddress: ApiGen_Concepts_Address): AddressFormModel {
    const model = new AddressFormModel();
    model.id = apiAddress.id ?? undefined;
    model.rowVersion = apiAddress.rowVersion ?? undefined;
    model.streetAddress1 = apiAddress.streetAddress1 ?? undefined;
    model.streetAddress2 = apiAddress.streetAddress2 ?? undefined;
    model.streetAddress3 = apiAddress.streetAddress3 ?? undefined;
    model.municipality = apiAddress.municipality ?? undefined;
    model.postal = apiAddress.postal ?? undefined;
    model.province = apiAddress.province ?? undefined;
    model.country = apiAddress.country ?? undefined;

    return model;
  }

  toApi(): ApiGen_Concepts_Address | null {
    // Only submit valid addresses to the backend
    if (!this.isValid()) {
      return null;
    }

    return {
      id: this.id ?? null,
      rowVersion: this.rowVersion ?? null,
      streetAddress1: this.streetAddress1 ?? null,
      streetAddress2: this.streetAddress2 ?? null,
      streetAddress3: this.streetAddress3 ?? null,
      municipality: this.municipality ?? null,
      postal: this.postal ?? null,
      province: this.province ?? null,
      country: this.country ?? null,
      comment: null,
      countryId: null,
      countryOther: null,
      district: null,
      latitude: null,
      longitude: null,
      provinceStateId: null,
      region: null,
    };
  }

  private isValid(): boolean {
    if (isEmpty(this.streetAddress1) && isEmpty(this.municipality) && isEmpty(this.postal)) {
      return false;
    }
    return true;
  }
}

export class UpdatePropertyDetailsFormModel {
  id?: number;
  rowVersion?: number;
  pid?: number;
  pin?: number;
  planNumber?: string;
  zoning?: string;
  zoningPotential?: string;
  municipalZoning?: string;
  notes?: string;

  name?: string;
  description?: string;
  isSensitive?: boolean;
  pphStatusTypeCode?: string;
  pphStatusUpdateUserid?: string;
  pphStatusUpdateUserGuid?: string;
  pphStatusUpdateTimestamp?: Date;
  isRwyBeltDomPatent?: boolean;

  latitude?: number;
  longitude?: number;

  address?: AddressFormModel;
  generalLocation?: string;

  landArea?: number;
  landLegalDescription?: string;
  areaUnitTypeCode?: string;

  isVolumetricParcel?: string; // radio buttons only support string values, not booleans
  volumetricMeasurement?: number;
  volumetricUnitTypeCode?: string;
  volumetricParcelTypeCode?: string;

  propertyTypeCode?: string;
  statusTypeCode?: string;

  regionTypeCode?: number;
  regionTypeCodeDescription?: string;

  districtTypeCode?: number;
  districtTypeCodeDescription?: string;

  // multi-selects
  anomalies?: PropertyAnomalyFormModel[];
  tenures?: PropertyTenureFormModel[];
  roadTypes?: PropertyRoadFormModel[];

  // map layer metadata for this property location (lat,lng)
  isALR?: boolean;
  motiRegion?: GeoJsonProperties;
  highwaysDistrict?: GeoJsonProperties;
  electoralDistrict?: GeoJsonProperties;
  firstNations?: GeoJsonProperties;

  static fromApi(base: ApiGen_Concepts_Property): UpdatePropertyDetailsFormModel {
    const model = new UpdatePropertyDetailsFormModel();
    model.id = base.id;
    model.rowVersion = base.rowVersion ?? undefined;
    model.pid = base.pid ?? undefined;
    model.pin = base.pin ?? undefined;
    model.planNumber = base.planNumber ?? undefined;
    model.zoning = base.zoning ?? undefined;
    model.zoningPotential = base.zoningPotential ?? undefined;
    model.municipalZoning = base.municipalZoning ?? undefined;
    model.notes = base.notes ?? undefined;

    model.name = base.name ?? undefined;
    model.description = base.description ?? undefined;
    model.isSensitive = base.isSensitive;
    model.pphStatusTypeCode = base.pphStatusTypeCode ?? 'UNKNOWN';
    model.isRwyBeltDomPatent = base.isRwyBeltDomPatent ?? undefined;
    model.pphStatusUpdateUserid = base.pphStatusUpdateUserid ?? undefined;
    model.pphStatusUpdateUserGuid = base.pphStatusUpdateUserGuid ?? undefined;
    model.pphStatusUpdateTimestamp = exists(base.pphStatusUpdateTimestamp)
      ? new Date(base.pphStatusUpdateTimestamp)
      : undefined;

    model.latitude = base.latitude ?? undefined;
    model.longitude = base.longitude ?? undefined;

    model.address = exists(base.address)
      ? AddressFormModel.fromApi(base.address)
      : new AddressFormModel();

    model.generalLocation = base.generalLocation ?? undefined;

    model.landLegalDescription = base.landLegalDescription ?? undefined;

    model.landArea = base.landArea ?? undefined;
    model.areaUnitTypeCode = fromTypeCode(base.areaUnit) ?? undefined;

    model.isVolumetricParcel = booleanToString(base.isVolumetricParcel);
    model.volumetricMeasurement = base.volumetricMeasurement ?? 0;
    model.volumetricUnitTypeCode = fromTypeCode(base.volumetricUnit) ?? undefined;
    model.volumetricParcelTypeCode = fromTypeCode(base.volumetricType) ?? undefined;

    model.propertyTypeCode = fromTypeCode(base.propertyType) ?? undefined;
    model.statusTypeCode = fromTypeCode(base.status) ?? undefined;

    model.districtTypeCode = fromTypeCode<number>(base.district) ?? undefined;
    model.districtTypeCodeDescription = base.district?.description ?? undefined;

    model.regionTypeCode = fromTypeCode(base.region) ?? undefined;
    model.regionTypeCodeDescription = base.region?.description ?? undefined;

    // multi-selects
    model.anomalies = base.anomalies?.map(e => PropertyAnomalyFormModel.fromApi(e));
    model.tenures = base.tenures?.map(e => PropertyTenureFormModel.fromApi(e));
    model.roadTypes = base.roadTypes?.map(e => PropertyRoadFormModel.fromApi(e));

    return model;
  }

  toApi(): ApiGen_Concepts_Property {
    return {
      id: this.id ?? 0,
      rowVersion: this.rowVersion ?? null,
      pid: this.pid ?? null,
      pin: this.pin ?? null,
      planNumber: this.planNumber ?? null,
      zoning: this.zoning ?? null,
      zoningPotential: this.zoningPotential ?? null,
      municipalZoning: this.municipalZoning ?? null,
      notes: this.notes ?? null,
      name: this.name ?? null,
      description: this.description ?? null,
      isSensitive: this.isSensitive ?? false,
      pphStatusTypeCode: this.pphStatusTypeCode ?? null,
      isRwyBeltDomPatent: this.isRwyBeltDomPatent ?? null,
      latitude: this.latitude ?? null,
      longitude: this.longitude ?? null,
      location: { coordinate: { x: this.longitude ?? 0, y: this.latitude ?? 0 } },
      landLegalDescription: this.landLegalDescription ?? null,
      landArea: this.landArea ?? null,
      areaUnit: toTypeCodeNullable(this.areaUnitTypeCode),
      isVolumetricParcel: stringToBoolean(this.isVolumetricParcel as string),
      volumetricMeasurement: this.volumetricMeasurement ?? null,
      volumetricUnit: toTypeCodeNullable(this.volumetricUnitTypeCode),
      volumetricType: toTypeCodeNullable(this.volumetricParcelTypeCode),
      propertyType: toTypeCodeNullable(this.propertyTypeCode),
      status: toTypeCodeNullable(this.statusTypeCode),
      district: toTypeCodeNullable(this.districtTypeCode),
      region: toTypeCodeNullable(this.regionTypeCode),
      address: exists(this.address) ? this.address.toApi() : null,
      generalLocation: stringToNull(this.generalLocation),
      // multi-selects
      anomalies: this.anomalies?.map(e => e.toApi()) ?? null,
      tenures: this.tenures?.map(e => e.toApi()) ?? null,
      roadTypes: this.roadTypes?.map(e => e.toApi()) ?? null,

      boundary: null,
      dataSource: null,
      dataSourceEffectiveDateOnly: EpochIsoDateTime,
      isProvincialPublicHwy: null,
      pphStatusUpdateUserid: null,
      pphStatusUpdateTimestamp: null,
      pphStatusUpdateUserGuid: null,
      isOwned: false,
      isPropertyOfInterest: false,
      isVisibleToOtherAgencies: false,
      propertyContacts: null,
      surplusDeclarationType: null,
      surplusDeclarationComment: null,
      surplusDeclarationDate: EpochIsoDateTime,
    };
  }
}
