export interface IParentConcurrencyGuard<T extends object> {
  payload: T;
  parentId: number;
  parentRowVersion: number;
}
