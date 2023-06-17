import { Api_ResearchFile, Api_ResearchFileProperty } from '@/models/api/ResearchFile';

import { PropertyForm } from '../../shared/models';
import { ResearchFileProjectFormModel } from '../common/models';

export class ResearchForm {
  public id?: number;
  public name: string;
  public properties: PropertyForm[];
  public researchFileProjects: ResearchFileProjectFormModel[];
  public rowVersion?: number;

  constructor() {
    this.name = '';
    this.properties = [];
    this.researchFileProjects = [];
  }

  public toApi(): Api_ResearchFile {
    return {
      id: this.id,
      fileName: this.name,
      fileProperties: this.properties.map<Api_ResearchFileProperty>(x => {
        return {
          id: x.id,
          property: x.toApi(),
          propertyId: x.apiId,
          researchFile: { id: this.id },
          propertyName: x.name,
          rowVersion: x.rowVersion,
        };
      }),
      researchFileProjects: ResearchFileProjectFormModel.toApiList(this.researchFileProjects),
      rowVersion: this.rowVersion,
    };
  }

  public static fromApi(model: Api_ResearchFile): ResearchForm {
    const newForm = new ResearchForm();
    newForm.id = model.id;
    newForm.name = model.fileName || '';
    newForm.properties = model.fileProperties?.map(x => PropertyForm.fromApi(x)) || [];
    newForm.researchFileProjects =
      model.researchFileProjects?.map(x => ResearchFileProjectFormModel.fromApi(x)) || [];
    newForm.rowVersion = model.rowVersion;

    return newForm;
  }
}
