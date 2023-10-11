import { Api_Person } from '@/models/api/Person';
import { formatApiPersonNames } from '@/utils/personUtils';

export interface Api_AcquisitionFilter {
  acquisitionFileStatusTypeCode: string;
  acquisitionFileNameOrNumber: string;
  acquisitionTeamMemberPersonId: string;
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
      acquisitionTeamMemberPersonId:
        this.acquisitionTeamMembers.length > 0 ? this.acquisitionTeamMembers[0].id : '',
      projectNameOrNumber: this.projectNameOrNumber,
      searchBy: this.searchBy,
      pin: this.pin,
      pid: this.pid,
      address: this.address,
    };
  }

  static fromApi(model: Api_AcquisitionFilter, teamMembers: Api_Person[]): AcquisitionFilterModel {
    const newModel = new AcquisitionFilterModel();

    var person = teamMembers.find(p => p.id === Number(model.acquisitionTeamMemberPersonId));

    newModel.acquisitionFileStatusTypeCode = model.acquisitionFileStatusTypeCode;
    newModel.acquisitionFileNameOrNumber = model.acquisitionFileNameOrNumber;
    newModel.acquisitionTeamMembers =
      person?.id === undefined
        ? []
        : [{ id: person.id.toString(), text: formatApiPersonNames(person) }];
    newModel.projectNameOrNumber = model.projectNameOrNumber;
    newModel.searchBy = model.searchBy;
    newModel.pin = model.pin;
    newModel.pid = model.pid;
    newModel.address = model.address;

    return newModel;
  }
}

export interface MultiSelectOption {
  id: string;
  text: string;
}
