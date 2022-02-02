import { NumberFieldValue } from 'typings/NumberFieldValue';

import { IBaseModel } from './IBaseModel';
import { IOrganization } from './IOrganization';
import { IPerson } from './IPerson';
import ITypeCode from './ITypeCode';

export interface ILeaseSecurityDeposit extends IBaseModel {
  id?: number;
  description: string;
  amountPaid: number;
  depositDate?: string;
  depositType: ITypeCode<string>;
  otherTypeDescription?: string;
  personDepositHolderId?: number;
  personDepositHolder?: IPerson;
  organizationDepositHolderId?: number;
  organizationDepositHolder?: IOrganization;
}

export class FormLeaseDeposit implements IBaseModel {
  public id?: number;
  public description: string;
  public amountPaid: NumberFieldValue;
  public depositDate: string;
  public depositTypeCode: string;
  public otherTypeDescription: string;
  public personDepositHolderId: NumberFieldValue;
  public organizationDepositHolderId: NumberFieldValue;
  public rowVersion: number;

  private constructor() {
    this.description = '';
    this.amountPaid = '';
    this.depositDate = '';
    this.depositTypeCode = '';
    this.otherTypeDescription = '';
    this.personDepositHolderId = '';
    this.organizationDepositHolderId = '';
    this.rowVersion = 0;
  }

  public static createEmpty(): FormLeaseDeposit {
    return new FormLeaseDeposit();
  }

  public static createFromModel(baseModel: ILeaseSecurityDeposit): FormLeaseDeposit {
    let model = new FormLeaseDeposit();
    model.id = baseModel.id;
    model.description = baseModel.description;
    model.amountPaid = baseModel.amountPaid;
    model.depositDate = baseModel.depositDate || '';
    model.depositTypeCode = baseModel.depositType.id;
    model.otherTypeDescription = baseModel.otherTypeDescription || '';
    model.personDepositHolderId = baseModel.personDepositHolderId || '';
    model.organizationDepositHolderId = baseModel.organizationDepositHolderId || '';
    model.rowVersion = baseModel.rowVersion;
    return model;
  }

  public toInterfaceModel(): ILeaseSecurityDeposit {
    return {
      id: this.id,
      description: this.description,
      amountPaid: this.amountPaid === '' ? 0 : this.amountPaid,
      depositDate: this.depositDate === '' ? undefined : this.depositDate,
      depositType: { id: this.depositTypeCode },
      otherTypeDescription:
        this.otherTypeDescription === '' ? undefined : this.otherTypeDescription,
      personDepositHolderId:
        this.personDepositHolderId === '' ? undefined : this.personDepositHolderId,
      organizationDepositHolderId:
        this.organizationDepositHolderId === '' ? undefined : this.organizationDepositHolderId,
      rowVersion: this.rowVersion,
    };
  }
}
