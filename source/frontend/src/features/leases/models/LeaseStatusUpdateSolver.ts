import { IUpdateChecklistStrategy } from '@/features/mapSideBar/compensation/models/IUpdateChecklistStrategy';
import { IUpdateCompensationStrategy } from '@/features/mapSideBar/compensation/models/IUpdateCompensationStrategy';
import { LeasePageNames } from '@/features/mapSideBar/lease/LeaseContainer';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';

export class LeaseStatusUpdateSolver
  implements IUpdateCompensationStrategy, IUpdateChecklistStrategy
{
  constructor(readonly fileStatus: ApiGen_Base_CodeType<string> | null = null) {
    this.fileStatus = fileStatus;
  }

  canEditLeasePage(leasePageName: LeasePageNames): boolean {
    switch (leasePageName) {
      case LeasePageNames.CHECKLIST:
        return this.canEditChecklists();
      case LeasePageNames.CONSULTATIONS:
        return this.canEditConsulations();
      case LeasePageNames.DEPOSIT:
        return this.canEditDeposits();
      case LeasePageNames.DETAILS:
        return this.canEditDetails();
      case LeasePageNames.DOCUMENTS:
        return this.canEditDocuments();
      case LeasePageNames.PAYEE:
      case LeasePageNames.TENANT:
        return this.canEditTenants();
      case LeasePageNames.IMPROVEMENTS:
        return this.canEditImprovements();
      case LeasePageNames.INSURANCE:
        return this.canEditInsurance();
      case LeasePageNames.PAYMENTS:
        return this.canEditPayments();
      default:
        return false;
    }
  }

  canEditDetails(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
        canEdit = false;
        break;
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
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
        canEdit = false;
        break;
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditConsulations(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
        canEdit = false;
        break;
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditTenants(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
        canEdit = false;
        break;
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditImprovements(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
        canEdit = false;
        break;
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditInsurance(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
        canEdit = false;
        break;
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditDeposits(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
        canEdit = false;
        break;
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  canEditPayments(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus.id;
    let canEdit = false;

    switch (statusCode) {
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        canEdit = true;
        break;
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
        canEdit = false;
        break;
      default:
        canEdit = true;
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
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        canEdit = isDraftCompensation ?? isAdmin ?? true;
        break;
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
        canEdit = false;
        break;
      default:
        canEdit = true;
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
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        canEdit = true;
        break;
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
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        canEdit = true;
        break;
      default:
        canEdit = true;
        break;
    }

    return canEdit;
  }

  isAdminProtected(): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    const statusCode = this.fileStatus?.id;
    let isProtected: boolean;

    switch (statusCode) {
      case ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE:
      case ApiGen_CodeTypes_LeaseStatusTypes.DRAFT:
      case ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE:
      case ApiGen_CodeTypes_LeaseStatusTypes.EXPIRED:
      case ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE:
        isProtected = false;
        break;
      case ApiGen_CodeTypes_LeaseStatusTypes.DISCARD:
      case ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED:
      case ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED:
        isProtected = true;
        break;
    }

    return isProtected;
  }
}
