import { Api_PropertyFile } from '@/models/api/PropertyFile';

import { Api_ConcurrentVersion } from './ConcurrentVersion';
import Api_TypeCode from './TypeCode';

export interface Api_File extends Api_ConcurrentVersion {
  id?: number;
  fileName?: string;
  fileNumber?: string;
  fileStatusTypeCode?: Api_TypeCode<string>;
  appCreateTimestamp?: string;
  appLastUpdateTimestamp?: string;
  appLastUpdateUserid?: string;
  appCreateUserid?: string;
  appCreateUserGuid?: string;
  appLastUpdateUserGuid?: string;
  fileProperties?: Api_PropertyFile[];
}

export interface Api_LastUpdatedBy {
  parentId: number;
  appLastUpdateTimestamp: string | null;
  appLastUpdateUserid: string | null;
  appLastUpdateUserGuid: string | null;
}
