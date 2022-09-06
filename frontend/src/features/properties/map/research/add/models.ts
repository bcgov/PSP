import { IMapProperty } from 'features/properties/selector/models';
import { Api_Property } from 'models/api/Property';
import { Api_ResearchFile, Api_ResearchFileProperty } from 'models/api/ResearchFile';
import { pidParser } from 'utils';
import { toTypeCode } from 'utils/formUtils';

export class ResearchForm {
  public id?: number;
  public name: string;
  public properties: PropertyForm[];
  public rowVersion?: number;
  constructor() {
    this.name = '';
    this.properties = [];
  }

  public toApi(): Api_ResearchFile {
    return {
      id: this.id,
      name: this.name,
      researchProperties: this.properties.map<Api_ResearchFileProperty>(x => {
        return {
          id: x.researchFilePropertyId,
          property: x.toApi(),
          researchFile: { id: this.id },
          propertyName: x.name,
          rowVersion: x.rowVersion,
        };
      }),
      rowVersion: this.rowVersion,
    };
  }

  public static fromApi(model: Api_ResearchFile): ResearchForm {
    const newForm = new ResearchForm();
    newForm.id = model.id;
    newForm.name = model.name || '';
    newForm.properties = model.researchProperties?.map(x => PropertyForm.fromApi(x)) || [];
    newForm.rowVersion = model.rowVersion;

    return newForm;
  }
}

export class PropertyForm {
  public researchFilePropertyId?: number;
  public apiId?: number;
  public pid?: string;
  public pin?: string;
  public latitude?: number;
  public longitude?: number;
  public planNumber?: string;
  public name?: string;
  public regionId?: number;
  public districtId?: number;
  public rowVersion?: number;

  private constructor() {}

  public static fromMapProperty(model: IMapProperty): PropertyForm {
    const newForm = new PropertyForm();
    newForm.pid = model.pid;
    newForm.pin = model.pin;
    newForm.latitude = model.latitude;
    newForm.longitude = model.longitude;
    newForm.planNumber = model.planNumber;
    newForm.regionId = model.region;
    newForm.districtId = model.district;

    return newForm;
  }

  public static fromApi(model: Api_ResearchFileProperty): PropertyForm {
    const newForm = new PropertyForm();
    newForm.researchFilePropertyId = model.id;
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

  public toApi(): Api_Property {
    return {
      id: this.apiId,
      pid: pidParser(this.pid),
      pin: this.pin !== undefined ? Number(this.pin) : undefined,
      planNumber: this.planNumber,
      landArea: 0,
      location: { coordinate: { x: this.longitude, y: this.latitude } },
      region: toTypeCode(this.regionId),
      district: toTypeCode(this.districtId),
    };
  }
}
