import { Api_File } from 'models/api/File';
import { Api_ResearchFile, Api_ResearchFileProperty } from 'models/api/ResearchFile';

import { PropertyForm } from '../../shared/models';

export class ResearchForm {
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
      fileProperties: this.properties.map<Api_ResearchFileProperty>(x => {
        return {
          id: x.id,
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
    newForm.name = model.fileName || '';
    newForm.properties = model.fileProperties?.map(x => PropertyForm.fromApi(x)) || [];
    newForm.rowVersion = model.rowVersion;

    return newForm;
  }
}
