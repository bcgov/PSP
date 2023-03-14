import {
  Api_AcquisitionFile,
  Api_AcquisitionFileChecklistItem,
  Api_AcquisitionFileChecklistItemType,
} from 'models/api/AcquisitionFile';
import { ILookupCode } from 'store/slices/lookupCodes';
import { fromTypeCode, toTypeCode } from 'utils/formUtils';

function byDisplayOrder(a: Api_AcquisitionFileChecklistItem, b: Api_AcquisitionFileChecklistItem) {
  return (a.itemType?.displayOrder ?? 0) - (b.itemType?.displayOrder ?? 0);
}

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
        checklist.filter(c => c.itemType?.sectionCode === section.id).sort(byDisplayOrder),
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
    };
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

export class AcquisitionChecklistItemFormModel {
  id?: number;
  acquisitionFileId?: number;
  itemType?: Api_AcquisitionFileChecklistItemType;
  statusType?: string;
  rowVersion?: number;

  static fromApi(
    apiChecklistItem: Api_AcquisitionFileChecklistItem,
  ): AcquisitionChecklistItemFormModel {
    const model = new AcquisitionChecklistItemFormModel();
    model.id = apiChecklistItem.id;
    model.acquisitionFileId = apiChecklistItem.acquisitionFileId;
    model.itemType = apiChecklistItem.itemType;
    model.statusType = fromTypeCode(apiChecklistItem.statusTypeCode);
    model.rowVersion = apiChecklistItem.rowVersion;
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
