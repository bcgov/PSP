import { ApiGen_CodeTypes_AcquisitionStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionStatusTypes';
import { ApiGen_CodeTypes_AcquisitionTakeStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionTakeStatusTypes';
import { ApiGen_CodeTypes_AgreementStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementStatusTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';

class StatusUpdateSolver {
  constructor(private readonly acquisitionFile: ApiGen_Concepts_AcquisitionFile | null = null) {
    this.acquisitionFile = acquisitionFile;
  }

  canEditDetails(): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    const statusCode = this.acquisitionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canDeleteTake(takeStatus: ApiGen_CodeTypes_AcquisitionTakeStatusTypes): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    let canDelete = false;
    switch (takeStatus) {
      case ApiGen_CodeTypes_AcquisitionTakeStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_AcquisitionTakeStatusTypes.INPROGRESS:
        canDelete = true;
        break;
      case ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE:
        canDelete = false;
        break;
      default:
        canDelete = false;
        break;
    }

    return canDelete;
  }

  canEditOrDeleteCompensation(isDraftCompensation: boolean | null): boolean {
    if (this.acquisitionFile === null) {
      return false;
    }

    const statusCode = this.acquisitionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
        canEdit = isDraftCompensation ?? true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = false;
        break;
    }

    return canEdit;
  }
}

export default StatusUpdateSolver;
