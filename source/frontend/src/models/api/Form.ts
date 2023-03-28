import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';
export interface Api_FileForm extends Api_ConcurrentVersion {
  id: number | null;
  fileId: number;
  formTypeCode: Api_Form;
}

export interface Api_Form extends Api_TypeCode<string> {
  name: string | null;
}
