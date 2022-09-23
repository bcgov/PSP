export enum UpdateOperation {
  ADD = 'add',
  DELETE = 'delete',
  UPDATE = 'update',
}

export interface IEntryModification<T> {
  operation: UpdateOperation;
  entry: T;
}

export interface IBatchUpdateRequest<T> {
  payload: IEntryModification<T>[];
}

export interface IBatchUpdateReply<T> {
  payload: T[];
  errorMessages: string[];
}
