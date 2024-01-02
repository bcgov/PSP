import {
  fromApiOrganization,
  fromApiPerson,
  IContactSearchResult,
} from '@/interfaces/IContactSearchResult';
import { Api_Organization } from '@/models/api/Organization';
import { Api_Person } from '@/models/api/Person';
import {
  Api_PropertyActivity,
  Api_PropertyActivityInvoice,
  Api_PropertyActivityInvolvedParty,
  Api_PropertyActivityProperty,
  Api_PropertyMinistryContact,
} from '@/models/api/PropertyActivity';
import { emptyStringtoNullable, toTypeCode } from '@/utils/formUtils';

export class ActivityPropertyFormModel {
  id: number = 0;
  propertyActivityId: number = 0;
  propertyId: number = 0;
  rowVersion: number = 0;

  toApi(): Api_PropertyActivityProperty {
    return {
      id: this.id,
      propertyActivityId: this.propertyActivityId,
      propertyActivityModel: null,
      propertyId: this.propertyId,
      property: null,
      rowVersion: this.rowVersion,
    };
  }

  static fromApi(model: Api_PropertyActivityProperty | undefined): ActivityPropertyFormModel {
    const newFormModel = new ActivityPropertyFormModel();

    if (model !== undefined) {
      newFormModel.id = model.id;
      newFormModel.propertyActivityId = model.propertyActivityId;
      newFormModel.propertyId = model.propertyId;
      newFormModel.rowVersion = model.rowVersion || 0;
    }

    return newFormModel;
  }
}

export class ActivityInvoiceFormModel {
  id: number = 0;
  invoiceDateTime: string = '';
  invoiceNum: string = '';
  description: string = '';

  pretaxAmount: number = 0;
  gstAmount: number = 0;
  pstAmount: number = 0;
  totalAmount: number = 0;
  isPstRequired: boolean = false;

  isDisabled: boolean = false;
  propertyActivityId: number = 0;
  propertyActivity: string = '';
  rowVersion: number = 0;

  toApi(propertyActivityId: number): Api_PropertyActivityInvoice {
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
      propertyActivityId: propertyActivityId,
      propertyActivity: null,
      rowVersion: this.rowVersion,
    };
  }

  static fromApi(model: Api_PropertyActivityInvoice | undefined): ActivityInvoiceFormModel {
    const formModel = new ActivityInvoiceFormModel();

    if (model !== undefined) {
      formModel.id = model.id;
      formModel.invoiceDateTime = model.invoiceDateTime;
      formModel.invoiceNum = model.invoiceNum;
      formModel.description = model.description;
      formModel.pretaxAmount = model.pretaxAmount;
      formModel.gstAmount = model.gstAmount || 0;
      formModel.pstAmount = model.pstAmount || 0;
      formModel.totalAmount = model.totalAmount || 0;
      formModel.isPstRequired = model.isPstRequired || false;
      formModel.isDisabled = model.isDisabled || false;
      formModel.propertyActivityId = model.propertyActivityId || 0;
      //formModel.propertyActivity = model.propertyActivity;
      formModel.rowVersion = model.rowVersion || 0;
    }

    return formModel;
  }
}

export class PropertyActivityFormModel {
  id: number = 0;
  activityTypeCode: string = '';
  activitySubtypeCode: string = '';
  activityStatusCode: string = '';
  requestedDate: string = '';
  completionDate: string = '';
  description: string = '';
  ministryContacts: (IContactSearchResult | null)[] = [null];
  requestedSource: string = '';
  involvedParties: (IContactSearchResult | null)[] = [null];
  serviceProvider: IContactSearchResult | null = null;
  invoices: ActivityInvoiceFormModel[] = [];

  pretaxAmount: number = 0;
  gstAmount: number = 0;
  pstAmount: number = 0;
  totalAmount: number = 0;

  rowNumber: number = 0;

  activityProperties: ActivityPropertyFormModel[] = [];

  toApi(propertyId: number): Api_PropertyActivity {
    const apiActivity: Api_PropertyActivity = {
      id: this.id,
      activityTypeCode: toTypeCode(this.activityTypeCode) || {},
      activitySubtypeCode: toTypeCode(this.activitySubtypeCode) || {},
      activityStatusTypeCode: toTypeCode(this.activityStatusCode) || {},
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
        .filter((x): x is IContactSearchResult => x !== null)
        .map<Api_PropertyActivityInvolvedParty>(x => {
          return {
            id: 0,
            organizationId: x.organizationId ?? null,
            organization: null,
            personId: x.personId ?? null,
            person: null,
            propertyActivityId: this.id,
            propertyActivity: null,
            rowVersion: 0,
          };
        }),
      ministryContacts: this.ministryContacts
        .filter((x): x is IContactSearchResult => x !== null)
        .map<Api_PropertyMinistryContact>(x => {
          return {
            id: 0,
            personId: x.personId || 0,
            person: null,
            propertyActivityId: this.id,
            pimsPropertyActivity: null,
            rowVersion: 0,
          };
        }),
      activityProperties: [],
      invoices: this.invoices.map(i => i.toApi(this.id)),

      pretaxAmt: this.pretaxAmount,
      gstAmt: this.gstAmount,
      pstAmt: this.pstAmount,
      totalAmt: this.totalAmount,
      rowVersion: this.rowNumber,
    };

    if (this.activityProperties.length > 0) {
      apiActivity.activityProperties = this.activityProperties.map<Api_PropertyActivityProperty>(
        x => x.toApi(),
      );
    } else {
      const newProperty = new ActivityPropertyFormModel();
      newProperty.propertyId = propertyId;
      newProperty.propertyActivityId = this.id;

      apiActivity.activityProperties = [newProperty.toApi()];
    }

    return apiActivity;
  }
  static fromApi(model: Api_PropertyActivity | undefined): PropertyActivityFormModel {
    const formModel = new PropertyActivityFormModel();
    if (model !== undefined) {
      formModel.id = model.id;
      formModel.activityTypeCode = model.activityTypeCode.id || '';
      formModel.activitySubtypeCode = model.activitySubtypeCode.id || '';
      formModel.activityStatusCode = model.activityStatusTypeCode.id || '';
      formModel.requestedDate = model.requestAddedDateOnly;
      formModel.completionDate = model.completionDateOnly || '';
      formModel.description = model.description;

      if (model.ministryContacts.length > 0) {
        formModel.ministryContacts = model.ministryContacts.map(c =>
          fromApiPersonOrApiOrganization(c.person, null),
        );
      }
      formModel.requestedSource = model.requestSource;
      if (model.involvedParties.length > 0) {
        formModel.involvedParties = model.involvedParties.map(c =>
          fromApiPersonOrApiOrganization(c.person, c.organization),
        );
      }

      formModel.serviceProvider = fromApiPersonOrApiOrganization(
        model.serviceProviderPerson,
        model.serviceProviderOrg,
      );
      formModel.invoices = model.invoices.map(i => ActivityInvoiceFormModel.fromApi(i));
      formModel.pretaxAmount = model.pretaxAmt || 0;
      formModel.gstAmount = model.gstAmt || 0;
      formModel.pstAmount = model.pstAmt || 0;
      formModel.totalAmount = model.totalAmt || 0;
      formModel.rowNumber = model.rowVersion || 0;
      formModel.activityProperties = model.activityProperties.map(p =>
        ActivityPropertyFormModel.fromApi(p),
      );
    }
    return formModel;
  }
}

function fromApiPersonOrApiOrganization(
  person: Api_Person | null,
  organization: Api_Organization | null,
): IContactSearchResult | null {
  if (person !== null) {
    return fromApiPerson(person);
  } else if (organization !== null) {
    return fromApiOrganization(organization);
  }
  return null;
}
