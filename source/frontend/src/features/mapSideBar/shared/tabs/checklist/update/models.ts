import { Api_AuditFields } from '@/models/api/AuditFields';
import { Api_FileWithChecklist } from '@/models/api/File';
import {
  Api_FileChecklistItem,
  Api_FileChecklistItemType,
  lastModifiedBy,
  sortByDisplayOrder,
} from '@/models/api/File';
import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { fromTypeCode, toTypeCode } from '@/utils/formUtils';

export class ChecklistFormModel {
  id?: number;
  rowVersion?: number;
  checklistSections: ChecklistSectionFormModel[] = [];

  static fromApi(apiFile: Api_FileWithChecklist, sectionTypes: ILookupCode[]): ChecklistFormModel {
    const checklist = apiFile.fileChecklistItems || [];
    const model = new ChecklistFormModel();
    model.id = apiFile.id;
    model.rowVersion = apiFile.rowVersion;
    model.checklistSections = sectionTypes.map(section =>
      ChecklistSectionFormModel.fromApi(
        section,
        checklist.filter(c => c.itemType?.sectionCode === section.id).sort(sortByDisplayOrder),
      ),
    );

    return model;
  }

  toApi(): Api_FileWithChecklist {
    const allChecklistItems = this.checklistSections.reduce(
      (acc: ChecklistItemFormModel[], section) => {
        return acc.concat(section.items);
      },
      [],
    );

    return {
      id: this.id,
      rowVersion: this.rowVersion,
      fileChecklistItems: allChecklistItems.map(c => c.toApi()),
    };
  }

  lastModifiedBy(): Api_AuditFields | undefined {
    const allChecklistItems = this.checklistSections.reduce((acc: Api_AuditFields[], section) => {
      return acc.concat(section.items);
    }, []);

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
    apiChecklistItems: Api_FileChecklistItem[],
  ): ChecklistSectionFormModel {
    const model = new ChecklistSectionFormModel();
    model.id = apiLookup.id as string;
    model.name = apiLookup.name;
    model.displayOrder = apiLookup.displayOrder;
    model.items = apiChecklistItems.map(c => ChecklistItemFormModel.fromApi(c));
    return model;
  }
}

export class ChecklistItemFormModel implements Api_AuditFields {
  id?: number;
  fileId?: number;
  itemType?: Api_FileChecklistItemType;
  statusType?: string;
  rowVersion?: number;
  appCreateTimestamp?: UtcIsoDateTime;
  appLastUpdateTimestamp?: UtcIsoDateTime;
  appLastUpdateUserid?: string;
  appCreateUserid?: string;
  appLastUpdateUserGuid?: string;
  appCreateUserGuid?: string;

  static fromApi(apiChecklistItem: Api_FileChecklistItem): ChecklistItemFormModel {
    const model = new ChecklistItemFormModel();
    model.id = apiChecklistItem.id;
    model.fileId = apiChecklistItem.fileId;
    model.itemType = apiChecklistItem.itemType;
    model.statusType = fromTypeCode(apiChecklistItem.statusTypeCode);
    model.rowVersion = apiChecklistItem.rowVersion;
    model.appCreateTimestamp = apiChecklistItem.appCreateTimestamp;
    model.appLastUpdateTimestamp = apiChecklistItem.appLastUpdateTimestamp;
    model.appLastUpdateUserid = apiChecklistItem.appLastUpdateUserid;
    model.appCreateUserid = apiChecklistItem.appCreateUserid;
    model.appLastUpdateUserGuid = apiChecklistItem.appLastUpdateUserGuid;
    model.appCreateUserGuid = apiChecklistItem.appCreateUserGuid;
    return model;
  }

  toApi(): Api_FileChecklistItem {
    return {
      id: this.id,
      fileId: this.fileId,
      itemType: this.itemType,
      statusTypeCode: toTypeCode(this.statusType),
      rowVersion: this.rowVersion,
    };
  }
}
