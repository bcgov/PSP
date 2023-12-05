import { Api_Contact } from '@/models/api/Contact';
import { Api_Organization } from '@/models/api/Organization';
import { Api_Person } from '@/models/api/Person';
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
  person?: Api_Person;

  surname?: string;
  firstName?: string;
  middleNames?: string;

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
  organization?: Api_Organization;
  primaryOrgContact?: Api_Person;
  primaryOrgContactId?: number;
  organizationName?: string;
}

export function isPersonResult(
  contactResult: IContactSearchResult,
): contactResult is PersonContactResult {
  return contactResult.id.startsWith('P') && contactResult.personId !== undefined;
}

export type IContactSearchResult = PersonContactResult | OrganizationContactResult;

export function fromContact(baseModel: Api_Contact): IContactSearchResult {
  // TODO: Api_Contact could contain valid organization even if its a person.
  //       However, it does not contain valid person information.
  if (baseModel.id.startsWith('P')) {
    return {
      id: baseModel.id,
      person: baseModel.person,
      personId: baseModel.person?.id,
      isDisabled: baseModel.person?.isDisabled || false,
      summary: formatApiPersonNames(baseModel.person),
      surname: baseModel.person?.surname,
      firstName: baseModel.person?.firstName,
      middleNames: baseModel.person?.middleNames,
      email: '',
      mailingAddress: '',
      municipalityName: '',
      provinceState: '',
      provinceStateId: 0,
    };
  } else {
    return {
      id: baseModel.id,
      organization: baseModel.organization,
      organizationId: baseModel.organization?.id,
      isDisabled: baseModel.organization?.isDisabled || false,
      summary: baseModel.organization?.name,
      organizationName: baseModel.organization?.name,
      email: '',
      mailingAddress: '',
      municipalityName: '',
      provinceState: '',
      provinceStateId: 0,
    };
  }
}

export function fromApiPerson(baseModel: Api_Person): IContactSearchResult {
  return {
    id: 'P' + baseModel?.id,
    personId: baseModel?.id,
    person: baseModel,
    isDisabled: baseModel?.isDisabled,
    summary: baseModel?.firstName + ' ' + baseModel?.surname,
    surname: baseModel?.surname,
    firstName: baseModel?.firstName,
    middleNames: baseModel?.middleNames,
    email: '',
    mailingAddress: '',
    municipalityName: '',
    provinceState: '',
    provinceStateId: 0,
  };
}

export function fromApiOrganization(baseModel: Api_Organization): IContactSearchResult {
  return {
    id: 'O' + baseModel.id,
    organizationId: baseModel.id,
    organization: baseModel,
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
  if (isPersonResult(baseModel)) {
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
  if (baseModel === undefined || !isPersonResult(baseModel)) {
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
  if (baseModel === undefined || isPersonResult(baseModel)) {
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
