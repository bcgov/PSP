import { Api_PropertyTenure } from 'models/api/Property';

export class PropertyTenureFormModel {
  id?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromApi(base: Api_PropertyTenure): PropertyTenureFormModel {
    var newModel = new PropertyTenureFormModel();
    newModel.id = base.id;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyTenureTypeCode?.id;
    newModel.typeDescription = base.propertyTenureTypeCode?.description;
    return newModel;
  }

  toApi(): Api_PropertyTenure {
    return {
      id: this.id,
      propertyId: this.propertyId,
      propertyTenureTypeCode: {
        id: this.typeCode,
        description: this.typeDescription,
      },
    };
  }
}
