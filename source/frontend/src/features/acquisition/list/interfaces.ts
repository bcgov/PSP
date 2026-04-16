import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { formatApiPersonNames, getParameterIdFromOptions } from '@/utils/personUtils';

export interface ApiGen_Concepts_AcquisitionFilter {
  acquisitionFileStatusTypeCode: string;
  acquisitionFileNameOrNumber: string;
  acquisitionTeamMemberPersonId: string;
  acquisitionTeamMemberOrganizationId: string;
  projectNameOrNumber: string;
  ownerName: string;
  searchBy: string;
  hasNoticeOfClaim: boolean;
  pin: string;
  pid: string;
  address: string;
  regions: string[];
}

export class AcquisitionFilterModel {
  acquisitionFileStatusTypeCode = '';
  acquisitionFileNameOrNumber = '';
  acquisitionTeamMembers: MultiSelectOption[] = [];
  projectNameOrNumber = '';
  hasNoticeOfClaim = false;
  ownerName = '';
  searchBy = 'address';
  pin = '';
  pid = '';
  address = '';
  regions: MultiSelectOption[] = [];

  constructor(initialRegions: MultiSelectOption[] = []) {
    this.regions = initialRegions;
  }

  toApi(): ApiGen_Concepts_AcquisitionFilter {
    return {
      acquisitionFileStatusTypeCode: this.acquisitionFileStatusTypeCode,
      acquisitionFileNameOrNumber: this.acquisitionFileNameOrNumber,
      acquisitionTeamMemberPersonId: getParameterIdFromOptions(this.acquisitionTeamMembers, 'P'),
      acquisitionTeamMemberOrganizationId: getParameterIdFromOptions(
        this.acquisitionTeamMembers,
        'O',
      ),
      projectNameOrNumber: this.projectNameOrNumber,
      ownerName: this.ownerName,
      searchBy: this.searchBy,
      hasNoticeOfClaim: this.hasNoticeOfClaim,
      pin: this.pin,
      pid: this.pid,
      address: this.address,
      regions: this.regions.map(x => x.id),
    };
  }

  static fromApi(
    model: ApiGen_Concepts_AcquisitionFilter,
    teamMembers: ApiGen_Concepts_AcquisitionFileTeam[],
    userRegions: MultiSelectOption[],
  ): AcquisitionFilterModel {
    const newModel = new AcquisitionFilterModel();
    newModel.acquisitionFileStatusTypeCode = model.acquisitionFileStatusTypeCode;
    newModel.acquisitionFileNameOrNumber = model.acquisitionFileNameOrNumber;
    newModel.projectNameOrNumber = model.projectNameOrNumber;
    newModel.ownerName = model.ownerName;
    newModel.searchBy = model.searchBy;
    newModel.pin = model.pin;
    newModel.pid = model.pid;
    newModel.hasNoticeOfClaim = model.hasNoticeOfClaim;
    newModel.address = model.address;
    newModel.regions = userRegions ?? [];

    if (model.acquisitionTeamMemberPersonId) {
      const memberPerson = teamMembers.find(
        p => p.personId === Number(model.acquisitionTeamMemberPersonId),
      );

      newModel.acquisitionTeamMembers = [
        {
          id: `P-${memberPerson?.personId}`,
          text: formatApiPersonNames(memberPerson?.person),
        },
      ];
    }

    if (model.acquisitionTeamMemberOrganizationId) {
      const memberOrganization = teamMembers.find(
        p => p.organizationId === Number(model.acquisitionTeamMemberOrganizationId),
      );

      newModel.acquisitionTeamMembers = [
        {
          id: `O-${memberOrganization?.organizationId}`,
          text: `${memberOrganization?.organization?.name}`,
        },
      ];
    }

    return newModel;
  }
}
