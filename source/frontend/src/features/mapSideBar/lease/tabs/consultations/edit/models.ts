import { fromApiOrganization, fromApiPerson, IContactSearchResult } from '@/interfaces';
import { ApiGen_Concepts_ConsultationLease } from '@/models/api/generated/ApiGen_Concepts_ConsultationLease';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists } from '@/utils';
import { emptyStringtoNullable, toNullableId, toTypeCodeNullable } from '@/utils/formUtils';

export class ConsultationFormModel {
  public id: number;
  public leaseId: number;
  public contact: IContactSearchResult;
  public primaryContactId: number;
  public consultationTypeCode: string;
  public consultationTypeDescription: string;
  public consultationStatusTypeDescription: string;
  public consultationOutcomeTypeCode: string;
  public consultationOutcomeTypeCodeDescription: string;
  public otherDescription: string;
  public requestedOn: string;
  public isResponseReceived: boolean;
  public responseReceivedDate: string;
  public comment: string;
  public rowVersion: number;

  constructor(leaseId: number) {
    this.leaseId = leaseId;
    this.id = 0;
    this.consultationTypeCode = '';
    this.consultationTypeDescription = '';
    this.consultationStatusTypeDescription = '';
    this.consultationOutcomeTypeCode = '';
    this.consultationOutcomeTypeCodeDescription = '';
    this.otherDescription = '';
    this.requestedOn = '';
    this.isResponseReceived = false;
    this.responseReceivedDate = '';
    this.comment = '';
    this.rowVersion = 0;
  }

  static fromApi(
    apiModel: ApiGen_Concepts_ConsultationLease,
    person: ApiGen_Concepts_Person | null,
    organization: ApiGen_Concepts_Organization | null,
  ): ConsultationFormModel {
    const consultation = new ConsultationFormModel(apiModel.leaseId);
    consultation.id = apiModel.id;
    consultation.consultationTypeCode = apiModel.consultationTypeCode?.id ?? '';
    consultation.consultationOutcomeTypeCode = apiModel.consultationOutcomeTypeCode?.id ?? '';
    consultation.consultationTypeDescription = apiModel.consultationTypeCode?.description ?? '';
    consultation.consultationStatusTypeDescription =
      apiModel.consultationStatusTypeCode?.description ?? '';
    consultation.consultationOutcomeTypeCodeDescription =
      apiModel.consultationOutcomeTypeCode?.description ?? '';
    consultation.otherDescription = apiModel.otherDescription ?? '';
    consultation.requestedOn = apiModel.requestedOn ?? '';
    consultation.isResponseReceived = apiModel.isResponseReceived ?? false;
    consultation.responseReceivedDate = apiModel.responseReceivedDate ?? '';
    consultation.comment = apiModel.comment ?? '';
    consultation.rowVersion = apiModel.rowVersion ?? 0;

    const contact: IContactSearchResult | undefined = exists(person)
      ? fromApiPerson(person)
      : exists(organization)
      ? fromApiOrganization(organization)
      : undefined;

    consultation.primaryContactId = apiModel.primaryContactId;

    consultation.contact = contact;

    return consultation;
  }

  public toApi(): ApiGen_Concepts_ConsultationLease {
    const personId = this.contact?.personId ?? null;
    const organizationId = !personId ? this.contact?.organizationId ?? null : null;

    return {
      id: this.id,
      leaseId: this.leaseId,
      personId: toNullableId(personId),
      organizationId: toNullableId(organizationId),
      primaryContactId: toNullableId(this.primaryContactId),
      consultationTypeCode: toTypeCodeNullable(this.consultationTypeCode),
      consultationStatusTypeCode: toTypeCodeNullable('UNKNOWN'),
      consultationOutcomeTypeCode: toTypeCodeNullable(this.consultationOutcomeTypeCode),
      otherDescription: emptyStringtoNullable(this.otherDescription),
      requestedOn: emptyStringtoNullable(this.requestedOn),
      isResponseReceived: this.isResponseReceived,
      responseReceivedDate: emptyStringtoNullable(this.responseReceivedDate),
      comment: emptyStringtoNullable(this.comment),
      lease: null,
      person: null,
      organization: null,
      primaryContact: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
