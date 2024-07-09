import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

export const getEmptyFileProperty = (): ApiGen_Concepts_FileProperty => {
  return {
    id: 0,
    propertyName: null,
    displayOrder: null,
    property: null,
    propertyId: 0,
    fileId: 0,
    file: null,
    location: null,
    ...getEmptyBaseAudit(),
  };
};
