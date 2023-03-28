import { IAutocompletePrediction } from 'interfaces';
import { defaultProject } from 'models/api/Project';
import { Api_ResearchFileProject } from 'models/api/ResearchFile';

export class ResearchFileProjectFormModel {
  public id: number | undefined;
  public fileId: number | undefined;
  public project: IAutocompletePrediction | undefined;
  public rowVersion: number | undefined;
  public isDisabled: boolean | undefined;

  public static fromApi(base: Api_ResearchFileProject): ResearchFileProjectFormModel {
    const newModel = new ResearchFileProjectFormModel();
    newModel.id = base.id;
    newModel.fileId = base.fileId;
    newModel.rowVersion = base.rowVersion;
    newModel.isDisabled = base.isDisabled;
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
      isDisabled: this.isDisabled,
      project:
        this.project?.id !== undefined && this.project?.id !== 0
          ? {
              ...defaultProject,
              id: this.project?.id,
            }
          : undefined,
    };
  }
}
