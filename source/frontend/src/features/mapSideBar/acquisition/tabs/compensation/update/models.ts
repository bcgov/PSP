import { Api_AcquisitionFileOwner, Api_AcquisitionFilePerson } from '@/models/api/AcquisitionFile';
import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_CompensationPayee } from '@/models/api/CompensationPayee';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_FinancialCode } from '@/models/api/FinancialCode';
import { Api_InterestHolder } from '@/models/api/InterestHolder';
import { isNullOrWhitespace } from '@/utils';
import { booleanToString, stringToBoolean, stringToUndefined, toTypeCode } from '@/utils/formUtils';
import { formatApiPersonNames } from '@/utils/personUtils';

export class CompensationRequisitionFormModel {
  id: number | null = null;
  acquisitionFileId: number;
  status: string = '';
  fiscalYear: string = '';
  stob: string = '';
  yearlyFinancial: Api_FinancialCode | null = null;
  serviceLine: string = '';
  chartOfAccounts: Api_FinancialCode | null = null;
  responsibilityCentre: string = '';
  Responsibility: Api_FinancialCode | null = null;
  agreementDateTime: string = '';
  expropriationNoticeServedDateTime: string = '';
  expropriationVestingDateTime: string = '';
  generationDatetTime: string = '';
  specialInstruction: string = '';
  detailedRemarks: string = '';
  financials: FinancialActivityFormModel[] = [];
  payees: AcquisitionPayeeFormModel[] = [];
  isDisabled: string = '';
  rowVersion: number | null = null;
  payeeKey: string = '';

  constructor(id: number | null, acquisitionFileId: number = 0) {
    this.id = id;
    this.acquisitionFileId = acquisitionFileId;
  }

  toApi(payeeOptions: PayeeOption[]): Api_CompensationRequisition {
    let apiPayee: Api_CompensationPayee | null = PayeeOption.toApi(
      this.payees[0]._id,
      this.id,
      this.payeeKey,
      payeeOptions,
    );

    if (apiPayee) {
      apiPayee.id = this.payees[0]._id;
      apiPayee.gstNumber = this.payees[0].gstNumber;
      apiPayee.isPaymentInTrust = this.payees[0].isPaymentInTrust;
      apiPayee.rowVersion = this.payees[0].rowVersion;
    }

    return {
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
      isDraft: this.status === 'draft' ? true : false,
      fiscalYear: stringToUndefined(this.fiscalYear),
      yearlyFinancialId: this.stob === '' ? null : Number(this.stob),
      yearlyFinancial: null,
      chartOfAccountsId: this.serviceLine === '' ? null : Number(this.serviceLine),
      chartOfAccounts: null,
      responsibilityId: this.responsibilityCentre === '' ? null : Number(this.responsibilityCentre),
      responsibility: null,
      agreementDate: stringToUndefined(this.agreementDateTime),
      expropriationNoticeServedDate: stringToUndefined(this.expropriationNoticeServedDateTime),
      expropriationVestingDate: stringToUndefined(this.expropriationVestingDateTime),
      generationDate: stringToUndefined(this.generationDatetTime),
      specialInstruction: stringToUndefined(this.specialInstruction),
      detailedRemarks: stringToUndefined(this.detailedRemarks),
      isDisabled: stringToBoolean(this.isDisabled),
      financials: this.financials
        .filter(x => !x.isEmpty())
        .map<Api_CompensationFinancial>(x => x.toApi()),
      payees: apiPayee === null ? [] : [apiPayee],
      rowVersion: this.rowVersion ?? undefined,
    };
  }

  static fromApi(apiModel: Api_CompensationRequisition): CompensationRequisitionFormModel {
    const compensation = new CompensationRequisitionFormModel(
      apiModel.id,
      apiModel.acquisitionFileId,
    );

    compensation.status =
      apiModel.isDraft === true ? 'draft' : apiModel.isDraft === null ? '' : 'final';
    compensation.fiscalYear = apiModel.fiscalYear || '';
    compensation.stob = apiModel.yearlyFinancialId?.toString() || '';
    compensation.yearlyFinancial = apiModel.yearlyFinancial;
    compensation.serviceLine = apiModel.chartOfAccountsId?.toString() || '';
    compensation.chartOfAccounts = apiModel.chartOfAccounts;
    compensation.responsibilityCentre = apiModel.responsibilityId?.toString() || '';
    compensation.Responsibility = apiModel.responsibility;
    compensation.agreementDateTime = apiModel.agreementDate || '';
    compensation.expropriationNoticeServedDateTime = apiModel.expropriationNoticeServedDate || '';
    compensation.expropriationVestingDateTime = apiModel.expropriationVestingDate || '';
    compensation.generationDatetTime = apiModel.generationDate || '';
    compensation.specialInstruction = apiModel.specialInstruction || '';
    compensation.financials =
      apiModel.financials?.map(x => FinancialActivityFormModel.fromApi(x)) || [];
    compensation.detailedRemarks = apiModel.detailedRemarks || '';
    compensation.payees = apiModel.payees?.map(x => AcquisitionPayeeFormModel.fromApi(x)) || [];
    compensation.payeeKey = PayeeOption.fromApi(apiModel.payees);
    compensation.isDisabled = booleanToString(apiModel.isDisabled);
    compensation.rowVersion = apiModel.rowVersion ?? null;

    const payeePretaxAmount = apiModel?.financials
      .map(f => f.pretaxAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    const payeeTaxAmount = apiModel?.financials
      .map(f => f.taxAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    const payeeTotalAmount = apiModel?.financials
      .map(f => f.totalAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    if (compensation.payees.length === 0) {
      let defaultPayee = AcquisitionPayeeFormModel.defatultPayee(compensation.id!);
      defaultPayee.pretaxAmount = payeePretaxAmount;
      defaultPayee.taxAmount = payeeTaxAmount;
      defaultPayee.taxAmount = payeeTotalAmount;

      compensation.payees.push(defaultPayee);
    } else {
      compensation.payees[0].pretaxAmount = payeePretaxAmount;
      compensation.payees[0].taxAmount = payeeTaxAmount;
      compensation.payees[0].totalAmount = payeeTotalAmount;
    }

    return compensation;
  }
}

export class FinancialActivityFormModel {
  readonly _id: number | null = null;
  readonly _compensationRequisitionId: number;

  financialActivityCodeId: string = '';
  financialActivityCode: Api_FinancialCode | null = null;
  pretaxAmount: number = 0;
  isGstRequired: string = '';
  taxAmount: number = 0;
  totalAmount: number = 0;
  rowVersion: number | null = null;
  isDisabled: string = '';

  constructor(id: number | null = null, compensationRequisitionId: number) {
    this._id = id;
    this._compensationRequisitionId = compensationRequisitionId;
  }

  isEmpty(): boolean {
    return this.financialActivityCode === null && this.pretaxAmount === 0;
  }

  toApi(): Api_CompensationFinancial {
    return {
      id: this._id,
      compensationId: this._compensationRequisitionId,
      financialActivityCodeId: +this.financialActivityCodeId,
      financialActivityCode: this.financialActivityCode
        ? toTypeCode<number>(+this.financialActivityCode) ?? null
        : null,
      pretaxAmount: this.pretaxAmount,
      isGstRequired: stringToBoolean(this.isGstRequired),
      taxAmount: this.taxAmount,
      totalAmount: this.totalAmount,
      rowVersion: this.rowVersion ?? null,
      isDisabled: stringToBoolean(this.isDisabled),
    };
  }

  static fromApi(model: Api_CompensationFinancial): FinancialActivityFormModel {
    const newForm = new FinancialActivityFormModel(model.id, model.compensationId);
    newForm.pretaxAmount = model.pretaxAmount ?? 0;
    newForm.isGstRequired = booleanToString(model.isGstRequired);
    newForm.taxAmount = model.taxAmount ?? 0;
    newForm.totalAmount = model.totalAmount ?? 0;
    newForm.financialActivityCodeId = model.financialActivityCodeId.toString() ?? '';
    newForm.financialActivityCode = model.financialActivityCode;
    newForm.rowVersion = model.rowVersion ?? null;
    newForm.isDisabled = booleanToString(model.isDisabled);

    return newForm;
  }
}

export class AcquisitionPayeeFormModel {
  readonly _id: number | null;
  readonly _compensationRequisitionId: number | null;

  payeeSelectedOption: string = '';
  gstNumber: string = '';
  isPaymentInTrust: boolean = false;
  pretaxAmount: number = 0;
  taxAmount: number = 0;
  totalAmount: number = 0;
  acquisitionOwnerId: string = '';
  interestHolderId: string = '';
  acquisitionFilePersonId: string = '';
  rowVersion: number | null = null;
  isDisabled: string = '';

  constructor(id: number | null = null, compensationRequisitionId: number | null = null) {
    this._id = id;
    this._compensationRequisitionId = compensationRequisitionId;
  }

  static fromApi(apiModel: Api_CompensationPayee): AcquisitionPayeeFormModel {
    const payeeModel = new AcquisitionPayeeFormModel(
      apiModel.id,
      apiModel.compensationRequisitionId,
    );

    payeeModel.isPaymentInTrust = apiModel.isPaymentInTrust ?? false;
    payeeModel.gstNumber = apiModel.gstNumber ?? '';
    payeeModel.acquisitionOwnerId = apiModel.acquisitionOwnerId?.toString() ?? '';
    payeeModel.interestHolderId = apiModel.interestHolderId?.toString() ?? '';
    payeeModel.acquisitionFilePersonId = apiModel.acquisitionFilePersonId?.toString() ?? '';
    payeeModel.rowVersion = apiModel.rowVersion ?? null;

    return payeeModel;
  }

  static defatultPayee(compensationRequisitionId: number): AcquisitionPayeeFormModel {
    let payeeModel = new AcquisitionPayeeFormModel(null, compensationRequisitionId);

    return payeeModel;
  }

  toApi(): Api_CompensationPayee {
    return {
      id: this._id,
      compensationRequisitionId: this._compensationRequisitionId ?? null,
      compensationRequisition: null,
      isPaymentInTrust: stringToBoolean(this.isPaymentInTrust),
      gstNumber: this.gstNumber,
      acquisitionOwnerId: this.acquisitionOwnerId === '' ? null : +this.acquisitionOwnerId,
      interestHolderId: null,
      interestHolder: null,
      motiSolicitor: null,
      motiSolicitorId: null,
      acquisitionFilePersonId: null,
      acquisitionOwner: null,
      isDisabled: stringToBoolean(this.isDisabled),
      rowVersion: this.rowVersion ?? null,
    };
  }
}

enum PayeeType {
  AcquisitionTeam = 'ACQUISITION_TEAM',
  OwnerRepresentative = 'OWNER_REPRESENTATIVE',
  OwnerSolicitor = 'OWNER_SOLICITOR',
  Owner = 'OWNER',
  InterestHolder = 'INTEREST_HOLDER',
}

export class PayeeOption {
  public readonly api_id: number;
  public readonly text: string;
  public readonly fullText: string;
  public readonly value: string;
  public readonly payeeType: PayeeType;

  private constructor(
    api_id: number,
    name: string,
    key: string,
    value: string,
    payeeType: PayeeType,
  ) {
    this.api_id = api_id;
    this.fullText = `${name}(${key})`;
    this.text = `${PayeeOption.truncateName(name)}(${key})`;
    this.value = value;
    this.payeeType = payeeType;
  }

  public static fromApi(payeeModels: Api_CompensationPayee[]): string {
    if (payeeModels.length === 1) {
      const apiModel = payeeModels[0];
      if (apiModel.acquisitionOwnerId) {
        return PayeeOption.generateKey(apiModel.acquisitionOwnerId, PayeeType.Owner);
      }

      /*if (apiModel.ownerRepresentativeId) {
        return PayeeOption.generateKey(
          apiModel.ownerRepresentativeId,
          PayeeType.OwnerRepresentative,
        );
      }

      if (apiModel.ownerSolicitorId) {
        return PayeeOption.generateKey(apiModel.ownerSolicitorId, PayeeType.OwnerSolicitor);
      }*/

      if (apiModel.motiSolicitorId) {
        return PayeeOption.generateKey(apiModel.motiSolicitorId, PayeeType.AcquisitionTeam);
      }

      if (apiModel.interestHolderId) {
        return PayeeOption.generateKey(apiModel.interestHolderId, PayeeType.InterestHolder);
      }
    }
    return '';
  }

  public static toApi(
    id: number | null,
    compensationRequisitionId: number | null,
    value: string,
    options: PayeeOption[],
  ): Api_CompensationPayee | null {
    if (isNullOrWhitespace(value)) {
      return null;
    }

    const payeeOption = options.find(x => x.value === value);

    if (payeeOption === undefined) {
      return null;
    }

    const payee: Api_CompensationPayee = {
      id: id,
      compensationRequisitionId: compensationRequisitionId || 0,
      isPaymentInTrust: null,
      gstNumber: null,
      acquisitionOwnerId: null,
      interestHolderId: null,
      motiSolicitorId: null,
      isDisabled: null,
      motiSolicitor: null,
      acquisitionOwner: null,
      compensationRequisition: null,
      interestHolder: null,
      acquisitionFilePersonId: null,
      rowVersion: null,
    };

    switch (payeeOption.payeeType) {
      case PayeeType.AcquisitionTeam:
        payee.motiSolicitorId = payeeOption.api_id;
        break;
      /*case PayeeType.OwnerRepresentative:
        payee.ownerRepresentativeId = payeeOption.api_id;
        break;
      case PayeeType.OwnerSolicitor:
        payee.ownerSolicitorId = payeeOption.api_id;
        break;*/
      case PayeeType.Owner:
        payee.acquisitionOwnerId = payeeOption.api_id;
        break;
      case PayeeType.InterestHolder:
        payee.interestHolderId = payeeOption.api_id;
        break;
      default:
        return null;
    }

    return payee;
  }

  private static truncateName(name: string): string {
    if (name.length > 50) {
      return name.slice(0, 50) + '...';
    } else {
      return name;
    }
  }

  public static createOwner(model: Api_AcquisitionFileOwner): PayeeOption {
    let name = model.isOrganization
      ? `${model.lastNameAndCorpName}, Inc. No. ${model.incorporationNumber} (OR Reg. No. ${model.registrationNumber})`
      : [model.givenName, model.lastNameAndCorpName, model.otherName].filter(x => !!x).join(' ');
    return new PayeeOption(
      model.id || 0,
      name,
      'Owner',
      PayeeOption.generateKey(model.id, PayeeType.Owner),
      PayeeType.Owner,
    );
  }

  /*public static createOwnerSolicitor(model: Api_AcquisitionFileSolicitor): PayeeOption {
    let name = '';
    if (model.person) {
      name = formatApiPersonNames(model.person);
    } else {
      name = model.organization?.name || '';
    }
    return new PayeeOption(
      model.id || 0,
      name,
      `Owner's Solicitor`,
      PayeeOption.generateKey(model.id, PayeeType.OwnerSolicitor),
      PayeeType.OwnerSolicitor,
    );
  }

  public static createOwnerRepresentative(model: Api_AcquisitionFileRepresentative): PayeeOption {
    let name = formatApiPersonNames(model.person);
    return new PayeeOption(
      model.id || 0,
      name,
      `Owner's Representative`,
      PayeeOption.generateKey(model.id, PayeeType.OwnerRepresentative),
      PayeeType.OwnerRepresentative,
    );
  }*/

  public static createTeamMember(model: Api_AcquisitionFilePerson): PayeeOption {
    let name = formatApiPersonNames(model.person);
    return new PayeeOption(
      model.id || 0,
      name,
      `${model.personProfileType?.description}`,
      PayeeOption.generateKey(model.id, PayeeType.AcquisitionTeam),
      PayeeType.AcquisitionTeam,
    );
  }

  public static createInterestHolder(model: Api_InterestHolder): PayeeOption {
    let name = '';
    if (model.person) {
      name = formatApiPersonNames(model.person);
    } else {
      name = model.organization?.name || '';
    }

    // The interest holders should always have a property
    const typeDescription =
      model.interestHolderProperties.length > 0
        ? model.interestHolderProperties[0].propertyInterestTypes[0]?.description
        : 'ERROR: Missing interest type';

    return new PayeeOption(
      model.interestHolderId || 0,
      name,
      `${typeDescription}`,
      PayeeOption.generateKey(model.interestHolderId, PayeeType.InterestHolder),
      PayeeType.InterestHolder,
    );
  }

  private static generateKey(modelId: number | null | undefined, payeeType: PayeeType) {
    return `${payeeType}-${modelId}`;
  }
}
