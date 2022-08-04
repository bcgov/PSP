export interface ExternalResult<T> {
  status: ExternalResultStatus;
  message: string;
  payload: T;
}

export enum ExternalResultStatus {
  Success = 'Success',
  Error = 'Error',
}
