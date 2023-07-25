import { Api_Project } from '@/models/api/Project';

export class Api_GenerateProject {
  business_function: string;
  work_activity: string;
  cost_type: string;
  number: string;
  name: string;

  constructor(project: Api_Project | null) {
    this.business_function = project?.businessFunctionCode?.description ?? '';
    this.work_activity = project?.workActivityCode?.description ?? '';
    this.cost_type = project?.costTypeCode?.description ?? '';
    this.number = project?.code ?? '';
    this.name = project?.description ?? '';
  }
}
