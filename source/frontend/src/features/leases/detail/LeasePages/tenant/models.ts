import { ContactMethodTypes } from '@/constants/contactMethodType';
import {
  getApiPersonOrOrgMailingAddress,
  getDefaultContact,
} from '@/features/contacts/contactUtils';
import { IAddress, IContactSearchResult } from '@/interfaces';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { ApiGen_Concepts_LeaseTenant } from '@/models/api/generated/ApiGen_Concepts_LeaseTenant';
import { ApiGen_Concepts_OrganizationPerson } from '@/models/api/generated/ApiGen_Concepts_OrganizationPerson';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { getPreferredContactMethodValue } from '@/utils/contactMethodUtil';
import { fromTypeCode, toTypeCode, toTypeCodeNullable } from '@/utils/formUtils';
import { formatApiPersonNames } from '@/utils/personUtils';
import { exists, isValidId } from '@/utils/utils';

export class FormAddress {
  public readonly country?: string;
  public readonly streetAddress1?: string;
  public readonly streetAddress2?: string;
  public readonly streetAddress3?: string;
  public readonly municipality?: string;
  public readonly postal?: string;
  public readonly province?: string;

  constructor(baseModel: ApiGen_Concepts_Address | null) {
    this.province = baseModel?.province?.description ?? undefined;
    this.country = baseModel?.country?.description ?? undefined;
    this.streetAddress1 = baseModel?.streetAddress1 ?? undefined;
    this.streetAddress2 = baseModel?.streetAddress2 ?? undefined;
    this.streetAddress3 = baseModel?.streetAddress3 ?? undefined;
    this.municipality = baseModel?.municipality ?? undefined;
    this.postal = baseModel?.postal ?? undefined;
  }
}

export class FormTenant {
  public readonly id?: string;
  public readonly personId?: number;
  public readonly summary?: string;
  public readonly leaseId?: number;
  public readonly rowVersion?: number;
  public readonly leaseTenantId?: number;
  public readonly email?: string;
  public readonly mailingAddress?: FormAddress;
  public readonly municipalityName?: string;
  public readonly note?: string;
  public readonly organizationId?: number;
  public readonly landline?: string;
  public readonly mobile?: string;
  public readonly isDisabled?: boolean;
  public readonly organizationPersons?: ApiGen_Concepts_OrganizationPerson[];
  public readonly primaryContactId?: number;
  public readonly initialPrimaryContact?: ApiGen_Concepts_Person;
  public readonly lessorTypeCode?: ApiGen_Base_CodeType<string>;
  public readonly tenantType?: string;
  public readonly original?: IContactSearchResult;
  public readonly provinceState?: string;

  public static toContactSearchResult = (model: FormTenant): IContactSearchResult => {
    if (!model.id) {
      throw Error('Invalid tenant id');
    }

    if (model.original) {
      return model.original;
    }
    const contact: IContactSearchResult = {
      id: model.id,
      summary: model.summary,
      tenantType: model.tenantType,

      mailingAddress: model.mailingAddress?.streetAddress1,
      municipalityName: model.municipalityName,
      note: model.note,

      mobile: model.mobile,
      landline: model.landline,
      provinceState: model.provinceState,
    };

    if (model.personId) {
      contact.personId = model.personId;
    } else if (model.organizationId) {
      contact.organizationId = model.organizationId;
    }

    return contact;
  };

  public static toApi(model: FormTenant): ApiGen_Concepts_LeaseTenant {
    return {
      personId: model.personId ?? null,
      organizationId: !isValidId(model.personId) ? model.organizationId ?? null : null,
      lessorType: model.lessorTypeCode ?? null,
      tenantTypeCode: toTypeCodeNullable(model.tenantType),
      primaryContactId: !isValidId(model.personId) ? model.primaryContactId ?? null : null,
      note: model.note ?? null,
      leaseId: model.leaseId ?? 0,
      leaseTenantId: null,
      organization: null,
      person: null,
      primaryContact: null,
      ...getEmptyBaseAudit(model.rowVersion),
    };
  }

  constructor(apiModel?: ApiGen_Concepts_LeaseTenant, selectedContactModel?: IContactSearchResult) {
    if (exists(apiModel)) {
      // convert an api tenant to a form tenant.
      const tenant = apiModel.person ?? apiModel.organization;
      const address = tenant ? getApiPersonOrOrgMailingAddress(tenant) : null;
      this.id =
        apiModel.lessorType?.id === 'PER' ? `P${apiModel.personId}` : `O${apiModel.organizationId}`;
      this.personId = apiModel.personId ?? undefined;
      this.summary = exists(apiModel.person)
        ? formatApiPersonNames(apiModel.person)
        : apiModel.organization?.name ?? undefined;
      this.leaseId = apiModel.leaseId;
      this.rowVersion = apiModel.rowVersion ?? undefined;
      this.email =
        getPreferredContactMethodValue(
          tenant?.contactMethods,
          ContactMethodTypes.WorkEmail,
          ContactMethodTypes.PersonalEmail,
        ) ?? undefined;
      this.mailingAddress = new FormAddress(address);
      this.municipalityName = address?.municipality ?? '';
      this.note = apiModel.note ?? '';
      this.organizationPersons = apiModel?.organization?.organizationPersons ?? undefined;
      this.organizationId = apiModel.organizationId ?? undefined;
      this.landline =
        getPreferredContactMethodValue(tenant?.contactMethods, ContactMethodTypes.WorkPhone) ??
        undefined;
      this.mobile =
        getPreferredContactMethodValue(tenant?.contactMethods, ContactMethodTypes.WorkMobile) ??
        undefined;
      this.lessorTypeCode = apiModel.lessorType ?? undefined;
      this.tenantType = fromTypeCode(apiModel.tenantTypeCode) ?? undefined;
      this.primaryContactId = apiModel.primaryContactId ?? undefined;
      this.initialPrimaryContact = apiModel.primaryContact ?? undefined;
    } else if (exists(selectedContactModel)) {
      // In this case, construct a tenant using a contact.

      this.id = selectedContactModel?.id;

      this.summary = selectedContactModel.summary;
      this.email = selectedContactModel.email;
      this.mailingAddress = { streetAddress1: selectedContactModel.mailingAddress } as IAddress;
      this.municipalityName = selectedContactModel?.municipalityName;
      this.note = selectedContactModel.note;

      this.landline = selectedContactModel.landline;
      this.mobile = selectedContactModel.mobile;
      this.tenantType = selectedContactModel.tenantType;

      if (selectedContactModel.personId) {
        this.lessorTypeCode = toTypeCode('PER');
        this.personId = selectedContactModel.personId;
      } else {
        this.lessorTypeCode = toTypeCode('ORG');
        this.organizationId = selectedContactModel.organizationId;
        this.organizationPersons = selectedContactModel?.organization?.organizationPersons ?? [];

        const primaryContact = getDefaultContact(selectedContactModel.organization);
        this.primaryContactId = primaryContact?.id;
        this.initialPrimaryContact = primaryContact ?? undefined;
      }

      this.original = selectedContactModel;
      this.provinceState = selectedContactModel.provinceState;
    }
  }
}
