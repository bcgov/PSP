import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';

export type IFilePropertyImprovements = {
  property: ApiGen_Concepts_Property;
  improvements: ApiGen_Concepts_PropertyImprovement[];
};
