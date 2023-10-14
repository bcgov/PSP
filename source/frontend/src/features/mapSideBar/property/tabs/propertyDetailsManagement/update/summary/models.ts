import { Api_PropertyManagement, Api_PropertyManagementPurpose } from '@/models/api/Property';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { prettyFormatDate } from '@/utils';
import { stringToNull } from '@/utils/formUtils';

export class PropertyManagementFormModel {
  id: number = 0;
  rowVersion: number = 0;
  managementPurposes: ManagementPurposeModel[] = [];
  additionalDetails: string = '';
  isUtilitiesPayable: boolean | null = null;
  isTaxesPayable: boolean | null = null;
  isLeaseActive: boolean = false;
  isLeaseExpired: boolean = false;
  leaseExpiryDate: string | null = null;

  static fromApi(base: Api_PropertyManagement | null): PropertyManagementFormModel {
    const newFormModel = new PropertyManagementFormModel();
    newFormModel.id = base?.id || 0;
    newFormModel.rowVersion = base?.rowVersion || 0;
    newFormModel.managementPurposes =
      base?.managementPurposes?.map(p => ManagementPurposeModel.fromApi(p)) || [];
    newFormModel.additionalDetails = base?.additionalDetails || '';
    newFormModel.isUtilitiesPayable = base?.isUtilitiesPayable ?? null;
    newFormModel.isTaxesPayable = base?.isTaxesPayable ?? null;
    newFormModel.isLeaseActive = base?.isLeaseActive || false;
    newFormModel.isLeaseExpired = base?.isLeaseExpired || false;
    newFormModel.leaseExpiryDate = base?.leaseExpiryDate || '';

    return newFormModel;
  }

  toApi(): Api_PropertyManagement {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      managementPurposes: this.managementPurposes.map(p => ({
        ...p.toApi(),
        propertyId: this.id,
      })),
      additionalDetails: stringToNull(this.additionalDetails),
      isUtilitiesPayable: this.isUtilitiesPayable,
      isTaxesPayable: this.isTaxesPayable,
      isLeaseActive: this.isLeaseActive,
      isLeaseExpired: this.isLeaseExpired,
      leaseExpiryDate: stringToNull(this.leaseExpiryDate),
    };
  }

  formatLeaseInformation(): string {
    if (this.isLeaseActive) {
      const expiryDate = this.leaseExpiryDate ? `(${prettyFormatDate(this.leaseExpiryDate)})` : '';
      return this.isLeaseExpired ? `Expired ${expiryDate}`.trim() : `Yes ${expiryDate}`.trim();
    } else {
      return 'No active Lease/License';
    }
  }
}

export class ManagementPurposeModel {
  id: number = 0;
  rowVersion: number = 0;
  propertyId: number | null = null;
  typeCode: string = '';
  typeDescription: string = '';

  static fromLookup(base: ILookupCode): ManagementPurposeModel {
    var newModel = new ManagementPurposeModel();
    newModel.typeCode = base.id.toString();
    newModel.typeDescription = base.name;
    return newModel;
  }

  static fromApi(base: Api_PropertyManagementPurpose | null): ManagementPurposeModel {
    var newModel = new ManagementPurposeModel();
    newModel.id = base?.id || 0;
    newModel.rowVersion = base?.rowVersion || 0;
    newModel.propertyId = base?.propertyId ?? null;
    newModel.typeCode = base?.propertyPurposeTypeCode?.id || '';
    newModel.typeDescription = base?.propertyPurposeTypeCode?.description || '';
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
