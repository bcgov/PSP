import { IMapProperty } from 'features/properties/selector/models';
import { Api_Property } from 'models/api/Property';
import { Api_ResearchFile, Api_ResearchFileProperty } from 'models/api/ResearchFile';
import { pidParser } from 'utils';

export class ResearchForm {
  public id?: number;
  public name: string;
  public properties: PropertyForm[];
  constructor() {
    this.name = '';
    this.properties = [];
  }

  public toApi(): Api_ResearchFile {
    return {
      name: this.name,
      researchProperties: this.properties.map<Api_ResearchFileProperty>(x => {
        return {
          property: x.toApi(),
          researchFile: { id: this.id },
          propertyName: x.name,
        };
      }),
    };
  }
}

export class PropertyForm {
  public pid?: string;
  public pin?: string;
  public latitude?: number;
  public longitude?: number;
  public planNumber?: string;
  public name?: string;
  //public regionId?: number;
  //public districtId?: number;

  constructor(property: IMapProperty) {
    this.pid = property.pid;
    this.pin = property.pin;
    this.latitude = property.latitude;
    this.longitude = property.longitude;
    this.planNumber = property.planNumber;
    //this.regionId = property.dis;
    //this.districtId = 0; //property.district;
  }

  public toApi(): Api_Property {
    return {
      pid: pidParser(this.pid),
      pin: this.pin !== undefined ? Number(this.pin) : undefined,
      landArea: 0,
      location: { coordinate: { x: this.longitude, y: this.latitude } },
      /*region: { id: this.regionId },
      district: { id: this.districtId },*/
    };
  }
}
