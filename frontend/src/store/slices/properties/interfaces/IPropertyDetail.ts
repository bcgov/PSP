import { PropertyTypes } from 'constants/propertyTypes';
import { IProperty } from 'interfaces';

export interface IPropertyDetail {
  propertyTypeId?: PropertyTypes;
  propertyDetail: IProperty | null;
  position?: [number, number]; // (optional) a way to override the positioning of the map popup
}
