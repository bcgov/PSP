import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';

export interface IPropertyPidLookupResult {
  foundInPims: boolean;
  property?: ApiGen_Concepts_Property;
}
