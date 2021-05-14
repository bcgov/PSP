import { IParcel, IBuilding } from 'interfaces';

export interface IStorePropertyDetail {
  property: IParcel | IBuilding | null;
  position?: [number, number];
}
