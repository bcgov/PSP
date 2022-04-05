import { fromContact, IContactSearchResult, toContact } from 'interfaces';
import { Api_SecurityDeposit } from 'models/api/SecurityDeposit';
import { NumberFieldValue } from 'typings/NumberFieldValue';

export class FormLeaseDeposit {
  public id?: number;
  public description: string;
  public amountPaid: NumberFieldValue;
  public depositDate: string;
  public depositTypeCode: string;
  public otherTypeDescription: string;
  public contactHolder?: IContactSearchResult;
  public rowVersion: number;

  private constructor() {
    this.description = '';
    this.amountPaid = '';
    this.depositDate = '';
    this.depositTypeCode = '';
    this.otherTypeDescription = '';
    this.rowVersion = 0;
  }

  public static createEmpty(): FormLeaseDeposit {
    return new FormLeaseDeposit();
  }

  public static createFromModel(baseModel: Api_SecurityDeposit): FormLeaseDeposit {
    let model = new FormLeaseDeposit();
    model.id = baseModel.id;
    model.description = baseModel.description;
    model.amountPaid = baseModel.amountPaid;
    model.depositDate = baseModel.depositDate || '';
    model.depositTypeCode = baseModel.depositType.id;
    model.otherTypeDescription = baseModel.otherTypeDescription || '';
    model.contactHolder =
      baseModel.contactHolder !== undefined ? fromContact(baseModel.contactHolder) : undefined;

    model.rowVersion = baseModel.rowVersion || 0;
    return model;
  }

  public toInterfaceModel(): Api_SecurityDeposit {
    return {
      id: this.id,
      description: this.description,
      amountPaid: this.amountPaid === '' ? 0 : this.amountPaid,
      depositDate: this.depositDate === '' ? undefined : this.depositDate,
      depositType: { id: this.depositTypeCode },
      otherTypeDescription:
        this.otherTypeDescription === '' ? undefined : this.otherTypeDescription,
      contactHolder: this.contactHolder !== undefined ? toContact(this.contactHolder) : undefined,
      depositReturns: [],
      rowVersion: this.rowVersion,
    };
  }
}
