import { Api_PropertyRoad } from 'models/api/Property';

export class PropertyRoadFormModel {
  id?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromApi(base: Api_PropertyRoad): PropertyRoadFormModel {
    var newModel = new PropertyRoadFormModel();
    newModel.id = base.id;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyRoadTypeCode?.id;
    newModel.typeDescription = base.propertyRoadTypeCode?.description;
    return newModel;
  }

  toApi(): Api_PropertyRoad {
    return {
      id: this.id,
      propertyId: this.propertyId,
      propertyRoadTypeCode: {
        id: this.typeCode,
        description: this.typeDescription,
      },
    };
  }
}
