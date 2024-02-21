import { fromContact, IContactSearchResult, toContact } from '@/interfaces';
import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { isValidIsoDateTime } from '@/utils';
import { toTypeCodeNullable } from '@/utils/formUtils';

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

  public static fromApi(baseModel: ApiGen_Concepts_SecurityDeposit): FormLeaseDeposit {
    const model = new FormLeaseDeposit();
    model.leaseId = baseModel.leaseId ?? undefined;
    model.id = baseModel.id ?? undefined;
    model.description = baseModel.description ?? '';
    model.amountPaid = baseModel.amountPaid;
    model.depositDate = isValidIsoDateTime(baseModel.depositDateOnly)
      ? baseModel.depositDateOnly
      : '';
    model.depositTypeCode = baseModel.depositType?.id ?? '';
    model.otherTypeDescription = baseModel.otherTypeDescription || '';
    model.contactHolder =
      baseModel.contactHolder !== null ? fromContact(baseModel.contactHolder) : undefined;

    model.rowVersion = baseModel.rowVersion || 0;
    return model;
  }

  public toApi(): ApiGen_Concepts_SecurityDeposit {
    return {
      id: this.id ?? null,
      leaseId: this.leaseId ?? 0,
      description: this.description,
      amountPaid: this.amountPaid === '' ? 0 : this.amountPaid,
      depositDateOnly: this.depositDate,
      depositType: toTypeCodeNullable(this.depositTypeCode),
      otherTypeDescription: this.otherTypeDescription === '' ? null : this.otherTypeDescription,
      contactHolder: this.contactHolder !== undefined ? toContact(this.contactHolder) : null,
      depositReturns: [],
      rowVersion: this.rowVersion,
    };
  }
}
