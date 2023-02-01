import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_CodeType extends Api_ConcurrentVersion {
  id?: number;
  code?: string;
  description?: string;
  displayOrder?: number;
}
