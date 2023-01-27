/**
 *
 * @export
 * @interface Altos1ChargeCorrection
 */
export interface Altos1ChargeCorrection {
  /**
   * System generated correction number
   * @type {string}
   * @memberof Altos1ChargeCorrection
   */
  number: string;
  /**
   * Free text
   * @type {string}
   * @memberof Altos1ChargeCorrection
   */
  referenceDescription: string;
  /**
   * Entered date and time
   * @type {Date}
   * @memberof Altos1ChargeCorrection
   */
  enteredDate?: Date;
  /**
   * Correction date and time
   * @type {Date}
   * @memberof Altos1ChargeCorrection
   */
  correctionDate?: Date;
  /**
   * Previous correction number
   * @type {string}
   * @memberof Altos1ChargeCorrection
   */
  previousCorrectionNumber?: string;
  /**
   * Charge correction text
   * @type {string}
   * @memberof Altos1ChargeCorrection
   */
  correctionText: string;
}
/**
 *
 * @export
 * @interface Altos1LegalNotationCorrection
 */
export interface Altos1LegalNotationCorrection {
  /**
   * Correction number
   * @type {string}
   * @memberof Altos1LegalNotationCorrection
   */
  number: string;
  /**
   * Free text
   * @type {string}
   * @memberof Altos1LegalNotationCorrection
   */
  referenceDescription: string;
  /**
   * Entered date and time
   * @type {Date}
   * @memberof Altos1LegalNotationCorrection
   */
  enteredDate: Date;
  /**
   * Correction date and time
   * @type {Date}
   * @memberof Altos1LegalNotationCorrection
   */
  correctionDate: Date;
  /**
   * Previous correction number
   * @type {string}
   * @memberof Altos1LegalNotationCorrection
   */
  previousCorrectionNumber?: string;
  /**
   * Charge correction text
   * @type {string}
   * @memberof Altos1LegalNotationCorrection
   */
  correctionText: string;
}
/**
 *
 * @export
 * @interface Altos1TitleCorrection
 */
export interface Altos1TitleCorrection {
  /**
   * System generated correction number
   * @type {string}
   * @memberof Altos1TitleCorrection
   */
  number: string;
  /**
   * Free text
   * @type {string}
   * @memberof Altos1TitleCorrection
   */
  referenceDescription: string;
  /**
   * Entered date and time
   * @type {Date}
   * @memberof Altos1TitleCorrection
   */
  enteredDate?: Date;
  /**
   * Correction date and time
   * @type {Date}
   * @memberof Altos1TitleCorrection
   */
  correctionDate?: Date;
  /**
   * Previous correction number
   * @type {string}
   * @memberof Altos1TitleCorrection
   */
  previousCorrectionNumber?: string;
  /**
   * Text of title correction
   * @type {string}
   * @memberof Altos1TitleCorrection
   */
  correctionText?: string;
}
/**
 *
 * @export
 * @interface AssociatedPlan
 */
export interface AssociatedPlan {
  /**
   * Type of the associated plan
   * @type {string}
   * @memberof AssociatedPlan
   */
  planType: AssociatedPlan.PlanTypeEnum;
  /**
   * Plan number of the associated plan
   * @type {string}
   * @memberof AssociatedPlan
   */
  planNumber: string;
}

/**
 * @export
 * @namespace AssociatedPlan
 */
export namespace AssociatedPlan {
  /**
   * @export
   * @enum {string}
   */
  export enum PlanTypeEnum {
    PLAN = 'PLAN' as any,
    BARELANDSTRATAPLAN = 'BARELAND STRATA PLAN' as any,
    AIRSPACEPLAN = 'AIRSPACE PLAN' as any,
    BLOCKOUTLINEPLAN = 'BLOCK OUTLINE PLAN' as any,
    BYLAWPLAN = 'BYLAW PLAN' as any,
    CONSOLIDATIONPLAN = 'CONSOLIDATION PLAN' as any,
    CROWNGRANTPLAN = 'CROWN GRANT PLAN' as any,
    DEDICATIONPLAN = 'DEDICATION PLAN' as any,
    ENBLOCKPLAN = 'E&N BLOCK PLAN' as any,
    EASEMENTPLAN = 'EASEMENT PLAN' as any,
    HIGHWAYPLAN = 'HIGHWAY PLAN' as any,
    KEYPLAN = 'KEY PLAN' as any,
    LEASEPLAN = 'LEASE PLAN' as any,
    LEASEOFPARTOFBUILDINGPLAN = 'LEASE OF PART OF BUILDING PLAN' as any,
    MISCELLANEOUSPLAN = 'MISCELLANEOUS PLAN' as any,
    OFFICIALSURVEYPLAN = 'OFFICIAL SURVEY PLAN' as any,
    REFERENCEPLAN = 'REFERENCE PLAN' as any,
    RESTRICTIVECOVENANTPLAN = 'RESTRICTIVE COVENANT PLAN' as any,
    RIGHTOFWAYPLAN = 'RIGHT OF WAY PLAN' as any,
    ROADPLAN = 'ROAD PLAN' as any,
    STATUTORYRIGHTOFWAYPLAN = 'STATUTORY RIGHT OF WAY PLAN' as any,
    STRATAPLAN = 'STRATA PLAN' as any,
    SUBDIVISIONPLAN = 'SUBDIVISION PLAN' as any,
    WELLSITEPLAN = 'WELL SITE PLAN' as any,
    PLANTYPENOTIDENTIFIED = 'PLAN TYPE NOT IDENTIFIED' as any,
    REFERENCEWITHDEDICATION = 'REFERENCE WITH DEDICATION' as any,
    COVENANTPLAN = 'COVENANT PLAN' as any,
    POSTINGPLAN = 'POSTING PLAN' as any,
  }
}
/**
 * Indicate the billing information for the order Read-only for third party software
 * @export
 * @interface BillingInfo
 */
export interface BillingInfo {
  /**
   * Billing model as per myLTSA deposit account setting # One of: * REG – Regular account * TAX – Local government tax-purpose account * PROV – Provincial government account
   * @type {string}
   * @memberof BillingInfo
   */
  billingModel?: BillingInfo.BillingModelEnum;
  /**
   * Product name
   * @type {string}
   * @memberof BillingInfo
   */
  productName?: string;
  /**
   * Product code
   * @type {string}
   * @memberof BillingInfo
   */
  productCode?: string;
  /**
   * Indicate if the billing transaction is fee exempted or not
   * @type {boolean}
   * @memberof BillingInfo
   */
  feeExempted?: boolean;
  /**
   * Product fee
   * @type {number}
   * @memberof BillingInfo
   */
  productFee?: number;
  /**
   * Tax on service charge
   * @type {number}
   * @memberof BillingInfo
   */
  serviceCharge?: number;
  /**
   * Product fee + service charge
   * @type {number}
   * @memberof BillingInfo
   */
  subtotalFee?: number;
  /**
   * Tax on product fee
   * @type {number}
   * @memberof BillingInfo
   */
  productFeeTax?: number;
  /**
   * Tax on service charge
   * @type {number}
   * @memberof BillingInfo
   */
  serviceChargeTax?: number;
  /**
   * Tax on product fee + tax on service charge
   * @type {number}
   * @memberof BillingInfo
   */
  totalTax?: number;
  /**
   * Subtotal fee + total tax
   * @type {number}
   * @memberof BillingInfo
   */
  totalFee?: number;
}

/**
 * @export
 * @namespace BillingInfo
 */
export namespace BillingInfo {
  /**
   * @export
   * @enum {string}
   */
  export enum BillingModelEnum {
    REG = 'REG' as any,
    TAX = 'TAX' as any,
    PROV = 'PROV' as any,
  }
}
/**
 * Canadian Province
 * @export
 * @enum {string}
 */
export enum CanadianProvince {
  ABALBERTA = 'AB - ALBERTA' as any,
  BCBRITISHCOLUMBIA = 'BC - BRITISH COLUMBIA' as any,
  MBMANITOBA = 'MB - MANITOBA' as any,
  NBNEWBRUNSWICK = 'NB - NEW BRUNSWICK' as any,
  NFNEWFOUNDLAND = 'NF - NEWFOUNDLAND' as any,
  NSNOVASCOTIA = 'NS - NOVA SCOTIA' as any,
  NTNORTHWESTTERRITORIES = 'NT - NORTHWEST TERRITORIES' as any,
  ONONTARIO = 'ON - ONTARIO' as any,
  PEPRINCEEDWARDISLAND = 'PE - PRINCE EDWARD ISLAND' as any,
  PQQUEBEC = 'PQ - QUEBEC' as any,
  SKSASKATCHEWAN = 'SK - SASKATCHEWAN' as any,
  YTYUKON = 'YT - YUKON' as any,
}
/**
 *
 * @export
 * @interface CertificateDelivery
 */
export interface CertificateDelivery {
  /**
   * Free text used to store recipient and/or delivery information for converted ALTOS1 certificates.
   * @type {string}
   * @memberof CertificateDelivery
   */
  certificateText?: string;
  /**
   * The Last Name of the person who is going to hold (be responsible for) the certificate.
   * @type {string}
   * @memberof CertificateDelivery
   */
  intendedRecipientsurname: string;
  /**
   * The Given Name of the person who is going to hold (be responsible for) the certificate.
   * @type {string}
   * @memberof CertificateDelivery
   */
  intendedRecipientGivenName?: string;
  /**
   *
   * @type {Array<OwnerAddress>}
   * @memberof CertificateDelivery
   */
  address?: Array<OwnerAddress>;
}
/**
 *
 * @export
 * @interface CertificateIdentifier
 */
export interface CertificateIdentifier {
  /**
   * Certificate Number
   * @type {string}
   * @memberof CertificateIdentifier
   */
  documentNumber: string;
  /**
   *
   * @type {LandTitleDistrictCode}
   * @memberof CertificateIdentifier
   */
  documentDistrictCode: LandTitleDistrictCode;
}
/**
 *
 * @export
 * @interface CertificateOfCharge
 */
export interface CertificateOfCharge {
  /**
   * Certificate Number
   * @type {string}
   * @memberof CertificateOfCharge
   */
  number: string;
  /**
   * Type of certificate
   * @type {string}
   * @memberof CertificateOfCharge
   */
  type: string;
  /**
   * The date that the certificate is deemed to have been issued.
   * @type {Date}
   * @memberof CertificateOfCharge
   */
  issuedDate: Date;
  /**
   * The date that the certificate was surrended.
   * @type {Date}
   * @memberof CertificateOfCharge
   */
  surrenderDate?: Date;
}
/**
 *
 * @export
 * @interface Charge
 */
export interface Charge {
  /**
   * In practice, the same value as the Application number that raised the Charge.
   * @type {string}
   * @memberof Charge
   */
  chargeNumber: string;
  /**
   * Approximately 80 types currently active. They indicate the nature of the charge.
   * @type {string}
   * @memberof Charge
   */
  transactionType: string;
  /**
   * The Received Date and Time of the Application that raised the Charge. It is synonymous with the Registration Date/Time and comes from the Originating Document where is recorded as Document Acceptance Date/Time.
   * @type {Date}
   * @memberof Charge
   */
  applicationReceivedDate?: Date;
  /**
   *
   * @type {Array<ChargeOwnershipGroup>}
   * @memberof Charge
   */
  chargeOwnershipGroups: Array<ChargeOwnershipGroup>;
  /**
   *
   * @type {Array<CertificateOfCharge>}
   * @memberof Charge
   */
  certificatesOfCharge?: Array<CertificateOfCharge>;
  /**
   *
   * @type {Array<Altos1ChargeCorrection>}
   * @memberof Charge
   */
  correctionsAltos1?: Array<Altos1ChargeCorrection>;
  /**
   *
   * @type {Array<ChargeCorrection>}
   * @memberof Charge
   */
  corrections?: Array<ChargeCorrection>;
}
/**
 *
 * @export
 * @interface ChargeCorrection
 */
export interface ChargeCorrection {
  /**
   * Charge correction reason
   * @type {string}
   * @memberof ChargeCorrection
   */
  reason: string;
  /**
   * The Application that initiated the Charge correction
   * @type {string}
   * @memberof ChargeCorrection
   */
  originatingCorrectionApplication: string;
  /**
   * Entered date and time
   * @type {Date}
   * @memberof ChargeCorrection
   */
  enteredDate: Date;
}
/**
 *
 * @export
 * @interface ChargeOnTitle
 */
export interface ChargeOnTitle {
  /**
   * Charge Number
   * @type {string}
   * @memberof ChargeOnTitle
   */
  chargeNumber: string;
  /**
   * The current state of the charge in relationship with the title.  Only active Charges are presented on Current View of Title. Note: Pending Release - appears that this state is used only as an examination aid, it is not publicly shown, and appears to control ALTOS behavior so that the Examiner can see which charge they are about to release.
   * @type {string}
   * @memberof ChargeOnTitle
   */
  status: ChargeOnTitle.StatusEnum;
  /**
   * Records the way the Charge was canceled. \"I - Inactive\", \"M - Merged\", \"R - Released\"
   * @type {string}
   * @memberof ChargeOnTitle
   */
  cancellationType?: ChargeOnTitle.CancellationTypeEnum;
  /**
   * Date when the Charge status was changed from Pending to Registered.  This data is only for internal purposes, it does not appear on a Title View.
   * @type {Date}
   * @memberof ChargeOnTitle
   */
  enteredDate?: Date;
  /**
   * Latin for \"\"Among Others\"\", It is set to \"\"Yes\"\" when the specified charge impacts more than one title. However, ALTOS does not reset to \"\"No\"\" when the number of titles impacted are reduced to one # Note: The business does not want to automatically change this Charge Impact Remark on Title when number of titles impacted are reduced to one.
   * @type {string}
   * @memberof ChargeOnTitle
   */
  interAlia: ChargeOnTitle.InterAliaEnum;
  /**
   * Charge remarks
   * @type {string}
   * @memberof ChargeOnTitle
   */
  chargeRemarks: string;
  /**
   *
   * @type {ChargeRelease}
   * @memberof ChargeOnTitle
   */
  chargeRelease?: ChargeRelease;
  /**
   *
   * @type {Charge}
   * @memberof ChargeOnTitle
   */
  charge: Charge;
}

/**
 * @export
 * @namespace ChargeOnTitle
 */
export namespace ChargeOnTitle {
  /**
   * @export
   * @enum {string}
   */
  export enum StatusEnum {
    CANCELLED = 'CANCELLED' as any,
    REGISTERED = 'REGISTERED' as any,
  }
  /**
   * @export
   * @enum {string}
   */
  export enum CancellationTypeEnum {
    I = 'I' as any,
    M = 'M' as any,
    R = 'R' as any,
  }
  /**
   * @export
   * @enum {string}
   */
  export enum InterAliaEnum {
    YES = 'YES' as any,
    NO = 'NO' as any,
  }
}
/**
 *
 * @export
 * @interface ChargeOwnershipGroup
 */
export interface ChargeOwnershipGroup {
  /**
   * Indicator for joint tenancy
   * @type {boolean}
   * @memberof ChargeOwnershipGroup
   */
  jointTenancyIndication: boolean;
  /**
   * The Transfer of Charge Ownership application that initiated the creation of this charge ownership group.
   * @type {string}
   * @memberof ChargeOwnershipGroup
   */
  creatingApplicationNumber?: string;
  /**
   * Indicates the state with respect to the Transfer of Charge Ownership lifecycle.
   * @type {string}
   * @memberof ChargeOwnershipGroup
   */
  creatingApplicationStatus?: ChargeOwnershipGroup.CreatingApplicationStatusEnum;
  /**
   * The Transfer of Charge Ownership application that cancelled this charge ownership group.
   * @type {string}
   * @memberof ChargeOwnershipGroup
   */
  cancellingApplicationNumber?: string;
  /**
   * Indicates the state with respect to the Transfer of Charge Ownership lifecycle.
   * @type {string}
   * @memberof ChargeOwnershipGroup
   */
  cancellingApplicationStatus?: ChargeOwnershipGroup.CancellingApplicationStatusEnum;
  /**
   * Interest Fraction Numerator. It is used if not 1 (1/1), assumed to be equal division amongst ownership groups unless specified otherwise.
   * @type {string}
   * @memberof ChargeOwnershipGroup
   */
  interestFractionNumerator?: string;
  /**
   * Interest Fraction Denominator. It is used if not 1 (1/1), assumed to be equal division amongst ownership groups unless specified otherwise.
   * @type {string}
   * @memberof ChargeOwnershipGroup
   */
  interestFractionDenominator?: string;
  /**
   * Charge Ownership Remarks
   * @type {string}
   * @memberof ChargeOwnershipGroup
   */
  ownershipRemarks?: string;
  /**
   *
   * @type {Array<ChargeOwnershipGroupChargeOwner>}
   * @memberof ChargeOwnershipGroup
   */
  chargeOwners: Array<ChargeOwnershipGroupChargeOwner>;
}

/**
 * @export
 * @namespace ChargeOwnershipGroup
 */
export namespace ChargeOwnershipGroup {
  /**
   * @export
   * @enum {string}
   */
  export enum CreatingApplicationStatusEnum {
    Draft = 'Draft' as any,
    Registered = 'Registered' as any,
  }
  /**
   * @export
   * @enum {string}
   */
  export enum CancellingApplicationStatusEnum {
    Draft = 'Draft' as any,
    Registered = 'Registered' as any,
  }
}
/**
 *
 * @export
 * @interface ChargeOwnershipGroupChargeOwner
 */
export interface ChargeOwnershipGroupChargeOwner {
  /**
   * Free text containing either the Owner Last Name or the Corporate Name.
   * @type {string}
   * @memberof ChargeOwnershipGroupChargeOwner
   */
  surnameOrCorpName1: string;
  /**
   * Provides additional space for storing the Owner Last Name or the Corporate Name
   * @type {string}
   * @memberof ChargeOwnershipGroupChargeOwner
   */
  surnameOrCorpName2?: string;
  /**
   * Owner Given Name; this attribute is empty if the Owner is a corporation.
   * @type {string}
   * @memberof ChargeOwnershipGroupChargeOwner
   */
  givenName?: string;
  /**
   * Incorporation Number; this is required only for BC Corporations.
   * @type {string}
   * @memberof ChargeOwnershipGroupChargeOwner
   */
  incorporationNumber?: string;
}
/**
 *
 * @export
 * @interface ChargeRelease
 */
export interface ChargeRelease {
  /**
   * The Document bearing the application that caused the charge to be canceled on this Title.
   * @type {string}
   * @memberof ChargeRelease
   */
  documentNumber?: string;
  /**
   * The acceptance date and time of the document of the charge release.
   * @type {Date}
   * @memberof ChargeRelease
   */
  documentAcceptanceDate?: Date;
}
/**
 *
 * @export
 * @interface ChargesOnStrataCommonProperty
 */
export interface ChargesOnStrataCommonProperty {
  /**
   * The current status of the charge in relationship with the Strata Common Property.  Only active Charges are presented on Current View of Title.
   * @type {string}
   * @memberof ChargesOnStrataCommonProperty
   */
  status: ChargesOnStrataCommonProperty.StatusEnum;
  /**
   * Records the way the Charge was canceled. \"I - Inactive\", \"M - Merged\", \"R - Released\"
   * @type {string}
   * @memberof ChargesOnStrataCommonProperty
   */
  cancellationType?: ChargesOnStrataCommonProperty.CancellationTypeEnum;
  /**
   * Date and Time when the Charge status was changed from Pending to Registered.  This data is only for internal purposes.
   * @type {Date}
   * @memberof ChargesOnStrataCommonProperty
   */
  enteredDate?: Date;
  /**
   * Latin for \"Among Others\", It is set to \"Yes\" when the specified charge impacts more than one Strata Common Property. However, it is never reset to \"No\" when the number of Strata Common Properties impacted are reduced to one #Note: The business does not want to automatically change this Charge Impact Remark on Strata Common Property when number of Strata Common Properties impacted are reduced to one.
   * @type {boolean}
   * @memberof ChargesOnStrataCommonProperty
   */
  interAlia: boolean;
  /**
   * Charge Number
   * @type {string}
   * @memberof ChargesOnStrataCommonProperty
   */
  chargeNumber: string;
  /**
   * Charge remarks
   * @type {string}
   * @memberof ChargesOnStrataCommonProperty
   */
  chargeRemarks: string;
  /**
   *
   * @type {Charge}
   * @memberof ChargesOnStrataCommonProperty
   */
  charge: Charge;
  /**
   *
   * @type {ChargeRelease}
   * @memberof ChargesOnStrataCommonProperty
   */
  chargeRelease?: ChargeRelease;
}

/**
 * @export
 * @namespace ChargesOnStrataCommonProperty
 */
export namespace ChargesOnStrataCommonProperty {
  /**
   * @export
   * @enum {string}
   */
  export enum StatusEnum {
    CANCELLED = 'CANCELLED' as any,
    REGISTERED = 'REGISTERED' as any,
  }
  /**
   * @export
   * @enum {string}
   */
  export enum CancellationTypeEnum {
    I = 'I' as any,
    M = 'M' as any,
    R = 'R' as any,
  }
}
/**
 *
 * @export
 * @interface DescriptionOfLand
 */
export interface DescriptionOfLand {
  /**
   * Parcel Identifier (PID) of Parcel to which Title is held.
   * @type {string}
   * @memberof DescriptionOfLand
   */
  parcelIdentifier: string;
  /**
   * The full legal description of the parcel.
   * @type {string}
   * @memberof DescriptionOfLand
   */
  fullLegalDescription: string;
  /**
   * Parcel Status
   * @type {string}
   * @memberof DescriptionOfLand
   */
  parcelStatus: DescriptionOfLand.ParcelStatusEnum;
}

/**
 * @export
 * @namespace DescriptionOfLand
 */
export namespace DescriptionOfLand {
  /**
   * @export
   * @enum {string}
   */
  export enum ParcelStatusEnum {
    AActive = 'A - Active' as any,
    IInactive = 'I - Inactive' as any,
  }
}
/**
 *
 * @export
 * @interface DocOrPlanOrder
 */
export interface DocOrPlanOrder extends OrderParent {
  /**
   *
   * @type {DocumentOrPlanOrderParameters}
   * @memberof DocOrPlanOrder
   */
  productOrderParameters?: DocumentOrPlanOrderParameters;
}

/**
 * @export
 * @namespace DocOrPlanOrder
 */
export namespace DocOrPlanOrder {}
/**
 *
 * @export
 * @interface DocOrPlanSummary
 */
export interface DocOrPlanSummary {
  /**
   * Document or plan number
   * @type {string}
   * @memberof DocOrPlanSummary
   */
  docOrPlanNumber?: string;
  /**
   * Document district (e.g. Victoria) represents the Land Title Office that received the document for processing.
   * @type {string}
   * @memberof DocOrPlanSummary
   */
  documentDistrict?: string;
  /**
   *
   * @type {LandTitleDistrictCode}
   * @memberof DocOrPlanSummary
   */
  documentDistrictCode?: LandTitleDistrictCode;
  /**
   * Status of the document or plan. Note: Only document or plan with the status “Found” can be ordered via this interface.
   * @type {string}
   * @memberof DocOrPlanSummary
   */
  status?: DocOrPlanSummary.StatusEnum;
}

/**
 * @export
 * @namespace DocOrPlanSummary
 */
export namespace DocOrPlanSummary {
  /**
   * @export
   * @enum {string}
   */
  export enum StatusEnum {
    Found = 'Found.' as any,
    ObtainAtLandTitleOffice = 'Obtain at Land Title Office.' as any,
    PlanWillBeScanned = 'Plan will be scanned.' as any,
    DocumentWillBeScanned = 'Document will be scanned.' as any,
  }
}
/**
 * Parameters required for ordering a Document or Plan
 * @export
 * @interface DocumentOrPlanOrderParameters
 */
export interface DocumentOrPlanOrderParameters {
  /**
   * Document or plan number # Note: All plan numbers must include the plan’s prefix and/or suffix
   * @type {string}
   * @memberof DocumentOrPlanOrderParameters
   */
  docOrPlanNumber: string;
  /**
   * Optional remarks that describe the order
   * @type {string}
   * @memberof DocumentOrPlanOrderParameters
   */
  orderRemarks?: string;
  /**
   * Indicate whether to include the Plan Certification page for an electronic plan.  This parameter will be ignored if the ordered product is not an electronic plan.
   * @type {boolean}
   * @memberof DocumentOrPlanOrderParameters
   */
  includePlanCertificationPage?: boolean;
  /**
   *
   * @type {LandTitleDistrictCode}
   * @memberof DocumentOrPlanOrderParameters
   */
  documentDistrictCode?: LandTitleDistrictCode;
}
/**
 *
 * @export
 * @interface DuplicateCertificate
 */
export interface DuplicateCertificate {
  /**
   * The date that the certificate is deemed to have been issued.
   * @type {Date}
   * @memberof DuplicateCertificate
   */
  issuedDate: Date;
  /**
   * The date that the certificate was surrended.
   * @type {Date}
   * @memberof DuplicateCertificate
   */
  surrenderDate?: Date;
  /**
   *
   * @type {CertificateIdentifier}
   * @memberof DuplicateCertificate
   */
  certificateIdentifier: CertificateIdentifier;
  /**
   *
   * @type {CertificateDelivery}
   * @memberof DuplicateCertificate
   */
  certificateDelivery: CertificateDelivery;
}
/**
 * Geographical district determined by location of associated Parcel(s). Needed to identify Titles issued prior introduction of province-wide unique Title Number in 1990s. (e.g. Victoria)
 * @export
 * @enum {string}
 */
export enum LandTitleDistrict {
  KAMLOOPS = 'KAMLOOPS' as any,
  LOWERMAINLAND = 'LOWER MAINLAND' as any,
  NELSON = 'NELSON' as any,
  NEWWESTMINSTER = 'NEW WESTMINSTER' as any,
  PRINCEGEORGE = 'PRINCE GEORGE' as any,
  PRINCERUPERT = 'PRINCE RUPERT' as any,
  VANCOUVER = 'VANCOUVER' as any,
  VICTORIA = 'VICTORIA' as any,
}
/**
 * Land title district code (e.g. VI)
 * @export
 * @enum {string}
 */
export enum LandTitleDistrictCode {
  KA = 'KA' as any,
  LM = 'LM' as any,
  NE = 'NE' as any,
  NW = 'NW' as any,
  PG = 'PG' as any,
  PR = 'PR' as any,
  VA = 'VA' as any,
  VI = 'VI' as any,
}
/**
 *
 * @export
 * @interface LegalDescription
 */
export interface LegalDescription {
  /**
   * Full legal description
   * @type {string}
   * @memberof LegalDescription
   */
  fullLegalDescription: string;
  /**
   *
   * @type {Array<SubdividedShortLegalDescription>}
   * @memberof LegalDescription
   */
  subdividedShortLegals?: Array<SubdividedShortLegalDescription>;
  /**
   *
   * @type {Array<UnsubdividedShortLegalDescription>}
   * @memberof LegalDescription
   */
  unsubdividedShortLegals?: Array<UnsubdividedShortLegalDescription>;
}
/**
 *
 * @export
 * @interface LegalNotation
 */
export interface LegalNotation {
  /**
   * The Received Date of the Application that raised the Legal Notation. It is synonymous with the Registration Date/Time and comes from the Originating Document where is recorded as Document Acceptance Date/Time.
   * @type {Date}
   * @memberof LegalNotation
   */
  applicationReceivedDate?: Date;
  /**
   * When present, this is the legal notation being modified or cancelled by the current legal notation. Modifications appear on current and historical titles. Both Modification and Cancellation add Legal Notation text to Title. Cancellation text (and the text of the original Legal Notation) are only seen on the historical title, not the current title.
   * @type {string}
   * @memberof LegalNotation
   */
  originalLegalNotationNumber?: string;
  /**
   * Legal Notation Plan Identifier, where one exists.
   * @type {string}
   * @memberof LegalNotation
   */
  planIdentifier?: string;
  /**
   * Legal Notation Text
   * @type {string}
   * @memberof LegalNotation
   */
  legalNotationText: string;
  /**
   *
   * @type {Array<Altos1LegalNotationCorrection>}
   * @memberof LegalNotation
   */
  correctionsAltos1?: Array<Altos1LegalNotationCorrection>;
  /**
   *
   * @type {Array<LegalNotationCorrection>}
   * @memberof LegalNotation
   */
  corrections?: Array<LegalNotationCorrection>;
}
/**
 *
 * @export
 * @interface LegalNotationCorrection
 */
export interface LegalNotationCorrection {
  /**
   * Legal Notation correction reason
   * @type {string}
   * @memberof LegalNotationCorrection
   */
  reason?: string;
  /**
   * The Application that initiated the Title correction
   * @type {string}
   * @memberof LegalNotationCorrection
   */
  originatingCorrectionApplication: string;
  /**
   * Date and Time when the correction was applied to Legal Notation
   * @type {Date}
   * @memberof LegalNotationCorrection
   */
  enteredDate?: Date;
}
/**
 *
 * @export
 * @interface LegalNotationOnTitle
 */
export interface LegalNotationOnTitle {
  /**
   * Legal Notation Number
   * @type {string}
   * @memberof LegalNotationOnTitle
   */
  legalNotationNumber: string;
  /**
   * State of the Legal Notation on Title. A Legal Notation is cancelled by a subsequent \"\"CAN\"\" Legal Notation. A Legal Notation may be modified by a subsequent \"\"MOD\"\" Legal Notation. This relationship is captured by the Legal Notation Entity. Modifications and Cancellations are Title specific.
   * @type {string}
   * @memberof LegalNotationOnTitle
   */
  status: string;
  /**
   * Legal Notation Cancellation Date - only appears on a historic view of the title.
   * @type {string}
   * @memberof LegalNotationOnTitle
   */
  cancellationDate?: string;
  /**
   *
   * @type {LegalNotation}
   * @memberof LegalNotationOnTitle
   */
  legalNotation: LegalNotation;
}
/**
 *
 * @export
 * @interface LegalNotationsOnStrataCommonProperty
 */
export interface LegalNotationsOnStrataCommonProperty {
  /**
   * Legal Notation Number
   * @type {string}
   * @memberof LegalNotationsOnStrataCommonProperty
   */
  legalNotationNumber: string;
  /**
   * State of the Legal Notation on Title.  Only active Legal Notation are presented on Current View of Title.
   * @type {string}
   * @memberof LegalNotationsOnStrataCommonProperty
   */
  status: LegalNotationsOnStrataCommonProperty.StatusEnum;
  /**
   * Legal Notation Cancellation Date
   * @type {Date}
   * @memberof LegalNotationsOnStrataCommonProperty
   */
  cancellationDate?: Date;
  /**
   *
   * @type {LegalNotation}
   * @memberof LegalNotationsOnStrataCommonProperty
   */
  legalNotation: LegalNotation;
}

/**
 * @export
 * @namespace LegalNotationsOnStrataCommonProperty
 */
export namespace LegalNotationsOnStrataCommonProperty {
  /**
   * @export
   * @enum {string}
   */
  export enum StatusEnum {
    ACTIVE = 'ACTIVE' as any,
    CANCELLED = 'CANCELLED' as any,
  }
}
/**
 *
 * @export
 * @interface ModelError
 */
export interface ModelError {
  /**
   *
   * @type {string}
   * @memberof ModelError
   */
  errorMessages?: string;
}
/**
 *
 * @export
 * @interface NatureOfTransfer
 */
export interface NatureOfTransfer {
  /**
   * Record the 1st Nature of Transfer. Sample values: * CROWN GRANT * CHANGE OF NAME * CROWN GRANT * ESCHEAT etc. Captured for benefit of Assessors.
   * @type {string}
   * @memberof NatureOfTransfer
   */
  transferReason: string;
}
/**
 * An Order
 * @export
 * @interface Order
 */
export interface Order {}
/**
 * Order Id for cancellation
 * @export
 * @interface OrderCancellation
 */
export interface OrderCancellation {
  /**
   *
   * @type {string}
   * @memberof OrderCancellation
   */
  orderId: string;
}
/**
 * Order cancellation record
 * @export
 * @interface OrderCancellationResponse
 */
export interface OrderCancellationResponse {
  /**
   *
   * @type {string}
   * @memberof OrderCancellationResponse
   */
  orderId?: string;
  /**
   *
   * @type {string}
   * @memberof OrderCancellationResponse
   */
  orderCancellationID?: string;
  /**
   *
   * @type {string}
   * @memberof OrderCancellationResponse
   */
  status?: string;
}
/**
 *
 * @export
 * @interface OrderParent
 */
export interface OrderParent {
  /**
   * Indicate the type of the search product requested for an order
   * @type {string}
   * @memberof OrderParent
   */
  productType: OrderParent.ProductTypeEnum;
  /**
   * This file reference will show in myLTSA account statements to identify the order transaction
   * @type {string}
   * @memberof OrderParent
   */
  fileReference?: string;
  /**
   * System generated unique identifier assigned to the order Read-only for third party software
   * @type {string}
   * @memberof OrderParent
   */
  orderId?: string;
  /**
   * Indicate the status for the order # One of:  1. Processing - Order has been created and is being processed.  Both the fielded data (JSON) and the PDF are not yet available. 1. Fulfilled - Order has been partially fulfilled.  Fielded data (JSON) is available, but PDF is not yet available. 1. Completed - Order has been fully completed.  PDF is available.  Fielded data (JSON) is available (as applicable for the product type). 1. Cancelled - Order has been cancelled. The ordered product is not available for a cancelled order.  Third party software may cancel an outstanding order by changing this status to “Cancelled” via the Put Order service.
   * @type {string}
   * @memberof OrderParent
   */
  status?: OrderParent.StatusEnum;
  /**
   *
   * @type {BillingInfo}
   * @memberof OrderParent
   */
  billingInfo?: BillingInfo;
}

/**
 * @export
 * @namespace OrderParent
 */
export namespace OrderParent {
  /**
   * @export
   * @enum {string}
   */
  export enum ProductTypeEnum {
    Title = 'title' as any,
    CommonProperty = 'commonProperty' as any,
    ParcelInfo = 'parcelInfo' as any,
    DocOrPlan = 'docOrPlan' as any,
    Stc = 'stc' as any,
  }
  /**
   * @export
   * @enum {string}
   */
  export enum StatusEnum {
    Processing = 'Processing' as any,
    Fulfilled = 'Fulfilled' as any,
    Completed = 'Completed' as any,
    Cancelled = 'Cancelled' as any,
  }
}
/**
 * Order Status change for cancellation
 * @export
 * @interface OrderStatus
 */
export interface OrderStatus {
  /**
   * Indicate the status for the order  1. Cancelled - the order should be cancelled.
   * @type {string}
   * @memberof OrderStatus
   */
  status?: OrderStatus.StatusEnum;
}

/**
 * @export
 * @namespace OrderStatus
 */
export namespace OrderStatus {
  /**
   * @export
   * @enum {string}
   */
  export enum StatusEnum {
    Cancelled = 'Cancelled' as any,
  }
}
/**
 *
 * @export
 * @interface OwnerAddress
 */
export interface OwnerAddress {
  /**
   * Mailing address destination
   * @type {string}
   * @memberof OwnerAddress
   */
  addressLine1?: string;
  /**
   * Mailing address destination
   * @type {string}
   * @memberof OwnerAddress
   */
  addressLine2?: string;
  /**
   * City
   * @type {string}
   * @memberof OwnerAddress
   */
  city?: string;
  /**
   *
   * @type {CanadianProvince}
   * @memberof OwnerAddress
   */
  province?: CanadianProvince;
  /**
   * Non-Canadian Province / geographic division
   * @type {string}
   * @memberof OwnerAddress
   */
  provinceName?: string;
  /**
   * Country
   * @type {string}
   * @memberof OwnerAddress
   */
  country?: string;
  /**
   * Postal Code
   * @type {string}
   * @memberof OwnerAddress
   */
  postalCode?: string;
}
/**
 *
 * @export
 * @interface ParcelInfo
 */
export interface ParcelInfo {
  /**
   * The unique 9 character identifier for a parcel of land in the province of British Columbia. Each character is a digit in the range 0-9.
   * @type {string}
   * @memberof ParcelInfo
   */
  parcelIdentifier: string;
  /**
   * Indicates the state with respect to Parcel lifecycle.
   * @type {string}
   * @memberof ParcelInfo
   */
  status: ParcelInfo.StatusEnum;
  /**
   * Count of registered titles on parcel
   * @type {number}
   * @memberof ParcelInfo
   */
  registeredTitlesCount: number;
  /**
   * Count of pending applications
   * @type {number}
   * @memberof ParcelInfo
   */
  pendingApplicationCount: number;
  /**
   * Miscellaneous Note
   * @type {string}
   * @memberof ParcelInfo
   */
  miscellaneousNotes: string;
  /**
   *
   * @type {ParcelTombstone}
   * @memberof ParcelInfo
   */
  tombstone?: ParcelTombstone;
  /**
   *
   * @type {Array<LegalDescription>}
   * @memberof ParcelInfo
   */
  legalDescription: Array<LegalDescription>;
  /**
   *
   * @type {Array<AssociatedPlan>}
   * @memberof ParcelInfo
   */
  associatedPlans?: Array<AssociatedPlan>;
}

/**
 * @export
 * @namespace ParcelInfo
 */
export namespace ParcelInfo {
  /**
   * @export
   * @enum {string}
   */
  export enum StatusEnum {
    ACTIVE = 'ACTIVE' as any,
    INACTIVE = 'INACTIVE' as any,
  }
}
/**
 *
 * @export
 * @interface ParcelInfoOrder
 */
export interface ParcelInfoOrder extends OrderParent {
  /**
   *
   * @type {ParcelInfoOrderParameters}
   * @memberof ParcelInfoOrder
   */
  productOrderParameters?: ParcelInfoOrderParameters;
  /**
   *
   * @type {ProductParent & any}
   * @memberof ParcelInfoOrder
   */
  orderedProduct?: ProductParent & any;
}

/**
 * @export
 * @namespace ParcelInfoOrder
 */
export namespace ParcelInfoOrder {}
/**
 * Parameters required for ordering a Parcel Info
 * @export
 * @interface ParcelInfoOrderParameters
 */
export interface ParcelInfoOrderParameters {
  /**
   * Parcel identifier in Land Title Registry – format 123-123-123 or 123123123
   * @type {string}
   * @memberof ParcelInfoOrderParameters
   */
  parcelIdentifier: string;
}
/**
 *
 * @export
 * @interface ParcelInfoProduct
 */
export interface ParcelInfoProduct extends ProductParent {
  /**
   *
   * @type {ParcelInfo}
   * @memberof ParcelInfoProduct
   */
  parcelInfo?: ParcelInfo;
}
/**
 *
 * @export
 * @interface ParcelTombstone
 */
export interface ParcelTombstone {
  /**
   * This is a short free text cue to identify an absolute fee book reference.
   * @type {string}
   * @memberof ParcelTombstone
   */
  absoluteFeeReferenceDescription?: string;
  /**
   *
   * @type {Array<TaxAuthority>}
   * @memberof ParcelTombstone
   */
  taxAuthorities?: Array<TaxAuthority>;
}
/**
 *
 * @export
 * @interface PendingApplication
 */
export interface PendingApplication {
  /**
   * Application number of the pending application
   * @type {string}
   * @memberof PendingApplication
   */
  applicationNumber?: string;
  /**
   * Description of the transaction type
   * @type {string}
   * @memberof PendingApplication
   */
  transactionType?: string;
  /**
   * Defected indicator
   * @type {boolean}
   * @memberof PendingApplication
   */
  defected?: boolean;
}
/**
 *
 * @export
 * @interface Product
 */
export interface Product {}
/**
 *
 * @export
 * @interface ProductParent
 */
export interface ProductParent {
  /**
   * URI to retrieve the ordered product PDF.  # It is suggested that third party software confirms the order status is completed before retrieving the ordered product via this URI.  The system will return a 404 (Not Found) response if the ordered product PDF is not yet available.
   * @type {string}
   * @memberof ProductParent
   */
  href?: string;
}
/**
 * Address of the recipient to be printed on the STC
 * @export
 * @interface RecipientAddress
 */
export interface RecipientAddress {
  /**
   * Address line 1 of the recipient address
   * @type {string}
   * @memberof RecipientAddress
   */
  addressLine1?: string;
  /**
   * Address line 2 (optional) of the recipient address
   * @type {string}
   * @memberof RecipientAddress
   */
  addressLine2?: string;
  /**
   * City of the recipient address
   * @type {string}
   * @memberof RecipientAddress
   */
  city?: string;
  /**
   * Province of the recipient address
   * @type {string}
   * @memberof RecipientAddress
   */
  province?: string;
  /**
   * Postal code of the recipient address
   * @type {string}
   * @memberof RecipientAddress
   */
  postalCode?: string;
  /**
   * Country of the recipient address
   * @type {string}
   * @memberof RecipientAddress
   */
  country?: string;
}
/**
 *
 * @export
 * @interface SpcpOrder
 */
export interface SpcpOrder extends OrderParent {
  /**
   *
   * @type {StrataPlanCommonPropertyOrderParameters}
   * @memberof SpcpOrder
   */
  productOrderParameters?: StrataPlanCommonPropertyOrderParameters;
  /**
   *
   * @type {ProductParent & any}
   * @memberof SpcpOrder
   */
  orderedProduct?: ProductParent & any;
}

/**
 * @export
 * @namespace SpcpOrder
 */
export namespace SpcpOrder {}
/**
 * Parameteres required for ordering a State Title Certificate
 * @export
 * @interface StateTitleCertificateOrderParameters
 */
export interface StateTitleCertificateOrderParameters {
  /**
   * Title number of a registered or pending title
   * @type {string}
   * @memberof StateTitleCertificateOrderParameters
   */
  titleNumber: string;
  /**
   * Optional application number of a pending application for the title
   * @type {string}
   * @memberof StateTitleCertificateOrderParameters
   */
  pendingApplicationNumber?: string;
  /**
   * LTO client number One of LTO client number or recipient name/address must be provided # Note: If the LTO client number is provided, the recipient name/address will be ignored if also provided.
   * @type {string}
   * @memberof StateTitleCertificateOrderParameters
   */
  ltoClientNumber?: string;
  /**
   * Name of the recipient to be printed on the STC
   * @type {string}
   * @memberof StateTitleCertificateOrderParameters
   */
  recipientName?: string;
  /**
   *
   * @type {LandTitleDistrictCode}
   * @memberof StateTitleCertificateOrderParameters
   */
  landTitleDistrictCode?: LandTitleDistrictCode;
  /**
   *
   * @type {RecipientAddress}
   * @memberof StateTitleCertificateOrderParameters
   */
  recipientAddress?: RecipientAddress;
}
/**
 *
 * @export
 * @interface StcOrder
 */
export interface StcOrder extends OrderParent {
  /**
   *
   * @type {StateTitleCertificateOrderParameters}
   * @memberof StcOrder
   */
  productOrderParameters?: StateTitleCertificateOrderParameters;
}

/**
 * @export
 * @namespace StcOrder
 */
export namespace StcOrder {}
/**
 *
 * @export
 * @interface StrataPlanCommonProperty
 */
export interface StrataPlanCommonProperty {
  /**
   *
   * @type {StrataPlanIdentifier}
   * @memberof StrataPlanCommonProperty
   */
  strataPlanIdentifier?: StrataPlanIdentifier;
  /**
   *
   * @type {Array<LegalNotationsOnStrataCommonProperty>}
   * @memberof StrataPlanCommonProperty
   */
  legalNotationsOnSCP?: Array<LegalNotationsOnStrataCommonProperty>;
  /**
   *
   * @type {Array<ChargesOnStrataCommonProperty>}
   * @memberof StrataPlanCommonProperty
   */
  chargesOnSCP?: Array<ChargesOnStrataCommonProperty>;
}
/**
 * Parameters required for ordering a Strata Plan Common Property
 * @export
 * @interface StrataPlanCommonPropertyOrderParameters
 */
export interface StrataPlanCommonPropertyOrderParameters {
  /**
   * Strata plan number for the common property Mandatory parameter # Note: All plan numbers must include the plan’s prefix and/or suffix
   * @type {string}
   * @memberof StrataPlanCommonPropertyOrderParameters
   */
  strataPlanNumber: string;
  /**
   * Indicate whether to include cancelled charges, legal notations and corrections on the strata plan common property
   * @type {boolean}
   * @memberof StrataPlanCommonPropertyOrderParameters
   */
  includeCancelledInfo?: boolean;
}
/**
 *
 * @export
 * @interface StrataPlanCommonPropertyProduct
 */
export interface StrataPlanCommonPropertyProduct extends ProductParent {
  /**
   *
   * @type {StrataPlanCommonProperty}
   * @memberof StrataPlanCommonPropertyProduct
   */
  strataPlanCommonProperty?: StrataPlanCommonProperty;
}
/**
 *
 * @export
 * @interface StrataPlanIdentifier
 */
export interface StrataPlanIdentifier {
  /**
   * Application Number that created the first strata parcel. Uniquely identifies the Strata Common Property.
   * @type {string}
   * @memberof StrataPlanIdentifier
   */
  strataPlanNumber?: string;
  /**
   *
   * @type {LandTitleDistrict}
   * @memberof StrataPlanIdentifier
   */
  landTitleDistrict?: LandTitleDistrict;
}
/**
 *
 * @export
 * @interface SubdividedShortLegalDescription
 */
export interface SubdividedShortLegalDescription {
  /**
   * Plan
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  planNumber1: string;
  /**
   * Township/Townsite
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  townshipOrTownSite2?: string;
  /**
   * Range
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  range3?: string;
  /**
   * Block
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  block4?: string;
  /**
   * Subdivision
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  subdivision5?: string;
  /**
   * District Lot/Lot/Sublot
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  lotOrDistrictLotOrSubLot6?: string;
  /**
   * Subdivision
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  subdivision7?: string;
  /**
   * Lot/Parcel
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  lotOrParcel8?: string;
  /**
   * Section
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  section9?: string;
  /**
   * Quadrant
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  quadrant10?: string;
  /**
   * Block/Lot
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  blockOrLot11?: string;
  /**
   * Lot/Parcel
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  lotOrParcel12?: string;
  /**
   * Parcel/Block
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  parcelOrBlock13?: string;
  /**
   * Parcel Short Legal obtained by concatenating all previous Short Legal search fields
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  concatShortLegal: string;
  /**
   * Marginal Notes
   * @type {string}
   * @memberof SubdividedShortLegalDescription
   */
  marginalNotes?: string;
}
/**
 *
 * @export
 * @interface TaxAuthority
 */
export interface TaxAuthority {
  /**
   * For canceled titles these represent the historical Tax Authorities, as assigned at title cancelation time. For registered titles these are the Names of the current Tax Authorities.
   * @type {string}
   * @memberof TaxAuthority
   */
  authorityName: string;
}
/**
 *
 * @export
 * @interface Title
 */
export interface Title {
  /**
   * Indicates the state with respect to Title lifecycle.
   * @type {string}
   * @memberof Title
   */
  titleStatus: Title.TitleStatusEnum;
  /**
   *
   * @type {TitleIdentifier}
   * @memberof Title
   */
  titleIdentifier: TitleIdentifier;
  /**
   *
   * @type {TitleTombstone}
   * @memberof Title
   */
  tombstone: TitleTombstone;
  /**
   *
   * @type {Array<TitleOwnershipGroup>}
   * @memberof Title
   */
  ownershipGroups: Array<TitleOwnershipGroup>;
  /**
   *
   * @type {Array<TaxAuthority>}
   * @memberof Title
   */
  taxAuthorities: Array<TaxAuthority>;
  /**
   *
   * @type {Array<DescriptionOfLand>}
   * @memberof Title
   */
  descriptionsOfLand: Array<DescriptionOfLand>;
  /**
   *
   * @type {Array<LegalNotationOnTitle>}
   * @memberof Title
   */
  legalNotationsOnTitle?: Array<LegalNotationOnTitle>;
  /**
   *
   * @type {Array<ChargeOnTitle>}
   * @memberof Title
   */
  chargesOnTitle?: Array<ChargeOnTitle>;
  /**
   *
   * @type {Array<DuplicateCertificate>}
   * @memberof Title
   */
  duplicateCertificatesOfTitle?: Array<DuplicateCertificate>;
  /**
   *
   * @type {Array<TitleTransferDisposition>}
   * @memberof Title
   */
  titleTransfersOrDispositions?: Array<TitleTransferDisposition>;
  /**
   *
   * @type {Array<Altos1TitleCorrection>}
   * @memberof Title
   */
  correctionsAltos1?: Array<Altos1TitleCorrection>;
  /**
   *
   * @type {Array<TitleCorrection>}
   * @memberof Title
   */
  corrections?: Array<TitleCorrection>;
}

/**
 * @export
 * @namespace Title
 */
export namespace Title {
  /**
   * @export
   * @enum {string}
   */
  export enum TitleStatusEnum {
    REGISTERED = 'REGISTERED' as any,
    CANCELLED = 'CANCELLED' as any,
  }
}
/**
 *
 * @export
 * @interface TitleCorrection
 */
export interface TitleCorrection {
  /**
   * Title correction reason
   * @type {string}
   * @memberof TitleCorrection
   */
  reason: string;
  /**
   * The Application that initiated the Title correction
   * @type {string}
   * @memberof TitleCorrection
   */
  originatingCorrectionApplication: string;
  /**
   * Entered date and time
   * @type {Date}
   * @memberof TitleCorrection
   */
  enteredDate: Date;
  /**
   * The Charge affected by this correction, if any.
   * @type {string}
   * @memberof TitleCorrection
   */
  relatedChargeNumber: string;
  /**
   * The Legal Notation affected by this correction, if any.
   * @type {string}
   * @memberof TitleCorrection
   */
  relatedLegalNotationNumber: string;
}
/**
 *
 * @export
 * @interface TitleIdentifier
 */
export interface TitleIdentifier {
  /**
   * In practice, the same value as the Application number that raised the Title.
   * @type {string}
   * @memberof TitleIdentifier
   */
  titleNumber: string;
  /**
   *
   * @type {LandTitleDistrict}
   * @memberof TitleIdentifier
   */
  landLandDistrict?: LandTitleDistrict;
}
/**
 * A Title Order
 * @export
 * @interface TitleOrder
 */
export interface TitleOrder extends OrderParent {
  /**
   *
   * @type {TitleOrderParameters}
   * @memberof TitleOrder
   */
  productOrderParameters?: TitleOrderParameters;
  /**
   *
   * @type {ProductParent & any}
   * @memberof TitleOrder
   */
  orderedProduct?: ProductParent & any;
}

/**
 * @export
 * @namespace TitleOrder
 */
export namespace TitleOrder {}
/**
 * Parameters required for a title order
 * @export
 * @interface TitleOrderParameters
 */
export interface TitleOrderParameters {
  /**
   * Title number of the title to be ordered
   * @type {string}
   * @memberof TitleOrderParameters
   */
  titleNumber: string;
  /**
   *
   * @type {LandTitleDistrictCode}
   * @memberof TitleOrderParameters
   */
  landTitleDistrictCode?: LandTitleDistrictCode;
  /**
   * Indicate whether to include cancelled charges, legal notations and corrections on the title
   * @type {boolean}
   * @memberof TitleOrderParameters
   */
  includeCancelledInfo?: boolean;
}
/**
 *
 * @export
 * @interface TitleOwner
 */
export interface TitleOwner {
  /**
   * Free text containing either the Owner Last Name or the Corporate Name.
   * @type {string}
   * @memberof TitleOwner
   */
  lastNameOrCorpName1: string;
  /**
   * Provides additional space for storing the Owner Last Name or the Corporate Name.
   * @type {string}
   * @memberof TitleOwner
   */
  lastNameOrCorpName2?: string;
  /**
   * Owner Given Name; this attribute is empty if the Owner is a corporation.
   * @type {string}
   * @memberof TitleOwner
   */
  givenName?: string;
  /**
   * Free text specifying the Incorporation Number.
   * @type {string}
   * @memberof TitleOwner
   */
  incorporationNumber?: string;
  /**
   * Free text field used to retain information  describing the occupation of an individual who is a title holder.
   * @type {string}
   * @memberof TitleOwner
   */
  occupationDescription?: string;
  /**
   *
   * @type {OwnerAddress}
   * @memberof TitleOwner
   */
  address?: OwnerAddress;
}
/**
 *
 * @export
 * @interface TitleOwnershipGroup
 */
export interface TitleOwnershipGroup {
  /**
   * Indicator for joint tenancy
   * @type {string}
   * @memberof TitleOwnershipGroup
   */
  jointTenancyIndication?: string;
  /**
   * Interest Fraction Numerator. It is used if not 1 (1/1), assumed to be equal division amongst ownership groups unless specified otherwise.
   * @type {string}
   * @memberof TitleOwnershipGroup
   */
  interestFractionNumerator?: string;
  /**
   * Interest Fraction Denominator. It is used if not 1 (1/1), assumed to be equal division amongst ownership groups unless specified otherwise.
   * @type {string}
   * @memberof TitleOwnershipGroup
   */
  interestFractionDenominator?: string;
  /**
   * Remarks on title ownership.  It may contain an address from ALTOS1. Can also be used for other purposes, e.g., to indicate Corporate Sole, Trust.
   * @type {string}
   * @memberof TitleOwnershipGroup
   */
  ownershipRemarks: string;
  /**
   *
   * @type {Array<TitleOwner>}
   * @memberof TitleOwnershipGroup
   */
  titleOwners: Array<TitleOwner>;
}
/**
 *
 * @export
 * @interface TitleProduct
 */
export interface TitleProduct extends ProductParent {
  /**
   *
   * @type {Title}
   * @memberof TitleProduct
   */
  title?: Title;
}
/**
 *
 * @export
 * @interface TitleSummary
 */
export interface TitleSummary {
  /**
   * Title number of the title
   * @type {string}
   * @memberof TitleSummary
   */
  titleNumber?: string;
  /**
   * Land title district (e.g. Victoria) where the title belongs
   * @type {string}
   * @memberof TitleSummary
   */
  landTitleDistrict?: string;
  /**
   *
   * @type {LandTitleDistrictCode}
   * @memberof TitleSummary
   */
  landTitleDistrictCode?: LandTitleDistrictCode;
  /**
   * Parcel identifier in Land Title Registry, format: 123-123-123
   * @type {string}
   * @memberof TitleSummary
   */
  parcelIdentifier?: string;
  /**
   * Status of the title
   * @type {string}
   * @memberof TitleSummary
   */
  status?: TitleSummary.StatusEnum;
  /**
   * First registered owner name on title
   * @type {string}
   * @memberof TitleSummary
   */
  firstOwner?: string;
}

/**
 * @export
 * @namespace TitleSummary
 */
export namespace TitleSummary {
  /**
   * @export
   * @enum {string}
   */
  export enum StatusEnum {
    REGISTERED = 'REGISTERED' as any,
    CANCELLED = 'CANCELLED' as any,
  }
}
/**
 *
 * @export
 * @interface TitleTombstone
 */
export interface TitleTombstone {
  /**
   * The Received Date and Time of the Application that raised the Title. It is synonymous with the Registration Date/Time and comes from the Originating Document where is recorded as Document Acceptance Date/Time.
   * @type {Date}
   * @memberof TitleTombstone
   */
  applicationReceivedDate: Date;
  /**
   * System-generated date/timestamp for when the Title record was inserted into Register database.
   * @type {Date}
   * @memberof TitleTombstone
   */
  enteredDate: Date;
  /**
   * Title remarks may be used indicate: * Determinable Fee Simple: results in a Possibility of Reverter charge on Title * Fee Simple on Condition: results in a Right of Entry charge on Title  #Note: LifeEstate is registered as a charge, so it is not referenced here.
   * @type {string}
   * @memberof TitleTombstone
   */
  titleRemarks?: string;
  /**
   * Describes the source of the first title. It may contain: * AFB reference number - \"\"AFB <book, folio and page number>\"\" * The text \"\"Crown Grant\"\"
   * @type {string}
   * @memberof TitleTombstone
   */
  rootOfTitle?: string;
  /**
   * Title cancellation date and time. Not set for a Draft or Registered Title
   * @type {Date}
   * @memberof TitleTombstone
   */
  cancellationDate?: Date;
  /**
   * Indicates property value at the time of title application. Used by property assessors.
   * @type {string}
   * @memberof TitleTombstone
   */
  marketValueAmount?: string;
  /**
   *
   * @type {Array<TitleIdentifier>}
   * @memberof TitleTombstone
   */
  fromTitles?: Array<TitleIdentifier>;
  /**
   *
   * @type {Array<NatureOfTransfer>}
   * @memberof TitleTombstone
   */
  natureOfTransfers: Array<NatureOfTransfer>;
}
/**
 *
 * @export
 * @interface TitleTransferDisposition
 */
export interface TitleTransferDisposition {
  /**
   * Text of reason for the disposition.
   * @type {string}
   * @memberof TitleTransferDisposition
   */
  disposition?: string;
  /**
   * The date the disposition was entered
   * @type {Date}
   * @memberof TitleTransferDisposition
   */
  dispositionDate?: Date;
  /**
   * To title acceptance data and time
   * @type {Date}
   * @memberof TitleTransferDisposition
   */
  acceptanceDate?: Date;
  /**
   * Title Number of the To Title
   * @type {string}
   * @memberof TitleTransferDisposition
   */
  titleNumber: string;
  /**
   *
   * @type {LandTitleDistrict}
   * @memberof TitleTransferDisposition
   */
  landLandDistrict: LandTitleDistrict;
}
/**
 *
 * @export
 * @interface UnsubdividedShortLegalDescription
 */
export interface UnsubdividedShortLegalDescription {
  /**
   * Land District
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  landDistrict1: string;
  /**
   * Meridian
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  meridian2?: string;
  /**
   * Range
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  range3?: string;
  /**
   * Township/Island
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  townshipOrIsland4?: string;
  /**
   * Group
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  group5?: string;
  /**
   * Block
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  block6?: string;
  /**
   * District Lot/Lot/Section
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  districtLotOrLotOrSection7?: string;
  /**
   * Section
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  section8?: string;
  /**
   * Quadrant
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  quadrant9?: string;
  /**
   * Block
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  blockOrSection10?: string;
  /**
   * Legal Subdivision
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  subdivision11?: string;
  /**
   * Lot/Sublot/Parcel
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  lotOrSubLotOrParcel12?: string;
  /**
   * Lot/Parcel
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  lotOrParcel13?: string;
  /**
   * Mineral Claim / Indian Reserve Name
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  mineralClaimIOrIndianReserveName14?: string;
  /**
   * Mineral Claim Number / Indian Reserve Number
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  mineralClaimOrIndianReserveNumber15?: string;
  /**
   * Parcel Short Legal obtained by concatenating all previous Short Legal search fields
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  concatShortLegal: string;
  /**
   * Marginal Notes
   * @type {string}
   * @memberof UnsubdividedShortLegalDescription
   */
  marginalNotes?: string;
}

export interface LtsaOrders {
  parcelInfo: ParcelInfoOrder;
  titleOrders: TitleOrder[];
}
