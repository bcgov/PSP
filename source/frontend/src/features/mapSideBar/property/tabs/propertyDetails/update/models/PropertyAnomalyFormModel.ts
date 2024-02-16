import { ApiGen_Concepts_PropertyAnomaly } from '@/models/api/generated/ApiGen_Concepts_PropertyAnomaly';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class PropertyAnomalyFormModel {
  id?: number;
  rowVersion?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromLookup(base: ILookupCode): PropertyAnomalyFormModel {
    const newModel = new PropertyAnomalyFormModel();
    newModel.typeCode = base.id.toString();
    newModel.typeDescription = base.name;
    return newModel;
  }

  static fromApi(base: ApiGen_Concepts_PropertyAnomaly): PropertyAnomalyFormModel {
    const newModel = new PropertyAnomalyFormModel();
    newModel.id = base.id;
    newModel.rowVersion = base.rowVersion ?? undefined;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyAnomalyTypeCode?.id ?? undefined;
    newModel.typeDescription = base.propertyAnomalyTypeCode?.description ?? undefined;
    return newModel;
  }

  toApi(): ApiGen_Concepts_PropertyAnomaly {
    return {
      id: this.id ?? 0,
      propertyId: this.propertyId ?? 0,
      propertyAnomalyTypeCode: {
        id: this.typeCode ?? null,
        description: this.typeDescription ?? null,
        displayOrder: null,
        isDisabled: false,
      },
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
