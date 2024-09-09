import { IUpdateCompensationStrategy } from '@/features/mapSideBar/compensation/models/UpdateCompensationStrategy';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_CodeTypes_AcquisitionStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionStatusTypes';
import { ApiGen_CodeTypes_AgreementStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementStatusTypes';

class AcquisitionFileStatusUpdateSolver implements IUpdateCompensationStrategy {
  constructor(private readonly fileStatus: ApiGen_Base_CodeType<string> | null = null) {
    this.fileStatus = fileStatus;
  }

  canEditDetails(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
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
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
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

  canEditOrDeleteCompensation(isDraftCompensation: boolean): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
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
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
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
        canEdit = agreementStatusCode !== ApiGen_CodeTypes_AgreementStatusTypes.FINAL;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditDocuments(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
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
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
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
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
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
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
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
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
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

export default AcquisitionFileStatusUpdateSolver;
