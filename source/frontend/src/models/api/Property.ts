import { Api_Address } from './Address';
import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion, Api_ConcurrentVersion_Null } from './ConcurrentVersion';
import { DateOnly } from './DateOnly';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import Api_TypeCode from './TypeCode';

export interface Api_Coordinate {
  x?: number;
  y?: number;
}

export interface Api_Geometry {
  coordinate?: Api_Coordinate;
}

export interface Api_Boundary {
  coordinates?: Api_Coordinate[];
}

export interface Api_Property extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  propertyType?: Api_TypeCode<string>;

  // multi-selects
  anomalies?: Api_PropertyAnomaly[];
  tenures?: Api_PropertyTenure[];
  roadTypes?: Api_PropertyRoad[];
  adjacentLands?: Api_PropertyAdjacentLand[];

  status?: Api_TypeCode<string>;
  region?: Api_TypeCode<number>;
  district?: Api_TypeCode<number>;

  dataSource?: Api_TypeCode<string>;
  dataSourceEffectiveDateOnly?: DateOnly;

  latitude?: number;
  longitude?: number;
  location?: Api_Geometry;
  boundary?: Api_Boundary;

  name?: string;
  description?: string;
  pid?: number;
  pin?: number;
  planNumber?: string;

  address?: Api_Address;

  generalLocation?: string;

  isSensitive?: boolean;
  isVisibleToOtherAgencies?: boolean;
  isProvincialPublicHwy?: boolean;
  isOwned?: boolean;
  isPropertyOfInterest?: boolean;
  isOtherInterest?: boolean;
  isDisposed?: boolean;
  pphStatusTypeCode?: string;
  isRwyBeltDomPatent?: boolean;
  pphStatusUpdateTimestamp?: Date;
  pphStatusUpdateUserGuid?: string;
  pphStatusUpdateUserid?: string;

  landArea?: number;
  areaUnit?: Api_TypeCode<string>;
  landLegalDescription?: string;

  isVolumetricParcel?: boolean;
  volumetricMeasurement?: number;
  volumetricUnit?: Api_TypeCode<string>;
  volumetricType?: Api_TypeCode<string>;

  encumbranceReason?: string;

  zoning?: string;
  zoningPotential?: string;
  municipalZoning?: string;

  notes?: string;
  isDisabled?: boolean;

  surplusDeclarationType?: Api_TypeCode<string>;
  surplusDeclarationComment?: string;
  surplusDeclarationDate?: string;
}

export interface Api_PropertyAnomaly extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  propertyId?: number;
  propertyAnomalyTypeCode?: Api_TypeCode<string>;
}

export interface Api_PropertyRoad extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  propertyId?: number;
  propertyRoadTypeCode?: Api_TypeCode<string>;
}

export interface Api_PropertyAdjacentLand extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  propertyId?: number;
  propertyAdjacentLandTypeCode?: Api_TypeCode<string>;
}

export interface Api_PropertyTenure extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  propertyId?: number;
  propertyTenureTypeCode?: Api_TypeCode<string>;
}

export interface Api_PropertyAssociations {
  id?: string;
  pid?: string;
  leaseAssociations?: Api_PropertyAssociation[];
  researchAssociations?: Api_PropertyAssociation[];
  acquisitionAssociations?: Api_PropertyAssociation[];
  dispositionAssociations?: Api_PropertyAssociation[];
}

export interface Api_PropertyAssociation {
  id?: number;
  fileNumber?: string;
  fileName?: string;
  createdDateTime?: string;
  createdBy?: string;
  createdByGuid?: string;
  status?: string;
}

export interface Api_PropertyContact extends Api_ConcurrentVersion_Null {
  id: number;
  propertyId: number;
  organizationId: number | null;
  organization: Api_Organization | null;
  personId: number | null;
  person: Api_Person | null;
  primaryContactId: number | null;
  primaryContact: Api_Person | null;
  purpose: string | null;
}

export interface Api_PropertyManagement extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number;
  managementPurposes: Api_PropertyManagementPurpose[];
  additionalDetails: string | null;
  isUtilitiesPayable: boolean | null;
  isTaxesPayable: boolean | null;
  relatedLeases: number;
  leaseExpiryDate: DateOnly | null;
}

export interface Api_PropertyManagementPurpose extends Api_ConcurrentVersion_Null, Api_AuditFields {
  id: number | null;
  propertyId: number | null;
  propertyPurposeTypeCode: Api_TypeCode<string> | null;
}
