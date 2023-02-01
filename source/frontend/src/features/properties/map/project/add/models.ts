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
  rowVersion?: number;

  toApi(): Api_Project {
    return {
      id: this.id,
      code: this.projectNumber,
      description: this.projectName,
      projectStatusTypeCode: toTypeCode<string>(this.projectStatusType),
      regionCode: this.region ? toTypeCode<number>(+this.region) : undefined,
      note: this.summary,
      rowVersion: this.rowVersion,
    };
  }

  static fromApi(model: Api_Project): ProjectForm {
    const newForm = new ProjectForm();
    newForm.id = model.id;
    newForm.projectName = model.description;
    newForm.projectNumber = model.code;
    newForm.projectStatusType = model.projectStatusTypeCode?.id;
    newForm.region = model.regionCode?.id;
    newForm.summary = model.note;
    newForm.rowVersion = model.rowVersion;

    return newForm;
  }
}
