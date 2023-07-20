import { Moment } from 'moment';

import {
  PropertyAreaUnitTypes,
  PropertyClassificationTypes,
  PropertyDataSourceTypes,
  PropertyStatusTypes,
  PropertyTenureTypes,
} from '@/constants/index';
import { IAddress, IOrganization } from '@/interfaces';

import IPropertySurplus from './IPropertySurplus';
import ITypeCode from './ITypeCode';

/**
 * A property entity represents a land, or other type of property.
 */
export interface IProperty {
  id?: number;
  pid: string;
  pin?: string | number;
  statusId?: PropertyStatusTypes;
  status?: string;
  dataSourceId?: PropertyDataSourceTypes;
  dataSource?: string;
  dataSourceEffectiveDate?: Date | string | Moment;
  classificationId?: PropertyClassificationTypes;
  classification?: string;
  tenureId?: PropertyTenureTypes;
  tenure?: string;
  name?: string;
  description?: string;
  addressId?: number;
  address: IAddress;
  regionId?: number;
  region?: string;
  districtId?: number;
  district?: string;
  planNumber?: string;

  latitude?: number;
  longitude?: number;

  areaUnitId?: PropertyAreaUnitTypes;
  areaUnit?: string;
  areaUnitType?: ITypeCode<string>;
  landArea?: number;
  landLegalDescription?: string;
  encumbranceReason?: string;
  isSensitive?: boolean;
  isOwned?: boolean;
  isPropertyOfInterest?: boolean;
  isVisibleToOtherAgencies?: boolean;
  isPayableLease?: boolean;
  zoning?: string;
  zoningPotential?: string;

  organizations?: IOrganization[];
  surplusDeclaration?: IPropertySurplus;

  appCreateTimestamp?: Date | string | Moment;
  updatedOn?: Date | string | Moment;
  updatedByEmail?: string;
  updatedByName?: string;
  rowVersion?: number;
}

export interface IFormProperty
  extends ExtendOverride<
    IProperty,
    {
      areaUnitType?: ITypeCode<string>;
      address?: IAddress;
      landArea?: number;
      landLegalDescription?: string;
      coordinates?: string;
    }
  > {}
