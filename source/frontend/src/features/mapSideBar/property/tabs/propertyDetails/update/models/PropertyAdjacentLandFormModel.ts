import { Api_PropertyAdjacentLand } from '@/models/api/Property';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class PropertyAdjacentLandFormModel {
  id?: number;
  rowVersion?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromLookup(base: ILookupCode): PropertyAdjacentLandFormModel {
    var newModel = new PropertyAdjacentLandFormModel();
    newModel.typeCode = base.id.toString();
    newModel.typeDescription = base.name;
    return newModel;
  }

  static fromApi(base: Api_PropertyAdjacentLand): PropertyAdjacentLandFormModel {
    var newModel = new PropertyAdjacentLandFormModel();
    newModel.id = base.id;
    newModel.rowVersion = base.rowVersion;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyAdjacentLandTypeCode?.id;
    newModel.typeDescription = base.propertyAdjacentLandTypeCode?.description;
    return newModel;
  }

  toApi(): Api_PropertyAdjacentLand {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      propertyId: this.propertyId,
      propertyAdjacentLandTypeCode: {
        id: this.typeCode,
        description: this.typeDescription,
      },
    };
  }
}
