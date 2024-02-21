import { IAutocompletePrediction } from '@/interfaces';
import { ApiGen_Concepts_ResearchFileProject } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProject';
import { exists, isValidId } from '@/utils/utils';

export class ResearchFileProjectFormModel {
  public id: number | undefined;
  public fileId: number | undefined;
  public project: IAutocompletePrediction | undefined;
  public rowVersion: number | undefined;

  public static fromApi(base: ApiGen_Concepts_ResearchFileProject): ResearchFileProjectFormModel {
    const newModel = new ResearchFileProjectFormModel();
    newModel.id = base.id;
    newModel.fileId = base.fileId;
    newModel.rowVersion = base.rowVersion ?? undefined;
    newModel.project = exists(base.project)
      ? { id: base.project.id, text: base.project.description || '' }
      : undefined;
    return newModel;
  }

  public toApi(): ApiGen_Concepts_ResearchFileProject {
    return {
      id: this.id ?? 0,
      fileId: this.fileId ?? 0,
      file: null,
      rowVersion: this.rowVersion ?? null,
      project: null,
      projectId: isValidId(this.project?.id) ? this.project!.id : null,
    };
  }

  public static toApiList(
    researchProjects: ResearchFileProjectFormModel[] | undefined,
  ): ApiGen_Concepts_ResearchFileProject[] {
    if (researchProjects === undefined) {
      return [];
    } else {
      return researchProjects.map(x => x.toApi()).filter(rp => isValidId(rp?.projectId));
    }
  }
}
