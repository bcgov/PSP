import { Api_ConcurrentVersion } from 'models/api/ConcurrentVersion';
import Api_TypeCode from 'models/api/TypeCode';
import { Api_User } from 'models/api/User';

export interface Api_RegionUser extends Api_ConcurrentVersion {
  user?: Api_User;
  userId?: number;
  region: Api_TypeCode<number>;
  regionCode?: number;
}
