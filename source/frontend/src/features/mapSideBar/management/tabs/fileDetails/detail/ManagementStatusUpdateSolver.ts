import { IUpdateDocumentsStrategy } from '@/features/documents/models/IUpdateDocumentsStrategy';
import { IUpdateNotesStrategy } from '@/features/notes/models/IUpdateNotesStrategy';
import { ApiGen_CodeTypes_ManagementFileStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_ManagementFileStatusTypes';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';

class ManagementStatusUpdateSolver implements IUpdateDocumentsStrategy, IUpdateNotesStrategy {
  private readonly statusCode: string | null;

  constructor(readonly managementFile: ApiGen_Concepts_ManagementFile | null) {
    this.managementFile = managementFile ?? null;
    this.statusCode = managementFile ? managementFile.fileStatusTypeCode?.id : null;
  }

  public isAdminProtected(): boolean {
    if (this.managementFile === null) {
      return false;
    }

    let isProtected: boolean;
    switch (this.statusCode) {
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.THIRDRDPARTY:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.CANCELLED:
        isProtected = false;
        break;
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.COMPLETE:
        isProtected = true;
        break;
    }

    return isProtected;
  }

  public canEditDetails(): boolean {
    if (!this.managementFile) {
      return false;
    }
    let canEdit: boolean;

    switch (this.statusCode) {
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.THIRDRDPARTY:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD:
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

  public canEditDocuments(): boolean {
    if (this.managementFile === null) {
      return false;
    }
    let canEdit: boolean;

    switch (this.statusCode) {
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.COMPLETE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.THIRDRDPARTY:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ARCHIVED:
        canEdit = false;
        break;
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  public canEditNotes(): boolean {
    if (this.managementFile === null) {
      return false;
    }
    let canEditNotes: boolean;

    switch (this.statusCode) {
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.CANCELLED:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.COMPLETE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.THIRDRDPARTY:
        canEditNotes = true;
        break;
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ARCHIVED:
        canEditNotes = false;
        break;
      default:
        canEditNotes = true;
        break;
    }

    return canEditNotes;
  }

  public canEditActivities(): boolean {
    if (!this.managementFile) {
      return false;
    }
    let canEdit: boolean;

    switch (this.statusCode) {
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.THIRDRDPARTY:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD:
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

  public canEditProperties(): boolean {
    if (!this.managementFile) {
      return false;
    }
    let canEdit: boolean;

    switch (this.statusCode) {
      case ApiGen_CodeTypes_ManagementFileStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.DRAFT:
      case ApiGen_CodeTypes_ManagementFileStatusTypes.THIRDRDPARTY:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD:
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
}

export default ManagementStatusUpdateSolver;
