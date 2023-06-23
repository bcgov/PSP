import { NumberFieldValue } from '@/typings/NumberFieldValue';
export interface IUsersFilter {
  businessIdentifierValue?: string;
  email?: string;
  region?: NumberFieldValue;
  role?: NumberFieldValue;
  activeOnly?: boolean;
}
