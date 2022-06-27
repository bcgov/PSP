import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_EntityNote extends Api_ConcurrentVersion {
  id?: number;
  note: Api_Note;
  parent: Api_NoteParent;
}

export interface Api_Note extends Api_ConcurrentVersion {
  id?: number;
  note?: string;
  appCreateTimestamp: string;
  appLastUpdateUserid: string;
}

export interface Api_NoteParent extends Api_ConcurrentVersion {
  id: number;
}


export enum NoteType {
  Activity,
  File,
}

