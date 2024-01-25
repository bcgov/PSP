import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { isValidIsoDateTime } from '@/utils';

export class PropertyActivityRow {
  activityType: ApiGen_Base_CodeType<string> | null = null;
  activitySubType: ApiGen_Base_CodeType<string> | null = null;
  activityStatusType: ApiGen_Base_CodeType<string> | null = null;
  requestedAddedDate: string | null = null;
  displayOrder: number | null = null;

  constructor(
    readonly id: number | null,
    //readonly propertyId: number,
    readonly activityId: number,
  ) {}

  public static fromApi(model: ApiGen_Concepts_PropertyActivity): PropertyActivityRow {
    const row = new PropertyActivityRow(model.id, model.id);
    row.activityType = model.activityTypeCode;
    row.activitySubType = model.activitySubtypeCode;
    row.activityStatusType = model.activityStatusTypeCode;
    row.requestedAddedDate = isValidIsoDateTime(model.requestAddedDateOnly)
      ? model.requestAddedDateOnly
      : null;

    return row;
  }
}
