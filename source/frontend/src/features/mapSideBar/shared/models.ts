import { MultiPolygon, Polygon } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import { isNumber } from 'lodash';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { IMapProperty } from '@/components/propertySelector/models';
import { AreaUnitTypes, DistrictCodes, RegionCodes } from '@/constants';
import { ApiGen_CodeTypes_GeoJsonTypes } from '@/models/api/generated/ApiGen_CodeTypes_GeoJsonTypes';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import {
  EmptyPropertyLocation,
  PIMS_Property_Location_View,
} from '@/models/layers/pimsPropertyLocationView';
import {
  enumFromValue,
  exists,
  formatApiAddress,
  formatBcaAddress,
  getLatLng,
  isValidId,
  latLngToApiLocation,
  pidFromFeatureSet,
  pidParser,
  pinFromFeatureSet,
} from '@/utils';
import { toTypeCodeNullable } from '@/utils/formUtils';

export class FileForm {
  public id?: number;
  public name: string;
  public properties: PropertyForm[];
  public rowVersion?: number;
  constructor() {
    this.name = '';
    this.properties = [];
  }

  public toApi(): ApiGen_Concepts_File {
    return {
      id: this.id ?? 0,
      fileName: this.name,
      fileProperties: this.properties.map(x => this.toPropertyApi(x)),
      fileNumber: null,
      fileStatusTypeCode: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  private toPropertyApi(x: PropertyForm): ApiGen_Concepts_FileProperty {
    return {
      id: x.id ?? 0,
      fileId: this.id ?? 0,
      property: x.toApi(),
      propertyId: x.apiId ?? 0,
      propertyName: x.name ?? null,
      location: latLngToApiLocation(x.fileLocation?.lat, x.fileLocation?.lng),
      rowVersion: x.rowVersion ?? null,
      displayOrder: null,
      file: null,
    };
  }

  public static fromApi(model: ApiGen_Concepts_File): FileForm {
    const newForm = new FileForm();
    newForm.id = model.id;
    newForm.name = model.fileName || '';
    newForm.properties = model.fileProperties?.map(x => PropertyForm.fromApi(x)) || [];
    newForm.rowVersion = model.rowVersion ?? undefined;

    return newForm;
  }
}

export class PropertyForm {
  public id?: number;
  public fileId?: number;
  public apiId?: number;
  public pid?: string;
  public pin?: string;
  public latitude?: number;
  public longitude?: number;
  public fileLocation?: LatLngLiteral;
  public polygon?: Polygon | MultiPolygon;
  public planNumber?: string;
  public name?: string;
  public region?: number;
  public regionName?: string;
  public district?: number;
  public districtName?: string;
  public rowVersion?: number;
  public propertyRowVersion?: number;
  public legalDescription?: string;
  public formattedAddress?: string;
  public address?: AddressForm;
  public displayOrder?: number;
  public isOwned?: boolean;
  public landArea?: number;
  public areaUnit?: AreaUnitTypes;
  public isRetired?: boolean;

  private constructor(baseModel?: Partial<PropertyForm>) {
    Object.assign(this, baseModel);
  }

  public static fromMapProperty(model: IMapProperty): PropertyForm {
    return new PropertyForm({
      apiId: model.propertyId,
      pid: model.pid,
      pin: isValidId(Number(model.pin)) ? model.pin : undefined,
      latitude: model.latitude,
      longitude: model.longitude,
      fileLocation: model.fileLocation,
      polygon: model.polygon,
      planNumber: model.planNumber,
      region: model.region,
      regionName: model.regionName,
      district: model.district,
      districtName: model.districtName,
      legalDescription: model.legalDescription,
      formattedAddress: model.address,
      landArea: model.landArea,
      areaUnit: model.areaUnit,
    });
  }

  public static fromFeatureDataset(model: LocationFeatureDataset): PropertyForm {
    return new PropertyForm({
      apiId: +(model?.pimsFeature?.properties?.PROPERTY_ID ?? 0),
      pid: pidFromFeatureSet(model),
      pin: pinFromFeatureSet(model),
      latitude: model?.location?.lat,
      longitude: model?.location?.lng,
      fileLocation: model?.fileLocation ?? model?.location ?? undefined,
      planNumber:
        model?.pimsFeature?.properties?.SURVEY_PLAN_NUMBER ??
        model?.parcelFeature?.properties?.PLAN_NUMBER ??
        '',
      polygon:
        model?.parcelFeature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.Polygon
          ? (model?.parcelFeature?.geometry as Polygon)
          : model?.parcelFeature?.geometry?.type === ApiGen_CodeTypes_GeoJsonTypes.MultiPolygon
          ? (model?.parcelFeature?.geometry as MultiPolygon)
          : undefined,
      region: isNumber(model?.regionFeature?.properties?.REGION_NUMBER)
        ? model?.regionFeature?.properties?.REGION_NUMBER
        : RegionCodes.Unknown,
      regionName: model?.regionFeature?.properties?.REGION_NAME ?? 'Cannot determine',
      district: isNumber(model?.districtFeature?.properties?.DISTRICT_NUMBER)
        ? model?.districtFeature?.properties?.DISTRICT_NUMBER
        : DistrictCodes.Unknown,
      districtName: model?.districtFeature?.properties?.DISTRICT_NAME ?? 'Cannot determine',
      formattedAddress: 'unknown',
      landArea: model?.pimsFeature?.properties?.LAND_AREA
        ? +model?.pimsFeature?.properties?.LAND_AREA
        : model?.parcelFeature?.properties?.FEATURE_AREA_SQM ?? 0,
      areaUnit: model?.pimsFeature?.properties?.PROPERTY_AREA_UNIT_TYPE_CODE
        ? enumFromValue(model?.pimsFeature?.properties?.PROPERTY_AREA_UNIT_TYPE_CODE, AreaUnitTypes)
        : AreaUnitTypes.SquareMeters,
      isRetired: model?.pimsFeature?.properties?.IS_RETIRED ?? false,
      legalDescription: model?.pimsFeature?.properties?.LAND_LEGAL_DESCRIPTION ?? '',
    });
  }

  public toMapProperty(): IMapProperty {
    return {
      pid: this.pid,
      pin: isValidId(Number(this.pin)) ? this.pin : null,
      latitude: this.latitude,
      longitude: this.longitude,
      fileLocation: this.fileLocation,
      planNumber: this.planNumber,
      polygon: this.polygon,
      region: this.region,
      regionName: this.regionName,
      district: this.district,
      districtName: this.districtName,
      legalDescription: this.legalDescription,
      address: this.address ? formatApiAddress(this.address.toApi()) : this.formattedAddress,
    };
  }

  public toFeatureDataset(): LocationFeatureDataset {
    return {
      parcelFeature: null,
      selectingComponentId: null,
      pimsFeature: {
        properties: {
          ...EmptyPropertyLocation,
          PROPERTY_ID: this.apiId,
          NAME: this.name,
          PID: this.pid ? +this.pid.replaceAll(/-/g, '') : null,
          PID_PADDED: this?.pid?.padStart(9, '0'),
          PIN: this.pin ? +this.pin : null,
          SURVEY_PLAN_NUMBER: this.planNumber,
          REGION_CODE: this.region,
          DISTRICT_CODE: this.district,
          LAND_AREA: this.landArea,
          PROPERTY_AREA_UNIT_TYPE_CODE: this.areaUnit,
          STREET_ADDRESS_1: this.address?.streetAddress1 ?? this.formattedAddress,
          STREET_ADDRESS_2: this.address?.streetAddress2,
          STREET_ADDRESS_3: this.address?.streetAddress3,
          MUNICIPALITY_NAME: this.address?.municipality,
          POSTAL_CODE: this.address?.postalCode,
          IS_RETIRED: this.isRetired,
          LAND_LEGAL_DESCRIPTION: this.legalDescription,
        },
        type: 'Feature',
        geometry: this.polygon ? this.polygon : null,
      },
      location: { lat: this.latitude, lng: this.longitude },
      fileLocation: this.fileLocation ?? { lat: this.latitude, lng: this.longitude },
      regionFeature: {
        properties: {
          REGION_NAME: this.regionName,
          REGION_NUMBER: this.region,
          FEATURE_AREA_SQM: this.landArea,
          FEATURE_CODE: null,
          OBJECTID: null,
          SE_ANNO_CAD_DATA: null,
          FEATURE_LENGTH_M: null,
        },
        type: 'Feature',
        geometry: null,
      },
      districtFeature: {
        properties: {
          DISTRICT_NAME: this.districtName,
          DISTRICT_NUMBER: this.district,
          FEATURE_AREA_SQM: this.landArea,
          FEATURE_CODE: null,
          OBJECTID: null,
          SE_ANNO_CAD_DATA: null,
          FEATURE_LENGTH_M: null,
        },
        type: 'Feature',
        geometry: null,
      },
      municipalityFeature: null,
    };
  }

  public static fromApi(model: ApiGen_Concepts_FileProperty): PropertyForm {
    const newForm = new PropertyForm();
    newForm.id = model.id;
    newForm.fileId = model.fileId;
    newForm.apiId = model.property?.id ?? model.id;
    newForm.name = model.propertyName ?? undefined;
    newForm.pid = model.property?.pid?.toString();
    newForm.pin = model.property?.pin?.toString();
    newForm.latitude = model.property?.latitude ?? undefined;
    newForm.longitude = model.property?.longitude ?? undefined;
    newForm.fileLocation = getLatLng(model.location) ?? undefined;
    newForm.planNumber = model.property?.planNumber ?? undefined;
    newForm.region = model.property?.region?.id ?? undefined;
    newForm.district = model.property?.district?.id ?? undefined;
    newForm.rowVersion = model.rowVersion ?? undefined;
    newForm.propertyRowVersion = model.property?.rowVersion ?? undefined;
    newForm.displayOrder = model.displayOrder ?? undefined;
    newForm.isOwned = model.property?.isOwned;
    newForm.formattedAddress = exists(model.property?.address)
      ? formatApiAddress(model.property?.address)
      : '';
    newForm.address = model.property?.address
      ? AddressForm.fromApi(model.property?.address)
      : undefined;
    newForm.legalDescription = model.property?.landLegalDescription ?? undefined;
    newForm.isRetired = model.property?.isRetired ?? undefined;

    return newForm;
  }

  public static fromPropertyApi(model: ApiGen_Concepts_Property): PropertyForm {
    const newForm = new PropertyForm();
    newForm.id = model.id;
    newForm.apiId = model?.id;
    newForm.pid = model?.pid?.toString();
    newForm.pin = model?.pin?.toString();
    newForm.latitude = model?.latitude ?? undefined;
    newForm.longitude = model?.longitude ?? undefined;
    newForm.planNumber = model?.planNumber ?? undefined;
    newForm.region = model?.region?.id ?? undefined;
    newForm.district = model?.district?.id ?? undefined;
    newForm.rowVersion = model.rowVersion ?? undefined;
    newForm.propertyRowVersion = model?.rowVersion ?? undefined;
    newForm.isOwned = model?.isOwned;
    newForm.formattedAddress = exists(model?.address) ? formatApiAddress(model?.address) : '';
    newForm.address = model?.address ? AddressForm.fromApi(model?.address) : undefined;
    newForm.legalDescription = model?.landLegalDescription ?? undefined;
    newForm.landArea = model?.landArea ?? undefined;
    newForm.areaUnit = model?.areaUnit
      ? AreaUnitTypes[model?.areaUnit?.id as keyof typeof AreaUnitTypes]
      : undefined;

    return newForm;
  }

  public toApi(): ApiGen_Concepts_Property {
    return {
      id: this.apiId ?? 0,
      pid: pidParser(this.pid) ?? null,
      pin: isValidId(Number(this.pin)) ? Number(this.pin) : null,
      planNumber: this.planNumber ?? null,
      location: latLngToApiLocation(this.latitude, this.longitude),
      boundary: this.polygon ? this.polygon : null,
      region: toTypeCodeNullable(this.region),
      district: toTypeCodeNullable(this.district),
      rowVersion: this.propertyRowVersion ?? null,
      isOwned: this.isOwned ?? false,
      address: this.address?.toApi() ?? null,
      landLegalDescription: this.legalDescription ?? null,
      isRetired: this.isRetired ?? false,
      propertyType: null,
      anomalies: null,
      tenures: null,
      roadTypes: null,
      status: null,
      dataSource: null,
      dataSourceEffectiveDateOnly: EpochIsoDateTime,
      latitude: this.latitude ?? null,
      longitude: this.longitude ?? null,
      name: null,
      description: null,
      isSensitive: false,
      isProvincialPublicHwy: null,
      pphStatusUpdateUserid: null,
      pphStatusUpdateTimestamp: null,
      pphStatusUpdateUserGuid: null,
      isRwyBeltDomPatent: null,
      pphStatusTypeCode: null,
      isVisibleToOtherAgencies: false,
      areaUnit: this.areaUnit
        ? { id: this.areaUnit, description: null, isDisabled: false, displayOrder: 0 }
        : null,
      landArea: this.landArea ?? null,
      isVolumetricParcel: null,
      volumetricMeasurement: null,
      volumetricUnit: null,
      volumetricType: null,
      municipalZoning: null,
      zoning: null,
      zoningPotential: null,
      generalLocation: null,
      propertyContacts: null,
      notes: null,
      surplusDeclarationType: null,
      surplusDeclarationComment: null,
      historicalFileNumbers: null,
      surplusDeclarationDate: EpochIsoDateTime,
    };
  }
}

export class AddressForm {
  public id?: number;
  public apiId?: number;
  public rowVersion?: number;
  public isDisabled?: boolean;
  public displayOrder?: number;
  public streetAddress1?: string;
  public streetAddress2?: string;
  public streetAddress3?: string;
  public municipality?: string;
  public postalCode?: string;

  public static fromBcaAddress(model: IBcAssessmentSummary['ADDRESSES'][0]): AddressForm {
    const newForm = new AddressForm();
    newForm.streetAddress1 = formatBcaAddress(model);
    newForm.municipality = model.CITY;
    newForm.postalCode = model.POSTAL_CODE;

    return newForm;
  }

  public static fromPimsView(model: PIMS_Property_Location_View): AddressForm {
    const newForm = new AddressForm();
    newForm.id = model.ADDRESS_ID ?? undefined;
    newForm.streetAddress1 = model.STREET_ADDRESS_1 ?? undefined;
    newForm.streetAddress2 = model.STREET_ADDRESS_2 ?? undefined;
    newForm.streetAddress3 = model.STREET_ADDRESS_3 ?? undefined;
    newForm.municipality = model.MUNICIPALITY_NAME ?? undefined;
    newForm.postalCode = model.POSTAL_CODE ?? undefined;
    newForm.apiId = model?.ADDRESS_ID ?? undefined;

    return newForm;
  }

  public static fromApi(model: ApiGen_Concepts_Address): AddressForm {
    const newForm = new AddressForm();
    newForm.id = model.id ?? undefined;
    newForm.streetAddress1 = model.streetAddress1 ?? undefined;
    newForm.streetAddress2 = model.streetAddress2 ?? undefined;
    newForm.streetAddress3 = model.streetAddress3 ?? undefined;
    newForm.municipality = model.municipality ?? undefined;
    newForm.postalCode = model.postal ?? undefined;
    newForm.apiId = model?.id ?? undefined;
    newForm.rowVersion = model.rowVersion ?? undefined;

    return newForm;
  }

  public toApi(): ApiGen_Concepts_Address {
    return {
      id: this.apiId ?? null,
      rowVersion: this.rowVersion ?? null,
      streetAddress1: this.streetAddress1 ?? null,
      streetAddress2: this.streetAddress2 ?? null,
      streetAddress3: this.streetAddress3 ?? null,
      municipality: this.municipality ?? null,
      postal: this.postalCode ?? null,
      comment: null,
      country: null,
      countryId: null,
      countryOther: null,
      district: null,
      latitude: null,
      longitude: null,
      province: null,
      provinceStateId: null,
      region: null,
      regionCode: null,
      districtCode: null,
    };
  }
}
