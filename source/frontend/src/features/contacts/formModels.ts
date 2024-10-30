import { EmailContactMethods, PhoneContactMethods } from '@/constants/contactMethodType';
import { ApiGen_CodeTypes_AddressUsageTypes } from '@/models/api/generated/ApiGen_CodeTypes_AddressUsageTypes';
import { ApiGen_Concepts_ContactMethod } from '@/models/api/generated/ApiGen_Concepts_ContactMethod';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_OrganizationAddress } from '@/models/api/generated/ApiGen_Concepts_OrganizationAddress';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { ApiGen_Concepts_PersonAddress } from '@/models/api/generated/ApiGen_Concepts_PersonAddress';
import { ApiGen_Concepts_PersonOrganization } from '@/models/api/generated/ApiGen_Concepts_PersonOrganization';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { exists, isValidId } from '@/utils';
import {
  emptyStringtoNullable,
  fromTypeCode,
  stringToBoolean,
  toTypeCodeNullable,
} from '@/utils/formUtils';
import { formatApiPersonNames } from '@/utils/personUtils';

export interface IOrganizationLink {
  id: number;
  text: string;
}

export class IEditablePersonForm {
  id?: number;
  rowVersion?: number;
  surname = '';
  firstName: string;
  middleNames?: string;
  preferredName?: string;
  comment?: string;
  organization: IOrganizationLink | null = null;
  useOrganizationAddress = false;
  personOrganizationId?: number;
  personOrganizationRowVersion?: number;
  isDisabled: string | boolean = false;
  emailContactMethods: IEditableContactMethodForm[];
  phoneContactMethods: IEditableContactMethodForm[];
  mailingAddress: IEditablePersonAddressForm;
  propertyAddress: IEditablePersonAddressForm;
  billingAddress: IEditablePersonAddressForm;

  constructor() {
    this.isDisabled = false;
    this.firstName = '';
    this.middleNames = '';
    this.surname = '';
    this.preferredName = '';
    this.comment = '';
    this.organization = null;
    this.useOrganizationAddress = false;
    this.emailContactMethods = [new IEditableContactMethodForm()];
    this.phoneContactMethods = [new IEditableContactMethodForm()];
    this.mailingAddress = new IEditablePersonAddressForm(
      ApiGen_CodeTypes_AddressUsageTypes.MAILING,
    );
    this.propertyAddress = new IEditablePersonAddressForm(
      ApiGen_CodeTypes_AddressUsageTypes.RESIDNT,
    );
    this.billingAddress = new IEditablePersonAddressForm(
      ApiGen_CodeTypes_AddressUsageTypes.BILLING,
    );
  }

  static apiPersonToFormPerson(person?: ApiGen_Concepts_Person) {
    if (!person) {
      return undefined;
    }

    // exclude api-specific fields from form values
    const { personAddresses, contactMethods } = person;

    // split address array into sub-types: MAILING, RESIDENTIAL, BILLING
    const formAddresses =
      personAddresses?.map(IEditablePersonAddressForm.apiAddressToFormAddress).filter(exists) || [];

    // split contact methods array into phone and email values
    const formContactMethods =
      contactMethods
        ?.map(IEditableContactMethodForm.apiContactMethodToFormContactMethod)
        .filter(exists) || [];
    const emailContactMethods = formContactMethods.filter(isEmail);
    const phoneContactMethods = formContactMethods.filter(isPhone);

    const organizationLink = person.personOrganizations?.find(
      p => exists(p) && exists(p.organization),
    );

    const formPerson = new IEditablePersonForm();
    formPerson.id = person.id;
    formPerson.rowVersion = person.rowVersion ?? undefined;
    formPerson.surname = person.surname ?? '';
    formPerson.firstName = person.firstName ?? '';
    formPerson.middleNames = person.middleNames ?? undefined;
    formPerson.preferredName = person.preferredName ?? undefined;
    formPerson.comment = person.comment ?? undefined;

    formPerson.organization = exists(organizationLink?.organization)
      ? { id: organizationLink!.organization.id, text: organizationLink!.organization.name ?? '' }
      : null;
    formPerson.personOrganizationId = organizationLink?.id;
    formPerson.personOrganizationRowVersion = organizationLink?.rowVersion ?? undefined;

    formPerson.useOrganizationAddress = person.useOrganizationAddress ?? false;

    formPerson.isDisabled = person.isDisabled;

    formPerson.mailingAddress =
      formAddresses.find(a => a.addressTypeId === ApiGen_CodeTypes_AddressUsageTypes.MAILING) ??
      new IEditablePersonAddressForm(ApiGen_CodeTypes_AddressUsageTypes.MAILING);

    formPerson.propertyAddress =
      formAddresses.find(a => a.addressTypeId === ApiGen_CodeTypes_AddressUsageTypes.RESIDNT) ??
      new IEditablePersonAddressForm(ApiGen_CodeTypes_AddressUsageTypes.RESIDNT);
    formPerson.billingAddress =
      formAddresses.find(a => a.addressTypeId === ApiGen_CodeTypes_AddressUsageTypes.BILLING) ??
      new IEditablePersonAddressForm(ApiGen_CodeTypes_AddressUsageTypes.BILLING);
    formPerson.emailContactMethods =
      emailContactMethods.length > 0 ? emailContactMethods : [new IEditableContactMethodForm()];
    formPerson.phoneContactMethods =
      phoneContactMethods.length > 0 ? phoneContactMethods : [new IEditableContactMethodForm()];

    return formPerson;
  }

  formPersonToApiPerson(): ApiGen_Concepts_Person {
    const {
      mailingAddress,
      propertyAddress,
      billingAddress,
      emailContactMethods,
      phoneContactMethods,
    } = this;

    const personAddresses = [mailingAddress, propertyAddress, billingAddress]
      .filter(hasAddress)
      .map(a => a.formAddressToApiAddress());

    const contactMethods = [...emailContactMethods, ...phoneContactMethods]
      .filter(hasContactMethod)
      .map(a => a.formContactMethodToApiContactMethod());

    const personOrganization: ApiGen_Concepts_PersonOrganization | null = exists(
      this.organization?.id,
    )
      ? {
          id: this.personOrganizationId ?? 0,
          organization: null,
          organizationId: this.organization?.id ?? null,
          personId: this.id ?? null,
          rowVersion: this.personOrganizationRowVersion ?? null,
          person: null,
        }
      : null;

    const apiPerson: ApiGen_Concepts_Person = {
      id: this.id ?? 0,
      surname: this.surname,
      firstName: this.firstName,
      middleNames: this.middleNames ?? null,
      preferredName: this.preferredName ?? null,
      comment: this.comment ?? null,
      rowVersion: this.rowVersion ?? null,
      personOrganizations: exists(personOrganization) ? [personOrganization] : null,
      isDisabled: stringToBoolean(this.isDisabled),
      personAddresses,
      contactMethods,
      nameSuffix: null,
      birthDate: null,
      addressComment: null,
      useOrganizationAddress: this.useOrganizationAddress,
      propertyActivityId: null,
    };

    return apiPerson;
  }
}

export class IEditableOrganizationForm {
  id?: number;
  rowVersion?: number;
  name: string;
  alias?: string;
  incorporationNumber?: string;
  comment?: string;
  isDisabled: boolean;
  persons: Partial<ApiGen_Concepts_Person>[];
  emailContactMethods: IEditableContactMethodForm[];
  phoneContactMethods: IEditableContactMethodForm[];
  mailingAddress: IEditableOrganizationAddressForm;
  propertyAddress: IEditableOrganizationAddressForm;
  billingAddress: IEditableOrganizationAddressForm;

  constructor() {
    this.isDisabled = false;
    this.name = '';
    this.alias = '';
    this.incorporationNumber = '';
    this.comment = '';
    this.persons = [];
    this.emailContactMethods = [new IEditableContactMethodForm()];
    this.phoneContactMethods = [new IEditableContactMethodForm()];
    this.mailingAddress = new IEditableOrganizationAddressForm(
      ApiGen_CodeTypes_AddressUsageTypes.MAILING,
    );
    this.propertyAddress = new IEditableOrganizationAddressForm(
      ApiGen_CodeTypes_AddressUsageTypes.RESIDNT,
    );
    this.billingAddress = new IEditableOrganizationAddressForm(
      ApiGen_CodeTypes_AddressUsageTypes.BILLING,
    );
  }

  static apiOrganizationToFormOrganization(
    organization?: ApiGen_Concepts_Organization,
  ): IEditableOrganizationForm | undefined {
    if (!exists(organization)) {
      return undefined;
    }

    // exclude api-specific fields from form values
    const { organizationAddresses, contactMethods, organizationPersons } = organization;

    const formAddresses =
      organizationAddresses
        ?.map(IEditableOrganizationAddressForm.apiAddressToFormAddress)
        .filter(exists) || [];

    // split contact methods array into phone and email values
    const formContactMethods =
      contactMethods?.map(IEditableContactMethodForm.apiContactMethodToFormContactMethod) || [];
    const emailContactMethods = formContactMethods.filter(isEmail);
    const phoneContactMethods = formContactMethods.filter(isPhone);

    // Format person API values - need full names here
    const formPersonList: Partial<ApiGen_Concepts_Person>[] =
      organizationPersons
        ?.map(op => op.person)
        .filter(exists)
        .map(p => {
          return { id: p.id, fullName: formatApiPersonNames(p) };
        }) ?? [];

    const newForm = new IEditableOrganizationForm();

    newForm.id = organization.id;
    newForm.rowVersion = organization.rowVersion ?? undefined;
    newForm.name = organization.name ?? '';
    newForm.alias = organization.alias ?? '';
    newForm.incorporationNumber = organization.incorporationNumber ?? undefined;
    newForm.comment = organization.comment ?? undefined;
    newForm.isDisabled = organization.isDisabled;

    newForm.persons = formPersonList;
    newForm.mailingAddress =
      formAddresses.find(a => a.addressTypeId === ApiGen_CodeTypes_AddressUsageTypes.MAILING) ??
      new IEditableOrganizationAddressForm(ApiGen_CodeTypes_AddressUsageTypes.MAILING);
    newForm.propertyAddress =
      formAddresses.find(a => a.addressTypeId === ApiGen_CodeTypes_AddressUsageTypes.RESIDNT) ??
      new IEditableOrganizationAddressForm(ApiGen_CodeTypes_AddressUsageTypes.RESIDNT);
    newForm.billingAddress =
      formAddresses.find(a => a.addressTypeId === ApiGen_CodeTypes_AddressUsageTypes.BILLING) ??
      new IEditableOrganizationAddressForm(ApiGen_CodeTypes_AddressUsageTypes.BILLING);
    newForm.emailContactMethods =
      emailContactMethods.length > 0 ? emailContactMethods : [new IEditableContactMethodForm()];
    newForm.phoneContactMethods =
      phoneContactMethods.length > 0 ? phoneContactMethods : [new IEditableContactMethodForm()];

    return newForm;
  }

  formOrganizationToApiOrganization(): ApiGen_Concepts_Organization {
    const {
      mailingAddress,
      propertyAddress,
      billingAddress,
      emailContactMethods,
      phoneContactMethods,
    } = this;

    const organizationAddresses = [mailingAddress, propertyAddress, billingAddress]
      .filter(hasAddress)
      .map(a => a.formAddressToApiAddress());

    const contactMethods = [...emailContactMethods, ...phoneContactMethods]
      .filter(hasContactMethod)
      .map(a => a.formContactMethodToApiContactMethod());

    const apiOrganization: ApiGen_Concepts_Organization = {
      id: this.id ?? 0,
      alias: this.alias ?? null,
      comment: this.comment ?? null,
      incorporationNumber: this.incorporationNumber ?? null,
      name: this.name,
      rowVersion: this.rowVersion ?? null,
      isDisabled: stringToBoolean(this.isDisabled),
      organizationAddresses: organizationAddresses.length > 0 ? organizationAddresses : null,
      contactMethods,
      organizationPersons: null,
      parentOrganizationId: null,
      regionCode: null,
      districtCode: null,
      organizationTypeCode: null,
      identifierTypeCode: null,
      organizationIdentifier: null,
      website: null,
      parentOrganization: null,
    };

    return apiOrganization;
  }
}

export class IEditableContactMethodForm {
  id?: number;
  rowVersion?: number;
  personId?: number;
  organizationId?: number;
  contactMethodTypeCode: string;
  value: string;

  constructor() {
    this.contactMethodTypeCode = '';
    this.value = '';
  }

  formContactMethodToApiContactMethod(): ApiGen_Concepts_ContactMethod {
    return {
      id: this.id ?? 0,
      value: emptyStringtoNullable(this.value),
      contactMethodType: toTypeCodeNullable(this.contactMethodTypeCode),
      personId: this.personId ?? null,
      organizationId: this.organizationId ?? null,
      rowVersion: this.rowVersion ?? null,
    };
  }

  static apiContactMethodToFormContactMethod(
    contactMethod?: ApiGen_Concepts_ContactMethod,
  ): IEditableContactMethodForm | undefined {
    if (!contactMethod) {
      return undefined;
    }

    const newForm: IEditableContactMethodForm = new IEditableContactMethodForm();
    newForm.contactMethodTypeCode = fromTypeCode(contactMethod.contactMethodType) ?? '';
    newForm.id = contactMethod.id;
    newForm.rowVersion = contactMethod.rowVersion ?? undefined;
    newForm.value = contactMethod.value ?? '';
    newForm.personId = contactMethod.personId ?? undefined;
    newForm.organizationId = contactMethod.organizationId ?? undefined;
    return newForm;
  }
}

export class IEditablePersonAddressForm {
  id?: number | undefined;
  rowVersion?: number | undefined;
  streetAddress1 = '';
  streetAddress2?: string | undefined;
  streetAddress3?: string | undefined;
  municipality?: string | undefined;
  regionId?: number | undefined;
  districtId?: number | undefined;
  countryOther?: string | undefined;
  postal?: string | undefined;
  countryId: NumberFieldValue = '';
  provinceId: NumberFieldValue = '';
  addressTypeId = '';
  personId?: number;
  personAddressId?: number;
  personAddressRowVersion?: number;

  constructor(addressType: ApiGen_CodeTypes_AddressUsageTypes) {
    this.addressTypeId = addressType;
    this.streetAddress1 = '';
    this.streetAddress2 = '';
    this.streetAddress3 = '';
    this.municipality = '';
    this.countryOther = '';
    this.postal = '';
    this.countryId = '';
    this.provinceId = '';
  }

  static apiOrgAddressToFormAddress(
    personId?: number,
    orgAddress?: ApiGen_Concepts_OrganizationAddress,
  ) {
    if (!exists(orgAddress)) {
      return undefined;
    }

    const addressUsageTypeCode = orgAddress.addressUsageType?.id;
    const addressType: ApiGen_CodeTypes_AddressUsageTypes =
      ApiGen_CodeTypes_AddressUsageTypes[
        addressUsageTypeCode as keyof typeof ApiGen_CodeTypes_AddressUsageTypes
      ];

    const newForm = new IEditablePersonAddressForm(addressType);
    newForm.addressTypeId = fromTypeCode(orgAddress?.addressUsageType) ?? '';
    newForm.id = orgAddress.address?.id ?? undefined;
    newForm.rowVersion = orgAddress.address?.rowVersion ?? undefined;
    newForm.streetAddress1 = orgAddress.address?.streetAddress1 ?? '';
    newForm.streetAddress2 = orgAddress.address?.streetAddress2 ?? undefined;
    newForm.streetAddress3 = orgAddress.address?.streetAddress3 ?? undefined;
    newForm.municipality = orgAddress.address?.municipality ?? undefined;
    newForm.regionId = orgAddress.address?.regionCode ?? undefined;
    newForm.districtId = orgAddress.address?.districtCode ?? undefined;
    newForm.countryOther = orgAddress.address?.countryOther ?? undefined;
    newForm.postal = orgAddress.address?.postal ?? undefined;
    newForm.countryId = orgAddress.address?.countryId ?? '';
    newForm.provinceId = orgAddress.address?.provinceStateId ?? '';
    newForm.personId = personId;
    newForm.personAddressId = orgAddress.id;
    newForm.personAddressRowVersion = orgAddress.rowVersion ?? undefined;
    return newForm;
  }

  static apiAddressToFormAddress(personAddress?: ApiGen_Concepts_PersonAddress) {
    if (!exists(personAddress)) {
      return undefined;
    }

    const addressUsageTypeCode = personAddress.addressUsageType?.id;
    const addressType: ApiGen_CodeTypes_AddressUsageTypes =
      ApiGen_CodeTypes_AddressUsageTypes[
        addressUsageTypeCode as keyof typeof ApiGen_CodeTypes_AddressUsageTypes
      ];

    const newForm = new IEditablePersonAddressForm(addressType);
    newForm.addressTypeId = fromTypeCode(personAddress?.addressUsageType) ?? '';
    newForm.id = personAddress.address?.id ?? undefined;
    newForm.rowVersion = personAddress.address?.rowVersion ?? undefined;
    newForm.streetAddress1 = personAddress.address?.streetAddress1 ?? '';
    newForm.streetAddress2 = personAddress.address?.streetAddress2 ?? undefined;
    newForm.streetAddress3 = personAddress.address?.streetAddress3 ?? undefined;
    newForm.municipality = personAddress.address?.municipality ?? undefined;
    newForm.regionId = personAddress.address?.regionCode ?? undefined;
    newForm.districtId = personAddress.address?.districtCode ?? undefined;
    newForm.countryOther = personAddress.address?.countryOther ?? undefined;
    newForm.postal = personAddress.address?.postal ?? undefined;
    newForm.countryId = personAddress.address?.countryId ?? '';
    newForm.provinceId = personAddress.address?.provinceStateId ?? '';
    newForm.personId = personAddress.personId;
    newForm.personAddressId = personAddress.id;
    newForm.personAddressRowVersion = personAddress.rowVersion ?? undefined;
    return newForm;
  }

  formAddressToApiAddress(): ApiGen_Concepts_PersonAddress {
    return {
      id: this.personAddressId ?? 0,
      personId: this.personId ?? 0,
      rowVersion: this.personAddressRowVersion ?? null,
      addressUsageType: toTypeCodeNullable(this?.addressTypeId),
      address: {
        id: this.id ?? null,
        streetAddress1: this.streetAddress1,
        streetAddress2: this.streetAddress2 ?? null,
        streetAddress3: this.streetAddress3 ?? null,
        municipality: this.municipality ?? null,
        provinceStateId: isValidId(Number(this.provinceId)) ? Number(this.provinceId) : null,
        province: null,
        countryId: isValidId(Number(this.countryId)) ? Number(this.countryId) : null,
        country: null,
        district: null,
        districtCode: isValidId(this.districtId) ? this.districtId : null,
        region: null,
        regionCode: isValidId(this.regionId) ? this.regionId : null,
        countryOther: this.countryOther ?? null,
        postal: this.postal ?? null,
        latitude: null,
        longitude: null,
        comment: null,
        rowVersion: this.rowVersion ?? null,
      },
    };
  }
}

export class IEditableOrganizationAddressForm {
  id?: number | undefined;
  rowVersion?: number | undefined;
  streetAddress1 = '';
  streetAddress2?: string | undefined;
  streetAddress3?: string | undefined;
  municipality?: string | undefined;
  regionId?: number | undefined;
  districtId?: number | undefined;
  countryOther?: string | undefined;
  postal?: string | undefined;
  countryId: NumberFieldValue = '';
  provinceId: NumberFieldValue = '';
  addressTypeId = '';
  organizationId?: number;
  organizationAddressId?: number;
  organizationAddressRowVersion?: number;

  constructor(addressType: ApiGen_CodeTypes_AddressUsageTypes) {
    this.addressTypeId = addressType;
    this.streetAddress1 = '';
    this.streetAddress2 = '';
    this.streetAddress3 = '';
    this.municipality = '';
    this.countryOther = '';
    this.postal = '';
    this.countryId = '';
    this.provinceId = '';
  }

  static apiAddressToFormAddress(orgAddress?: ApiGen_Concepts_OrganizationAddress) {
    if (!exists(orgAddress)) {
      return undefined;
    }

    const addressUsageTypeCode = orgAddress.addressUsageType?.id;
    const addressType: ApiGen_CodeTypes_AddressUsageTypes =
      ApiGen_CodeTypes_AddressUsageTypes[
        addressUsageTypeCode as keyof typeof ApiGen_CodeTypes_AddressUsageTypes
      ];

    const newForm = new IEditableOrganizationAddressForm(addressType);
    newForm.addressTypeId = fromTypeCode(orgAddress?.addressUsageType) ?? '';
    newForm.id = orgAddress.address?.id ?? undefined;
    newForm.rowVersion = orgAddress.address?.rowVersion ?? undefined;
    newForm.streetAddress1 = orgAddress.address?.streetAddress1 ?? '';
    newForm.streetAddress2 = orgAddress.address?.streetAddress2 ?? undefined;
    newForm.streetAddress3 = orgAddress.address?.streetAddress3 ?? undefined;
    newForm.municipality = orgAddress.address?.municipality ?? undefined;
    newForm.regionId = orgAddress.address?.regionCode ?? undefined;
    newForm.districtId = orgAddress.address?.districtCode ?? undefined;
    newForm.countryOther = orgAddress.address?.countryOther ?? undefined;
    newForm.postal = orgAddress.address?.postal ?? undefined;
    newForm.countryId = orgAddress.address?.countryId ?? '';
    newForm.provinceId = orgAddress.address?.provinceStateId ?? '';
    newForm.organizationId = orgAddress.organizationId;
    newForm.organizationAddressId = orgAddress.id ?? undefined;
    newForm.organizationAddressRowVersion = orgAddress.rowVersion ?? undefined;
    return newForm;
  }

  formAddressToApiAddress(): ApiGen_Concepts_OrganizationAddress {
    return {
      id: this.organizationAddressId ?? 0,
      organizationId: this.organizationId ?? 0,
      rowVersion: this.organizationAddressRowVersion ?? null,
      addressUsageType: toTypeCodeNullable(this?.addressTypeId),
      address: {
        id: this.id ?? null,
        streetAddress1: this.streetAddress1,
        streetAddress2: this.streetAddress2 ?? null,
        streetAddress3: this.streetAddress3 ?? null,
        municipality: this.municipality ?? null,
        provinceStateId: isValidId(Number(this.provinceId)) ? Number(this.provinceId) : null,
        province: null,
        countryId: isValidId(Number(this.countryId)) ? Number(this.countryId) : null,
        country: null,
        district: null,
        districtCode: isValidId(this.districtId) ? this.districtId : null,
        region: null,
        regionCode: isValidId(this.regionId) ? this.regionId : null,
        countryOther: this.countryOther ?? null,
        postal: this.postal ?? null,
        latitude: null,
        longitude: null,
        comment: null,
        rowVersion: this.rowVersion ?? null,
      },
    };
  }
}

function hasContactMethod(formContactMethod?: IEditableContactMethodForm): boolean {
  if (!formContactMethod) {
    return false;
  }

  const { value, contactMethodTypeCode } = formContactMethod;
  return value !== '' && contactMethodTypeCode !== '';
}

function hasAddress<T extends IEditablePersonAddressForm | IEditableOrganizationAddressForm>(
  formAddress?: T,
): boolean {
  if (!formAddress) {
    return false;
  }

  const { streetAddress1, addressTypeId, countryId, municipality, postal } = formAddress;
  const parsedCountryId = parseInt(countryId.toString()) || 0;

  return (
    streetAddress1 !== '' &&
    addressTypeId !== '' &&
    municipality !== '' &&
    postal !== '' &&
    parsedCountryId > 0
  );
}

export const getDefaultContact = (organization?: {
  organizationPersons: ApiGen_Concepts_PersonOrganization[] | null;
}): ApiGen_Concepts_Person | null => {
  if (organization?.organizationPersons?.length === 1) {
    return organization.organizationPersons[0].person;
  }
  return null;
};

function isEmail(
  contactMethod?: IEditableContactMethodForm,
): contactMethod is IEditableContactMethodForm {
  return exists(contactMethod) && EmailContactMethods.includes(contactMethod.contactMethodTypeCode);
}

function isPhone(
  contactMethod?: IEditableContactMethodForm,
): contactMethod is IEditableContactMethodForm {
  return exists(contactMethod) && PhoneContactMethods.includes(contactMethod.contactMethodTypeCode);
}
