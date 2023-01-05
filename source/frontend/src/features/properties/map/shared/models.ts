import { IMapProperty } from 'components/propertySelector/models';
import { IBcAssessmentSummary } from 'hooks/useBcAssessmentLayer';
import { Api_Address } from 'models/api/Address';
import { Api_File } from 'models/api/File';
import { Api_Property } from 'models/api/Property';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import { formatApiAddress, pidParser } from 'utils';
import { toTypeCode } from 'utils/formUtils';

import { formatBcaAddress } from './../../../../utils/propertyUtils';

export class FileForm {
  public id?: number;
  public name: string;
  public properties: PropertyForm[];
  public rowVersion?: number;
  constructor() {
    this.name = '';
    this.properties = [];
  }

  public toApi(): Api_File {
    return {
      id: this.id,
      fileName: this.name,
      fileProperties: this.properties.map<Api_PropertyFile>(x => {
        return {
          id: x.id,
          fileId: this.id,
          property: x.toApi(),
          file: { id: this.id },
          propertyName: x.name,
          rowVersion: x.rowVersion,
        };
      }),
      rowVersion: this.rowVersion,
    };
  }

  public static fromApi(model: Api_File): FileForm {
    const newForm = new FileForm();
    newForm.id = model.id;
    newForm.name = model.fileName || '';
    newForm.properties = model.fileProperties?.map(x => PropertyForm.fromApi(x)) || [];
    newForm.rowVersion = model.rowVersion;

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
  public isDisabled?: boolean;
  public displayOrder?: number;
  public isOwned?: boolean;

  private constructor() {}

  public static fromMapProperty(model: IMapProperty): PropertyForm {
    const newForm = new PropertyForm();
    newForm.pid = model.pid;
    newForm.pin = model.pin;
    newForm.latitude = model.latitude;
    newForm.longitude = model.longitude;
    newForm.planNumber = model.planNumber;
    newForm.region = model.region;
    newForm.regionName = model.regionName;
    newForm.district = model.district;
    newForm.districtName = model.districtName;
    newForm.legalDescription = model.legalDescription;
    newForm.formattedAddress = model.address;

    return newForm;
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
      legalDescription: this.legalDescription,
      address: this.address ? formatApiAddress(this.address.toApi()) : this.formattedAddress,
    };
  }

  public static fromApi(model: Api_PropertyFile): PropertyForm {
    const newForm = new PropertyForm();
    newForm.id = model.id;
    newForm.fileId = model.fileId;
    newForm.apiId = model.property?.id ?? model.id;
    newForm.name = model.propertyName;
    newForm.pid = model.property?.pid?.toString();
    newForm.pin = model.property?.pin?.toString();
    newForm.latitude = model.property?.latitude;
    newForm.longitude = model.property?.longitude;
    newForm.planNumber = model.property?.planNumber;
    newForm.region = model.property?.region?.id;
    newForm.district = model.property?.district?.id;
    newForm.rowVersion = model.rowVersion;
    newForm.propertyRowVersion = model.property?.rowVersion;
    newForm.isDisabled = model.isDisabled;
    newForm.displayOrder = model.displayOrder;
    newForm.isOwned = model.property?.isOwned;
    newForm.formattedAddress = formatApiAddress(model.property?.address);
    newForm.address = model.property?.address
      ? AddressForm.fromApi(model.property?.address)
      : undefined;

    return newForm;
  }

  public toApi(): Api_Property {
    return {
      id: this.apiId,
      pid: pidParser(this.pid),
      pin: this.pin !== undefined ? Number(this.pin) : undefined,
      planNumber: this.planNumber,
      location: { coordinate: { x: this.longitude, y: this.latitude } },
      region: toTypeCode(this.region),
      district: toTypeCode(this.district),
      rowVersion: this.propertyRowVersion,
      isOwned: this.isOwned,
      address: this.address?.toApi(),
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

  private constructor() {}

  public static fromBcaAddress(model: IBcAssessmentSummary['ADDRESSES'][0]): AddressForm {
    const newForm = new AddressForm();
    newForm.streetAddress1 = formatBcaAddress(model);
    newForm.municipality = model.CITY;
    newForm.postalCode = model.POSTAL_CODE;

    return newForm;
  }

  public static fromApi(model: Api_Address): AddressForm {
    const newForm = new AddressForm();
    newForm.id = model.id;
    newForm.streetAddress1 = model.streetAddress1;
    newForm.streetAddress2 = model.streetAddress2;
    newForm.streetAddress3 = model.streetAddress3;
    newForm.municipality = model.municipality;
    newForm.postalCode = model.postal;
    newForm.apiId = model?.id;
    newForm.rowVersion = model.rowVersion;

    return newForm;
  }

  public toApi(): Api_Address {
    return {
      id: this.apiId,
      rowVersion: this.rowVersion,
      streetAddress1: this.streetAddress1,
      streetAddress2: this.streetAddress2,
      streetAddress3: this.streetAddress3,
      municipality: this.municipality,
      postal: this.postalCode,
    };
  }
}
