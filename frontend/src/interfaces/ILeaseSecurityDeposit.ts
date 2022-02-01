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

  public static toInterfaceModel(model: FormLeaseDeposit): ILeaseSecurityDeposit {
    return {
      id: model.id,
      description: model.description,
      amountPaid: model.amountPaid === '' ? 0 : model.amountPaid,
      depositDate: model.depositDate === '' ? undefined : model.depositDate,
      depositType: { id: model.depositTypeCode },
      otherTypeDescription:
        model.otherTypeDescription === '' ? undefined : model.otherTypeDescription,
      personDepositHolderId:
        model.personDepositHolderId === '' ? undefined : model.personDepositHolderId,
      organizationDepositHolderId:
        model.organizationDepositHolderId === '' ? undefined : model.organizationDepositHolderId,
      rowVersion: model.rowVersion,
    };
  }
}
