import { Polygon, Position } from 'geojson';

import { IMapProperty } from '@/components/propertySelector/models';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import { exists, formatApiAddress, formatBcaAddress, pidParser } from '@/utils';
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
  public polygon?: Polygon;
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

  private constructor(baseModel?: Partial<PropertyForm>) {
    Object.assign(this, baseModel);
  }

  public static fromMapProperty(model: IMapProperty): PropertyForm {
    return new PropertyForm({
      apiId: model.propertyId,
      pid: model.pid,
      pin: model.pin,
      latitude: model.latitude,
      longitude: model.longitude,
      polygon: model.polygon,
      planNumber: model.planNumber,
      region: model.region,
      regionName: model.regionName,
      district: model.district,
      districtName: model.districtName,
      formattedAddress: model.address,
    });
  }

  public toMapProperty(): IMapProperty {
    return {
      pid: this.pid,
      pin: this.pin,
      latitude: this.latitude,
      longitude: this.longitude,
      planNumber: this.planNumber,
      region: this.region,
      regionName: this.regionName,
      district: this.district,
      districtName: this.districtName,
      address: this.address ? formatApiAddress(this.address.toApi()) : this.formattedAddress,
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

    return newForm;
  }

  public toApi(): ApiGen_Concepts_Property {
    return {
      id: this.apiId ?? 0,
      pid: pidParser(this.pid) ?? null,
      pin: this.pin !== undefined ? Number(this.pin) : null,
      planNumber: this.planNumber ?? null,
      location: { coordinate: { x: this.longitude ?? 0, y: this.latitude ?? 0 } },
      boundary: this.polygon
        ? {
            coordinates: this.polygon.coordinates.flatMap(positions =>
              positions.map((p: Position) => ({ x: p[0], y: p[1] })),
            ),
          }
        : null,
      region: toTypeCodeNullable(this.region),
      district: toTypeCodeNullable(this.district),
      rowVersion: this.propertyRowVersion ?? null,
      isOwned: this.isOwned ?? false,
      address: this.address?.toApi() ?? null,
      landLegalDescription: this.legalDescription ?? null,
      propertyType: null,
      anomalies: null,
      tenures: null,
      roadTypes: null,
      status: null,
      dataSource: null,
      dataSourceEffectiveDateOnly: EpochIsoDateTime,
      latitude: null,
      longitude: null,
      name: null,
      description: null,
      isSensitive: false,
      isProvincialPublicHwy: null,
      pphStatusUpdateUserid: null,
      pphStatusUpdateTimestamp: null,
      pphStatusUpdateUserGuid: null,
      isRwyBeltDomPatent: null,
      pphStatusTypeCode: null,
      isPropertyOfInterest: false,
      isVisibleToOtherAgencies: false,
      areaUnit: null,
      landArea: null,
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
    };
  }
}
