export interface IApiError {
  details: string;
  error: string;
  errorCode: string;
  stackTrace: string;
  type: string;
}

export function isApiError(obj: IApiError | unknown): obj is IApiError {
  return (obj as IApiError).error !== undefined;
}
