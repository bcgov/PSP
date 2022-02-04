import { NumberFieldValue } from 'typings/NumberFieldValue';

import { IBaseModel } from './IBaseModel';
import { ILeaseSecurityDeposit } from './ILeaseSecurityDeposit';
import { IOrganization } from './IOrganization';
import { IPerson } from './IPerson';
import ITypeCode from './ITypeCode';

export interface ILeaseSecurityDepositReturn extends IBaseModel {
  id?: number;
  parentDepositId: number;
  depositType: ITypeCode<string>;
  terminationDate?: string;
  claimsAgainst?: number;
  returnAmount: number;
  returnDate?: string;
  payeeName: string;
  payeeAddress?: string;
  personDepositReturnHolder?: IPerson;
  personDepositReturnHolderId?: number;
  organizationDepositReturnHolder?: IOrganization;
  organizationDepositReturnHolderId?: number;
}

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
  public payeeName: string;
  public payeeAddress: string;
  public personDepositReturnHolderId: NumberFieldValue;
  public organizationDepositReturnHolderId: NumberFieldValue;
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
    this.payeeName = '';
    this.payeeAddress = '';
    this.personDepositReturnHolderId = '';
    this.organizationDepositReturnHolderId = '';
    this.parentDepositAmount = 0;
    this.rowVersion = 0;
  }

  public static createEmpty(deposit: ILeaseSecurityDeposit): FormLeaseDepositReturn {
    var returnDeposit = new FormLeaseDepositReturn();
    returnDeposit.parentDepositId = deposit.id || 0;
    returnDeposit.depositTypeCode = deposit.depositType.id;
    returnDeposit.depositTypeDescription = deposit.depositType.description || '';
    returnDeposit.parentDepositOtherDescription = deposit.otherTypeDescription || '';
    returnDeposit.parentDepositAmount = deposit.amountPaid;
    return returnDeposit;
  }

  public static createFromModel(
    baseModel: ILeaseSecurityDepositReturn,
    parentDeposit: ILeaseSecurityDeposit,
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
    model.payeeName = baseModel.payeeName;
    model.payeeAddress = baseModel.payeeAddress || '';
    model.personDepositReturnHolderId = baseModel.personDepositReturnHolderId || '';
    model.organizationDepositReturnHolderId = baseModel.organizationDepositReturnHolderId || '';
    model.rowVersion = baseModel.rowVersion;
    return model;
  }

  public toInterfaceModel(): ILeaseSecurityDepositReturn {
    return {
      id: this.id,
      parentDepositId: this.parentDepositId,
      depositType: { id: this.depositTypeCode },
      terminationDate: this.terminationDate === '' ? undefined : this.terminationDate,
      claimsAgainst: this.claimsAgainst === '' ? undefined : this.claimsAgainst,
      returnAmount: this.returnAmount === '' ? 0 : this.returnAmount,
      returnDate: this.returnDate === '' ? undefined : this.returnDate,
      payeeName: this.payeeName,
      payeeAddress: this.payeeAddress,
      personDepositReturnHolderId:
        this.personDepositReturnHolderId === '' ? undefined : this.personDepositReturnHolderId,
      organizationDepositReturnHolderId:
        this.organizationDepositReturnHolderId === ''
          ? undefined
          : this.organizationDepositReturnHolderId,
      rowVersion: this.rowVersion,
    };
  }
}
