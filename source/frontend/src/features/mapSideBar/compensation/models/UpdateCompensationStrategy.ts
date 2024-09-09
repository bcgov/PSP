export interface IUpdateCompensationStrategy {
  canEditOrDeleteCompensation(isDraftCompensation: boolean): boolean;
}
