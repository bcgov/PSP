import { Api_AcquisitionFileTeam } from '@/models/api/AcquisitionFile';
import { formatApiPersonNames } from '@/utils/personUtils';

type IdSelector = 'O' | 'P';

export interface Api_AcquisitionFilter {
  acquisitionFileStatusTypeCode: string;
  acquisitionFileNameOrNumber: string;
  acquisitionTeamMemberPersonId: string;
  acquisitionTeamMemberOrganizationId: string;
  projectNameOrNumber: string;
  searchBy: string;
  pin: string;
  pid: string;
  address: string;
}

export class AcquisitionFilterModel {
  acquisitionFileStatusTypeCode: string = 'ACTIVE';
  acquisitionFileNameOrNumber: string = '';
  acquisitionTeamMembers: MultiSelectOption[] = [];
  projectNameOrNumber: string = '';
  searchBy: string = 'address';
  pin: string = '';
  pid: string = '';
  address: string = '';

  toApi(): Api_AcquisitionFilter {
    return {
      acquisitionFileStatusTypeCode: this.acquisitionFileStatusTypeCode,
      acquisitionFileNameOrNumber: this.acquisitionFileNameOrNumber,
      acquisitionTeamMemberPersonId: getParameterIdFromOptions(this.acquisitionTeamMembers, 'P'),
      acquisitionTeamMemberOrganizationId: getParameterIdFromOptions(
        this.acquisitionTeamMembers,
        'O',
      ),
      projectNameOrNumber: this.projectNameOrNumber,
      searchBy: this.searchBy,
      pin: this.pin,
      pid: this.pid,
      address: this.address,
    };
  }

  static fromApi(
    model: Api_AcquisitionFilter,
    teamMembers: Api_AcquisitionFileTeam[],
  ): AcquisitionFilterModel {
    const newModel = new AcquisitionFilterModel();
    newModel.acquisitionFileStatusTypeCode = model.acquisitionFileStatusTypeCode;
    newModel.acquisitionFileNameOrNumber = model.acquisitionFileNameOrNumber;
    newModel.projectNameOrNumber = model.projectNameOrNumber;
    newModel.searchBy = model.searchBy;
    newModel.pin = model.pin;
    newModel.pid = model.pid;
    newModel.address = model.address;

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

export interface MultiSelectOption {
  id: string;
  text: string;
}

const getParameterIdFromOptions = (
  options: MultiSelectOption[],
  selector: IdSelector = 'P',
): string => {
  if (!options.length) {
    return '';
  }

  var filterOrgItems = options.filter(option => String(option.id).startsWith(selector));
  if (!filterOrgItems.length) {
    return '';
  }

  return filterOrgItems[0].id.split('-').pop() ?? '';
};
