import { ApiGen_Concepts_PropertyTenureCleanup } from '@/models/api/generated/ApiGen_Concepts_PropertyTenureCleanup';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class PropertyTenureCleanupFormModel {
  id?: number;
  rowVersion?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromLookup(base: ILookupCode): PropertyTenureCleanupFormModel {
    const newModel = new PropertyTenureCleanupFormModel();
    newModel.typeCode = base.id.toString();
    newModel.typeDescription = base.name;
    return newModel;
  }

  static fromApi(base: ApiGen_Concepts_PropertyTenureCleanup): PropertyTenureCleanupFormModel {
    const newModel = new PropertyTenureCleanupFormModel();
    newModel.id = base.id;
    newModel.rowVersion = base.rowVersion ?? undefined;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.tenureCleanupTypeCode?.id ?? undefined;
    newModel.typeDescription = base.tenureCleanupTypeCode?.description ?? undefined;
    return newModel;
  }

  toApi(): ApiGen_Concepts_PropertyTenureCleanup {
    return {
      id: this.id ?? 0,
      propertyId: this.propertyId ?? 0,
      tenureCleanupTypeCode: {
        id: this.typeCode ?? null,
        description: this.typeDescription ?? null,
        displayOrder: null,
        isDisabled: false,
      },
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
