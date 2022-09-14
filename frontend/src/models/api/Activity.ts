import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Property } from './Property';
import Api_TypeCode from './TypeCode';

export interface Api_FileActivity extends Api_ConcurrentVersion {
  fileId: number;
  activity: Api_Activity;
}

export interface Api_PropertyActivity extends Api_ConcurrentVersion {
  activityId?: number;
  propertyFileId?: number;
  property?: Api_Property;
}

export interface Api_Activity extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  activityTemplateId?: number;
  description: string;
  status: string;
  activityStatusTypeCode?: Api_TypeCode<string>;
  activityTemplate: Api_ActivityTemplate;
  activityDataJson: string;
  actInstPropAcqFiles?: Api_PropertyActivity[];
  actInstPropRsrchFiles?: Api_PropertyActivity[];
}

export interface Api_ActivityTemplate extends Api_ConcurrentVersion {
  id?: number;
  activityTemplateTypeCode?: Api_TypeCode<string>;
}
