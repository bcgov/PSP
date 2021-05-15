export interface IEvaluation {
  parcelId?: number;
  buildingId?: number;
  date?: Date | string;
  key: string;
  firm?: string;
  value: number | '';
  rowVersion?: string;
}
