import { ApiGen_Concepts_Project } from '../api/generated/ApiGen_Concepts_Project';

export class Api_GenerateProject {
  business_function: string;
  work_activity: string;
  cost_type: string;
  number: string;
  name: string;

  constructor(project: ApiGen_Concepts_Project | null) {
    this.business_function = project?.businessFunctionCode?.code ?? '';
    this.work_activity = project?.workActivityCode?.code ?? '';
    this.cost_type = project?.costTypeCode?.code ?? '';
    this.number = project?.code ?? '';
    this.name = project?.description ?? '';
  }
}
