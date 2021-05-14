import { LeasedLandTypes } from 'interfaces';

export interface ILeasedLand {
  ownershipNote: string;
  type: LeasedLandTypes;
  parcelId?: number;
}
