import {
  fromApiOrganization,
  fromApiPerson,
  IContactSearchResult,
} from '@/interfaces/IContactSearchResult';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';
import { ApiGen_Concepts_PropertyManagementPurpose } from '@/models/api/generated/ApiGen_Concepts_PropertyManagementPurpose';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { exists, formatApiPropertyManagementLease, isValidId } from '@/utils';
import { stringToNull, toNullableId } from '@/utils/formUtils';

export class PropertyManagementFormModel {
  id = 0;
  rowVersion = 0;
  managementPurposes: ManagementPurposeModel[] = [];
  additionalDetails = '';
  isUtilitiesPayable: boolean | null = null;
  isTaxesPayable: boolean | null = null;
  responsiblePayer: IContactSearchResult | null = null;
  responsiblePayerPrimaryContactId: number | null = null;
  formattedLeaseInformation: string | null = null;

  static fromApi(base: ApiGen_Concepts_PropertyManagement | null): PropertyManagementFormModel {
    const newFormModel = new PropertyManagementFormModel();
    newFormModel.id = base?.id || 0;
    newFormModel.rowVersion = base?.rowVersion || 0;
    newFormModel.managementPurposes =
      base?.managementPurposes?.map(p => ManagementPurposeModel.fromApi(p)) || [];
    newFormModel.additionalDetails = base?.additionalDetails || '';
    newFormModel.isUtilitiesPayable = base?.isUtilitiesPayable ?? null;
    newFormModel.isTaxesPayable = base?.isTaxesPayable ?? null;
    newFormModel.formattedLeaseInformation = formatApiPropertyManagementLease(base);

    const contact: IContactSearchResult | null = exists(base?.responsiblePayerPerson)
      ? fromApiPerson(base?.responsiblePayerPerson)
      : exists(base?.responsiblePayerOrganization)
      ? fromApiOrganization(base?.responsiblePayerOrganization)
      : null;

    newFormModel.responsiblePayerPrimaryContactId = base?.responsiblePayerPrimaryContactId ?? null;
    newFormModel.responsiblePayer = contact ?? null;

    return newFormModel;
  }

  toApi(): ApiGen_Concepts_PropertyManagement {
    const personId = this.responsiblePayer?.personId ?? null;
    const organizationId = !personId ? this.responsiblePayer?.organizationId ?? null : null;

    return {
      id: this.id,
      managementPurposes: this.managementPurposes.map(p => ({
        ...p.toApi(),
        propertyId: this.id,
      })),
      additionalDetails: stringToNull(this.additionalDetails),
      isUtilitiesPayable: this.isUtilitiesPayable,
      isTaxesPayable: this.isTaxesPayable,
      responsiblePayerPersonId: toNullableId(personId),
      responsiblePayerPerson: null,
      responsiblePayerOrganizationId: toNullableId(organizationId),
      responsiblePayerOrganization: null,
      responsiblePayerPrimaryContactId: isValidId(personId)
        ? null
        : toNullableId(this.responsiblePayerPrimaryContactId),
      responsiblePayerPrimaryContact: null,
      relatedLeases: 0,
      leaseExpiryDate: null,
      hasActiveLease: null,
      activeLeaseHasExpiryDate: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}

export class ManagementPurposeModel {
  id = 0;
  rowVersion = 0;
  propertyId: number | null = null;
  typeCode = '';
  typeDescription = '';

  static fromLookup(base: ILookupCode): ManagementPurposeModel {
    const newModel = new ManagementPurposeModel();
    newModel.typeCode = base.id.toString();
    newModel.typeDescription = base.name;
    return newModel;
  }

  static fromApi(base: ApiGen_Concepts_PropertyManagementPurpose | null): ManagementPurposeModel {
    const newModel = new ManagementPurposeModel();
    newModel.id = base?.id || 0;
    newModel.rowVersion = base?.rowVersion || 0;
    newModel.propertyId = base?.propertyId ?? null;
    newModel.typeCode = base?.propertyPurposeTypeCode?.id || '';
    newModel.typeDescription = base?.propertyPurposeTypeCode?.description || '';
    return newModel;
  }

  toApi(): ApiGen_Concepts_PropertyManagementPurpose {
    return {
      id: this.id,
      propertyId: this.propertyId ?? 0,
      propertyPurposeTypeCode: {
        id: this.typeCode,
        description: this.typeDescription,
        displayOrder: null,
        isDisabled: false,
      },
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
