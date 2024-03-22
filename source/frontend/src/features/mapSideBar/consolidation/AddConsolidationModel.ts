import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_CodeTypes_PropertyOperationTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyOperationTypes';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists } from '@/utils';

export class ConsolidationFormModel {
  sourceProperties: ApiGen_Concepts_Property[] = [];
  destinationProperty: ApiGen_Concepts_Property | null = null;
  propertyOperationNo: number | null = null;
  propertyOperationTypeCode: ApiGen_Base_CodeType<string> | null = {
    id: ApiGen_CodeTypes_PropertyOperationTypes.CONSOLIDATE,
    isDisabled: false,
    displayOrder: 0,
    description: '',
  };
  searchBy = 'pid';
  pid = '';

  toApi(): ApiGen_Concepts_PropertyOperation[] {
    return this.sourceProperties?.map(sp => ({
      ...getEmptyBaseAudit(0),
      id: 0,
      destinationPropertyId: this.destinationProperty?.id ?? 0,
      destinationProperty: this.destinationProperty,
      sourceProperty: sp,
      sourcePropertyId: sp?.id ?? 0,
      operationDt: null,
      isDisabled: false,
      propertyOperationNo: this.propertyOperationNo ?? 0,
      propertyOperationTypeCode: this.propertyOperationTypeCode,
    }));
  }

  static fromApi(operations: ApiGen_Concepts_PropertyOperation[]): ConsolidationFormModel {
    const subdivisionForm = new ConsolidationFormModel();

    if (operations.length === 0) {
      subdivisionForm.destinationProperty = null;
      subdivisionForm.sourceProperties = [];
      subdivisionForm.propertyOperationNo = null;
      subdivisionForm.propertyOperationTypeCode = null;
    }
    subdivisionForm.destinationProperty = operations[0].destinationProperty;
    subdivisionForm.sourceProperties = operations.map(op => op.sourceProperty).filter(exists) ?? [];
    subdivisionForm.propertyOperationNo = operations[0].propertyOperationNo;
    subdivisionForm.propertyOperationTypeCode = operations[0].propertyOperationTypeCode;
    return subdivisionForm;
  }
}
