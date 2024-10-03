import { IUpdateCompensationStrategy } from '@/features/mapSideBar/compensation/models/UpdateCompensationStrategy';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';

export class LeaseStatusUpdateSolver implements IUpdateCompensationStrategy {
  constructor(private readonly fileStatus: ApiGen_Base_CodeType<string> | null = null) {
    this.fileStatus = fileStatus;
  }

  canEditOrDeleteCompensation(isDraftCompensation: boolean): boolean {
    if (this.fileStatus === null) {
      return false;
    }

    return isDraftCompensation;
  }
}
