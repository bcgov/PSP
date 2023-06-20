import { ContactMethodTypes } from '@/constants/contactMethodType';
import {
  getApiPersonOrOrgMailingAddress,
  getDefaultContact,
} from '@/features/contacts/contactUtils';
import { IAddress, IContactSearchResult } from '@/interfaces';
import ITypeCode from '@/interfaces/ITypeCode';
import { Api_Address } from '@/models/api/Address';
import { Api_LeaseTenant } from '@/models/api/LeaseTenant';
import { Api_OrganizationPerson } from '@/models/api/Organization';
import { Api_Person } from '@/models/api/Person';
import { getPreferredContactMethodValue } from '@/utils/contactMethodUtil';
import { fromTypeCode, toTypeCode } from '@/utils/formUtils';
import { formatApiPersonNames } from '@/utils/personUtils';

export class FormAddress {
  public readonly country?: string;
  public readonly streetAddress1?: string;
  public readonly streetAddress2?: string;
  public readonly streetAddress3?: string;
  public readonly municipality?: string;
  public readonly postal?: string;
  public readonly province?: string;

  constructor(baseModel?: Api_Address) {
    this.province = baseModel?.province?.description;
    this.country = baseModel?.country?.description;
    this.streetAddress1 = baseModel?.streetAddress1;
    this.streetAddress2 = baseModel?.streetAddress2;
    this.streetAddress3 = baseModel?.streetAddress3;
    this.municipality = baseModel?.municipality;
    this.postal = baseModel?.postal;
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
  public readonly organizationPersons?: Api_OrganizationPerson[];
  public readonly primaryContactId?: number;
  public readonly initialPrimaryContact?: Api_Person;
  public readonly lessorTypeCode?: ITypeCode<string>;
  public readonly tenantType?: string;
  public readonly original?: IContactSearchResult;
  public readonly provinceState?: string;

  public static toContactSearchResult = (model: FormTenant): IContactSearchResult => {
    if (!model.id) {
      throw Error('Invalid tenant id');
    }
    return (
      model.original ?? {
        id: model.id,
        personId: model.personId,
        summary: model.summary,
        tenantType: model.tenantType,
        organization: !!model.organizationId
          ? {
              id: model.organizationId,
              organizationPersons: model.organizationPersons,
            }
          : undefined,
        mailingAddress: model.mailingAddress?.streetAddress1,
        municipalityName: model.municipalityName,
        note: model.note,
        organizationId: model.organizationId,
        mobile: model.mobile,
        landline: model.landline,
        provinceState: model.provinceState,
      }
    );
  };

  public static toApi(model: FormTenant): Api_LeaseTenant {
    return {
      personId: model.personId,
      organizationId: model.organizationId,
      lessorType: model.lessorTypeCode,
      tenantTypeCode: toTypeCode(model.tenantType),
      primaryContactId: model.primaryContactId,
      note: model.note,
      rowVersion: model.rowVersion,
      leaseId: model.leaseId ?? 0,
    };
  }

  constructor(apiModel?: Api_LeaseTenant, selectedContactModel?: IContactSearchResult) {
    if (!!apiModel) {
      // convert an api tenant to a form tenant.
      const tenant = apiModel.person ?? apiModel.organization;
      const address = !!tenant ? getApiPersonOrOrgMailingAddress(tenant) : undefined;
      this.id =
        apiModel.lessorType?.id === 'PER' ? `P${apiModel.personId}` : `O${apiModel.organizationId}`;
      this.personId = apiModel.personId;
      this.summary = apiModel.person ? formatApiPersonNames(tenant) : apiModel.organization?.name;
      this.leaseId = apiModel.leaseId;
      this.rowVersion = apiModel.rowVersion;
      this.email = getPreferredContactMethodValue(
        tenant?.contactMethods,
        ContactMethodTypes.WorkEmail,
        ContactMethodTypes.PersonalEmail,
      );
      this.mailingAddress = new FormAddress(address);
      this.municipalityName = address?.municipality ?? '';
      this.note = apiModel.note ?? '';
      this.organizationPersons = apiModel?.organization?.organizationPersons;
      this.organizationId = apiModel.organizationId;
      this.landline = getPreferredContactMethodValue(
        tenant?.contactMethods,
        ContactMethodTypes.WorkPhone,
      );
      this.mobile = getPreferredContactMethodValue(
        tenant?.contactMethods,
        ContactMethodTypes.WorkMobile,
      );
      this.lessorTypeCode = apiModel.lessorType;
      this.tenantType = fromTypeCode(apiModel.tenantTypeCode);
      this.primaryContactId = apiModel.primaryContactId;
      this.initialPrimaryContact = apiModel.primaryContact;
    } else if (!!selectedContactModel) {
      // In this case, construct a tenant using a contact.
      const primaryContact = getDefaultContact(selectedContactModel.organization);
      this.id = selectedContactModel?.id;
      this.personId = selectedContactModel.personId;
      this.summary = selectedContactModel.summary;
      this.email = selectedContactModel.email;
      this.mailingAddress = { streetAddress1: selectedContactModel.mailingAddress } as IAddress;
      this.municipalityName = selectedContactModel?.municipalityName;
      this.note = selectedContactModel.note;
      this.organizationId = selectedContactModel.organizationId;
      this.landline = selectedContactModel.landline;
      this.mobile = selectedContactModel.mobile;
      this.lessorTypeCode = !!this.personId ? { id: 'PER' } : { id: 'ORG' };
      this.tenantType = selectedContactModel.tenantType;
      this.organizationPersons = selectedContactModel?.organization?.organizationPersons;
      this.primaryContactId = primaryContact?.id;
      this.initialPrimaryContact = primaryContact;
      this.original = selectedContactModel;
      this.provinceState = selectedContactModel.provinceState;
    }
  }
}
