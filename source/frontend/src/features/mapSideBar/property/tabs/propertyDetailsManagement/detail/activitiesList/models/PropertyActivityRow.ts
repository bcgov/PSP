import { Api_PropertyManagementActivity } from '@/models/api/Property';
import Api_TypeCode from '@/models/api/TypeCode';

export class PropertyActivityRow {
  activityType: Api_TypeCode<string> | null = null;
  activitySubType: Api_TypeCode<string> | null = null;
  activityStatusType: Api_TypeCode<string> | null = null;
  requestedAddedDate: string | null = null;

  constructor(
    readonly id: number | null,
    readonly propertyId: number,
    readonly activityId: number,
  ) {}

  public static fromApi(model: Api_PropertyManagementActivity): PropertyActivityRow {
    const row = new PropertyActivityRow(model.id, model.propertyId, model.propertyActivityId);
    row.activityType = model.activity.activityType;
    row.activitySubType = model.activity.activitySubType;
    row.activityStatusType = model.activity.activityStatusType;
    row.requestedAddedDate = model.activity.requestedAddedDate;

    return row;
  }
}
