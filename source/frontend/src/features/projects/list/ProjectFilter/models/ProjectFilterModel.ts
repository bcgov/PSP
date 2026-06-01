import { IProjectFilter } from '@/features/projects/interfaces';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { ApiGen_Concepts_ProjectPerson } from '@/models/api/generated/ApiGen_Concepts_ProjectPerson';
import { formatApiPersonNames, getParameterIdFromOptions } from '@/utils/personUtils';

export class ProjectFilterModel {
  projectName = '';
  projectNumber = '';
  projectStatusCode = '';
  projectCreatedBy: MultiSelectOption[] = [];
  projectTeamMembers: MultiSelectOption[] = [];
  regions: MultiSelectOption[] = [];

  constructor(initialRegions: MultiSelectOption[] = []) {
    this.regions = initialRegions;
  }

  toApi(): IProjectFilter {
    const projectTeamPersonId = getParameterIdFromOptions(this.projectTeamMembers, 'P');

    return {
      projectStatusCode: this.projectStatusCode?.trim() ? this.projectStatusCode : null,
      projectName: this.projectName?.trim() ? this.projectName : null,
      projectNumber: this.projectNumber?.trim() ? this.projectNumber : null,
      projectCreatedBy: this.projectCreatedBy?.[0]?.id ?? null,
      teamMemberPersonId: projectTeamPersonId,
      regions: this.regions?.map(x => x.id) ?? [],
    };
  }

  static fromApi(
    model: IProjectFilter,
    teamMembers: ApiGen_Concepts_ProjectPerson[],
    userRegions: MultiSelectOption[],
    createdByOptions: MultiSelectOption[],
  ): ProjectFilterModel {
    const newModel = new ProjectFilterModel();
    newModel.projectName = model.projectName;
    newModel.projectNumber = model.projectNumber;
    newModel.projectStatusCode = model.projectStatusCode;
    newModel.projectCreatedBy = model.projectCreatedBy
      ? createdByOptions.filter(x => x.id === model.projectCreatedBy)
      : [];
    newModel.regions = userRegions ?? [];

    if (model.teamMemberPersonId) {
      const memberPerson = teamMembers.find(p => p.personId === Number(model.teamMemberPersonId));

      newModel.projectTeamMembers = [
        {
          id: `P-${memberPerson?.personId}`,
          text: formatApiPersonNames(memberPerson?.person),
        },
      ];
    }

    return newModel;
  }
}
