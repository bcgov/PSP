import { fromContact, IContactSearchResult, toContact } from '@/interfaces';
import { Api_SecurityDeposit, Api_SecurityDepositReturn } from '@/models/api/SecurityDeposit';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
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

  public static createEmpty(deposit: Api_SecurityDeposit): FormLeaseDepositReturn {
    var returnDeposit = new FormLeaseDepositReturn();
    returnDeposit.parentDepositId = deposit.id || 0;
    returnDeposit.depositTypeCode = deposit.depositType.id;
    returnDeposit.depositTypeDescription = deposit.depositType.description || '';
    returnDeposit.parentDepositOtherDescription = deposit.otherTypeDescription || '';
    returnDeposit.parentDepositAmount = deposit.amountPaid;
    return returnDeposit;
  }

  public static fromApi(
    baseModel: Api_SecurityDepositReturn,
    parentDeposit: Api_SecurityDeposit,
  ): FormLeaseDepositReturn {
    let model = new FormLeaseDepositReturn();

    // Parent fields
    model.depositTypeCode = parentDeposit.depositType.id;
    model.depositTypeDescription = parentDeposit.depositType.description || '';
    model.parentDepositOtherDescription = parentDeposit.otherTypeDescription || '';
    model.parentDepositAmount = parentDeposit.amountPaid;

    model.id = baseModel.id ?? undefined;
    model.parentDepositId = baseModel.parentDepositId;
    model.terminationDate = baseModel.terminationDate || '';
    model.claimsAgainst = baseModel.claimsAgainst || '';
    model.returnAmount = baseModel.returnAmount || '';
    model.interestPaid = baseModel.interestPaid || '';
    model.returnDate = baseModel.returnDate || '';
    model.contactHolder =
      baseModel.contactHolder !== null ? fromContact(baseModel.contactHolder) : undefined;
    model.rowVersion = baseModel.rowVersion || 0;
    return model;
  }

  public toApi(): Api_SecurityDepositReturn {
    return {
      id: this.id ?? null,
      parentDepositId: this.parentDepositId,
      terminationDate: this.terminationDate,
      claimsAgainst: numberFieldToRequiredNumber(this.claimsAgainst),
      returnAmount: numberFieldToRequiredNumber(this.returnAmount),
      interestPaid: numberFieldToRequiredNumber(this.interestPaid),
      returnDate: this.returnDate,
      contactHolder: this.contactHolder !== undefined ? toContact(this.contactHolder) : null,
      rowVersion: this.rowVersion,
    };
  }
}
