export interface IFiscal {
  parcelId?: number;
  buildingId?: number;
  fiscalYear?: number | '';
  key: string;
  value: number | '';
  rowVersion?: string;
}
