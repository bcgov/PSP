import { IContactSearchResult } from '@/interfaces/IContactSearchResult';

export class DispositionSalePurchaserModel {
  contact: IContactSearchResult | null = null;
  primaryContactId: string = '';

  constructor(
    readonly id: number | null = null,
    readonly rowVersion: number | null = null,
    contact: IContactSearchResult | null = null,
  ) {
    this.id = id;
    this.contact = contact;
    this.rowVersion = rowVersion;
  }
}
