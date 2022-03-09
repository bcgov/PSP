import {
  PropertyAreaUnitTypes,
  PropertyClassificationTypes,
  PropertyDataSourceTypes,
  PropertyStatusTypes,
  PropertyTenureTypes,
} from 'constants/index';
import {
  IAddress,
  IOrganization,
  IProject,
  IProjectActivity,
  IPropertyEvaluation,
  IPropertyServiceFile,
} from 'interfaces';
import { Moment } from 'moment';

import { ILease } from './ILease';
import IPropertySurplus from './IPropertySurplus';

/**
 * A property entity represents a land, or other type of property.
 */
export interface IProperty {
  id?: number;
  pid: string;
  pin?: number | '';
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

  latitude?: number;
  longitude?: number;

  areaUnitId?: PropertyAreaUnitTypes;
  areaUnit?: string;
  landArea: number;
  landLegalDescription: string;
  encumbranceReason?: string;
  isSensitive?: boolean;
  isOwned?: boolean;
  isPropertyOfInterest?: boolean;
  isVisibleToOtherAgencies?: boolean;
  zoning?: string;
  zoningPotential?: string;

  organizations?: IOrganization[];
  serviceFiles?: IPropertyServiceFile[];
  projects?: IProject[];
  projectActivities?: IProjectActivity[];
  evaluations?: IPropertyEvaluation[];
  surplusDeclaration?: IPropertySurplus;

  appCreateTimestamp?: Date | string | Moment;
  updatedOn?: Date | string | Moment;
  updatedByEmail?: string;
  updatedByName?: string;
  rowVersion?: number;
  leases?: ILease[];
}

export interface IFormProperty
  extends ExtendOverride<
    IProperty,
    {
      areaUnitType?: string;
      address?: IAddress;
      landArea?: number;
      landLegalDescription?: string;
    }
  > {}
