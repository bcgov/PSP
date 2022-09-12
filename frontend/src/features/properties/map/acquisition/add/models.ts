import { IMapProperty } from 'features/properties/selector/models';
import { Api_AcquisitionFile, Api_AcquisitionFileProperty } from 'models/api/AcquisitionFile';
import { Api_Property } from 'models/api/Property';
import { pidParser } from 'utils';
import { fromTypeCode, toTypeCode } from 'utils/formUtils';

export class AcquisitionForm {
  id?: number;
  fileName?: string = '';
  assignedDate?: string;
  deliveryDate?: string;
  rowVersion?: number;
  // Code Tables
  acquisitionFileStatusType?: string;
  acquisitionPhysFileStatusType?: string;
  acquisitionType?: string;
  // MOTI region
  region?: string;
  properties: AcquisitionPropertyForm[] = [];

  toApi(): Api_AcquisitionFile {
    return {
      id: this.id,
      fileName: this.fileName,
      rowVersion: this.rowVersion,
      assignedDate: this.assignedDate,
      deliveryDate: this.deliveryDate,
      fileStatusTypeCode: toTypeCode(this.acquisitionFileStatusType),
      acquisitionPhysFileStatusTypeCode: toTypeCode(this.acquisitionPhysFileStatusType),
      acquisitionTypeCode: toTypeCode(this.acquisitionType),
      regionCode: toTypeCode(Number(this.region)),
      // ACQ file properties
      acquisitionProperties: this.properties.map<Api_AcquisitionFileProperty>(x => {
        return {
          id: x.acquisitionFilePropertyId,
          propertyName: x.name,
          isDisabled: x.isDisabled,
          displayOrder: x.displayOrder,
          rowVersion: x.rowVersion,
          property: x.toApi(),
          acquisitionFile: { id: this.id },
        };
      }),
    };
  }

  static fromApi(model: Api_AcquisitionFile): AcquisitionForm {
    const newForm = new AcquisitionForm();
    newForm.id = model.id;
    newForm.fileName = model.fileName || '';
    newForm.rowVersion = model.rowVersion;
    newForm.assignedDate = model.assignedDate;
    newForm.deliveryDate = model.deliveryDate;
    newForm.acquisitionFileStatusType = fromTypeCode(model.fileStatusTypeCode);
    newForm.acquisitionPhysFileStatusType = fromTypeCode(model.acquisitionPhysFileStatusTypeCode);
    newForm.acquisitionType = fromTypeCode(model.acquisitionTypeCode);
    newForm.region = fromTypeCode(model.regionCode)?.toString();
    // ACQ file properties
    newForm.properties =
      model.acquisitionProperties?.map(x => AcquisitionPropertyForm.fromApi(x)) || [];

    return newForm;
  }
}

export class AcquisitionPropertyForm {
  acquisitionFilePropertyId?: number;
  apiId?: number;
  pid?: string;
  pin?: string;
  latitude?: number;
  longitude?: number;
  planNumber?: string;
  name?: string;
  isDisabled?: boolean;
  displayOrder?: number;
  regionId?: number;
  districtId?: number;
  rowVersion?: number;

  static fromMapProperty(model: IMapProperty): AcquisitionPropertyForm {
    const newForm = new AcquisitionPropertyForm();
    newForm.pid = model.pid;
    newForm.pin = model.pin;
    newForm.latitude = model.latitude;
    newForm.longitude = model.longitude;
    newForm.planNumber = model.planNumber;
    newForm.regionId = model.region;
    newForm.districtId = model.district;

    return newForm;
  }

  static fromApi(model: Api_AcquisitionFileProperty): AcquisitionPropertyForm {
    const newForm = new AcquisitionPropertyForm();
    newForm.acquisitionFilePropertyId = model.id;
    newForm.apiId = model.property?.id;
    newForm.name = model.propertyName;
    newForm.pid = model.property?.pid?.toString();
    newForm.pin = model.property?.pin?.toString();
    newForm.latitude = model.property?.latitude;
    newForm.longitude = model.property?.longitude;
    newForm.planNumber = model.property?.planNumber;
    newForm.regionId = model.property?.region?.id;
    newForm.districtId = model.property?.district?.id;
    newForm.rowVersion = model.rowVersion;

    return newForm;
  }

  toApi(): Api_Property {
    return {
      id: this.apiId,
      pid: pidParser(this.pid),
      pin: this.pin !== undefined ? Number(this.pin) : undefined,
      landArea: 0,
      location: { coordinate: { x: this.longitude, y: this.latitude } },
      region: toTypeCode(this.regionId),
      district: toTypeCode(this.districtId),
    };
  }
}
