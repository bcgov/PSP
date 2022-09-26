import { fromApiPerson, IContactSearchResult } from 'interfaces';
import { Api_AcquisitionFilePerson } from 'models/api/AcquisitionFile';
import { fromTypeCode, toTypeCode } from 'utils/formUtils';

export interface WithAcquisitionTeam {
  team: AcquisitionTeamFormModel[];
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
