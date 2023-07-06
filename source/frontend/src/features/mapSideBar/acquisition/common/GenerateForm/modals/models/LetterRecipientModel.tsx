import { Api_AcquisitionFileSolicitor } from '@/models/api/AcquisitionFile';
import { Api_InterestHolder } from '@/models/api/InterestHolder';
import { Api_Person } from '@/models/api/Person';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';
import { formatApiPersonNames } from '@/utils/personUtils';

type RecipientType = 'OWNR' | 'HLDR' | 'SLTR' | 'REPT';

export class LetterRecipientModel {
  readonly id: number;
  interestType: RecipientType;
  generateModel: Api_GenerateOwner;
  conceptModel: Api_Person | Api_InterestHolder | Api_AcquisitionFileSolicitor | null;

  constructor(
    id: number,
    type: RecipientType,
    model: Api_GenerateOwner,
    conceptModel: Api_Person | Api_InterestHolder | Api_AcquisitionFileSolicitor | null = null,
  ) {
    this.id = id;
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
    }

    if (this.interestType === 'REPT') {
      return formatApiPersonNames(this.conceptModel as Api_Person) ?? '';
    }

    if (this.interestType === 'HLDR') {
      const model = this.conceptModel as Api_InterestHolder;
      return model.personId ? formatApiPersonNames(model.person) : model.organization?.name ?? '';
    }

    if (this.interestType === 'SLTR') {
      const model = this.conceptModel as Api_AcquisitionFileSolicitor;
      return model.personId ? formatApiPersonNames(model.person) : model.organization?.name ?? '';
    }

    return null;
  }

  public getContactRouteParam(): string | null {
    let paramString = '';
    if (this.interestType === 'OWNR') {
      return null;
    }

    if (this.interestType === 'REPT') {
      const contactId = (this.conceptModel as Api_Person).id ?? null;
      paramString = `P${contactId}`;
    }

    if (this.interestType === 'HLDR') {
      const model = this.conceptModel as Api_InterestHolder;
      paramString = model.personId ? `P${model.personId}` : `O${model.organizationId}`;
    }

    if (this.interestType === 'SLTR') {
      const model = this.conceptModel as Api_AcquisitionFileSolicitor;
      paramString = model.personId ? `P${model.personId}` : `O${model.organizationId}`;
    }

    return paramString;
  }
}
