import { Api_PropertyActivity } from '@/models/api/PropertyActivity';
import Api_TypeCode from '@/models/api/TypeCode';

export class PropertyActivityRow {
  activityType: Api_TypeCode<string> | null = null;
  activitySubType: Api_TypeCode<string> | null = null;
  activityStatusType: Api_TypeCode<string> | null = null;
  requestedAddedDate: string | null = null;
  displayOrder: number | null = null;

  constructor(
    readonly id: number | null,
    //readonly propertyId: number,
    readonly activityId: number,
  ) {}

  public static fromApi(model: Api_PropertyActivity): PropertyActivityRow {
    const row = new PropertyActivityRow(model.id, model.id);
    row.activityType = model.activityTypeCode;
    row.activitySubType = model.activitySubtypeCode;
    row.activityStatusType = model.activityStatusTypeCode;
    row.requestedAddedDate = model.requestAddedDateTime;

    return row;
  }
}
