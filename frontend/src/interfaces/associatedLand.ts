import { IBuilding, ILeasedLand, IParcel } from 'interfaces';

export interface IAssociatedLand extends IBuilding {
  parcels: IParcel[];
  leasedLandMetadata: ILeasedLand[];
}
