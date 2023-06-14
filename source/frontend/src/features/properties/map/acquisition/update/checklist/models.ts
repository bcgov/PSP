import {
  Api_AcquisitionFile,
  Api_AcquisitionFileChecklistItem,
  Api_AcquisitionFileChecklistItemType,
  lastModifiedBy,
  sortByDisplayOrder,
} from '@/models/api/AcquisitionFile';
import { Api_AuditFields } from '@/models/api/AuditFields';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { fromTypeCode, toTypeCode } from '@/utils/formUtils';

export class AcquisitionChecklistFormModel {
  id?: number;
  rowVersion?: number;
  checklistSections: AcquisitionChecklistSectionFormModel[] = [];

  static fromApi(
    apiAcquisitionFile: Api_AcquisitionFile,
    sectionTypes: ILookupCode[],
  ): AcquisitionChecklistFormModel {
    const checklist = apiAcquisitionFile.acquisitionFileChecklist || [];
    const model = new AcquisitionChecklistFormModel();
    model.id = apiAcquisitionFile.id;
    model.rowVersion = apiAcquisitionFile.rowVersion;
    model.checklistSections = sectionTypes.map(section =>
      AcquisitionChecklistSectionFormModel.fromApi(
        section,
        checklist.filter(c => c.itemType?.sectionCode === section.id).sort(sortByDisplayOrder),
      ),
    );

    return model;
  }

  toApi(): Api_AcquisitionFile {
    const allChecklistItems = this.checklistSections.reduce(
      (acc: AcquisitionChecklistItemFormModel[], section) => {
        return acc.concat(section.items);
      },
      [],
    );

    return {
      id: this.id,
      rowVersion: this.rowVersion,
      acquisitionFileChecklist: allChecklistItems.map(c => c.toApi()),
      projectId: null,
      productId: null,
    };
  }

  lastModifiedBy(): Api_AuditFields | undefined {
    const allChecklistItems = this.checklistSections.reduce((acc: Api_AuditFields[], section) => {
      return acc.concat(section.items);
    }, []);

    return lastModifiedBy(allChecklistItems);
  }
}

export class AcquisitionChecklistSectionFormModel {
  id?: string;
  name?: string;
  displayOrder?: number;
  items: AcquisitionChecklistItemFormModel[] = [];

  static fromApi(
    apiLookup: ILookupCode,
    apiChecklistItems: Api_AcquisitionFileChecklistItem[],
  ): AcquisitionChecklistSectionFormModel {
    const model = new AcquisitionChecklistSectionFormModel();
    model.id = apiLookup.id as string;
    model.name = apiLookup.name;
    model.displayOrder = apiLookup.displayOrder;
    model.items = apiChecklistItems.map(c => AcquisitionChecklistItemFormModel.fromApi(c));
    return model;
  }
}

export class AcquisitionChecklistItemFormModel implements Api_AuditFields {
  id?: number;
  acquisitionFileId?: number;
  itemType?: Api_AcquisitionFileChecklistItemType;
  statusType?: string;
  rowVersion?: number;
  appCreateTimestamp?: string;
  appLastUpdateTimestamp?: string;
  appLastUpdateUserid?: string;
  appCreateUserid?: string;
  appLastUpdateUserGuid?: string;
  appCreateUserGuid?: string;

  static fromApi(
    apiChecklistItem: Api_AcquisitionFileChecklistItem,
  ): AcquisitionChecklistItemFormModel {
    const model = new AcquisitionChecklistItemFormModel();
    model.id = apiChecklistItem.id;
    model.acquisitionFileId = apiChecklistItem.acquisitionFileId;
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

  toApi(): Api_AcquisitionFileChecklistItem {
    return {
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
      itemType: this.itemType,
      statusTypeCode: toTypeCode(this.statusType),
      rowVersion: this.rowVersion,
    };
  }
}
