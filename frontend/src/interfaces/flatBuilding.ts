import { IProperty, LeasedLandTypes } from 'interfaces';

export interface IFlatBuilding extends IProperty {
  parcelId: number;
  address: string;
  administrativeArea: string;
  postal: string;
  province: string;
  buildingFloorCount?: number | '';
  buildingConstructionType?: string;
  buildingConstructionTypeId: number | '';
  buildingPredominateUse?: string;
  buildingPredominateUseId: number | '';
  buildingOccupantType?: string;
  buildingOccupantTypeId: number | '';
  classificationId: number | '';
  classification: string;
  leaseExpiry?: string;
  occupantName: string;
  transferLeaseOnSale: boolean;
  buildingTenancy: string;
  rentableArea: number | '';
  organizationCode: string;
  assessedLand: number | '';
  assessedBuilding: number | '';
  netBook: number | '';
  leasedLand: {
    type: LeasedLandTypes;
  };
}
