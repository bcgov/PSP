import { fromApiPerson, IContactSearchResult } from 'interfaces';
import { Api_AcquisitionFile, Api_AcquisitionFilePerson } from 'models/api/AcquisitionFile';
import { fromTypeCode, toTypeCode } from 'utils/formUtils';

export class UpdateAcquisitionSummaryFormModel {
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
  team: AcquisitionTeamFormModel[] = [];

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
      acquisitionTeam: this.team
        .filter(x => !!x.contact && !!x.contactTypeCode)
        .map<Api_AcquisitionFilePerson>(x => x.toApi()),
    };
  }

  static fromApi(model: Api_AcquisitionFile): UpdateAcquisitionSummaryFormModel {
    const newForm = new UpdateAcquisitionSummaryFormModel();
    newForm.id = model.id;
    newForm.fileName = model.fileName || '';
    newForm.rowVersion = model.rowVersion;
    newForm.assignedDate = model.assignedDate;
    newForm.deliveryDate = model.deliveryDate;
    newForm.acquisitionFileStatusType = fromTypeCode(model.fileStatusTypeCode);
    newForm.acquisitionPhysFileStatusType = fromTypeCode(model.acquisitionPhysFileStatusTypeCode);
    newForm.acquisitionType = fromTypeCode(model.acquisitionTypeCode);
    newForm.region = fromTypeCode(model.regionCode)?.toString();
    newForm.team = model.acquisitionTeam?.map(x => AcquisitionTeamFormModel.fromApi(x)) || [];

    return newForm;
  }
}

export class AcquisitionTeamFormModel {
  contact?: IContactSearchResult;
  contactTypeCode: string;

  constructor(contactTypeCode: string, contact?: IContactSearchResult) {
    this.contactTypeCode = contactTypeCode;
    this.contact = contact;
  }

  toApi(): Api_AcquisitionFilePerson {
    return {
      personId: this.contact?.personId || 0,
      person: { id: this.contact?.personId || 0 },
      personProfileType: toTypeCode(this.contactTypeCode),
    };
  }

  static fromApi(model: Api_AcquisitionFilePerson): AcquisitionTeamFormModel {
    const newForm = new AcquisitionTeamFormModel(
      fromTypeCode(model.personProfileType) || '',
      model.person !== undefined ? fromApiPerson(model.person) : undefined,
    );
    return newForm;
  }
}
