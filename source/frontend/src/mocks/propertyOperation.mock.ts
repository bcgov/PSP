import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';

export const getEmptyPropertyOperation = (): ApiGen_Concepts_PropertyOperation => {
  return {
    id: 0,
    sourcePropertyId: 0,
    sourceProperty: null,
    destinationPropertyId: 0,
    destinationProperty: null,
    propertyOperationNo: 0,
    propertyOperationTypeCode: null,
    operationDt: null,
    isDisabled: null,
    ...getEmptyBaseAudit(),
  };
};
