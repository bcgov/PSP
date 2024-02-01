import { AcquisitionStatus } from '@/constants/acquisitionFileStatus';
import { ApiGen_CodeTypes_AgreementStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementStatusTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';

class StatusUpdateSolver {
  private readonly acquisitionFile: ApiGen_Concepts_AcquisitionFile | null;

  constructor(apiModel: ApiGen_Concepts_AcquisitionFile | undefined | null) {
    this.acquisitionFile = apiModel ?? null;
  }

  canEditDetails(): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    const statusCode = this.acquisitionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case AcquisitionStatus.Active:
      case AcquisitionStatus.Draft:
        canEdit = true;
        break;
      case AcquisitionStatus.Archived:
      case AcquisitionStatus.Cancelled:
      case AcquisitionStatus.Closed:
      case AcquisitionStatus.Complete:
      case AcquisitionStatus.Hold:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditTakes(): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    const statusCode = this.acquisitionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case AcquisitionStatus.Active:
      case AcquisitionStatus.Draft:
        canEdit = true;
        break;
      case AcquisitionStatus.Archived:
      case AcquisitionStatus.Cancelled:
      case AcquisitionStatus.Closed:
      case AcquisitionStatus.Complete:
      case AcquisitionStatus.Hold:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditOrDeleteCompensation(isDraftCompensation: boolean | null): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    const statusCode = this.acquisitionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case AcquisitionStatus.Active:
      case AcquisitionStatus.Draft:
        canEdit = true;
        break;
      case AcquisitionStatus.Archived:
      case AcquisitionStatus.Cancelled:
      case AcquisitionStatus.Closed:
      case AcquisitionStatus.Complete:
      case AcquisitionStatus.Hold:
        canEdit = isDraftCompensation ?? true;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditOrDeleteAgreement(agreementStatusCode: string | null): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    const statusCode = this.acquisitionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case AcquisitionStatus.Active:
      case AcquisitionStatus.Draft:
        canEdit = true;
        break;
      case AcquisitionStatus.Archived:
      case AcquisitionStatus.Cancelled:
      case AcquisitionStatus.Closed:
      case AcquisitionStatus.Complete:
      case AcquisitionStatus.Hold:
        canEdit = agreementStatusCode !== ApiGen_CodeTypes_AgreementStatusTypes.FINAL ?? true;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditDocuments(): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    const statusCode = this.acquisitionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case AcquisitionStatus.Active:
      case AcquisitionStatus.Draft:
      case AcquisitionStatus.Archived:
      case AcquisitionStatus.Cancelled:
      case AcquisitionStatus.Closed:
      case AcquisitionStatus.Complete:
      case AcquisitionStatus.Hold:
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditNotes(): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    const statusCode = this.acquisitionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case AcquisitionStatus.Active:
      case AcquisitionStatus.Draft:
      case AcquisitionStatus.Archived:
      case AcquisitionStatus.Cancelled:
      case AcquisitionStatus.Closed:
      case AcquisitionStatus.Complete:
      case AcquisitionStatus.Hold:
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditChecklists(): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    const statusCode = this.acquisitionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case AcquisitionStatus.Active:
      case AcquisitionStatus.Draft:
      case AcquisitionStatus.Archived:
      case AcquisitionStatus.Cancelled:
      case AcquisitionStatus.Closed:
      case AcquisitionStatus.Complete:
      case AcquisitionStatus.Hold:
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditStakeholders(): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    const statusCode = this.acquisitionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case AcquisitionStatus.Active:
      case AcquisitionStatus.Draft:
      case AcquisitionStatus.Archived:
      case AcquisitionStatus.Cancelled:
      case AcquisitionStatus.Closed:
      case AcquisitionStatus.Complete:
      case AcquisitionStatus.Hold:
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditProperties(): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    const statusCode = this.acquisitionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case AcquisitionStatus.Active:
      case AcquisitionStatus.Draft:
      case AcquisitionStatus.Archived:
      case AcquisitionStatus.Cancelled:
      case AcquisitionStatus.Closed:
      case AcquisitionStatus.Complete:
      case AcquisitionStatus.Hold:
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }
}

export default StatusUpdateSolver;
