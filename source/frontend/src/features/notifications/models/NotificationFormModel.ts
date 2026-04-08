import { ApiGen_Concepts_Notification } from '@/models/api/generated/ApiGen_Concepts_Notification';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { stringToNull } from '@/utils/formUtils';
import { isValidId, isValidIsoDateTime } from '@/utils/utils';

export class NotificationFormModel {
  id: number | null;
  rowVersion: number | null = null;
  notificationType: string | null = null;
  triggerDate = '';
  message: string | null = null;
  // FK to related entities (e.g. acquisitionFileId, leaseId, etc.) can be added here as needed
  acquisitionFileId: number | null;
  dispositionFileId: number | null;
  researchFileId: number | null;
  managementFileId: number | null;
  leaseId: number | null;
  takeId: number | null;
  insuranceId: number | null;
  leaseConsultationId: number | null;
  noticeOfClaimId: number | null;
  leaseRenewalId: number | null;
  expropOwnerHistoryId: number | null;
  agreementId: number | null;

  toApi(): ApiGen_Concepts_Notification {
    return {
      ...getEmptyBaseAudit(this.rowVersion),
      notificationId: this.id,
      notificationTypeCode: stringToNull(this.notificationType),
      notificationTriggerDate: isValidIsoDateTime(this.triggerDate) ? this.triggerDate : null,
      notificationMessage: stringToNull(this.message),
      acquisitionFileId: isValidId(this.acquisitionFileId) ? this.acquisitionFileId : null,
      dispositionFileId: isValidId(this.dispositionFileId) ? this.dispositionFileId : null,
      researchFileId: isValidId(this.researchFileId) ? this.researchFileId : null,
      managementFileId: isValidId(this.managementFileId) ? this.managementFileId : null,
      leaseId: isValidId(this.leaseId) ? this.leaseId : null,
      takeId: isValidId(this.takeId) ? this.takeId : null,
      insuranceId: isValidId(this.insuranceId) ? this.insuranceId : null,
      leaseConsultationId: isValidId(this.leaseConsultationId) ? this.leaseConsultationId : null,
      noticeOfClaimId: isValidId(this.noticeOfClaimId) ? this.noticeOfClaimId : null,
      leaseRenewalId: isValidId(this.leaseRenewalId) ? this.leaseRenewalId : null,
      expropOwnerHistoryId: isValidId(this.expropOwnerHistoryId) ? this.expropOwnerHistoryId : null,
      agreementId: isValidId(this.agreementId) ? this.agreementId : null,
    };
  }

  static fromApi(model: ApiGen_Concepts_Notification): NotificationFormModel {
    const notification = new NotificationFormModel();
    notification.id = model.notificationId ?? null;
    notification.rowVersion = model.rowVersion ?? null;
    notification.notificationType = model.notificationTypeCode ?? null;
    notification.triggerDate = isValidIsoDateTime(model.notificationTriggerDate)
      ? model.notificationTriggerDate
      : null;
    notification.message = model.notificationMessage ?? null;
    notification.acquisitionFileId = model.acquisitionFileId ?? null;
    notification.dispositionFileId = model.dispositionFileId ?? null;
    notification.researchFileId = model.researchFileId ?? null;
    notification.managementFileId = model.managementFileId ?? null;
    notification.leaseId = model.leaseId ?? null;
    notification.takeId = model.takeId ?? null;
    notification.insuranceId = model.insuranceId ?? null;
    notification.leaseConsultationId = model.leaseConsultationId ?? null;
    notification.noticeOfClaimId = model.noticeOfClaimId ?? null;
    notification.leaseRenewalId = model.leaseRenewalId ?? null;
    notification.expropOwnerHistoryId = model.expropOwnerHistoryId ?? null;
    notification.agreementId = model.agreementId ?? null;
    return notification;
  }
}
