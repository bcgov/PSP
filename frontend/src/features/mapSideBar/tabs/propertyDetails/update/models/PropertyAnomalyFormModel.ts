import { Api_PropertyAnomaly } from 'models/api/Property';

export class PropertyAnomalyFormModel {
  id?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromApi(base: Api_PropertyAnomaly): PropertyAnomalyFormModel {
    var newModel = new PropertyAnomalyFormModel();
    newModel.id = base.id;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyAnomalyTypeCode?.id;
    newModel.typeDescription = base.propertyAnomalyTypeCode?.description;
    return newModel;
  }

  toApi(): Api_PropertyAnomaly {
    return {
      id: this.id,
      propertyAnomalyTypeCode: {
        id: this.typeCode,
        description: this.typeDescription,
      },
    };
  }
}
