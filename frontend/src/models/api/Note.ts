import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_EntityNote extends Api_ConcurrentVersion {
  id?: number;
  note: Api_Note;
  parent: Api_NoteParent;
}

export interface Api_Note extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  note?: string;
  isSystemGenerated?: boolean;
}

export interface Api_NoteParent extends Api_ConcurrentVersion {
  id: number;
}
