import { Api_Project } from 'models/api/Project';
import { toTypeCode } from 'utils/formUtils';

export class ProjectForm {
  id?: number;
  projectName?: string;
  projectNumber?: string;
  projectStatusType?: string;
  region?: string;
  summary?: string;

  toApi(): Api_Project {
    return {
      id: this.id,
      code: Number(this.projectNumber) ?? 0,
      description: this.projectName,
      projectStatusTypeCode: toTypeCode(this.projectStatusType),
      note: this.summary,
    };
  }
}
