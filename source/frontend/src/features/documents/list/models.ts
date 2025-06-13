import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';

export interface DocRelation {
  id: number;
  relationType: ApiGen_CodeTypes_DocumentRelationType;
  fileNumber: string;
}
