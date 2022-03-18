import { Api_Contact } from 'models/api/Contact';
import { Api_Organization } from 'models/api/Organization';
import { Api_Person } from 'models/api/Person';

export interface IContactSearchResult {
  id: string;
  personId?: number;
  organizationId?: number;
  leaseTenantId?: number;
  isDisabled: boolean;
  summary: string;
  surname?: string;
  firstName?: string;
  middleNames?: string;
  organizationName?: string;
  email: string;
  mailingAddress: string;
  municipalityName: string;
  provinceState: string;
  provinceStateId: number;
  note?: string;
}

export function fromContact(baseModel: Api_Contact): IContactSearchResult {
  return {
    id: baseModel.id,
    personId: baseModel.person?.id,
    organizationId: baseModel.organization?.id,

    isDisabled: baseModel.person?.isDisabled || baseModel.organization?.isDisabled || false,
    summary:
      baseModel.organization !== undefined
        ? baseModel.organization.name || ''
        : baseModel.person?.firstName + ' ' + baseModel.person?.surname,
    surname: baseModel.person?.surname,
    firstName: baseModel.person?.firstName,
    middleNames: baseModel.person?.middleNames,
    organizationName: baseModel.organization?.name,
    email: '',
    mailingAddress: '',
    municipalityName: '',
    provinceState: '',
    provinceStateId: 0,
  };
}

export function toContact(baseModel: IContactSearchResult): Api_Contact {
  if (baseModel.id.startsWith('P')) {
    return {
      id: baseModel.id,
      person: baseModel.personId !== undefined ? toPerson(baseModel) : undefined,
    };
  } else {
    return {
      id: baseModel.id,
      organization: baseModel.organizationId !== undefined ? toOrganization(baseModel) : undefined,
    };
  }
}

function toPerson(baseModel: IContactSearchResult): Api_Person {
  return {
    id: baseModel.personId || 0,
    firstName: baseModel.firstName || '',
    middleNames: baseModel.middleNames || '',
    surname: baseModel.surname || '',
    preferredName: '',
    isDisabled: baseModel.isDisabled,
    comment: '',
    rowVersion: 0,
  };
}

function toOrganization(baseModel: IContactSearchResult): Api_Organization {
  return {
    id: baseModel.organizationId || 0,
    name: baseModel.organizationName || '',
    isDisabled: baseModel.isDisabled,
    alias: '',
    comment: '',
    incorporationNumber: '',
    rowVersion: 0,
  };
}
