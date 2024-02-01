import { ApiGen_Concepts_Contact } from '@/models/api/generated/ApiGen_Concepts_Contact';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { exists, isValidId } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

export interface IContactSearchResult {
  id: string;
  personId?: number;
  person?: ApiGen_Concepts_Person;
  organizationId?: number;
  organization?: ApiGen_Concepts_Organization;
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

export function fromContact(baseModel: ApiGen_Concepts_Contact): IContactSearchResult {
  return {
    id: baseModel.id ?? '',
    personId: baseModel.person?.id,
    organizationId: baseModel.organization?.id,

    isDisabled: baseModel.person?.isDisabled || baseModel.organization?.isDisabled || false,
    summary: !!baseModel.person
      ? formatApiPersonNames(baseModel.person)
      : baseModel.organization?.name ?? undefined,
    surname: baseModel.person?.surname ?? undefined,
    firstName: baseModel.person?.firstName ?? undefined,
    middleNames: baseModel.person?.middleNames ?? undefined,
    organizationName: baseModel.organization?.name ?? undefined,
    email: '',
    mailingAddress: '',
    municipalityName: '',
    provinceState: '',
    provinceStateId: 0,
  };
}

export function fromApiPerson(baseModel: ApiGen_Concepts_Person): IContactSearchResult {
  var personOrganizations = exists(baseModel?.personOrganizations)
    ? baseModel.personOrganizations
    : undefined;

  var organization =
    exists(personOrganizations) && personOrganizations.length > 0
      ? personOrganizations[0].organization
      : undefined;

  return {
    id: 'P' + baseModel?.id,
    personId: baseModel?.id,
    organizationId: organization?.id,
    isDisabled: baseModel?.isDisabled,
    summary: baseModel?.firstName + ' ' + baseModel?.surname,
    surname: baseModel?.surname ?? undefined,
    firstName: baseModel?.firstName ?? undefined,
    middleNames: baseModel?.middleNames ?? undefined,
    organizationName: organization?.name ?? undefined,
    email: '',
    mailingAddress: '',
    municipalityName: '',
    provinceState: '',
    provinceStateId: 0,
  };
}

export function fromApiOrganization(baseModel: ApiGen_Concepts_Organization): IContactSearchResult {
  return {
    id: 'O' + baseModel.id,
    organizationId: baseModel.id,
    isDisabled: baseModel.isDisabled,
    summary: baseModel.name || '',
    organizationName: baseModel.name ?? undefined,
    email: '',
    mailingAddress: '',
    municipalityName: '',
    provinceState: '',
    provinceStateId: 0,
  };
}

export function toContact(baseModel: IContactSearchResult): ApiGen_Concepts_Contact {
  if (baseModel.id.startsWith('P')) {
    return {
      id: baseModel.id,
      person: isValidId(baseModel.personId) ? toPerson(baseModel) : null,
      organization: null,
    };
  } else {
    return {
      id: baseModel.id,
      organization: isValidId(baseModel.organizationId) ? toOrganization(baseModel) : null,
      person: null,
    };
  }
}

export function toPerson(baseModel?: IContactSearchResult): ApiGen_Concepts_Person | null {
  if (baseModel === undefined || baseModel.id.startsWith('O')) {
    return null;
  }
  return {
    id: baseModel.personId || 0,
    firstName: baseModel.firstName || '',
    middleNames: baseModel.middleNames || '',
    surname: baseModel.surname || '',
    preferredName: '',
    isDisabled: baseModel.isDisabled ?? false,
    comment: '',
    rowVersion: 0,
    contactMethods: null,
    personAddresses: null,
    personOrganizations: null,
  };
}

export function toOrganization(
  baseModel?: IContactSearchResult,
): ApiGen_Concepts_Organization | null {
  if (baseModel === undefined || baseModel.id.startsWith('P')) {
    return null;
  }
  return {
    id: baseModel.organizationId || 0,
    name: baseModel.organizationName || '',
    isDisabled: baseModel.isDisabled ?? false,
    alias: '',
    comment: '',
    incorporationNumber: '',
    rowVersion: 0,
    contactMethods: null,
    organizationAddresses: null,
    organizationPersons: null,
  };
}
