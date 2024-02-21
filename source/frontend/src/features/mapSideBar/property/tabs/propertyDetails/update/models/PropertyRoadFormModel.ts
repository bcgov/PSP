import { ApiGen_Concepts_PropertyRoad } from '@/models/api/generated/ApiGen_Concepts_PropertyRoad';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class PropertyRoadFormModel {
  id?: number;
  rowVersion?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromLookup(base: ILookupCode): PropertyRoadFormModel {
    const newModel = new PropertyRoadFormModel();
    newModel.typeCode = base.id.toString();
    newModel.typeDescription = base.name;
    return newModel;
  }

  static fromApi(base: ApiGen_Concepts_PropertyRoad): PropertyRoadFormModel {
    const newModel = new PropertyRoadFormModel();
    newModel.id = base.id;
    newModel.rowVersion = base.rowVersion ?? undefined;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyRoadTypeCode?.id ?? undefined;
    newModel.typeDescription = base.propertyRoadTypeCode?.description ?? undefined;
    return newModel;
  }

  toApi(): ApiGen_Concepts_PropertyRoad {
    return {
      id: this.id ?? 0,
      propertyId: this.propertyId ?? 0,
      propertyRoadTypeCode: {
        id: this.typeCode ?? null,
        description: this.typeDescription ?? null,
        displayOrder: null,
        isDisabled: false,
      },
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
