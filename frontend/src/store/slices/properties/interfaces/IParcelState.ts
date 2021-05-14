import { PointFeature } from 'components/maps/types';
import { IProperty } from 'interfaces';
import { IPropertyDetail } from '.';

export interface IParcelState {
  parcels: IProperty[];
  draftParcels: PointFeature[];
  propertyDetail?: IPropertyDetail | null;
  associatedBuildingDetail: IPropertyDetail | null;
  pid: number;
}
