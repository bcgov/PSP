import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_Note extends Api_ConcurrentVersion {
  id?: number;
  note?: string;
  parentId?: number;
}
