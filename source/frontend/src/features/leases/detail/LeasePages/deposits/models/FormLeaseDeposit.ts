import { fromContact, IContactSearchResult, toContact } from '@/interfaces';
import { Api_SecurityDeposit } from '@/models/api/SecurityDeposit';
import { NumberFieldValue } from '@/typings/NumberFieldValue';

export class FormLeaseDeposit {
  public id?: number;
  public leaseId?: number;
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

  public static createEmpty(leaseId: number): FormLeaseDeposit {
    const deposit = new FormLeaseDeposit();
    deposit.leaseId = leaseId;
    return deposit;
  }

  public static fromApi(baseModel: Api_SecurityDeposit): FormLeaseDeposit {
    let model = new FormLeaseDeposit();
    model.leaseId = baseModel.leaseId ?? undefined;
    model.id = baseModel.id ?? undefined;
    model.description = baseModel.description;
    model.amountPaid = baseModel.amountPaid;
    model.depositDate = baseModel.depositDate || '';
    model.depositTypeCode = baseModel.depositType.id;
    model.otherTypeDescription = baseModel.otherTypeDescription || '';
    model.contactHolder =
      baseModel.contactHolder !== null ? fromContact(baseModel.contactHolder) : undefined;

    model.rowVersion = baseModel.rowVersion || 0;
    return model;
  }

  public toApi(): Api_SecurityDeposit {
    return {
      id: this.id ?? null,
      leaseId: this.leaseId ?? null,
      description: this.description,
      amountPaid: this.amountPaid === '' ? 0 : this.amountPaid,
      depositDate: this.depositDate,
      depositType: { id: this.depositTypeCode },
      otherTypeDescription: this.otherTypeDescription === '' ? null : this.otherTypeDescription,
      contactHolder: this.contactHolder !== undefined ? toContact(this.contactHolder) : null,
      depositReturns: [],
      rowVersion: this.rowVersion,
    };
  }
}
