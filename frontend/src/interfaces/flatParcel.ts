import { IBuilding, IProperty } from 'interfaces';

export interface IFlatParcel extends IProperty {
  pid?: string;
  pin?: number | '';
  classification?: string;
  classificationId: number | '';
  address: string;
  administrativeArea: string;
  postal: string;
  landArea: number | '';
  landLegalDescription: string;
  zoning: string;
  zoningPotential: string;
  organizationId: number | '';
  isSensitive: boolean;
  buildings: IBuilding[];
  assessedLand: number | '';
  assessedBuilding: number | '';
  netBook: number | '';
}
