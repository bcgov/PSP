import { Api_PropertyTenure } from '@/models/api/Property';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class PropertyTenureFormModel {
  id?: number;
  rowVersion?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromLookup(base: ILookupCode): PropertyTenureFormModel {
    var newModel = new PropertyTenureFormModel();
    newModel.typeCode = base.id.toString();
    newModel.typeDescription = base.name;
    return newModel;
  }

  static fromApi(base: Api_PropertyTenure): PropertyTenureFormModel {
    var newModel = new PropertyTenureFormModel();
    newModel.id = base.id;
    newModel.rowVersion = base.rowVersion;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyTenureTypeCode?.id;
    newModel.typeDescription = base.propertyTenureTypeCode?.description;
    return newModel;
  }

  toApi(): Api_PropertyTenure {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      propertyId: this.propertyId,
      propertyTenureTypeCode: {
        id: this.typeCode,
        description: this.typeDescription,
      },
    };
  }
}
