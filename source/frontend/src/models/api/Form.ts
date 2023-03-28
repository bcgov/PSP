import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';
export interface Api_FileForm extends Api_ConcurrentVersion {
  fileId: number;
  formTypeCode: Api_TypeCode<string>;
}

export interface Api_Form extends Api_ConcurrentVersion {
  id: number;
}
