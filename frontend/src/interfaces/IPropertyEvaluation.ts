import { Moment } from 'moment';

/**
 * A property financial evaluation for a given date.
 */
export interface IPropertyEvaluation {
  id?: number;
  propertyId?: number;
  evaluatedOn: Date | string | Moment;
  key: number;
  value: number;
  note?: string;
  rowVersion?: number;
}
