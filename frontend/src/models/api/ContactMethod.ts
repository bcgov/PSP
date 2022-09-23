import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_ContactMethod extends Api_ConcurrentVersion {
  id?: number;
  contactMethodType?: Api_TypeCode<string>;
  value?: string;
  isPreferredMethod?: boolean;
}
