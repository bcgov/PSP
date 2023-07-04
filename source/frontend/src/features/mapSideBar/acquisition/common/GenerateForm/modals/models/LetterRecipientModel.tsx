import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';

type RecipientType = 'OWNR' | 'HLDR' | 'SLTR' | 'REPT';

export class LetterRecipientModel {
  readonly id: number;
  interestType: RecipientType;
  generateModel: Api_GenerateOwner;

  constructor(id: number, type: RecipientType, model: Api_GenerateOwner) {
    this.id = id;
    this.interestType = type;
    this.generateModel = model;
  }

  public get InterestType(): string {
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
}
