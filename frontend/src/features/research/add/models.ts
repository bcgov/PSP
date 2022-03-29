import { Api_Property, Api_ResearchFile } from 'models/api/ResearchFile';

export class ResearchForm {
  public name: string;
  public properties: PropertyForm[];
  constructor() {
    this.name = '';
    this.properties = [];
  }

  public toApi(): Api_ResearchFile {
    return { name: this.name, properties: this.properties.map(x => x.toApi()) };
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
