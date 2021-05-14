import { IProperty, IAddress, IEvaluation, IFiscal, IParcel, ILeasedLand } from 'interfaces';

export interface IBuilding extends IProperty {
  parcelId: number | '';
  pid: number | '';
  address: IAddress;
  buildingFloorCount?: number | '';
  buildingConstructionType?: string;
  buildingConstructionTypeId: number | '';
  buildingPredominateUse?: string;
  buildingPredominateUseId: number | '';
  buildingOccupantType?: string;
  buildingOccupantTypeId: number | '';
  classificationId: number | '';
  classification: string;
  encumbranceReason: string;
  leaseExpiry?: string;
  occupantName: string;
  transferLeaseOnSale: boolean;
  buildingTenancy: string;
  buildingTenancyUpdatedOn?: string;
  rentableArea: number | '';
  totalArea: number | '';
  agencyCode: string;
  assessedLand: number | '';
  assessedBuilding: number | '';
  evaluations: IEvaluation[];
  fiscals: IFiscal[];
  parcels: IParcel[];
  leasedLandMetadata?: ILeasedLand[];
}
