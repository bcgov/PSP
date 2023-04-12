import { Api_Person } from 'models/api/Person';
export class GeneratePerson {
  given_name: string;
  middle_names: string;
  last_name: string;
  email: string;
  organizations: string;

  constructor(person: Api_Person | null | undefined) {
    this.given_name = person?.firstName ?? '';
    this.middle_names = person?.middleNames ?? '';
    this.last_name = person?.surname ?? '';
    const workEmail =
      person?.contactMethods?.filter(p => p.contactMethodType?.id === 'WORKEMAIL') ?? [];
    const personalEmail =
      person?.contactMethods?.filter(p => p.contactMethodType?.id === 'PERSEMAIL') ?? [];
    this.email =
      workEmail?.length > 0
        ? workEmail.map(p => p.value).join(', ')
        : personalEmail?.length > 0
        ? personalEmail.map(p => p.value).join(', ')
        : '';
    this.organizations =
      person?.personOrganizations?.map(o => o.organization?.name).join(', ') ?? '';
  }
}
