import { IMapProperty } from 'features/properties/selector/models';
import { Api_File } from 'models/api/File';
import { Api_Property } from 'models/api/Property';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import { pidParser } from 'utils';
import { toTypeCode } from 'utils/formUtils';

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
          fileId: x.fileId,
          property: x.toApi(),
          File: { id: this.id },
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
  public address?: string;
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
    newForm.address = model.address;

    return newForm;
  }

  public static fromApi(model: Api_PropertyFile): PropertyForm {
    const newForm = new PropertyForm();
    newForm.id = model.id;
    newForm.fileId = model.fileId;
    newForm.apiId = model.property?.id;
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

    return newForm;
  }

  public toApi(): Api_Property {
    return {
      id: this.apiId,
      pid: pidParser(this.pid),
      pin: this.pin !== undefined ? Number(this.pin) : undefined,
      planNumber: this.planNumber,
      landArea: 0,
      location: { coordinate: { x: this.longitude, y: this.latitude } },
      region: toTypeCode(this.region),
      district: toTypeCode(this.district),
      rowVersion: this.propertyRowVersion,
      isOwned: this.isOwned,
    };
  }
}
