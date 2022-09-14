import { Api_ActivityTemplate, Api_PropertyActivity } from 'models/api/Activity';
import Api_TypeCode from 'models/api/TypeCode';

import { FileTypes } from './../../../../../constants/fileTypes';
import { Api_Activity } from './../../../../../models/api/Activity';

export class ActivityModel {
  id?: number;
  activityTemplateId?: number;
  description?: string;
  activityStatusTypeCode?: Api_TypeCode<string>;
  activityTemplate?: Api_ActivityTemplate;
  activityDataJson?: string;
  actInstPropFiles?: Api_PropertyActivity[];
  fileType?: FileTypes;
  rowVersion?: number;

  toApi(): Api_Activity {
    return {
      id: this.id,
      activityTemplateId: this.activityTemplateId,
      description: this.description ?? '',
      activityStatusTypeCode: this.activityStatusTypeCode,
      activityTemplate: this.activityTemplate ?? {},
      activityDataJson: this.activityDataJson ?? '',
      actInstPropAcqFiles:
        this.fileType === FileTypes.Acquisition ? this.actInstPropFiles ?? [] : [],
      actInstPropRsrchFiles:
        this.fileType === FileTypes.Research ? this.actInstPropFiles ?? [] : [],
      rowVersion: this.rowVersion,
    };
  }
  static fromApi(model: Api_Activity, fileType: FileTypes): ActivityModel {
    const activity: ActivityModel = new ActivityModel();
    activity.id = model?.id;
    activity.activityTemplateId = model?.activityTemplateId;
    activity.description = model?.description;
    activity.activityStatusTypeCode = model?.activityStatusTypeCode;
    activity.activityTemplate = model?.activityTemplate;
    activity.activityDataJson = model?.activityDataJson;
    activity.actInstPropFiles = ActivityModel.getProperties(model, fileType);
    activity.fileType = fileType;
    activity.rowVersion = model.rowVersion;

    return activity;
  }

  static getProperties = (model: Api_Activity, fileType: FileTypes): Api_PropertyActivity[] => {
    switch (fileType) {
      case FileTypes.Acquisition:
        return model?.actInstPropAcqFiles ?? [];
      case FileTypes.Research:
        return model?.actInstPropRsrchFiles ?? [];
      default:
        throw Error('Invalid or no file type found.');
    }
  };
}
