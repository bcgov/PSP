import { fromContact, IContactSearchResult, toContact } from '@/interfaces';
import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';
import { ApiGen_Concepts_SecurityDepositReturn } from '@/models/api/generated/ApiGen_Concepts_SecurityDepositReturn';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { isValidIsoDateTime } from '@/utils';
import { numberFieldToRequiredNumber } from '@/utils/formUtils';

export class FormLeaseDepositReturn {
  public id?: number;
  public parentDepositId: number;
  public depositTypeCode: string;
  public depositTypeDescription: string;
  public parentDepositOtherDescription: string;
  public terminationDate: string;
  public claimsAgainst: NumberFieldValue;
  public returnAmount: NumberFieldValue;
  public interestPaid: NumberFieldValue;
  public returnDate: string;
  public contactHolder?: IContactSearchResult;
  public rowVersion: number;
  public parentDepositAmount: number;

  private constructor() {
    this.parentDepositId = 0;
    this.depositTypeCode = '';
    this.depositTypeDescription = '';
    this.parentDepositOtherDescription = '';
    this.terminationDate = '';
    this.claimsAgainst = '';
    this.returnAmount = '';
    this.interestPaid = '';
    this.returnDate = '';
    this.parentDepositAmount = 0;
    this.rowVersion = 0;
  }

  public static createEmpty(deposit: ApiGen_Concepts_SecurityDeposit): FormLeaseDepositReturn {
    const returnDeposit = new FormLeaseDepositReturn();
    returnDeposit.parentDepositId = deposit.id || 0;
    returnDeposit.depositTypeCode = deposit.depositType?.id ?? '';
    returnDeposit.depositTypeDescription = deposit.depositType?.description || '';
    returnDeposit.parentDepositOtherDescription = deposit.otherTypeDescription || '';
    returnDeposit.parentDepositAmount = deposit.amountPaid;
    return returnDeposit;
  }

  public static fromApi(
    baseModel: ApiGen_Concepts_SecurityDepositReturn,
    parentDeposit: ApiGen_Concepts_SecurityDeposit,
  ): FormLeaseDepositReturn {
    const model = new FormLeaseDepositReturn();

    // Parent fields
    model.depositTypeCode = parentDeposit.depositType?.id ?? '';
    model.depositTypeDescription = parentDeposit.depositType?.description || '';
    model.parentDepositOtherDescription = parentDeposit.otherTypeDescription || '';
    model.parentDepositAmount = parentDeposit.amountPaid;

    model.id = baseModel.id ?? undefined;
    model.parentDepositId = baseModel.parentDepositId;
    model.terminationDate = isValidIsoDateTime(baseModel.terminationDate)
      ? baseModel.terminationDate
      : '';
    model.claimsAgainst = baseModel.claimsAgainst || '';
    model.returnAmount = baseModel.returnAmount || '';
    model.interestPaid = baseModel.interestPaid || '';
    model.returnDate = isValidIsoDateTime(baseModel.returnDate) ? baseModel.returnDate : '';
    model.contactHolder =
      baseModel.contactHolder !== null ? fromContact(baseModel.contactHolder) : undefined;
    model.rowVersion = baseModel.rowVersion || 0;
    return model;
  }

  public toApi(): ApiGen_Concepts_SecurityDepositReturn {
    return {
      id: this.id ?? null,
      parentDepositId: this.parentDepositId,
      terminationDate: isValidIsoDateTime(this.terminationDate)
        ? this.terminationDate
        : EpochIsoDateTime,
      claimsAgainst: numberFieldToRequiredNumber(this.claimsAgainst),
      returnAmount: numberFieldToRequiredNumber(this.returnAmount),
      interestPaid: numberFieldToRequiredNumber(this.interestPaid),
      returnDate: isValidIsoDateTime(this.returnDate) ? this.returnDate : EpochIsoDateTime,
      contactHolder: this.contactHolder !== undefined ? toContact(this.contactHolder) : null,
      depositType: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}
