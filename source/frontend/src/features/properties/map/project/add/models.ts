import { Api_Project } from 'models/api/Project';
import { NumberFieldValue } from 'typings/NumberFieldValue';
import { toTypeCode } from 'utils/formUtils';

export class ProjectForm {
  id?: number;
  projectName?: string;
  projectNumber?: string;
  projectStatusType?: string;
  region?: NumberFieldValue;
  summary?: string;

  toApi(): Api_Project {
    return {
      id: this.id,
      code: this.projectNumber,
      description: this.projectName,
      projectStatusTypeCode: toTypeCode(this.projectStatusType),
      regionCode: this.region ? toTypeCode<number>(this.region) : undefined,
      note: this.summary,
    };
  }
}
