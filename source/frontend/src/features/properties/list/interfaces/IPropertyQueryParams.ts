import { PropertyClassificationTypes } from '@/constants/index';

/**
 * IPropertyQueryParams interface, provides a model for querying the API for properties.
 */
export interface IPropertyQueryParams {
  page: number;
  quantity: number;
  pid?: string;
  address?: string;
  name?: string;
  municipality?: string;
  classificationId?: PropertyClassificationTypes;
  organizations?: number | number[];
  minLandArea?: number;
  maxLandArea?: number;
  all?: boolean;
  parcelId?: number;
}
