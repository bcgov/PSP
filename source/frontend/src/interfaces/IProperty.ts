import { Moment } from 'moment';

import {
  PropertyAreaUnitTypes,
  PropertyClassificationTypes,
  PropertyDataSourceTypes,
  PropertyStatusTypes,
  PropertyTenureTypes,
} from '@/constants/index';
import { IAddress, IOrganization } from '@/interfaces';
import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';

import IPropertySurplus from './IPropertySurplus';

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

  appCreateTimestamp?: UtcIsoDateTime;
  updatedOn?: Date | string | Moment;
  updatedByEmail?: string;
  updatedByName?: string;
  rowVersion?: number;
}

export type IFormProperty = ExtendOverride<
  IProperty,
  {
    address?: IAddress;
    landArea?: number;
    landLegalDescription?: string;
    coordinates?: string;
  }
>;
