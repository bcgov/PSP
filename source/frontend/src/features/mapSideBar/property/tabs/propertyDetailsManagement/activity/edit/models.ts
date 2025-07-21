import {
  fromApiPersonOrApiOrganization,
  IContactSearchResult,
} from '@/interfaces/IContactSearchResult';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { ApiGen_Concepts_ManagementActivityInvoice } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityInvoice';
import { ApiGen_Concepts_ManagementActivityInvolvedParty } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityInvolvedParty';
import { ApiGen_Concepts_ManagementActivityProperty } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityProperty';
import { ApiGen_Concepts_PropertyMinistryContact } from '@/models/api/generated/ApiGen_Concepts_PropertyMinistryContact';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists, isValidIsoDateTime } from '@/utils';
import { emptyStringtoNullable, toTypeCodeNullable } from '@/utils/formUtils';

import { ManagementActivitySubTypeModel } from '../models/ManagementActivitySubType';

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

    if (model !== undefined) {
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

export class PropertyActivityFormModel {
  id = 0;
  activityTypeCode = '';
  activitySubtypeCodes: ManagementActivitySubTypeModel[] = [];
  activityStatusCode = '';
  requestedDate = '';
  completionDate = '';
  description = '';
  ministryContacts: (IContactSearchResult | null)[] = [null];
  requestedSource = '';
  involvedParties: (IContactSearchResult | null)[] = [null];
  serviceProvider: IContactSearchResult | null = null;
  invoices: ActivityInvoiceFormModel[] = [];

  pretaxAmount = 0;
  gstAmount = 0;
  pstAmount = 0;
  totalAmount = 0;

  rowNumber = 0;

  activityProperties: ActivityPropertyFormModel[] = [];

  toApi(propertyId: number): ApiGen_Concepts_ManagementActivity {
    const apiActivity: ApiGen_Concepts_ManagementActivity = {
      id: this.id,
      managementFileId: null,
      managementFile: null,
      activityTypeCode: toTypeCodeNullable(this.activityTypeCode),
      activitySubTypeCodes: this.activitySubtypeCodes?.map(x => x.toApi(this.id)) ?? [],
      activityStatusTypeCode: toTypeCodeNullable(this.activityStatusCode),
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
            managementActivityId: this.id,
            managementActivity: null,
            ...getEmptyBaseAudit(0),
          };
        }),
      ministryContacts: this.ministryContacts
        .filter(exists)
        .map<ApiGen_Concepts_PropertyMinistryContact>(x => {
          return {
            id: 0,
            personId: x.personId || 0,
            person: null,
            managementActivityId: this.id,
            managementActivity: null,
            ...getEmptyBaseAudit(0),
          };
        }),
      activityProperties: [],
      invoices: this.invoices.map(i => i.toApi(this.id)),

      ...getEmptyBaseAudit(this.rowNumber),
    };

    if (this.activityProperties.length > 0) {
      apiActivity.activityProperties =
        this.activityProperties.map<ApiGen_Concepts_ManagementActivityProperty>(x => x.toApi());
    } else {
      const newProperty = new ActivityPropertyFormModel();
      newProperty.propertyId = propertyId;
      newProperty.managementActivityId = this.id;

      apiActivity.activityProperties = [newProperty.toApi()];
    }

    return apiActivity;
  }

  static fromApi(
    model: ApiGen_Concepts_ManagementActivity | null | undefined,
  ): PropertyActivityFormModel {
    const formModel = new PropertyActivityFormModel();
    if (exists(model)) {
      formModel.id = model.id;
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
      formModel.rowNumber = model.rowVersion || 0;
      formModel.activityProperties =
        model.activityProperties?.map(p => ActivityPropertyFormModel.fromApi(p)) ?? [];
    }
    return formModel;
  }
}
