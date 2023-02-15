export interface DocumentQueryResult<T> {
  count: number;
  results: T[];
}

export interface Api_Storage_DocumentDetail {
  id: number;
  label: string;
  datetime_created: string;
  description: string;
  file_latest: Api_Storage_FileLatest;
  document_type: Api_Storage_DocumentType;
}

export interface Api_Storage_DocumentMetadata {
  document: Api_Storage_DocumentDetail;
  id: number;
  metadata_type: Api_Storage_MetadataType;
  url: string;
  value: string;
}

export interface Api_Storage_FileLatest {
  id: number;
  document_id: number;
  comment: string;
  encoding: string;
  fileName: string;
  mimetype: string;
  size: number;
  timeStamp: string;
}

export interface Api_Storage_DocumentType {
  delete_time_period?: number;
  delete_time_unit?: string;
  filename_generator_backend?: string;
  filename_generator_backend_arguments?: string;
  id?: number;
  label?: string;
  quick_label_list_url?: string;
  trash_time_period?: number;
  trash_time_unit?: string;
  url?: string;
}

export interface Api_FileDownload {
  filePayload: string;
  size: number;
  fileName: string;
  fileNameExtension: string;
  fileNameWithoutExtension: string;
  mimetype: string;
  encodingType: BufferEncoding;
}

export interface Api_Storage_DocumentTypeMetadataType {
  id?: number;
  document_type?: Api_Storage_DocumentType;
  metadata_type?: Api_Storage_MetadataType;
  required?: boolean;
  url?: string;
  value?: string;
}

export interface Api_Storage_MetadataType {
  default?: string;
  id?: number;
  label?: string;
  lookup?: string;
  name?: string;
  parser?: string;
  parser_arguments?: string;
  url?: string;
  validation?: string;
  validation_arguments?: string;
}
