import { GeoJsonProperties } from 'geojson';
import { isEmpty } from 'lodash';

import { ApiGen_CodeTypes_PropertyPPHStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyPPHStatusTypes';
import { ApiGen_CodeTypes_PropertyTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyTypes';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { ApiGen_Concepts_CodeType } from '@/models/api/generated/ApiGen_Concepts_CodeType';
import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import {
  booleanToString,
  emptyStringtoNullable,
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
      regionCode: null,
      districtCode: null,
    };
  }

  private isValid(): boolean {
    if (isEmpty(this.streetAddress1) && isEmpty(this.municipality) && isEmpty(this.postal)) {
      return false;
    }
    return true;
  }
}

export class HistoricalNumberForm {
  id: number | null = null;
  propertyId: number;
  historicalNumber = '';
  historicalNumberType = '';
  otherHistoricalNumberType = '';
  isDisabled = false;
  rowVersion: number | null = null;

  static fromApi(base: ApiGen_Concepts_HistoricalFileNumber): HistoricalNumberForm {
    const historicalNumberForm = new HistoricalNumberForm();
    historicalNumberForm.id = base.id;
    historicalNumberForm.propertyId = base.propertyId;
    historicalNumberForm.historicalNumber = base.historicalFileNumber ?? '';
    historicalNumberForm.historicalNumberType =
      fromTypeCode(base.historicalFileNumberTypeCode) ?? '';
    historicalNumberForm.otherHistoricalNumberType = base.otherHistFileNumberTypeCode ?? '';
    historicalNumberForm.isDisabled = base.isDisabled ?? false;
    historicalNumberForm.rowVersion = base.rowVersion ?? null;

    return historicalNumberForm;
  }

  toApi(): ApiGen_Concepts_HistoricalFileNumber {
    return {
      id: this.id ?? 0,
      propertyId: this.propertyId,
      property: null,
      historicalFileNumber: emptyStringtoNullable(this.historicalNumber),
      historicalFileNumberTypeCode: toTypeCodeNullable(this.historicalNumberType),
      otherHistFileNumberTypeCode: emptyStringtoNullable(this.otherHistoricalNumberType),
      isDisabled: this.isDisabled,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  isEmpty(): boolean {
    return this.historicalNumber.trim() === '' && this.historicalNumberType.trim() === '';
  }
}

export class UpdatePropertyDetailsFormModel {
  id?: number;
  rowVersion?: number;
  pid?: number;
  pin?: number;
  planNumber?: string;
  municipalZoning?: string;
  notes?: string;

  isOwned: boolean;

  isRetired?: boolean;
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

  // historical numbers
  historicalNumbers: HistoricalNumberForm[] = [];

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
    model.municipalZoning = base.municipalZoning ?? undefined;
    model.notes = base.notes ?? undefined;
    model.isRetired = base.isRetired;
    model.pphStatusTypeCode =
      base.pphStatusTypeCode ?? ApiGen_CodeTypes_PropertyPPHStatusTypes.UNKNOWN.toString();
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

    model.propertyTypeCode =
      exists(base.propertyType) && !base.propertyType.isDisabled
        ? fromTypeCode(base.propertyType)
        : ApiGen_CodeTypes_PropertyTypes.UNKNOWN.toString();

    model.statusTypeCode = fromTypeCode(base.status) ?? undefined;
    model.districtTypeCode = fromTypeCode<number>(base.district) ?? undefined;
    model.districtTypeCodeDescription = base.district?.description ?? undefined;

    model.regionTypeCode = fromTypeCode(base.region) ?? undefined;
    model.regionTypeCodeDescription = base.region?.description ?? undefined;

    model.isOwned = base.isOwned;

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
      municipalZoning: this.municipalZoning ?? null,
      notes: this.notes ?? null,
      isRetired: this.isRetired ?? false,
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
      isOwned: this.isOwned,
      // multi-selects
      anomalies: this.anomalies?.map(e => e.toApi()) ?? null,
      tenures: this.tenures?.map(e => e.toApi()) ?? null,
      roadTypes: this.roadTypes?.map(e => e.toApi()) ?? null,

      boundary: null,
      dataSource: null,
      dataSourceEffectiveDateOnly: EpochIsoDateTime,
      pphStatusUpdateUserid: null,
      pphStatusUpdateTimestamp: null,
      pphStatusUpdateUserGuid: null,
      surplusDeclarationType: null,
      surplusDeclarationComment: null,
      historicalFileNumbers: null,
      surplusDeclarationDate: EpochIsoDateTime,
    };
  }
}
