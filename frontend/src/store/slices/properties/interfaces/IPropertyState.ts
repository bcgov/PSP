import { PointFeature } from 'components/maps/types';
import { IProperty } from 'interfaces';

import { IPropertyDetail } from '.';

export interface IPropertyState {
  properties: IProperty[];
  draftProperties: PointFeature[];
  propertyDetail?: IPropertyDetail | null;
  pid?: number;
}
