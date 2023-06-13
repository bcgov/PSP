import { Api_PropertyRoad } from '@/models/api/Property';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class PropertyRoadFormModel {
  id?: number;
  rowVersion?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromLookup(base: ILookupCode): PropertyRoadFormModel {
    var newModel = new PropertyRoadFormModel();
    newModel.typeCode = base.id.toString();
    newModel.typeDescription = base.name;
    return newModel;
  }

  static fromApi(base: Api_PropertyRoad): PropertyRoadFormModel {
    var newModel = new PropertyRoadFormModel();
    newModel.id = base.id;
    newModel.rowVersion = base.rowVersion;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyRoadTypeCode?.id;
    newModel.typeDescription = base.propertyRoadTypeCode?.description;
    return newModel;
  }

  toApi(): Api_PropertyRoad {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      propertyId: this.propertyId,
      propertyRoadTypeCode: {
        id: this.typeCode,
        description: this.typeDescription,
      },
    };
  }
}
