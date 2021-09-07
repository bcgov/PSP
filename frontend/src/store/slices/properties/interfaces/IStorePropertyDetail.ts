import { IProperty } from 'interfaces';

export interface IStorePropertyDetail {
  property: IProperty | null;
  position?: [number, number];
}
