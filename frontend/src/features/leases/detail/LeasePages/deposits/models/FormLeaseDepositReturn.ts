import { fromContact, IContactSearchResult, toContact } from 'interfaces';
import { Api_SecurityDeposit, Api_SecurityDepositReturn } from 'models/api/SecurityDeposit';
import { NumberFieldValue } from 'typings/NumberFieldValue';

export class FormLeaseDepositReturn {
  public id?: number;
  public parentDepositId: number;
  public depositTypeCode: string;
  public depositTypeDescription: string;
  public parentDepositOtherDescription: string;
  public terminationDate: string;
  public claimsAgainst: NumberFieldValue;
  public returnAmount: NumberFieldValue;
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

  public static createFromModel(
    baseModel: Api_SecurityDepositReturn,
    parentDeposit: Api_SecurityDeposit,
  ): FormLeaseDepositReturn {
    let model = new FormLeaseDepositReturn();

    // Parent fields
    model.depositTypeCode = parentDeposit.depositType.id;
    model.depositTypeDescription = parentDeposit.depositType.description || '';
    model.parentDepositOtherDescription = parentDeposit.otherTypeDescription || '';
    model.parentDepositAmount = parentDeposit.amountPaid;

    model.id = baseModel.id;
    model.parentDepositId = baseModel.parentDepositId;
    model.terminationDate = baseModel.terminationDate || '';
    model.claimsAgainst = baseModel.claimsAgainst || '';
    model.returnAmount = baseModel.returnAmount;
    model.returnDate = baseModel.returnDate || '';
    model.contactHolder =
      baseModel.contactHolder !== undefined ? fromContact(baseModel.contactHolder) : undefined;
    model.rowVersion = baseModel.rowVersion;
    return model;
  }

  public toInterfaceModel(): Api_SecurityDepositReturn {
    return {
      id: this.id,
      parentDepositId: this.parentDepositId,
      terminationDate: this.terminationDate === '' ? undefined : this.terminationDate,
      claimsAgainst: this.claimsAgainst === '' ? undefined : this.claimsAgainst,
      returnAmount: this.returnAmount === '' ? 0 : this.returnAmount,
      returnDate: this.returnDate === '' ? undefined : this.returnDate,
      contactHolder: this.contactHolder !== undefined ? toContact(this.contactHolder) : undefined,
      rowVersion: this.rowVersion,
    };
  }
}
