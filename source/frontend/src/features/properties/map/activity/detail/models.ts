import { FileTypes } from '@/constants';
import { Api_Activity, Api_ActivityTemplate, Api_PropertyActivity } from '@/models/api/Activity';
import Api_TypeCode from '@/models/api/TypeCode';

export class ActivityModel {
  id?: number;
  activityTemplateId?: number;
  description?: string;
  status?: string;
  activityStatusTypeCode?: Api_TypeCode<string>;
  activityTemplate?: Api_ActivityTemplate;
  activityDataJson?: string;
  activityData: any;
  actInstPropFiles?: Api_PropertyActivity[];
  fileType?: FileTypes;
  rowVersion?: number;

  toApi(): Api_Activity {
    return {
      id: this.id,
      activityTemplateId: this.activityTemplateId,
      description: this.description ?? '',
      status: this.status ?? '',
      activityStatusTypeCode: this.activityStatusTypeCode,
      activityTemplate: this.activityTemplate ?? {},
      activityDataJson: this.activityData ? JSON.stringify(this.activityData) : '',
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
    activity.activityData = !!model?.activityDataJson
      ? JSON.parse(model.activityDataJson)
      : undefined;
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
