import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';
import { formatApiPersonNames } from '@/utils/personUtils';

export type RecipientType = 'OWNR' | 'HLDR' | 'SLTR' | 'REPT';

export class LetterRecipientModel {
  readonly id: string;
  interestType: RecipientType;
  generateModel: Api_GenerateOwner;
  conceptModel:
    | ApiGen_Concepts_Person
    | ApiGen_Concepts_InterestHolder
    | ApiGen_Concepts_AcquisitionFileOwner
    | null;

  constructor(
    conceptId: number,
    type: RecipientType,
    model: Api_GenerateOwner,
    conceptModel:
      | ApiGen_Concepts_Person
      | ApiGen_Concepts_InterestHolder
      | ApiGen_Concepts_AcquisitionFileOwner
      | null = null,
  ) {
    this.id = `${type}${conceptId}`;
    this.interestType = type;
    this.generateModel = model;
    this.conceptModel = conceptModel;
  }

  public getInterestTypeString(): string {
    let identifier = '';
    switch (this.interestType) {
      case 'HLDR':
        identifier = '(Interest Holder)';
        break;
      case 'OWNR':
        identifier = '(Owner)';
        break;
      case 'SLTR':
        identifier = '(Owner solicitor)';
        break;
      case 'REPT':
        identifier = '(Owner representative)';
        break;
      default:
        identifier = '';
    }

    return identifier;
  }

  public getDisplayName(): string | null {
    if (this.interestType === 'OWNR') {
      return this.generateModel.owner_string;
    } else {
      const model = this.conceptModel as ApiGen_Concepts_InterestHolder;
      return model.personId ? formatApiPersonNames(model.person) : model.organization?.name ?? '';
    }
  }

  public getContactRouteParam(): string | null {
    let paramString = '';
    if (this.interestType === 'OWNR') {
      return null;
    } else {
      const model = this.conceptModel as ApiGen_Concepts_InterestHolder;
      paramString = model.personId ? `P${model.personId}` : `O${model.organizationId}`;
    }

    return paramString;
  }
}
