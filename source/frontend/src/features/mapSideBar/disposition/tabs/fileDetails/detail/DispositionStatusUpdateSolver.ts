import { IUpdateChecklistStrategy } from '@/features/mapSideBar/compensation/models/IUpdateChecklistStrategy';
import { ApiGen_CodeTypes_DispositionFileStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_DispositionFileStatusTypes';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';

class DispositionStatusUpdateSolver implements IUpdateChecklistStrategy {
  private readonly dispositionFile: ApiGen_Concepts_DispositionFile | null;

  constructor(apiModel: ApiGen_Concepts_DispositionFile | undefined | null) {
    this.dispositionFile = apiModel ?? null;
  }

  canEditDetails(): boolean {
    if (this.dispositionFile === null) {
      return false;
    }

    const statusCode = this.dispositionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.COMPLETE:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditOfferSalesValues(): boolean {
    if (this.dispositionFile === null) {
      return false;
    }

    const statusCode = this.dispositionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.COMPLETE:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditDocuments(): boolean {
    if (this.dispositionFile === null) {
      return false;
    }

    const statusCode = this.dispositionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.COMPLETE:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.HOLD:
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditNotes(): boolean {
    if (this.dispositionFile === null) {
      return false;
    }

    const statusCode = this.dispositionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.COMPLETE:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.HOLD:
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditChecklists(): boolean {
    if (this.dispositionFile === null) {
      return false;
    }

    const statusCode = this.dispositionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.COMPLETE:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditProperties(): boolean {
    if (this.dispositionFile === null) {
      return false;
    }

    const statusCode = this.dispositionFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.HOLD:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.COMPLETE:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  isAdminProtected(): boolean {
    if (this.dispositionFile === null) {
      return false;
    }

    const statusCode = this.dispositionFile.fileStatusTypeCode?.id;
    let isProtected = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.HOLD:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.COMPLETE:
        isProtected = false;
        break;
      case ApiGen_CodeTypes_DispositionFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_DispositionFileStatusTypes.CANCELLED:
        isProtected = true;
        break;
    }

    return isProtected;
  }
}

export default DispositionStatusUpdateSolver;
