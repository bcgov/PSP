import isNumber from 'lodash/isNumber';

import { SelectOption } from '@/components/common/form';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_DispositionFileProperty } from '@/models/api/generated/ApiGen_Concepts_DispositionFileProperty';
import { ApiGen_Concepts_DispositionFileTeam } from '@/models/api/generated/ApiGen_Concepts_DispositionFileTeam';

export class DispositionFilterModel {
  searchBy = 'address';
  pin = '';
  pid = '';
  address = '';
  dispositionTeamMember: SelectOption | null = null;
  fileNameOrNumberOrReference = '';
  dispositionFileStatusCode = '';
  dispositionStatusCode = '';
  dispositionTypeCode = '';

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
        personMemberId && isNumber(+personMemberId) ? Number(personMemberId) : null,
      teamMemberOrganizationId: orgMemberId && isNumber(+orgMemberId) ? Number(orgMemberId) : null,
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
  fileNumber = '';
  fileName = '';
  fileReference = '';
  region = '';
  dispositionTypeCode = '';
  dispositionStatusTypeCode = '';
  dispositionFileStatusTypeCode = '';
  dispositionTeam: ApiGen_Concepts_DispositionFileTeam[] = [];
  fileProperties?: ApiGen_Concepts_DispositionFileProperty[] = [];

  static fromApi(base: ApiGen_Concepts_DispositionFile): DispositionSearchResultModel {
    const newModel = new DispositionSearchResultModel();
    newModel.id = base.id ?? null;
    newModel.fileNumber = base.fileNumber ?? '';
    newModel.fileName = base.fileName ?? '';
    newModel.fileReference = base.fileReference ?? '';
    newModel.region = base.regionCode?.description ?? '';
    newModel.dispositionTypeCode = base.dispositionTypeCode?.description ?? '';
    newModel.dispositionStatusTypeCode = base.dispositionStatusTypeCode?.description ?? '';
    newModel.dispositionFileStatusTypeCode = base.fileStatusTypeCode?.description ?? '';
    newModel.dispositionTeam = base.dispositionTeam || [];
    newModel.fileProperties = base.fileProperties || [];

    return newModel;
  }
}
