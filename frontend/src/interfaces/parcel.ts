import { IAddress, IBuilding, IEvaluation, IFiscal, IProperty } from 'interfaces';

export interface IParcel extends IProperty {
  pid?: string;
  pin?: number | '';
  classification?: string;
  classificationId: number | '';
  encumbranceReason: string;
  address?: IAddress;
  landArea: number | '';
  landLegalDescription: string;
  zoning: string;
  zoningPotential: string;
  buildings: IBuilding[];
  parcels: Partial<IParcel[]>;
  assessedLand: number | '';
  assessedBuilding: number | '';
  evaluations: IEvaluation[];
  fiscals: IFiscal[];
  rowVersion?: number;
}
