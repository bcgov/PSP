import { ApiGen_CodeTypes_ManagementFileStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_ManagementFileStatusTypes';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';

class ManagementStatusUpdateSolver {
  private readonly managementFile: ApiGen_Concepts_ManagementFile | null;

  constructor(apiModel: ApiGen_Concepts_ManagementFile | undefined | null) {
    this.managementFile = apiModel ?? null;
  }

  canEditDetails(): boolean {
    if (this.managementFile === null) {
      return false;
    }

    const statusCode = this.managementFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.THIRDRDPARTY:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.COMPLETE:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  canEditDocuments(): boolean {
    if (this.managementFile === null) {
      return false;
    }

    const statusCode = this.managementFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.COMPLETE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.THIRDRDPARTY:
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditNotes(): boolean {
    if (this.managementFile === null) {
      return false;
    }

    const statusCode = this.managementFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.COMPLETE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.THIRDRDPARTY:
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditProperties(): boolean {
    if (this.managementFile === null) {
      return false;
    }

    const statusCode = this.managementFile.fileStatusTypeCode?.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.THIRDRDPARTY:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.COMPLETE:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }

  isAdminProtected(): boolean {
    if (this.managementFile === null) {
      return false;
    }

    const statusCode = this.managementFile.fileStatusTypeCode?.id;
    let isProtected: boolean;

    switch (statusCode) {
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.COMPLETE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.THIRDRDPARTY:
        isProtected = false;
        break;
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.CANCELLED:
        isProtected = true;
        break;
    }

    return isProtected;
  }
}

export default ManagementStatusUpdateSolver;
