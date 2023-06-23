import { Api_PropertyAnomaly } from '@/models/api/Property';
import { ILookupCode } from '@/store/slices/lookupCodes';

export class PropertyAnomalyFormModel {
  id?: number;
  rowVersion?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromLookup(base: ILookupCode): PropertyAnomalyFormModel {
    var newModel = new PropertyAnomalyFormModel();
    newModel.typeCode = base.id.toString();
    newModel.typeDescription = base.name;
    return newModel;
  }

  static fromApi(base: Api_PropertyAnomaly): PropertyAnomalyFormModel {
    var newModel = new PropertyAnomalyFormModel();
    newModel.id = base.id;
    newModel.rowVersion = base.rowVersion;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyAnomalyTypeCode?.id;
    newModel.typeDescription = base.propertyAnomalyTypeCode?.description;
    return newModel;
  }

  toApi(): Api_PropertyAnomaly {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      propertyId: this.propertyId,
      propertyAnomalyTypeCode: {
        id: this.typeCode,
        description: this.typeDescription,
      },
    };
  }
}
