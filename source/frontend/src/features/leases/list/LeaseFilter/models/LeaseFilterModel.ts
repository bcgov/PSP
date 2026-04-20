import { ILeaseFilter } from '@/features/leases/interfaces';
import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { ApiGen_Concepts_LeaseFileTeam } from '@/models/api/generated/ApiGen_Concepts_LeaseFileTeam';
import { formatApiPersonNames, getParameterIdFromOptions } from '@/utils/personUtils';

export class LeaseFilterModel {
  pin = '';
  pid = '';
  lFileNo = '';
  searchBy = '';
  historical = '';
  leaseTeamMembers: MultiSelectOption[] = [];
  programs: MultiSelectOption[] = [];
  expiryStartDate = '';
  leaseStatusTypes: MultiSelectOption[] = [];
  tenantName = '';
  expiryEndDate = '';
  details = '';
  isReceivable: string | null;
  regions: MultiSelectOption[] = [];

  constructor(initialRegions: MultiSelectOption[] = [], initialStatuses: MultiSelectOption[] = []) {
    this.regions = initialRegions;
    this.leaseStatusTypes = initialStatuses;
    this.searchBy = 'lFileNo';
  }

  toApi(): ILeaseFilter {
    const leaseTeamPersonId = getParameterIdFromOptions(this.leaseTeamMembers, 'P');
    const leaseTeamOrganizationId = getParameterIdFromOptions(this.leaseTeamMembers, 'O');

    return {
      pid: this.pid,
      pin: this.pin,
      lFileNo: this.lFileNo,
      historical: this.historical,
      searchBy: this.searchBy,
      leaseStatusTypes: this.leaseStatusTypes.map(x => x.id),
      tenantName: this.tenantName,
      programs: this.programs.map(x => x.id),
      expiryStartDate: this.expiryStartDate,
      expiryEndDate: this.expiryEndDate,
      details: this.details,
      leaseTeamPersonId: leaseTeamPersonId ? +leaseTeamPersonId : null,
      leaseTeamOrganizationId: leaseTeamOrganizationId ? +leaseTeamOrganizationId : null,
      isReceivable: this.isReceivable ?? null,
      regions: this.regions.map(x => x.id),
    };
  }

  static fromApi(
    base: ILeaseFilter,
    teamMembers: ApiGen_Concepts_LeaseFileTeam[],
    userRegions: MultiSelectOption[],
    initialStatuses: MultiSelectOption[],
  ): LeaseFilterModel {
    const newModel = new LeaseFilterModel();

    newModel.pin = base.pin;
    newModel.pid = base.pid;
    newModel.lFileNo = base.lFileNo;
    newModel.searchBy = base.searchBy;
    newModel.tenantName = base.tenantName;

    newModel.regions = userRegions ?? [];
    newModel.leaseStatusTypes = initialStatuses ?? [];

    if (base.leaseTeamPersonId) {
      const memberPerson = teamMembers.find(p => p.personId === Number(base.leaseTeamPersonId));

      newModel.leaseTeamMembers = [
        {
          id: `P-${memberPerson?.personId}`,
          text: formatApiPersonNames(memberPerson?.person),
        },
      ];
    }

    if (base.leaseTeamOrganizationId) {
      const memberOrganization = teamMembers.find(
        p => p.organizationId === Number(base.leaseTeamOrganizationId),
      );

      newModel.leaseTeamMembers = [
        {
          id: `O-${memberOrganization?.organizationId}`,
          text: `${memberOrganization?.organization?.name}`,
        },
      ];
    }

    return newModel;
  }
}
