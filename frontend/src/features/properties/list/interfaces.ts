import { IAddress, IBuilding, IEvaluation, IFiscal } from 'interfaces';

/**
 * IProperty interface represents the model used for searching properties.
 */
export interface IProperty {
  id: number;
  propertyType: string;
  propertyTypeId: number;
  pid: string;
  pin?: number;
  name?: string;
  classificationId: number;
  classification: string;
  description: string;
  latitude: number;
  longitude: number;
  isSensitive: boolean;
  agencyId: number;
  agency: string;
  agencyCode: string;
  subAgency?: string;
  subAgencyCode?: string;

  addressId: number;
  address: string;
  administrativeArea: string;
  province: string;
  postal: string;
  city: string;

  // Financial Values
  market: number;
  marketFiscalYear?: number;
  netBook: number;
  netBookFiscalYear?: number;

  assessedLand?: number;
  assessedLandDate?: Date | string;
  assessedBuilding?: number;
  assessedBuildingDate?: Date | string;

  // Parcel Properties
  landArea: number;
  landLegalDescription: string;
  zoning?: string;
  zoningPotential?: string;

  // Building Properties
  parcelId?: number;
  constructionTypeId?: number;
  constructionType?: string;
  predominateUseId?: number;
  predominateUse?: string;
  occupantTypeId?: number;
  occupantType?: string;
  floorCount?: number;
  tenancy?: string;
  occupantName?: string;
  leaseExpiry?: Date | string;
  transferLeaseOnSale?: boolean;
  rentableArea?: number;
}

/**
 * IPropertyQueryParams interface, provides a model for querying the API for properties.
 */
export interface IPropertyQueryParams {
  page: number;
  quantity: number;
  pid?: string;
  address?: string;
  name?: string;
  administrativeArea?: string;
  classificationId?: number;
  agencies?: number | number[];
  minLandArea?: number;
  maxLandArea?: number;
  minLotArea?: number;
  maxLotArea?: number;
  all?: boolean;
  parcelId?: number;
  propertyType?: string;
  bareLandOnly?: boolean;
  maxNetBookValue?: number | string;
  maxAssessedValue?: number | string;
  maxMarketValue?: number | string;
}

export interface IParentParcel {
  pid: string;
  pin: number;
  id: number;
}

/**
 * IProperty interface represents the model used for displaying properties.
 */
export interface IFlatProperty {
  id: number;
  projectPropertyId?: number;
  propertyTypeId: number;
  propertyType: string;
  pid: string;
  pin?: number;
  classificationId: number;
  classification: string;
  name: string;
  description: string;
  projectNumbers?: string[];
  latitude: number;
  longitude: number;
  isSensitive: boolean;
  agencyId: number;
  agency: string;
  agencyCode: string;
  subAgency?: string;
  subAgencyCode?: string;

  addressId: number;
  address: string;
  administrativeArea: string;
  province: string;
  postal: string;

  // Financial Values
  market: number | '';
  marketFiscalYear?: number;
  marketRowVersion?: number;
  netBook: number | '';
  netBookFiscalYear?: number;
  netBookRowVersion?: number;

  assessedLand?: number | '';
  assessedLandDate?: Date | string;
  assessedLandFirm?: string;
  assessedLandRowVersion?: number;
  assessedBuilding?: number | '';
  assessedBuildingDate?: Date | string;
  assessedBuildingFirm?: string;
  assessedBuildingRowVersion?: number;

  // Parcel Properties
  landArea: number;
  landLegalDescription: string;
  zoning?: string;
  zoningPotential?: string;
  parcels?: IParentParcel[];

  // Building Properties
  parcelId?: number;
  constructionTypeId?: number;
  constructionType?: string;
  predominateUseId?: number;
  predominateUse?: string;
  occupantTypeId?: number;
  occupantType?: string;
  floorCount?: number;
  tenancy?: string;
  occupantName?: string;
  leaseExpiry?: Date | string;
  transferLeaseOnSale?: boolean;
  rentableArea?: number;
  rowVersion?: number;
}

export interface IApiProperty {
  id: number;
  parcelId?: number;
  buildingId?: number;
  propertyTypeId: number;
  pid?: string;
  pin?: number | '';
  projectNumbers: string[];
  latitude: number;
  longitude: number;
  classification?: string;
  classificationId: number;
  name: string;
  description: string;
  address?: IAddress;
  landArea: number;
  landLegalDescription: string;
  zoning: string;
  zoningPotential: string;
  agency?: string;
  subAgency?: string;
  agencyId: number;
  isSensitive: boolean;
  buildings: IBuilding[];
  evaluations: IEvaluation[];
  fiscals: IFiscal[];
  parcels?: IParentParcel[];
  rowVersion?: number;
}
