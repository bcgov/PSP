import { ApiGen_Concepts_Contact } from '@/models/api/generated/ApiGen_Concepts_Contact';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { formatApiPersonNames } from '@/utils/personUtils';

interface BaseContactResult {
  id: string;
  leaseTenantId?: number;
  isDisabled?: boolean;
  summary?: string;
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

interface PersonContactResult extends BaseContactResult {
  personId?: number;
  person?: ApiGen_Concepts_Person;

  surname?: string;
  firstName?: string;
  middleNames?: string;

  organizationName?: string;
  organizationId?: never;
  organization?: never;
  primaryOrgContact?: never;
  primaryOrgContactId?: never;
}

interface OrganizationContactResult extends BaseContactResult {
  personId?: never;
  person?: never;
  surname?: never;
  firstName?: never;
  middleNames?: never;

  organizationId?: number;
  organization?: ApiGen_Concepts_Organization;
  primaryOrgContact?: ApiGen_Concepts_Person;
  primaryOrgContactId?: number;
  organizationName?: string;
}

export function isPersonResult(
  contactResult: IContactSearchResult,
): contactResult is PersonContactResult {
  return contactResult.id.startsWith('P') && contactResult.personId !== undefined;
}

export type IContactSearchResult = PersonContactResult | OrganizationContactResult;

export function fromContact(baseModel: ApiGen_Concepts_Contact): IContactSearchResult {
  //NOTE: this will display a person's org if they have one, it will not display an org's person as there may be many.
  if (baseModel?.id?.startsWith('P') === true) {
    return {
      id: baseModel.id,
      person: baseModel.person ?? undefined,
      personId: baseModel.person?.id,
      isDisabled: baseModel.person?.isDisabled || false,
      summary: formatApiPersonNames(baseModel.person),
      surname: baseModel.person?.surname ?? undefined,
      firstName: baseModel.person?.firstName ?? undefined,
      middleNames: baseModel.person?.middleNames ?? undefined,
      email: '',
      mailingAddress: '',
      municipalityName: '',
      provinceState: '',
      provinceStateId: 0,
      organizationName: baseModel.organization?.name ?? undefined,
    };
  } else {
    return {
      id: baseModel.id ?? '',
      organization: baseModel.organization ?? undefined,
      organizationId: baseModel.organization?.id,
      isDisabled: baseModel.organization?.isDisabled || false,
      summary: baseModel.organization?.name ?? undefined,
      organizationName: baseModel.organization?.name ?? undefined,
      email: '',
      mailingAddress: '',
      municipalityName: '',
      provinceState: '',
      provinceStateId: 0,
    };
  }
}

export function fromApiPerson(baseModel: ApiGen_Concepts_Person): IContactSearchResult {
  const personOrganizations = baseModel?.personOrganizations;
  return {
    id: 'P' + baseModel?.id,
    personId: baseModel?.id,
    person: baseModel,
    isDisabled: baseModel?.isDisabled,
    summary: baseModel?.firstName + ' ' + baseModel?.surname,
    surname: baseModel?.surname ?? undefined,
    firstName: baseModel?.firstName ?? undefined,
    middleNames: baseModel?.middleNames ?? undefined,
    email: '',
    mailingAddress: '',
    municipalityName: '',
    provinceState: '',
    provinceStateId: 0,
    organizationName:
      personOrganizations && personOrganizations.length > 0
        ? personOrganizations[0].organization?.name ?? undefined
        : '',
  };
}

export function fromApiOrganization(baseModel: ApiGen_Concepts_Organization): IContactSearchResult {
  return {
    id: 'O' + baseModel.id,
    organizationId: baseModel.id,
    organization: baseModel,
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
  if (isPersonResult(baseModel)) {
    return {
      id: baseModel.id,
      person: baseModel.personId !== undefined ? toPerson(baseModel) ?? null : null,
      organization: null,
    };
  } else {
    return {
      id: baseModel.id,
      organization:
        baseModel.organizationId !== undefined ? toOrganization(baseModel) ?? null : null,
      person: null,
    };
  }
}

export function toPerson(baseModel?: IContactSearchResult): ApiGen_Concepts_Person | undefined {
  if (baseModel === undefined || !isPersonResult(baseModel)) {
    return undefined;
  }
  return {
    id: baseModel.personId || 0,
    firstName: baseModel.firstName || '',
    middleNames: baseModel.middleNames || '',
    surname: baseModel.surname || '',
    preferredName: '',
    isDisabled: !!baseModel.isDisabled,
    comment: '',
    rowVersion: 0,
    personOrganizations: baseModel.person?.personOrganizations || [],
    personAddresses: baseModel.person?.personAddresses || [],
    contactMethods: baseModel.person?.contactMethods || [],
  };
}

export function toOrganization(
  baseModel?: IContactSearchResult,
): ApiGen_Concepts_Organization | undefined {
  if (baseModel === undefined || isPersonResult(baseModel)) {
    return undefined;
  }
  return {
    id: baseModel.organizationId || 0,
    name: baseModel.organizationName || '',
    isDisabled: !!baseModel.isDisabled,
    alias: '',
    comment: '',
    incorporationNumber: '',
    rowVersion: 0,
    organizationPersons: baseModel.organization?.organizationPersons || [],
    organizationAddresses: baseModel.organization?.organizationAddresses || [],
    contactMethods: baseModel.organization?.contactMethods || [],
  };
}
