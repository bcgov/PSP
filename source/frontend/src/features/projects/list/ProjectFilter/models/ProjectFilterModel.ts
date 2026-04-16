import { IProjectFilter } from '@/features/projects/interfaces';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';

export class ProjectFilterModel {
  projectName: string;
  projectNumber: string;
  projectRegionCode: string;
  projectStatusCode: string;
  regions: MultiSelectOption[] = [];

  constructor(initialRegions: MultiSelectOption[] = []) {
    this.regions = initialRegions;
  }

  toApi(): IProjectFilter {
    return {
      projectRegionCode: this.projectRegionCode,
      projectStatusCode: this.projectStatusCode,
      projectName: this.projectName,
      projectNumber: this.projectNumber,
      regions: this.regions.map(x => x.id),
    };
  }

  static fromApi(model: IProjectFilter, userRegions: MultiSelectOption[]): ProjectFilterModel {
    const newModel = new ProjectFilterModel();
    newModel.projectName = model.projectName;
    newModel.projectNumber = model.projectNumber;
    newModel.projectRegionCode = model.projectRegionCode;
    newModel.projectStatusCode = model.projectStatusCode;
    newModel.regions = userRegions ?? [];

    return newModel;
  }
}
