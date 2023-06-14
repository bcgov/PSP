import { IAutocompletePrediction } from '@/interfaces';
import { Api_ResearchFileProject } from '@/models/api/ResearchFile';

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
      base.project !== undefined && base.project.id !== null
        ? { id: base.project.id, text: base.project.description || '' }
        : undefined;
    return newModel;
  }

  public toApi(): Api_ResearchFileProject {
    return {
      id: this.id,
      fileId: this.fileId,
      rowVersion: this.rowVersion,
      isDisabled: this.isDisabled,
      project: undefined,
      projectId: this.project?.id !== undefined && this.project?.id !== 0 ? this.project?.id : null,
    };
  }

  public static toApiList(
    researchProjects: ResearchFileProjectFormModel[] | undefined,
  ): Api_ResearchFileProject[] {
    if (researchProjects === undefined) {
      return [];
    } else {
      return researchProjects.map(x => x.toApi()).filter(rp => rp?.projectId !== null);
    }
  }
}
