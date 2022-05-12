import { Api_PropertyAdjacentLand } from 'models/api/Property';

export class PropertyAdjacentLandFormModel {
  id?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromApi(base: Api_PropertyAdjacentLand): PropertyAdjacentLandFormModel {
    var newModel = new PropertyAdjacentLandFormModel();
    newModel.id = base.id;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyAdjacentLandTypeCode?.id;
    newModel.typeDescription = base.propertyAdjacentLandTypeCode?.description;
    return newModel;
  }

  toApi(): Api_PropertyAdjacentLand {
    return {
      id: this.id,
      propertyAdjacentLandTypeCode: {
        id: this.typeCode,
        description: this.typeDescription,
      },
    };
  }
}
