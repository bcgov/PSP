import { ApiGen_Base_BaseAudit } from '@/models/api/generated/ApiGen_Base_BaseAudit';
import { ApiGen_Concepts_FileChecklistItem } from '@/models/api/generated/ApiGen_Concepts_FileChecklistItem';
import { ApiGen_Concepts_FileChecklistItemType } from '@/models/api/generated/ApiGen_Concepts_FileChecklistItemType';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';
import { EpochIsoDateTime, UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { lastModifiedBy, sortByDisplayOrder } from '@/utils/fileUtils';
import { fromTypeCode, toTypeCodeNullable } from '@/utils/formUtils';

export class ChecklistFormModel {
  id?: number;
  rowVersion?: number;
  checklistSections: ChecklistSectionFormModel[] = [];

  static fromApi(
    apiFile: ApiGen_Concepts_FileWithChecklist,
    sectionTypes: ILookupCode[],
  ): ChecklistFormModel {
    const checklist = apiFile.fileChecklistItems || [];
    const model = new ChecklistFormModel();
    model.id = apiFile.id;
    model.rowVersion = apiFile.rowVersion ?? undefined;
    model.checklistSections = sectionTypes.map(section =>
      ChecklistSectionFormModel.fromApi(
        section,
        checklist.filter(c => c.itemType?.sectionCode === section.id).sort(sortByDisplayOrder),
      ),
    );

    return model;
  }

  toApi(): ApiGen_Concepts_FileWithChecklist {
    const allChecklistItems = this.checklistSections.reduce(
      (acc: ChecklistItemFormModel[], section) => {
        return acc.concat(section.items);
      },
      [],
    );

    return {
      id: this.id ?? 0,
      fileChecklistItems: allChecklistItems.map(c => c.toApi()),
      fileName: null,
      fileNumber: null,
      fileProperties: null,
      fileStatusTypeCode: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  lastModifiedBy(): ApiGen_Base_BaseAudit | undefined {
    const allChecklistItems = this.checklistSections.reduce(
      (acc: ApiGen_Base_BaseAudit[], section) => {
        return acc.concat(section.items);
      },
      [],
    );

    return lastModifiedBy(allChecklistItems);
  }
}

export class ChecklistSectionFormModel {
  id?: string;
  name?: string;
  displayOrder?: number;
  items: ChecklistItemFormModel[] = [];

  static fromApi(
    apiLookup: ILookupCode,
    apiChecklistItems: ApiGen_Concepts_FileChecklistItem[],
  ): ChecklistSectionFormModel {
    const model = new ChecklistSectionFormModel();
    model.id = apiLookup.id as string;
    model.name = apiLookup.name;
    model.displayOrder = apiLookup.displayOrder;
    model.items = apiChecklistItems.map(c => ChecklistItemFormModel.fromApi(c));
    return model;
  }
}

export class ChecklistItemFormModel implements ApiGen_Base_BaseAudit {
  id?: number;
  fileId?: number;
  itemType?: ApiGen_Concepts_FileChecklistItemType;
  statusType?: string;
  rowVersion: number | null = null;
  appCreateTimestamp: UtcIsoDateTime = EpochIsoDateTime;
  appLastUpdateTimestamp: UtcIsoDateTime = EpochIsoDateTime;
  appLastUpdateUserid: string | null = null;
  appCreateUserid: string | null = null;
  appLastUpdateUserGuid: string | null = null;
  appCreateUserGuid: string | null = null;

  static fromApi(apiChecklistItem: ApiGen_Concepts_FileChecklistItem): ChecklistItemFormModel {
    const model = new ChecklistItemFormModel();
    model.id = apiChecklistItem.id;
    model.fileId = apiChecklistItem.fileId;
    model.itemType = apiChecklistItem.itemType ?? undefined;
    model.statusType = fromTypeCode(apiChecklistItem.statusTypeCode) ?? undefined;
    model.rowVersion = apiChecklistItem.rowVersion;
    model.appCreateTimestamp = apiChecklistItem.appCreateTimestamp;
    model.appLastUpdateTimestamp = apiChecklistItem.appLastUpdateTimestamp;
    model.appLastUpdateUserid = apiChecklistItem.appLastUpdateUserid;
    model.appCreateUserid = apiChecklistItem.appCreateUserid;
    model.appLastUpdateUserGuid = apiChecklistItem.appLastUpdateUserGuid;
    model.appCreateUserGuid = apiChecklistItem.appCreateUserGuid;
    return model;
  }

  toApi(): ApiGen_Concepts_FileChecklistItem {
    return {
      id: this.id ?? 0,
      fileId: this.fileId ?? 0,
      itemType: this.itemType ?? null,
      statusTypeCode: toTypeCodeNullable(this.statusType),
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
