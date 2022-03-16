/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

/**
 * Provides a contact-oriented address model.
 */
export interface PimsApiModelsConceptsAddressModel {
  /** @format int64 */
  rowVersion?: number;

  /** @format date-time */
  appCreateTimestamp?: string;

  /** @format date-time */
  updatedOn?: string;
  updatedByName?: string | null;

  /**
   * get/set - The primary key to identify the address.
   * @format int64
   */
  id?: number;

  /** get/set - The street address. */
  streetAddress1?: string | null;

  /** get/set - The street address. */
  streetAddress2?: string | null;

  /** get/set - The street address. */
  streetAddress3?: string | null;

  /** get/set - The name of the municipality name. */
  municipality?: string | null;
  province?: PimsApiModelsConceptsProvinceStateModel;
  country?: PimsApiModelsConceptsCountryModel;

  /** get/set - The free-form value of country when country code is "Other". */
  countryOther?: string | null;

  /** get/set - The postal code. */
  postal?: string | null;
}

/**
 * Provides a contact-oriented contact method model.
 */
export interface PimsApiModelsConceptsContactMethodModel {
  /** @format int64 */
  rowVersion?: number;

  /** @format date-time */
  appCreateTimestamp?: string;

  /** @format date-time */
  updatedOn?: string;
  updatedByName?: string | null;

  /**
   * get/set - The primary key to identify the contact method.
   * @format int64
   */
  id?: number;

  /** Provides a  type model. */
  contactMethodType?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;

  /** get/set - The contact method value. */
  value?: string | null;
}

export interface PimsApiModelsConceptsCountryModel {
  /** @format int64 */
  rowVersion?: number;

  /** @format date-time */
  appCreateTimestamp?: string;

  /** @format date-time */
  updatedOn?: string;
  updatedByName?: string | null;

  /** @format int32 */
  countryId?: number;
  countryCode?: string | null;
  description?: string | null;
}

export interface PimsApiModelsConceptsOrganizationAddressModel {
  /** @format int64 */
  rowVersion?: number;

  /** @format date-time */
  appCreateTimestamp?: string;

  /** @format date-time */
  updatedOn?: string;
  updatedByName?: string | null;

  /**
   * get/set - The relationship id.
   * @format int64
   */
  id?: number;

  /** get/set - The relationship's disabled status flag. */
  isDisabled?: boolean;

  /** Provides a contact-oriented organization model. */
  organization?: PimsApiModelsConceptsOrganizationModel;

  /** Provides a contact-oriented address model. */
  address?: PimsApiModelsConceptsAddressModel;

  /** Provides a  type model. */
  addressUsageType?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;
}

/**
 * Provides a contact-oriented organization model.
 */
export interface PimsApiModelsConceptsOrganizationModel {
  /** @format int64 */
  rowVersion?: number;

  /** @format date-time */
  appCreateTimestamp?: string;

  /** @format date-time */
  updatedOn?: string;
  updatedByName?: string | null;

  /**
   * get/set - The organization's id.
   * @format int64
   */
  id?: number;

  /** get/set - The organization's disabled status flag. */
  isDisabled?: boolean;

  /** get/set - The organization's name. */
  name?: string | null;

  /** get/set - The organization's alias name. */
  alias?: string | null;

  /** get/set - The organization's incorporation number. */
  incorporationNumber?: string | null;

  /** get/set - The organization and person relationships. */
  personOrganizations?: PimsApiModelsConceptsPersonOrganizationModel[] | null;

  /** get/set - The organization's addresses. */
  organizationAddresses?: PimsApiModelsConceptsOrganizationAddressModel[] | null;

  /** get/set - The organization's contact methods. */
  contactMethods?: PimsApiModelsConceptsContactMethodModel[] | null;

  /** get/set - The organization's Comment. */
  comment?: string | null;
}

export interface PimsApiModelsConceptsPersonAddressModel {
  /** @format int64 */
  rowVersion?: number;

  /** @format date-time */
  appCreateTimestamp?: string;

  /** @format date-time */
  updatedOn?: string;
  updatedByName?: string | null;

  /**
   * get/set - The relationship id.
   * @format int64
   */
  id?: number;

  /** get/set - The relationship's disabled status flag. */
  isDisabled?: boolean;
  person?: PimsApiModelsConceptsPersonModel;

  /** Provides a contact-oriented address model. */
  address?: PimsApiModelsConceptsAddressModel;

  /** Provides a  type model. */
  addressUsageType?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;
}

export interface PimsApiModelsConceptsPersonModel {
  /** @format int64 */
  rowVersion?: number;

  /** @format date-time */
  appCreateTimestamp?: string;

  /** @format date-time */
  updatedOn?: string;
  updatedByName?: string | null;

  /**
   * get/set - The person's id.
   * @format int64
   */
  id?: number;

  /** get/set - The person's disabled status flag. */
  isDisabled?: boolean;

  /** get/set - The person's surname. */
  surname?: string | null;

  /** get/set - The person's first name. */
  firstName?: string | null;

  /** get/set - The person's middle names. */
  middleNames?: string | null;

  /** get/set - The person's preferred name. */
  preferredName?: string | null;

  /** get/set - The person's organizations. */
  personOrganizations?: PimsApiModelsConceptsPersonOrganizationModel[] | null;

  /** get/set - The person's addresses. */
  personAddresses?: PimsApiModelsConceptsPersonAddressModel[] | null;

  /** get/set - The person's contact methods. */
  contactMethods?: PimsApiModelsConceptsContactMethodModel[] | null;

  /** get/set - The person's Comment. */
  comment?: string | null;
}

export interface PimsApiModelsConceptsPersonOrganizationModel {
  /** @format int64 */
  rowVersion?: number;

  /** @format date-time */
  appCreateTimestamp?: string;

  /** @format date-time */
  updatedOn?: string;
  updatedByName?: string | null;
  person?: PimsApiModelsConceptsPersonModel;

  /** Provides a contact-oriented organization model. */
  organization?: PimsApiModelsConceptsOrganizationModel;

  /** get/set - True if the model is disabled. */
  isDisabled?: boolean;
}

export interface PimsApiModelsConceptsProvinceStateModel {
  /** @format int64 */
  rowVersion?: number;

  /** @format date-time */
  appCreateTimestamp?: string;

  /** @format date-time */
  updatedOn?: string;
  updatedByName?: string | null;

  /** @format int32 */
  provinceStateId?: number;
  provinceStateCode?: string | null;
  description?: string | null;
}

/**
 * Provides a  type model.
 */
export interface PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E {
  /** get/set - Primary key to identify type. */
  id?: string | null;

  /** get/set - A description of the type. */
  description?: string | null;

  /** get/set - Whether this code is disabled. */
  isDisabled?: boolean;

  /**
   * get/set - The type display order
   * @format int32
   */
  displayOrder?: number | null;
}
