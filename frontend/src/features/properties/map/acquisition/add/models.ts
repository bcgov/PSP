import { IContactSearchResult } from 'interfaces';
import {
  Api_AcquisitionFile,
  Api_AcquisitionFilePerson,
  Api_AcquisitionFileProperty,
} from 'models/api/AcquisitionFile';
import { fromTypeCode, toTypeCode } from 'utils/formUtils';

import { PropertyForm } from '../../shared/models';

export class AcquisitionForm {
  id?: number;
  fileName?: string = '';
  assignedDate?: string;
  deliveryDate?: string;
  rowVersion?: number;
  // Code Tables
  acquisitionFileStatusType?: string;
  acquisitionPhysFileStatusType?: string;
  acquisitionType?: string;
  // MOTI region
  region?: string;
  properties: PropertyForm[] = [];
  team: AcquisitionTeamForm[] = [];

  toApi(): Api_AcquisitionFile {
    return {
      id: this.id,
      fileName: this.fileName,
      rowVersion: this.rowVersion,
      assignedDate: this.assignedDate,
      deliveryDate: this.deliveryDate,
      fileStatusTypeCode: toTypeCode(this.acquisitionFileStatusType),
      acquisitionPhysFileStatusTypeCode: toTypeCode(this.acquisitionPhysFileStatusType),
      acquisitionTypeCode: toTypeCode(this.acquisitionType),
      regionCode: toTypeCode(Number(this.region)),
      // ACQ file properties
      fileProperties: this.properties.map<Api_AcquisitionFileProperty>(x => {
        return {
          id: x.id,
          propertyName: x.name,
          isDisabled: x.isDisabled,
          displayOrder: x.displayOrder,
          rowVersion: x.rowVersion,
          property: x.toApi(),
          acquisitionFile: { id: this.id },
        };
      }),
      acquisitionTeam: AcquisitionTeamForm.toApi(this.team),
    };
  }

  static fromApi(model: Api_AcquisitionFile): AcquisitionForm {
    const newForm = new AcquisitionForm();
    newForm.id = model.id;
    newForm.fileName = model.fileName || '';
    newForm.rowVersion = model.rowVersion;
    newForm.assignedDate = model.assignedDate;
    newForm.deliveryDate = model.deliveryDate;
    newForm.acquisitionFileStatusType = fromTypeCode(model.fileStatusTypeCode);
    newForm.acquisitionPhysFileStatusType = fromTypeCode(model.acquisitionPhysFileStatusTypeCode);
    newForm.acquisitionType = fromTypeCode(model.acquisitionTypeCode);
    newForm.region = fromTypeCode(model.regionCode)?.toString();
    // ACQ file properties
    newForm.properties = model.fileProperties?.map(x => PropertyForm.fromApi(x)) || [];

    return newForm;
  }
}

export class AcquisitionTeamForm {
  contact?: IContactSearchResult;
  contactTypeCode: string;

  constructor(contactTypeCode: string, contact?: IContactSearchResult) {
    this.contactTypeCode = contactTypeCode;
    this.contact = contact;
  }

  static toApi(model: AcquisitionTeamForm[]): Api_AcquisitionFilePerson[] {
    return model
      .filter(x => !!x.contact && !!x.contactTypeCode)
      .map<Api_AcquisitionFilePerson>(x => {
        return {
          personId: x.contact?.personId || 0,
          personProfileTypeCode: x.contactTypeCode,
        };
      });
  }
}
