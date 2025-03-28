import { ApiGen_CodeTypes_ResearchFileStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_ResearchFileStatusTypes';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';

class ResearchStatusUpdateSolver {
  private readonly researchFile: ApiGen_Concepts_File | null;

  constructor(apiModel: ApiGen_Concepts_File | undefined | null) {
    this.researchFile = apiModel ?? null;
  }

  canEditDetails(): boolean {
    if (this.researchFile === null) {
      return false;
    }

    const statusCode = this.researchFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_ResearchFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.INACTIVE:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_ResearchFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.CLOSED:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditDocuments(): boolean {
    if (this.researchFile === null) {
      return false;
    }

    const statusCode = this.researchFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_ResearchFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.CLOSED:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.INACTIVE:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditNotes(): boolean {
    if (this.researchFile === null) {
      return false;
    }

    const statusCode = this.researchFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_ResearchFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.CLOSED:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.INACTIVE:
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditProperties(): boolean {
    if (this.researchFile === null) {
      return false;
    }

    const statusCode = this.researchFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_ResearchFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.INACTIVE:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_ResearchFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.CLOSED:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  isAdminProtected(): boolean {
    if (this.researchFile === null) {
      return false;
    }

    const statusCode = this.researchFile.fileStatusTypeCode.id;
    let isProtected = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_ResearchFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.INACTIVE:
      case ApiGen_CodeTypes_ResearchFileStatusTypes.CLOSED:
        isProtected = false;
        break;
      case ApiGen_CodeTypes_ResearchFileStatusTypes.ARCHIVED:
        isProtected = true;
        break;
    }

    return isProtected;
  }
}

export default ResearchStatusUpdateSolver;
