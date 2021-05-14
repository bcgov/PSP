import { PropertyTypes } from 'constants/propertyTypes';
import { IParcel, IBuilding } from 'interfaces';

export interface IPropertyDetail {
  propertyTypeId?: PropertyTypes;
  parcelDetail: IParcel | IBuilding | null;
  position?: [number, number]; // (optional) a way to override the positioning of the map popup
}
