import {
  AreaUnitTypes,
  PropertyStatusTypes,
  PropertyTypes,
  VolumetricParcelTypes,
  VolumeUnitTypes,
} from 'constants/index';
import { GeoJsonProperties } from 'geojson';
import { Api_Address } from 'models/api/Address';
import { Api_Property } from 'models/api/Property';
import Api_TypeCode from 'models/api/TypeCode';
import { booleanToString, stringToBoolean } from 'utils/formUtils';

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

  // multi-selects
  anomalies?: Api_TypeCode<string>[];
  tenure?: Api_TypeCode<string>[];
  roadType?: Api_TypeCode<string>[];
  adjacentLand?: Api_TypeCode<string>[];

  motiRegion?: GeoJsonProperties;
  highwaysDistrict?: GeoJsonProperties;
  electoralDistrict?: GeoJsonProperties;
  isALR?: boolean;
  firstNations?: {
    bandName?: string;
    reserveName?: string;
  };

  // TODO: finish off conversion of complex mappings
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
    model.latitude = base.latitude;
    model.longitude = base.longitude;
    model.landLegalDescription = base.landLegalDescription;
    model.landArea = base.landArea;
    model.areaUnitTypeCode = base.areaUnit?.id;

    model.isVolumetricParcel = booleanToString(base.isVolumetricParcel);
    model.volumetricMeasurement = base.volumetricMeasurement;
    model.volumetricUnitTypeCode = base.volumetricUnit?.id;
    model.volumetricParcelTypeCode = base.volumetricType?.id;

    // model.propertyTypeCode = base.propertyTypeCode;
    // model.statusTypeCode = base.statusTypeCode;

    // multi-selects
    // model.anomalies = base.anomalies;
    // model.tenure = base.tenure;
    // model.roadType = base.roadType;
    // model.adjacentLand = base.adjacentLand;

    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;
    // model. = base.;

    return model;
  }

  // TODO: finish off conversion of complex mappings
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
      latitude: this.latitude,
      longitude: this.longitude,
      landArea: this.landArea,
      landLegalDescription: this.landLegalDescription,
      isVolumetricParcel: stringToBoolean(this.isVolumetricParcel as string),
      volumetricMeasurement: this.volumetricMeasurement,
    };
  }
}
