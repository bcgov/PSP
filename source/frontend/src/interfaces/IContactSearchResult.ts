import { Api_Contact } from '@/models/api/Contact';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { Api_Organization } from '@/models/api/Organization';
import { Api_Person } from '@/models/api/Person';
import { formatApiPersonNames } from '@/utils/personUtils';

export interface IContactSearchResult {
  id: string;
  personId?: number;
  person?: Api_Person;
  organizationId?: number;
  organization?: Api_Organization;
  leaseTenantId?: number;
  isDisabled?: boolean;
  summary?: string;
  surname?: string | null;
  firstName?: string | null;
  middleNames?: string | null;
  organizationName?: string | null;
  email?: string;
  mailingAddress?: string;
  municipalityName?: string;
  provinceState?: string;
  provinceStateId?: number;
  note?: string;
  landline?: string;
  mobile?: string;
  tenantType?: string;
}

export function fromContact(baseModel: Api_Contact): IContactSearchResult {
  return {
    id: baseModel.id,
    personId: baseModel.person?.id,
    organizationId: baseModel.organization?.id,

    isDisabled: baseModel.person?.isDisabled || baseModel.organization?.isDisabled || false,
    summary: !!baseModel.person
      ? formatApiPersonNames(baseModel.person)
      : baseModel.organization?.name,
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

export function fromApiPerson(
  baseModel: ApiGen_Concepts_Person | Api_Person,
): IContactSearchResult {
  var personOrganizations =
    baseModel?.personOrganizations !== undefined ? baseModel.personOrganizations : undefined;

  var organization =
    personOrganizations && personOrganizations?.length > 0
      ? personOrganizations[0].organization
      : undefined;

  return {
    id: 'P' + baseModel?.id,
    personId: baseModel?.id,
    organizationId: organization?.id,
    isDisabled: baseModel?.isDisabled,
    summary: baseModel?.firstName + ' ' + baseModel?.surname,
    surname: baseModel?.surname,
    firstName: baseModel?.firstName,
    middleNames: baseModel?.middleNames,
    organizationName: organization?.name,
    email: '',
    mailingAddress: '',
    municipalityName: '',
    provinceState: '',
    provinceStateId: 0,
  };
}

export function fromApiOrganization(
  baseModel: ApiGen_Concepts_Organization | Api_Organization,
): IContactSearchResult {
  return {
    id: 'O' + baseModel.id,
    organizationId: baseModel.id,
    isDisabled: baseModel.isDisabled,
    summary: baseModel.name || '',
    organizationName: baseModel.name,
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

export function toPerson(baseModel?: IContactSearchResult): Api_Person | undefined {
  if (baseModel === undefined || baseModel.id.startsWith('O')) {
    return undefined;
  }
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

export function toOrganization(baseModel?: IContactSearchResult): Api_Organization | undefined {
  if (baseModel === undefined || baseModel.id.startsWith('P')) {
    return undefined;
  }
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
