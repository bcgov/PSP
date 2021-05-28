import { IBuilding, IParcel } from 'interfaces';

export interface IStorePropertyDetail {
  property: IParcel | IBuilding | null;
  position?: [number, number];
}
