import { IProjectFilter } from '@/features/projects/interfaces';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';

export class ProjectFilterModel {
  projectName = '';
  projectNumber = '';
  projectStatusCode = '';
  regions: MultiSelectOption[] = [];

  constructor(initialRegions: MultiSelectOption[] = []) {
    this.regions = initialRegions;
  }

  toApi(): IProjectFilter {
    return {
      projectStatusCode: this.projectStatusCode?.trim() ? this.projectStatusCode : null,
      projectName: this.projectName?.trim() ? this.projectName : null,
      projectNumber: this.projectNumber?.trim() ? this.projectNumber : null,
      regions: this.regions?.map(x => x.id) ?? [],
    };
  }

  static fromApi(model: IProjectFilter, userRegions: MultiSelectOption[]): ProjectFilterModel {
    const newModel = new ProjectFilterModel();
    newModel.projectName = model.projectName;
    newModel.projectNumber = model.projectNumber;
    newModel.projectStatusCode = model.projectStatusCode;
    newModel.regions = userRegions ?? [];

    return newModel;
  }
}
