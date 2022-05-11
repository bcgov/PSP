import Api_TypeCode from 'interfaces/ITypeCode';

import { Api_Address } from './Address';
import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_Coordinate {
  x?: number;
  y?: number;
}

export interface Api_Geometry {
  coordinate?: Api_Coordinate;
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
  district?: Api_TypeCode<string>;

  dataSource?: Api_TypeCode<string>;
  dataSourceEffectiveDate?: string;

  latitude?: number;
  longitude?: number;
  location?: Api_Geometry;

  name?: string;
  description?: string;
  pid?: number;
  pin?: number;
  planNumber?: string;

  address?: Api_Address;

  isSensitive?: boolean;
  isVisibleToOtherAgencies?: boolean;
  isOwned?: boolean;
  isPropertyOfInterest?: boolean;
  isProvincialPublicHwy?: boolean;

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
  status?: string;
}
