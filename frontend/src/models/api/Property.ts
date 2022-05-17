import Api_TypeCode from 'interfaces/ITypeCode';

import { Api_Address } from './Address';
import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_Coordinate {
  x?: number;
  y?: number;
}

export interface Api_Geometry {
  coordinate?: Api_Coordinate;
}

export interface Api_Property extends Api_ConcurrentVersion {
  id?: number;
  pid?: number;
  pin?: number;
  status?: string;
  dataSource?: string;
  dataSourceEffectiveDate?: string;
  classification?: string;
  tenure?: Api_TypeCode<string>;
  name?: string;
  description?: string;
  address?: Api_Address;
  region?: Api_TypeCode<number>;
  district?: Api_TypeCode<string>;
  location?: Api_Geometry;
  planNumber?: string;

  latitude?: number;
  longitude?: number;

  landArea?: number;
  landLegalDescription?: string;
  encumbranceReason?: string;
  isSensitive?: boolean;
  isOwned?: boolean;
  isPropertyOfInterest?: boolean;
  isVisibleToOtherAgencies?: boolean;
  zoning?: string;
  zoningPotential?: string;

  appCreateTimestamp?: string;
  updatedOn?: string;
  updatedByEmail?: string;
  updatedByName?: string;
}

export interface Api_PropertyAssociations {
  id?: string;
  pid?: string;
  leaseAssociations?: Api_PropertyAssociation[];
  researchAssociations?: Api_PropertyAssociation[];
  aquisitionAssociations?: Api_PropertyAssociation[];
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
