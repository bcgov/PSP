import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_CodeTypes_PropertyOperationTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyOperationTypes';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists } from '@/utils';

export class SubdivisionFormModel {
  sourceProperty: ApiGen_Concepts_Property | null = null;
  destinationProperties: ApiGen_Concepts_Property[] = [];
  propertyOperationNo: number | null = null;
  propertyOperationTypeCode: ApiGen_Base_CodeType<string> | null = {
    id: ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE,
    isDisabled: false,
    displayOrder: 0,
    description: '',
  };
  searchBy = 'pid';
  pid = '';

  toApi(): ApiGen_Concepts_PropertyOperation[] {
    // eslint-disable-next-line no-debugger
    return this.destinationProperties?.map(dp => ({
      ...getEmptyBaseAudit(0),
      id: 0,
      sourcePropertyId: this.sourceProperty?.id ?? 0,
      sourceProperty: this.sourceProperty,
      destinationProperty: dp,
      destinationPropertyId: dp?.id ?? 0,
      operationDt: null,
      isDisabled: false,
      propertyOperationNo: this.propertyOperationNo ?? 0,
      propertyOperationTypeCode: this.propertyOperationTypeCode,
    }));
  }

  static fromApi(operations: ApiGen_Concepts_PropertyOperation[]): SubdivisionFormModel {
    const subdivisionForm = new SubdivisionFormModel();

    if (operations.length === 0) {
      subdivisionForm.destinationProperties = [];
      subdivisionForm.sourceProperty = null;
      subdivisionForm.propertyOperationNo = null;
      subdivisionForm.propertyOperationTypeCode = null;
    }
    subdivisionForm.sourceProperty = operations[0].sourceProperty;
    subdivisionForm.destinationProperties =
      operations.map(op => op.destinationProperty).filter(exists) ?? [];
    subdivisionForm.propertyOperationNo = operations[0].propertyOperationNo;
    subdivisionForm.propertyOperationTypeCode = operations[0].propertyOperationTypeCode;
    return subdivisionForm;
  }
}
