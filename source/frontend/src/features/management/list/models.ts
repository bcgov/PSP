import isNumber from 'lodash/isNumber';

import { SelectOption } from '@/components/common/form';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { ApiGen_Concepts_ManagementFileProperty } from '@/models/api/generated/ApiGen_Concepts_ManagementFileProperty';
import { ApiGen_Concepts_ManagementFileTeam } from '@/models/api/generated/ApiGen_Concepts_ManagementFileTeam';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { Api_ManagementFilter } from '@/models/api/ManagementFilter';

export class ManagementFilterModel {
  searchBy = 'address';
  pin = '';
  pid = '';
  address = '';
  managementTeamMember: SelectOption | null = null;
  managementFileStatusCode = '';
  fileNameOrNumberOrReference = '';
  managementFilePurposeCode = '';
  projectNameOrNumber = '';

  toApi(): Api_ManagementFilter {
    const personMemberId =
      this.managementTeamMember?.value && String(this.managementTeamMember.value).startsWith('P-')
        ? String(this.managementTeamMember.value).substring(2)
        : null;
    const orgMemberId =
      this.managementTeamMember?.value && String(this.managementTeamMember.value).startsWith('O-')
        ? String(this.managementTeamMember.value).substring(2)
        : null;

    return {
      searchBy: this.searchBy,
      pin: this.pin,
      pid: this.pid,
      address: this.address,
      fileNameOrNumberOrReference: this.fileNameOrNumberOrReference,
      managementFileStatusCode: this.managementFileStatusCode,
      managementFilePurposeCode: this.managementFilePurposeCode,
      projectNameOrNumber: this.projectNameOrNumber,
      // management team members
      teamMemberPersonId:
        personMemberId && isNumber(+personMemberId) ? Number(personMemberId) : null,
      teamMemberOrganizationId: orgMemberId && isNumber(+orgMemberId) ? Number(orgMemberId) : null,
    };
  }

  static fromApi(
    base: Api_ManagementFilter,
    teamMemberOptions: SelectOption[] = [],
  ): ManagementFilterModel {
    const newModel = new ManagementFilterModel();
    newModel.searchBy = base.searchBy ?? 'address';
    newModel.pin = base.pin ?? '';
    newModel.pid = base.pid ?? '';
    newModel.address = base.address ?? '';
    newModel.fileNameOrNumberOrReference = base.fileNameOrNumberOrReference ?? '';
    newModel.managementFileStatusCode = base.managementFileStatusCode ?? '';
    newModel.managementFilePurposeCode = base.managementFilePurposeCode ?? '';
    newModel.projectNameOrNumber = base.projectNameOrNumber ?? '';
    // management team members
    newModel.managementTeamMember = base.teamMemberPersonId
      ? teamMemberOptions.find(c => c.value === `P-${base.teamMemberPersonId}`) ?? null
      : base.teamMemberOrganizationId
      ? teamMemberOptions.find(c => c.value === `O-${base.teamMemberOrganizationId}`) ?? null
      : null;

    return newModel;
  }
}

export class ManagementSearchResultModel {
  id: number | null = null;
  managementFileId: number | null = null;
  fileNumber = '';
  fileName = '';
  project: ApiGen_Concepts_Project | null = null;
  legacyFileNum = '';
  managementFileStatusTypeCode = '';
  managementFileProgramTypeCode = '';
  managementTeam: ApiGen_Concepts_ManagementFileTeam[] = [];
  fileProperties?: ApiGen_Concepts_ManagementFileProperty[] = [];

  static fromApi(base: ApiGen_Concepts_ManagementFile): ManagementSearchResultModel {
    const newModel = new ManagementSearchResultModel();
    newModel.id = base.id ?? null;
    newModel.managementFileId = base.id ?? null;
    newModel.fileNumber = base.fileNumber ?? '';
    newModel.fileName = base.fileName ?? '';
    newModel.project = base.project;
    newModel.legacyFileNum = base.legacyFileNum ?? '';
    newModel.managementFileStatusTypeCode = base.fileStatusTypeCode?.description ?? '';
    newModel.managementFileProgramTypeCode = base.programTypeCode?.description ?? '';
    newModel.managementTeam = base.managementTeam || [];
    newModel.fileProperties = base.fileProperties || [];

    return newModel;
  }
}
