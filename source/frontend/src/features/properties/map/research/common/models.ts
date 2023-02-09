import { IAutocompletePrediction } from 'interfaces';
import { Api_ResearchFileProject } from 'models/api/ResearchFile';

export class ResearchFileProjectFormModel {
  public id?: number;
  public fileId?: number;
  public project?: IAutocompletePrediction;
  public rowVersion?: number;

  public static fromApi(base: Api_ResearchFileProject): ResearchFileProjectFormModel {
    const newModel = new ResearchFileProjectFormModel();
    newModel.id = base.id;
    newModel.fileId = base.fileId;
    newModel.rowVersion = base.rowVersion;
    newModel.project =
      base.project !== undefined
        ? { id: base.project.id!, text: base.project.description || '' }
        : undefined;
    return newModel;
  }

  public toApi(): Api_ResearchFileProject {
    return {
      id: this.id,
      fileId: this.fileId,
      rowVersion: this.rowVersion,
      project:
        this.project?.id !== undefined && this.project?.id !== 0
          ? { id: this.project?.id }
          : undefined,
    };
  }
}
