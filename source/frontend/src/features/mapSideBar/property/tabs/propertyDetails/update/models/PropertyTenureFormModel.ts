import { ApiGen_Concepts_PropertyTenure } from '@/models/api/generated/ApiGen_Concepts_PropertyTenure';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class PropertyTenureFormModel {
  id?: number;
  rowVersion?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromLookup(base: ILookupCode): PropertyTenureFormModel {
    const newModel = new PropertyTenureFormModel();
    newModel.typeCode = base.id.toString();
    newModel.typeDescription = base.name;
    return newModel;
  }

  static fromApi(base: ApiGen_Concepts_PropertyTenure): PropertyTenureFormModel {
    const newModel = new PropertyTenureFormModel();
    newModel.id = base.id;
    newModel.rowVersion = base.rowVersion ?? undefined;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyTenureTypeCode?.id ?? undefined;
    newModel.typeDescription = base.propertyTenureTypeCode?.description ?? undefined;
    return newModel;
  }

  toApi(): ApiGen_Concepts_PropertyTenure {
    return {
      id: this.id ?? 0,
      propertyId: this.propertyId ?? 0,
      propertyTenureTypeCode: {
        id: this.typeCode ?? null,
        description: this.typeDescription ?? null,
        displayOrder: null,
        isDisabled: false,
      },
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
