import { Api_Property, Api_PropertyManagementPurpose } from '@/models/api/Property';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { stringToUndefined } from '@/utils/formUtils';

import { UpdatePropertyDetailsFormModel } from '../../../propertyDetails/update/models';

export class ManagementSummaryFormModel extends UpdatePropertyDetailsFormModel {
  managementPurposes?: PropertyManagementPurposeModel[]; // TODO
  additionalDetails?: string;
  isUtilitiesPayable?: boolean;
  isTaxesPayable?: boolean;

  static fromApi(base: Api_Property): ManagementSummaryFormModel {
    const propertyModel = UpdatePropertyDetailsFormModel.fromApi(base);
    const managementModel = Object.assign(new ManagementSummaryFormModel(), propertyModel);
    managementModel.additionalDetails = base.additionalDetails ?? '';
    managementModel.isUtilitiesPayable = base.isUtilitiesPayable;
    managementModel.isTaxesPayable = base.isTaxesPayable;

    return managementModel;
  }

  toApi(): Api_Property {
    const apiProperty = super.toApi();
    apiProperty.managementPurposes = []; // TODO
    apiProperty.additionalDetails = stringToUndefined(this.additionalDetails);
    // apiProperty.isUtilitiesPayable = booleanToYesNoUnknownString;

    return apiProperty;
  }
}

export class PropertyManagementPurposeModel {
  id?: number;
  rowVersion?: number;
  propertyId?: number;
  typeCode?: string;
  typeDescription?: string;

  static fromLookup(base: ILookupCode): PropertyManagementPurposeModel {
    var newModel = new PropertyManagementPurposeModel();
    newModel.typeCode = base.id.toString();
    newModel.typeDescription = base.name;
    return newModel;
  }

  static fromApi(base: Api_PropertyManagementPurpose): PropertyManagementPurposeModel {
    var newModel = new PropertyManagementPurposeModel();
    newModel.id = base.id;
    newModel.rowVersion = base.rowVersion;
    newModel.propertyId = base.propertyId;
    newModel.typeCode = base.propertyPurposeTypeCode?.id;
    newModel.typeDescription = base.propertyPurposeTypeCode?.description;
    return newModel;
  }

  toApi(): Api_PropertyManagementPurpose {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      propertyId: this.propertyId,
      propertyPurposeTypeCode: {
        id: this.typeCode,
        description: this.typeDescription,
      },
    };
  }
}
