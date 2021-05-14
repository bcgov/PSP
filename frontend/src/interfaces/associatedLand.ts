import { IBuilding, IParcel, ILeasedLand } from 'interfaces';

export interface IAssociatedLand extends IBuilding {
  parcels: IParcel[];
  leasedLandMetadata: ILeasedLand[];
}
