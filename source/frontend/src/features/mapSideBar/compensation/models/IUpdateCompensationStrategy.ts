export interface IUpdateCompensationStrategy {
  canEditOrDeleteCompensation(isDraftCompensation?: boolean, isAdmin?: boolean): boolean;
}
