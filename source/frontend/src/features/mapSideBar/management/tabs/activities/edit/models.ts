import {
  ActivityInvoiceFormModel,
  ActivityPropertyFormModel,
} from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/edit/models';
import { fromApiPersonOrApiOrganization, IContactSearchResult } from '@/interfaces';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { ApiGen_Concepts_PropertyActivityInvolvedParty } from '@/models/api/generated/ApiGen_Concepts_PropertyActivityInvolvedParty';
import { ApiGen_Concepts_PropertyActivityProperty } from '@/models/api/generated/ApiGen_Concepts_PropertyActivityProperty';
import { ApiGen_Concepts_PropertyMinistryContact } from '@/models/api/generated/ApiGen_Concepts_PropertyMinistryContact';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists, isValidIsoDateTime } from '@/utils';
import { emptyStringtoNullable, toTypeCodeNullable } from '@/utils/formUtils';

export class ManagementActivityFormModel {
  activityTypeCode = '';
  activitySubtypeCode = '';
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
  activityProperties: ActivityPropertyFormModel[] = [];

  constructor(
    readonly id: number | null = null,
    readonly managementFileId: number | null = null,
    readonly rowVersion: number | null = null,
  ) {
    this.id = id;
    this.managementFileId = managementFileId;
    this.rowVersion = rowVersion;
  }

  toApi(propertyId: number): ApiGen_Concepts_PropertyActivity {
    const apiActivity: ApiGen_Concepts_PropertyActivity = {
      id: this.id ?? 0,
      managementFileId: null,
      activityTypeCode: toTypeCodeNullable(this.activityTypeCode),
      activitySubtypeCode: toTypeCodeNullable(this.activitySubtypeCode),
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
        .map<ApiGen_Concepts_PropertyActivityInvolvedParty>(x => {
          return {
            id: 0,
            organizationId: x.organizationId ?? null,
            organization: null,
            personId: x.personId ?? null,
            person: null,
            propertyActivityId: this.id,
            propertyActivity: null,
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
            propertyActivityId: this.id,
            propertyActivity: null,
            ...getEmptyBaseAudit(0),
          };
        }),
      activityProperties: [],
      invoices: this.invoices.map(i => i.toApi(this.id)),

      ...getEmptyBaseAudit(this.rowVersion),
    };

    if (this.activityProperties.length > 0) {
      apiActivity.activityProperties =
        this.activityProperties.map<ApiGen_Concepts_PropertyActivityProperty>(x => x.toApi());
    } else {
      const newProperty = new ActivityPropertyFormModel();
      newProperty.propertyId = propertyId;
      newProperty.propertyActivityId = this.id;

      apiActivity.activityProperties = [newProperty.toApi()];
    }

    return apiActivity;
  }

  static fromApi(model: ApiGen_Concepts_PropertyActivity | undefined): ManagementActivityFormModel {
    const formModel = new ManagementActivityFormModel(
      model.id,
      model.managementFileId,
      model.rowVersion,
    );
    formModel.activityTypeCode = model.activityTypeCode?.id || '';
    formModel.activitySubtypeCode = model.activitySubtypeCode?.id || '';
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

    return formModel;
  }
}
