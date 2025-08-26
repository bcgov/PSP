import { ManagementActivitySubTypeModel } from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/models/ManagementActivitySubType';
import { fromApiPersonOrApiOrganization, IContactSearchResult } from '@/interfaces';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { ApiGen_Concepts_ManagementActivityInvoice } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityInvoice';
import { ApiGen_Concepts_ManagementActivityInvolvedParty } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityInvolvedParty';
import { ApiGen_Concepts_ManagementActivityProperty } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityProperty';
import { ApiGen_Concepts_ManagementFileProperty } from '@/models/api/generated/ApiGen_Concepts_ManagementFileProperty';
import { ApiGen_Concepts_PropertyMinistryContact } from '@/models/api/generated/ApiGen_Concepts_PropertyMinistryContact';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists, isValidIsoDateTime } from '@/utils';
import { emptyStringtoNullable, toTypeCodeNullable } from '@/utils/formUtils';

export class ManagementActivityFormModel {
  activityTypeCode = '';
  activitySubtypeCodes: ManagementActivitySubTypeModel[] = [];
  activityStatusCode = '';
  requestedDate = '';
  completionDate = '';
  description = '';
  requestedSource = '';
  requestorPersonId: number | null;
  requestorOrganizationId: number | null;
  requestorPrimaryContactId: number | null;
  ministryContacts: (IContactSearchResult | null)[] = [null];
  involvedParties: (IContactSearchResult | null)[] = [null];
  serviceProvider: IContactSearchResult | null = null;
  invoices: ActivityInvoiceFormModel[] = [];
  pretaxAmount = 0;
  gstAmount = 0;
  pstAmount = 0;
  totalAmount = 0;
  activityProperties: ActivityPropertyFormModel[] = [];
  selectedProperties: ApiGen_Concepts_FileProperty[] = [];

  constructor(
    readonly id: number | null = null,
    readonly managementFileId: number | null = null,
    readonly rowVersion: number | null = null,
  ) {
    this.id = id;
    this.managementFileId = managementFileId;
    this.rowVersion = rowVersion;
  }

  toApi(): ApiGen_Concepts_ManagementActivity {
    const apiActivity: ApiGen_Concepts_ManagementActivity = {
      id: this.id ?? 0,
      managementFileId: this.managementFileId,
      managementFile: null,
      activityTypeCode: toTypeCodeNullable(this.activityTypeCode),
      activitySubTypeCodes: this.activitySubtypeCodes?.map(x => x.toApi(this.id)) ?? [],
      activityStatusTypeCode: toTypeCodeNullable(this.activityStatusCode),
      requestorPersonId: this.requestorPersonId,
      requestorOrganizationId: this.requestorOrganizationId,
      requestorPrimaryContactId: this.requestorPrimaryContactId,
      requestAddedDateOnly: this.requestedDate,
      completionDateOnly: emptyStringtoNullable(this.completionDate),
      description: this.description,
      requestSource: this.requestedSource,

      isDisabled: false,
      serviceProviderOrgId: this.serviceProvider?.organizationId ?? null,
      serviceProviderOrg: null,
      serviceProviderPersonId: this.serviceProvider?.personId ?? null,
      serviceProviderPerson: null,

      involvedParties: this.involvedParties
        .filter(exists)
        .map<ApiGen_Concepts_ManagementActivityInvolvedParty>(x => {
          return {
            id: 0,
            organizationId: x.organizationId ?? null,
            organization: null,
            personId: x.personId ?? null,
            person: null,
            managementActivityId: this.id ?? 0,
            managementActivity: null,
            ...getEmptyBaseAudit(0),
          };
        }),
      ministryContacts: this.ministryContacts
        .filter(exists)
        .map<ApiGen_Concepts_PropertyMinistryContact>(x => {
          return {
            id: 0,
            personId: x.personId ?? 0,
            person: null,
            managementActivityId: this.id ?? 0,
            managementActivity: null,
            ...getEmptyBaseAudit(0),
          };
        }),
      activityProperties: this.selectedProperties.map(x => {
        const matched = this.activityProperties.find(y => y.propertyId === x.propertyId) ?? null;

        return {
          id: matched ? matched.id : 0,
          managementActivityId: this.id ?? 0,
          propertyId: x.propertyId,
          property: null,
          rowVersion: matched?.rowVersion ?? 0,
        } as ApiGen_Concepts_ManagementActivityProperty;
      }),
      invoices: this.invoices.map(i => i.toApi(this.id ?? 0)),
      ...getEmptyBaseAudit(this.rowVersion),
    };

    return apiActivity;
  }

  static fromApi(
    model: ApiGen_Concepts_ManagementActivity | undefined,
    fileProperties: ApiGen_Concepts_ManagementFileProperty[],
  ): ManagementActivityFormModel {
    const formModel = new ManagementActivityFormModel(
      model.id,
      model.managementFileId,
      model.rowVersion,
    );
    formModel.activityTypeCode = model.activityTypeCode?.id || '';
    formModel.activitySubtypeCodes =
      model.activitySubTypeCodes?.map(x => ManagementActivitySubTypeModel.fromApi(x)) ?? [];
    formModel.activityStatusCode = model.activityStatusTypeCode?.id || '';
    formModel.requestedDate = isValidIsoDateTime(model.requestAddedDateOnly)
      ? model.requestAddedDateOnly
      : '';
    formModel.completionDate = model.completionDateOnly || '';
    formModel.description = model.description || '';

    if (exists(model.ministryContacts) && model.ministryContacts.length > 0) {
      formModel.ministryContacts = model.ministryContacts.map(c =>
        fromApiPersonOrApiOrganization(c.person, null),
      );
    }
    formModel.requestedSource = model.requestSource || '';
    if (exists(model.involvedParties) && model.involvedParties.length > 0) {
      formModel.involvedParties = model.involvedParties.map(c =>
        fromApiPersonOrApiOrganization(c.person, c.organization),
      );
    }

    formModel.serviceProvider = fromApiPersonOrApiOrganization(
      model.serviceProviderPerson,
      model.serviceProviderOrg,
    );
    formModel.invoices = model.invoices?.map(i => ActivityInvoiceFormModel.fromApi(i)) ?? [];
    formModel.activityProperties =
      model.activityProperties?.map(p => ActivityPropertyFormModel.fromApi(p)) ?? [];

    formModel.selectedProperties = model.activityProperties.map(x => {
      const matchProperty =
        fileProperties.find(
          y => y.fileId === formModel.managementFileId && y.propertyId === x.propertyId,
        ) ?? null;

      return {
        id: matchProperty ? matchProperty.id : 0,
        fileId: formModel.managementFileId,
        propertyId: x.propertyId,
        property: x.property,
        rowVersion: matchProperty?.rowVersion ?? 0,
      } as ApiGen_Concepts_ManagementFileProperty;
    });

    return formModel;
  }
}

export class ActivityPropertyFormModel {
  id = 0;
  managementActivityId = 0;
  propertyId = 0;
  rowVersion = 0;

  toApi(): ApiGen_Concepts_ManagementActivityProperty {
    return {
      id: this.id,
      managementActivityId: this.managementActivityId,
      managementActivity: null,
      propertyId: this.propertyId,
      property: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(
    model: ApiGen_Concepts_ManagementActivityProperty | undefined,
  ): ActivityPropertyFormModel {
    const newFormModel = new ActivityPropertyFormModel();

    if (exists(model)) {
      newFormModel.id = model.id;
      newFormModel.managementActivityId = model.managementActivityId;
      newFormModel.propertyId = model.propertyId;
      newFormModel.rowVersion = model.rowVersion || 0;
    }

    return newFormModel;
  }
}

export class ActivityInvoiceFormModel {
  id = 0;
  invoiceDateTime = '';
  invoiceNum = '';
  description = '';

  pretaxAmount = 0;
  gstAmount = 0;
  pstAmount = 0;
  totalAmount = 0;
  isPstRequired = false;

  isDisabled = false;
  managementActivityId = 0;
  managementActivity = '';
  rowVersion = 0;

  toApi(managementActivityId: number): ApiGen_Concepts_ManagementActivityInvoice {
    return {
      id: this.id,
      invoiceDateTime: this.invoiceDateTime,
      invoiceNum: this.invoiceNum,
      description: this.description,
      pretaxAmount: Number(this.pretaxAmount),
      gstAmount: Number(this.gstAmount),
      pstAmount: Number(this.pstAmount),
      totalAmount: Number(this.totalAmount),
      isPstRequired: this.isPstRequired,
      isDisabled: this.isDisabled,
      managementActivityId: managementActivityId,
      managementActivity: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(
    model: ApiGen_Concepts_ManagementActivityInvoice | undefined,
  ): ActivityInvoiceFormModel {
    const formModel = new ActivityInvoiceFormModel();

    if (exists(model)) {
      formModel.id = model.id;
      formModel.invoiceDateTime = isValidIsoDateTime(model.invoiceDateTime)
        ? model.invoiceDateTime
        : '';
      formModel.invoiceNum = model.invoiceNum || '';
      formModel.description = model.description || '';
      formModel.pretaxAmount = model.pretaxAmount;
      formModel.gstAmount = model.gstAmount || 0;
      formModel.pstAmount = model.pstAmount || 0;
      formModel.totalAmount = model.totalAmount || 0;
      formModel.isPstRequired = model.isPstRequired || false;
      formModel.isDisabled = model.isDisabled || false;
      formModel.managementActivityId = model.managementActivityId || 0;
      formModel.rowVersion = model.rowVersion || 0;
    }

    return formModel;
  }
}
