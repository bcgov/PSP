import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { ApiGen_Concepts_ManagementActivitySubType } from '@/models/api/generated/ApiGen_Concepts_ManagementActivitySubType';
import { firstOrNull, getApiPropertyName, isValidIsoDateTime } from '@/utils';

export class PropertyActivityRow {
  activityType: ApiGen_Base_CodeType<string> | null = null;
  activitySubTypes: ApiGen_Concepts_ManagementActivitySubType[] | null = [];
  activityStatusType: ApiGen_Base_CodeType<string> | null = null;
  requestedAddedDate: string | null = null;
  displayOrder: number | null = null;
  managementFileId: number | null = null;
  adHocPropertyId: number | null = null;
  adHocPropertyName: string | null = null;

  constructor(
    readonly id: number | null,
    //readonly propertyId: number,
    readonly activityId: number,
  ) {}

  public static fromApi(model: ApiGen_Concepts_ManagementActivity): PropertyActivityRow {
    const row = new PropertyActivityRow(model.id, model.id);
    row.activityType = model.activityTypeCode;
    row.activitySubTypes = model.activitySubTypeCodes;
    row.activityStatusType = model.activityStatusTypeCode;
    row.requestedAddedDate = isValidIsoDateTime(model.requestAddedDateOnly)
      ? model.requestAddedDateOnly
      : null;
    row.managementFileId = model.managementFileId;
    row.adHocPropertyId = firstOrNull(model.activityProperties)?.propertyId; // This is based on the assumption that ad-hoc activities are related to one property.
    row.adHocPropertyName = getApiPropertyName(
      firstOrNull(model.activityProperties)?.property,
    ).value;

    return row;
  }
}
