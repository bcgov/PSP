export interface DocumentQueryResult<T> {
  count: number;
  results: T[];
}

export interface DocumentDetail {
  id: number;
  label: string;
  datetime_created: string;
  description: string;
  file_latest: FileLatest;
}

export interface FileLatest {
  id: number;
  document_id: number;
  comment: string;
  encoding: string;
  fileName: string;
  mimetype: string;
  size: number;
  timeStamp: string;
}

export interface FileDownload {
  filePayload: string;
  size: number;
  fileName: string;
  mimetype: string;
}
