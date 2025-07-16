import { ApiGen_Concepts_Contact } from '@/models/api/generated/ApiGen_Concepts_Contact';
import { ApiGen_Concepts_ContactSummary } from '@/models/api/generated/ApiGen_Concepts_ContactSummary';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { exists } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

interface PersonContactSummary
  extends Omit<
    ApiGen_Concepts_ContactSummary,
    'organizationId' | 'organization' | 'primaryOrgContact' | 'primaryOrgContactId'
  > {
  organizationId?: never;
  organization?: never;
  primaryOrgContact?: never;
  primaryOrgContactId?: never;
}

interface OrganizationContactSummary
  extends Omit<
    ApiGen_Concepts_ContactSummary,
    'personId' | 'person' | 'surname' | 'firstName' | 'middleNames'
  > {
  personId?: never;
  person?: never;
  surname?: never;
  firstName?: never;
  middleNames?: never;
}

export function isPersonSummary(
  contactResult: IContactSearchResult,
): contactResult is PersonContactSummary {
  return contactResult.id.startsWith('P') && contactResult.personId !== undefined;
}

export function isOrganizationSummary(
  contactResult: IContactSearchResult,
): contactResult is OrganizationContactSummary {
  return contactResult.id.startsWith('O');
}

export type IContactSearchResult = PersonContactSummary | OrganizationContactSummary;

export function fromContactSummary(
  baseModel: ApiGen_Concepts_ContactSummary,
): IContactSearchResult {
  //NOTE: this will display a person's org if they have one, it will not display an org's person as there may be many.
  const common = {
    id: baseModel.id,
    isDisabled: baseModel.isDisabled,
    summary: baseModel.summary,
    email: baseModel.email,
    mailingAddress: baseModel.mailingAddress,
    municipalityName: baseModel.municipalityName,
    provinceState: baseModel.provinceState,
    organizationName: baseModel.organizationName,
  };

  if (baseModel?.id?.startsWith('P') === true) {
    return {
      ...common,
      person: baseModel.person,
      personId: baseModel.personId,
      surname: baseModel.surname,
      firstName: baseModel.firstName,
      middleNames: baseModel.middleNames,
    };
  } else {
    return {
      ...common,
      organization: baseModel.organization,
      organizationId: baseModel.organizationId,
    };
  }
}

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
    organizationName:
      personOrganizations && personOrganizations.length > 0
        ? personOrganizations[0]?.organization?.name ?? undefined
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
  };
}

export function fromApiPersonOrApiOrganization(
  person: ApiGen_Concepts_Person | null,
  organization: ApiGen_Concepts_Organization | null,
): IContactSearchResult | null {
  if (exists(person)) {
    return fromApiPerson(person);
  } else if (exists(organization)) {
    return fromApiOrganization(organization);
  }
  return null;
}

export function toContact(baseModel: IContactSearchResult): ApiGen_Concepts_Contact {
  if (isPersonSummary(baseModel)) {
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
  if (baseModel === undefined || !isPersonSummary(baseModel)) {
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
    personOrganizations: baseModel.person?.personOrganizations ?? null,
    personAddresses: baseModel.person?.personAddresses ?? null,
    contactMethods: baseModel.person?.contactMethods ?? null,
    addressComment: null,
    birthDate: null,
    nameSuffix: null,
    managementActivityId: null,
    useOrganizationAddress: null,
  };
}

export function toOrganization(
  baseModel?: IContactSearchResult,
): ApiGen_Concepts_Organization | undefined {
  if (baseModel === undefined || isPersonSummary(baseModel)) {
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
    organizationPersons: baseModel.organization?.organizationPersons ?? null,
    organizationAddresses: baseModel.organization?.organizationAddresses ?? null,
    contactMethods: baseModel.organization?.contactMethods ?? null,
    districtCode: null,
    identifierTypeCode: null,
    organizationIdentifier: null,
    organizationTypeCode: null,
    parentOrganization: null,
    parentOrganizationId: null,
    regionCode: null,
    website: null,
  };
}
