import { GeoJsonProperties } from 'geojson';
import { Api_Property } from 'models/api/Property';
import { booleanToString, fromTypeCode, stringToBoolean, toTypeCode } from 'utils/formUtils';

import {
  PropertyAdjacentLandFormModel,
  PropertyAnomalyFormModel,
  PropertyRoadFormModel,
  PropertyTenureFormModel,
} from '.';

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
  isProvincialPublicHwy?: boolean;

  latitude?: number;
  longitude?: number;

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
  adjacentLands?: PropertyAdjacentLandFormModel[];

  // map layer metadata for this property location (lat,lng)
  isALR?: boolean;
  motiRegion?: GeoJsonProperties;
  highwaysDistrict?: GeoJsonProperties;
  electoralDistrict?: GeoJsonProperties;
  firstNations?: GeoJsonProperties;

  static fromApi(base: Api_Property): UpdatePropertyDetailsFormModel {
    const model = new UpdatePropertyDetailsFormModel();
    model.id = base.id;
    model.rowVersion = base.rowVersion;
    model.pid = base.pid;
    model.pin = base.pin;
    model.planNumber = base.planNumber;
    model.zoning = base.zoning;
    model.zoningPotential = base.zoningPotential;
    model.municipalZoning = base.municipalZoning;
    model.notes = base.notes;

    model.name = base.name;
    model.description = base.description;
    model.isSensitive = base.isSensitive;
    model.isProvincialPublicHwy = base.isProvincialPublicHwy;

    model.latitude = base.latitude;
    model.longitude = base.longitude;

    model.landLegalDescription = base.landLegalDescription;
    model.landArea = base.landArea;
    model.areaUnitTypeCode = fromTypeCode(base.areaUnit);

    model.isVolumetricParcel = booleanToString(base.isVolumetricParcel);
    model.volumetricMeasurement = base.volumetricMeasurement;
    model.volumetricUnitTypeCode = fromTypeCode(base.volumetricUnit);
    model.volumetricParcelTypeCode = fromTypeCode(base.volumetricType);

    model.propertyTypeCode = fromTypeCode(base.propertyType);
    model.statusTypeCode = fromTypeCode(base.status);

    model.districtTypeCode = fromTypeCode<number>(base.district);
    model.districtTypeCodeDescription = base.district?.description;

    model.regionTypeCode = fromTypeCode(base.region);
    model.regionTypeCodeDescription = base.region?.description;

    // multi-selects
    model.anomalies = base.anomalies?.map(e => PropertyAnomalyFormModel.fromApi(e));
    model.tenures = base.tenures?.map(e => PropertyTenureFormModel.fromApi(e));
    model.roadTypes = base.roadTypes?.map(e => PropertyRoadFormModel.fromApi(e));
    model.adjacentLands = base.adjacentLands?.map(e => PropertyAdjacentLandFormModel.fromApi(e));

    return model;
  }

  toApi(): Api_Property {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      pid: this.pid,
      pin: this.pin,
      planNumber: this.planNumber,
      zoning: this.zoning,
      zoningPotential: this.zoningPotential,
      municipalZoning: this.municipalZoning,
      notes: this.notes,
      name: this.name,
      description: this.description,
      isSensitive: this.isSensitive,
      isProvincialPublicHwy: this.isProvincialPublicHwy,
      latitude: this.latitude,
      longitude: this.longitude,
      location: { coordinate: { x: this.longitude, y: this.latitude } },
      landLegalDescription: this.landLegalDescription,
      landArea: this.landArea,
      areaUnit: toTypeCode(this.areaUnitTypeCode),
      isVolumetricParcel: stringToBoolean(this.isVolumetricParcel as string),
      volumetricMeasurement: this.volumetricMeasurement,
      volumetricUnit: toTypeCode(this.volumetricUnitTypeCode),
      volumetricType: toTypeCode(this.volumetricParcelTypeCode),
      propertyType: toTypeCode(this.propertyTypeCode),
      status: toTypeCode(this.statusTypeCode),
      district: toTypeCode(this.districtTypeCode),
      region: toTypeCode(this.regionTypeCode),
      // multi-selects
      anomalies: this.anomalies?.map(e => e.toApi()),
      tenures: this.tenures?.map(e => e.toApi()),
      roadTypes: this.roadTypes?.map(e => e.toApi()),
      adjacentLands: this.adjacentLands?.map(e => e.toApi()),
    };
  }
}
