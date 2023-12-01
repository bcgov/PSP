import isNumber from 'lodash/isNumber';

import { SelectOption } from '@/components/common/form';
import {
  Api_DispositionFile,
  Api_DispositionFileProperty,
  Api_DispositionFileTeam,
} from '@/models/api/DispositionFile';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';

export class DispositionFilterModel {
  searchBy: string = 'address';
  pin: string = '';
  pid: string = '';
  address: string = '';
  dispositionTeamMember: SelectOption | null = null;
  fileNameOrNumberOrReference: string = '';
  dispositionFileStatusCode: string = '';
  dispositionStatusCode: string = '';
  dispositionTypeCode: string = '';

  toApi(): Api_DispositionFilter {
    const personMemberId =
      this.dispositionTeamMember?.value && String(this.dispositionTeamMember.value).startsWith('P-')
        ? String(this.dispositionTeamMember.value).substring(2)
        : null;
    const orgMemberId =
      this.dispositionTeamMember?.value && String(this.dispositionTeamMember.value).startsWith('O-')
        ? String(this.dispositionTeamMember.value).substring(2)
        : null;

    return {
      searchBy: this.searchBy,
      pin: this.pin,
      pid: this.pid,
      address: this.address,
      fileNameOrNumberOrReference: this.fileNameOrNumberOrReference,
      dispositionFileStatusCode: this.dispositionFileStatusCode,
      dispositionStatusCode: this.dispositionStatusCode,
      dispositionTypeCode: this.dispositionTypeCode,
      // disposition team members
      teamMemberPersonId:
        personMemberId && isNumber(personMemberId) ? Number(personMemberId) : null,
      teamMemberOrganizationId: orgMemberId && isNumber(orgMemberId) ? Number(orgMemberId) : null,
    };
  }

  static fromApi(
    base: Api_DispositionFilter,
    teamMemberOptions: SelectOption[] = [],
  ): DispositionFilterModel {
    const newModel = new DispositionFilterModel();
    newModel.searchBy = base.searchBy ?? 'address';
    newModel.pin = base.pin ?? '';
    newModel.pid = base.pid ?? '';
    newModel.address = base.address ?? '';
    newModel.fileNameOrNumberOrReference = base.fileNameOrNumberOrReference ?? '';
    newModel.dispositionFileStatusCode = base.dispositionFileStatusCode ?? '';
    newModel.dispositionStatusCode = base.dispositionStatusCode ?? '';
    newModel.dispositionTypeCode = base.dispositionTypeCode ?? '';
    // disposition team members
    newModel.dispositionTeamMember = base.teamMemberPersonId
      ? teamMemberOptions.find(c => c.value === `P-${base.teamMemberPersonId}`) ?? null
      : base.teamMemberOrganizationId
      ? teamMemberOptions.find(c => c.value === `O-${base.teamMemberOrganizationId}`) ?? null
      : null;

    return newModel;
  }
}

export class DispositionSearchResultModel {
  id: number | null = null;
  fileNumber: string = '';
  fileName: string = '';
  referenceNumber: string = '';
  region: string = '';
  dispositionType: string = '';
  dispositionStatus: string = '';
  fileStatus: string = '';
  dispositionTeam: Api_DispositionFileTeam[] = [];
  fileProperties?: Api_DispositionFileProperty[] = [];

  static fromApi(base: Api_DispositionFile): DispositionSearchResultModel {
    var newModel = new DispositionSearchResultModel();
    newModel.id = base.id ?? null;
    newModel.fileNumber = base.fileNumber ?? '';
    newModel.fileName = base.fileName ?? '';
    newModel.referenceNumber = base.referenceNumber ?? '';
    newModel.region = base.regionCode?.description ?? '';
    newModel.dispositionType = base.dispositionTypeCode?.description ?? '';
    newModel.dispositionStatus = base.dispositionStatusTypeCode?.description ?? '';
    newModel.fileStatus = base.fileStatusTypeCode?.description ?? '';
    newModel.dispositionTeam = base.dispositionTeam || [];
    newModel.fileProperties = base.fileProperties || [];

    return newModel;
  }
}
