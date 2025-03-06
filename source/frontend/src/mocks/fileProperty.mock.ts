import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
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

export const getEmptyLeaseFileProperty = (): ApiGen_Concepts_PropertyLease => {
  const a: ApiGen_Concepts_PropertyLease = {
    file: undefined,
    leaseArea: 0,
    areaUnitType: undefined,
    id: 0,
    fileId: 0,
    propertyName: '',
    location: undefined,
    displayOrder: 0,
    property: undefined,
    propertyId: 0,
    ...getEmptyBaseAudit(),
  };

  return a;
};
