import { Api_Property } from 'models/api/Property';
import { Api_ResearchFile, Api_ResearchFileProperty } from 'models/api/ResearchFile';

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
        };
      }),
    };
  }
}

export class PropertyForm {
  public id: string;
  public pid: string;
  public description?: string;

  constructor(id: string, pid: string) {
    this.id = id;
    this.pid = pid;
  }

  public toApi(): Api_Property {
    // TODO: description
    return { pid: this.pid };
  }
}
