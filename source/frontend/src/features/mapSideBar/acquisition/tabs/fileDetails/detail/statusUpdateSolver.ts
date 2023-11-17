import { AcquisitionStatus } from '@/constants/acquisitionFileStatus';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';

class StatusUpdateSolver {
  private readonly aquisitionFile: Api_AcquisitionFile | null;

  constructor(apiModel: Api_AcquisitionFile | undefined | null) {
    this.aquisitionFile = apiModel ?? null;
  }

  canEditDetails(): boolean {
    if (this.aquisitionFile === null) {
      return false;
    }

    const statusCode = this.aquisitionFile.fileStatusTypeCode?.id;
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
    if (this.aquisitionFile === null) {
      return false;
    }

    const statusCode = this.aquisitionFile.fileStatusTypeCode?.id;
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
    if (this.aquisitionFile === null) {
      return false;
    }

    const statusCode = this.aquisitionFile.fileStatusTypeCode?.id;
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

  canEditOrDeleteAgreement(isDraftCompensation: boolean | null): boolean {
    if (this.aquisitionFile === null) {
      return false;
    }

    const statusCode = this.aquisitionFile.fileStatusTypeCode?.id;
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

  // Documents, Notes, Checklists, Stakeholders, Property Details

  canEditDocuments(): boolean {
    if (this.aquisitionFile === null) {
      return false;
    }

    const statusCode = this.aquisitionFile.fileStatusTypeCode?.id;
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
    if (this.aquisitionFile === null) {
      return false;
    }

    const statusCode = this.aquisitionFile.fileStatusTypeCode?.id;
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
    if (this.aquisitionFile === null) {
      return false;
    }

    const statusCode = this.aquisitionFile.fileStatusTypeCode?.id;
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
    if (this.aquisitionFile === null) {
      return false;
    }

    const statusCode = this.aquisitionFile.fileStatusTypeCode?.id;
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
    if (this.aquisitionFile === null) {
      return false;
    }

    const statusCode = this.aquisitionFile.fileStatusTypeCode?.id;
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
