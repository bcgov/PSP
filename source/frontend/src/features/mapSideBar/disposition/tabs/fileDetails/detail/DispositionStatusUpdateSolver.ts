import { DispositionFileStatus } from '@/constants/dispositionFileStatus';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';

class DispositionStatusUpdateSolver {
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
      case DispositionFileStatus.Active:
      case DispositionFileStatus.Draft:
        canEdit = true;
        break;
      case DispositionFileStatus.Archived:
      case DispositionFileStatus.Cancelled:
      case DispositionFileStatus.Closed:
      case DispositionFileStatus.Complete:
      case DispositionFileStatus.Hold:
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
      case DispositionFileStatus.Active:
      case DispositionFileStatus.Draft:
        canEdit = true;
        break;
      case DispositionFileStatus.Archived:
      case DispositionFileStatus.Cancelled:
      case DispositionFileStatus.Closed:
      case DispositionFileStatus.Complete:
      case DispositionFileStatus.Hold:
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
      case DispositionFileStatus.Active:
      case DispositionFileStatus.Draft:
      case DispositionFileStatus.Archived:
      case DispositionFileStatus.Cancelled:
      case DispositionFileStatus.Closed:
      case DispositionFileStatus.Complete:
      case DispositionFileStatus.Hold:
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
      case DispositionFileStatus.Active:
      case DispositionFileStatus.Draft:
      case DispositionFileStatus.Archived:
      case DispositionFileStatus.Cancelled:
      case DispositionFileStatus.Closed:
      case DispositionFileStatus.Complete:
      case DispositionFileStatus.Hold:
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
      case DispositionFileStatus.Active:
      case DispositionFileStatus.Draft:
      case DispositionFileStatus.Archived:
      case DispositionFileStatus.Cancelled:
      case DispositionFileStatus.Closed:
      case DispositionFileStatus.Complete:
      case DispositionFileStatus.Hold:
      default:
        canEdit = true;
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
      case DispositionFileStatus.Active:
      case DispositionFileStatus.Draft:
        canEdit = true;
        break;
      case DispositionFileStatus.Archived:
      case DispositionFileStatus.Cancelled:
      case DispositionFileStatus.Closed:
      case DispositionFileStatus.Complete:
      case DispositionFileStatus.Hold:
        canEdit = false;
        break;
      default:
        canEdit = false;
        break;
    }

    return canEdit;
  }
}

export default DispositionStatusUpdateSolver;
