import { IUpdateChecklistStrategy } from '@/features/mapSideBar/compensation/models/IUpdateChecklistStrategy';
import { IUpdateCompensationStrategy } from '@/features/mapSideBar/compensation/models/IUpdateCompensationStrategy';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_CodeTypes_AcquisitionStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionStatusTypes';

class AcquisitionFileStatusUpdateSolver
  implements IUpdateCompensationStrategy, IUpdateChecklistStrategy
{
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditOrDeleteCompensation(isDraftCompensation?: boolean, isAdmin?: boolean): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = isDraftCompensation ?? isAdmin ?? true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditOrDeleteCompensations(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditSubfiles(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditOrDeleteAgreement(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
        canEdit = false;
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
        canEdit = false;
        break;
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditExpropriation(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.DRAFT:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
        canEdit = false;
        break;
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
        canEdit = false;
        break;
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
      case ApiGen_CodeTypes_AcquisitionStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_AcquisitionStatusTypes.ARCHIV:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CANCEL:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.CLOSED:
      case ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT:
        canEdit = false;
        break;
    }

    return canEdit;
  }
}

export default AcquisitionFileStatusUpdateSolver;
