import { fromApiPerson, IContactSearchResult } from 'interfaces';
import { Api_AcquisitionFileOwner, Api_AcquisitionFilePerson } from 'models/api/AcquisitionFile';
import { fromTypeCode, toTypeCode } from 'utils/formUtils';

export interface WithAcquisitionTeam {
  team: AcquisitionTeamFormModel[];
}

export interface WithAcquisitionOwners {
  owners: AcquisitionOwnerFormModel[];
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
      personProfileTypeCode: this.contactTypeCode,
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

export class AcquisitionOwnerFormModel {
  id?: number;
  rowVersion?: number;
  acquisitionFileId?: number;
  lastNameOrCorp1?: string;
  lastNameOrCorp2?: string;
  givenName?: string;
  incorporationNumber?: string;

  toApi(): Api_AcquisitionFileOwner {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      acquisitionFileId: this.acquisitionFileId,
      lastNameOrCorp1: this.lastNameOrCorp1,
      lastNameOrCorp2: this.lastNameOrCorp2,
      givenName: this.givenName,
      incorporationNumber: this.incorporationNumber,
    };
  }

  static fromApi(model: Api_AcquisitionFileOwner): AcquisitionOwnerFormModel {
    const newForm = new AcquisitionOwnerFormModel();
    newForm.id = model.id;
    newForm.rowVersion = model.rowVersion;
    newForm.acquisitionFileId = model.acquisitionFileId;
    newForm.lastNameOrCorp1 = model.lastNameOrCorp1;
    newForm.lastNameOrCorp2 = model.lastNameOrCorp2;
    newForm.givenName = model.givenName;
    newForm.incorporationNumber = model.incorporationNumber;

    return newForm;
  }
}
