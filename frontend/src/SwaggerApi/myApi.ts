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

  /** A codified model. */
  province?: PimsApiModelsConceptsCodeTypeModel;

  /** A codified model. */
  country?: PimsApiModelsConceptsCodeTypeModel;

  /** A codified model. */
  district?: PimsApiModelsConceptsCodeTypeModel;

  /** A codified model. */
  region?: PimsApiModelsConceptsCodeTypeModel;

  /** get/set - The free-form value of country when country code is "Other". */
  countryOther?: string | null;

  /** get/set - The postal code. */
  postal?: string | null;

  /**
   * get/set - Addresss latitude coordinate.
   * @format double
   */
  latitude?: number | null;

  /**
   * get/set - Addresss longitude coordinate.
   * @format double
   */
  longitude?: number | null;

  /** get/set - Addresss comment. */
  comment?: string | null;
}

/**
 * A codified model.
 */
export interface PimsApiModelsConceptsCodeTypeModel {
  /**
   * get/set - The primary key to identify code type.
   * @format int32
   */
  id?: number;

  /** get/set - The model's code type. */
  code?: string | null;

  /** get/set - The description or long name. */
  description?: string | null;

  /**
   * get/set - The display order
   * @format int32
   */
  displayOrder?: number | null;
}

/**
 * Provides a Contact method model.
 */
export interface PimsApiModelsConceptsContactMethodModel {
  /** @format int64 */
  rowVersion?: number;

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

export interface PimsApiModelsConceptsCoordinateModel {
  /**
   * get/set - The value on the X axis.
   * @format double
   */
  x?: number;

  /**
   * get/set - The value on the y axis.
   * @format double
   */
  y?: number;
}

export interface PimsApiModelsConceptsGeometryModel {
  coordinate?: PimsApiModelsConceptsCoordinateModel;
}

export interface PimsApiModelsConceptsOrganizationAddressModel {
  /** @format int64 */
  rowVersion?: number;

  /**
   * get/set - The relationship id.
   * @format int64
   */
  id?: number;

  /** get/set - The relationship's disabled status flag. */
  isDisabled?: boolean;

  /**
   * get/set - The organization id associated with the address.
   * @format int64
   */
  organizationId?: number;

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
  organizationPersons?: PimsApiModelsConceptsOrganizationPersonModel[] | null;

  /** get/set - The organization's addresses. */
  organizationAddresses?: PimsApiModelsConceptsOrganizationAddressModel[] | null;

  /** get/set - The organization's contact methods. */
  contactMethods?: PimsApiModelsConceptsContactMethodModel[] | null;

  /** get/set - The organization's Comment. */
  comment?: string | null;
}

export interface PimsApiModelsConceptsOrganizationPersonModel {
  /** @format int64 */
  rowVersion?: number;
  person?: PimsApiModelsConceptsPersonModel;

  /**
   * get/set - The relationship organization id.
   * @format int64
   */
  organizationId?: number;

  /** get/set - True if the model is disabled. */
  isDisabled?: boolean;
}

export interface PimsApiModelsConceptsPersonAddressModel {
  /** @format int64 */
  rowVersion?: number;

  /**
   * get/set - The relationship id.
   * @format int64
   */
  id?: number;

  /** get/set - The relationship's disabled status flag. */
  isDisabled?: boolean;

  /**
   * get/set - The person id associated with the address.
   * @format int64
   */
  personId?: number;

  /** Provides a contact-oriented address model. */
  address?: PimsApiModelsConceptsAddressModel;

  /** Provides a  type model. */
  addressUsageType?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;
}

export interface PimsApiModelsConceptsPersonModel {
  /** @format int64 */
  rowVersion?: number;

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

  /**
   * get/set - The relationship person id.
   * @format int64
   */
  personId?: number;

  /** Provides a contact-oriented organization model. */
  organization?: PimsApiModelsConceptsOrganizationModel;

  /** get/set - True if the model is disabled. */
  isDisabled?: boolean;
}

/**
 * PropertyModel class, provides a model to represent the property whether Land or Building.
 */
export interface PimsApiModelsConceptsPropertyModel {
  /** @format int64 */
  rowVersion?: number;

  /**
   * get/set - The primary key to identify the property.
   * @format int64
   */
  id?: number;

  /** Provides a  type model. */
  propertyType?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;

  /** get/set - The property anomalies. */
  anomalies?:
    | PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E[]
    | null;

  /** get/set - The tenure description. */
  tenure?:
    | PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E[]
    | null;

  /** get/set - The road type description. */
  roadType?:
    | PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E[]
    | null;

  /** get/set - The adjacent land description. */
  adjacentLand?:
    | PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E[]
    | null;

  /** Provides a  type model. */
  status?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;

  /** Provides a  type model. */
  dataSource?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;

  /** Provides a  type model. */
  region?: PimsApiModelsTypeModel1SystemInt16SystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;

  /**
   * get/set - The data source effective date
   * @format date-time
   */
  dataSourceEffectiveDate?: string;

  /**
   * get/set - The GIS latitude location of the property.
   * @format double
   */
  latitude?: number | null;

  /**
   * get/set - The GIS latitude location of the property.
   * @format double
   */
  longitude?: number | null;

  /** get/set - The property name. */
  name?: string | null;

  /** get/set - The property description. */
  description?: string | null;

  /** get/set - Whether the property is sensitive data. */
  isSensitive?: boolean;

  /** get/set - Whether the property is a provincial highway. */
  isProvincialPublicHwy?: boolean | null;

  /** Provides a contact-oriented address model. */
  address?: PimsApiModelsConceptsAddressModel;

  /**
   * get/set - A unique identifier for the titled parcel.
   * @format int32
   */
  pid?: number | null;

  /**
   * get/set - A unique identifier for an untitled parcel.
   * @format int32
   */
  pin?: number | null;

  /** Provides a  type model. */
  areaUnit?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;

  /**
   * get/set - The land area of the parcel.
   * @format float
   */
  landArea?: number;

  /** get/set - Whether the property is a volumetric parcel. */
  isVolumetricParcel?: boolean | null;

  /**
   * get/set - The volumetric measurement of the parcel. Only applies if IsVolumetricParcel is true.
   * @format float
   */
  volumetricMeasurement?: number;

  /** Provides a  type model. */
  volumetricUnit?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;

  /** Provides a  type model. */
  volumetricType?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;

  /** get/set - The land legal description of the parcel. */
  landLegalDescription?: string | null;

  /** get/set - The property municipal zoning name. */
  municipalZoning?: string | null;

  /** get/set - The property zoning name. */
  zoning?: string | null;

  /** get/set - The property zoning potential. */
  zoningPotential?: string | null;
  location?: PimsApiModelsConceptsGeometryModel;

  /** A codified model. */
  district?: PimsApiModelsConceptsCodeTypeModel;

  /** get/set - The property notes. */
  notes?: string | null;
}

export interface PimsApiModelsConceptsResearchFileModel {
  /** @format int64 */
  rowVersion?: number;

  /** @format date-time */
  appCreateTimestamp?: string;

  /** @format date-time */
  appLastUpdateTimestamp?: string;
  appLastUpdateUserid?: string | null;
  appCreateUserid?: string | null;

  /**
   * get/set - The model id.
   * @format int64
   */
  id?: number;

  /** get/set - The research file name. */
  name?: string | null;
  roadName?: string | null;
  roadAlias?: string | null;

  /** get/set - The R-File number for this research file. */
  rfileNumber?: string | null;

  /** Provides a  type model. */
  statusTypeCode?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;

  /** get/set - A list of research property relationships. */
  researchProperties?: PimsApiModelsConceptsResearchFilePropertyModel[] | null;

  /** @format date-time */
  requestDate?: string | null;
  requestDescription?: string | null;
  requestSourceDescription?: string | null;
  researchResult?: string | null;

  /** @format date-time */
  researchCompletionDate?: string | null;
  isExpropriation?: boolean | null;
  expropriationNotes?: string | null;

  /** Provides a  type model. */
  requestSourceType?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;
  requestorPerson?: PimsApiModelsConceptsPersonModel;

  /** Provides a contact-oriented organization model. */
  requestorOrganization?: PimsApiModelsConceptsOrganizationModel;
  pimsResearchFilePurposes?: PimsApiModelsConceptsResearchFilePurposeModel[] | null;
}

export interface PimsApiModelsConceptsResearchFilePropertyModel {
  /** @format int64 */
  rowVersion?: number;

  /**
   * get/set - The relationship id.
   * @format int64
   */
  id?: number;

  /** get/set - The name of the property for this research file. */
  propertyName?: string | null;

  /** get/set - The relationship's disabled status flag. */
  isDisabled?: boolean;

  /**
   * get/set - The order to display the relationship.
   * @format int32
   */
  displayOrder?: number | null;

  /** PropertyModel class, provides a model to represent the property whether Land or Building. */
  property?: PimsApiModelsConceptsPropertyModel;
  researchFile?: PimsApiModelsConceptsResearchFileModel;
}

export interface PimsApiModelsConceptsResearchFilePurposeModel {
  /** @format int64 */
  rowVersion?: number;

  /** @format date-time */
  appCreateTimestamp?: string;

  /** @format date-time */
  appLastUpdateTimestamp?: string;
  appLastUpdateUserid?: string | null;
  appCreateUserid?: string | null;

  /** get/set - Research file purpose id. */
  id?: string | null;

  /** Provides a  type model. */
  researchPurposeTypeCode?: PimsApiModelsTypeModel1SystemStringSystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E;
}

/**
 * Provides a  type model.
 */
export interface PimsApiModelsTypeModel1SystemInt16SystemPrivateCoreLibVersion6000CultureNeutralPublicKeyToken7Cec85D7Bea7798E {
  /**
   * get/set - Primary key to identify type.
   * @format int32
   */
  id?: number;

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
