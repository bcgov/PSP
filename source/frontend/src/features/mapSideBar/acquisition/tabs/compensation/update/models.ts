import { InterestHolderType } from '@/constants/interestHolderTypes';
import { Api_AcquisitionFileOwner, Api_AcquisitionFilePerson } from '@/models/api/AcquisitionFile';
import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_FinancialCode } from '@/models/api/FinancialCode';
import { Api_InterestHolder } from '@/models/api/InterestHolder';
import { isNullOrWhitespace } from '@/utils';
import { booleanToString, stringToBoolean, stringToUndefined, toTypeCode } from '@/utils/formUtils';
import { formatApiPersonNames } from '@/utils/personUtils';

export class CompensationRequisitionFormModel {
  id: number | null;
  acquisitionFileId: number;
  status: string = '';
  fiscalYear: string = '';
  stob: string = '';
  yearlyFinancial: Api_FinancialCode | null = null;
  serviceLine: string = '';
  chartOfAccounts: Api_FinancialCode | null = null;
  responsibilityCentre: string = '';
  Responsibility: Api_FinancialCode | null = null;
  readonly finalizedDate: string;
  agreementDateTime: string = '';
  expropriationNoticeServedDateTime: string = '';
  expropriationVestingDateTime: string = '';
  generationDatetTime: string = '';
  specialInstruction: string = '';
  detailedRemarks: string = '';
  financials: FinancialActivityFormModel[] = [];
  payee: AcquisitionPayeeFormModel;
  isDisabled: string = '';
  rowVersion: number | null = null;

  constructor(id: number | null, acquisitionFileId: number = 0, finalizedDate: string) {
    this.id = id;
    this.acquisitionFileId = acquisitionFileId;
    this.payee = new AcquisitionPayeeFormModel();
    this.finalizedDate = finalizedDate;
  }

  toApi(payeeOptions: PayeeOption[]): Api_CompensationRequisition {
    let modelWithPayeeInformation = this.payee.toApi(payeeOptions);

    return {
      ...modelWithPayeeInformation,
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
      finalizedDate: stringToUndefined(this.finalizedDate),
      expropriationNoticeServedDate: stringToUndefined(this.expropriationNoticeServedDateTime),
      expropriationVestingDate: stringToUndefined(this.expropriationVestingDateTime),
      generationDate: stringToUndefined(this.generationDatetTime),
      specialInstruction: stringToUndefined(this.specialInstruction),
      detailedRemarks: stringToUndefined(this.detailedRemarks),
      isDisabled: stringToBoolean(this.isDisabled),
      financials: this.financials
        .filter(x => !x.isEmpty())
        .map<Api_CompensationFinancial>(x => x.toApi()),
      rowVersion: this.rowVersion ?? undefined,
    };
  }

  static fromApi(apiModel: Api_CompensationRequisition): CompensationRequisitionFormModel {
    const compensation = new CompensationRequisitionFormModel(
      apiModel.id,
      apiModel.acquisitionFileId,
      apiModel.finalizedDate ?? '',
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

    compensation.payee = AcquisitionPayeeFormModel.fromApi(apiModel);
    compensation.payee.pretaxAmount = payeePretaxAmount;
    compensation.payee.taxAmount = payeeTaxAmount;
    compensation.payee.totalAmount = payeeTotalAmount;

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
  payeeKey: string = '';
  gstNumber: string = '';
  isPaymentInTrust: boolean = false;

  pretaxAmount: number = 0;
  taxAmount: number = 0;
  totalAmount: number = 0;

  static fromApi(apiModel: Api_CompensationRequisition): AcquisitionPayeeFormModel {
    const payeeModel = new AcquisitionPayeeFormModel();

    payeeModel.payeeKey = PayeeOption.fromApi(apiModel);
    payeeModel.isPaymentInTrust = apiModel.isPaymentInTrust ?? false;
    payeeModel.gstNumber = apiModel.gstNumber ?? '';

    return payeeModel;
  }

  toApi(payeeOptions: PayeeOption[]): Api_CompensationRequisition {
    let modelWithPayeeInformation: Api_CompensationRequisition = PayeeOption.toApi(
      this.payeeKey,
      payeeOptions,
    );

    return {
      ...modelWithPayeeInformation,
      isPaymentInTrust: stringToBoolean(this.isPaymentInTrust),
      gstNumber: this.gstNumber,
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

  public static fromApi(apiModel: Api_CompensationRequisition): string {
    if (apiModel.acquisitionOwnerId) {
      return PayeeOption.generateKey(apiModel.acquisitionOwnerId, PayeeType.Owner);
    }

    if (apiModel.acquisitionFilePersonId) {
      return PayeeOption.generateKey(apiModel.acquisitionFilePersonId, PayeeType.AcquisitionTeam);
    }

    if (apiModel.interestHolderId) {
      if (
        apiModel.interestHolder?.interestHolderType?.id === InterestHolderType.OWNER_REPRESENTATIVE
      ) {
        return PayeeOption.generateKey(apiModel.interestHolderId, PayeeType.OwnerRepresentative);
      } else if (
        apiModel.interestHolder?.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR
      ) {
        return PayeeOption.generateKey(apiModel.interestHolderId, PayeeType.OwnerSolicitor);
      } else {
        return PayeeOption.generateKey(apiModel.interestHolderId, PayeeType.InterestHolder);
      }
    }

    return '';
  }

  public static toApi(payeeKey: string, options: PayeeOption[]): Api_CompensationRequisition {
    const compensationModel: Api_CompensationRequisition = {
      isPaymentInTrust: null,
      gstNumber: null,
      acquisitionOwnerId: null,
      interestHolderId: null,
      acquisitionFilePerson: null,
      isDisabled: null,
      acquisitionOwner: null,
      interestHolder: null,
      acquisitionFilePersonId: null,
      id: null,
      acquisitionFileId: 0,
      acquisitionFile: null,
      isDraft: null,
      fiscalYear: null,
      yearlyFinancialId: null,
      yearlyFinancial: null,
      chartOfAccountsId: null,
      chartOfAccounts: null,
      responsibilityId: null,
      responsibility: null,
      agreementDate: null,
      expropriationNoticeServedDate: null,
      expropriationVestingDate: null,
      generationDate: null,
      financials: [],
      legacyPayee: null,
      finalizedDate: null,
      specialInstruction: null,
      detailedRemarks: null,
    };

    if (isNullOrWhitespace(payeeKey)) {
      return compensationModel;
    }

    const payeeOption = options.find(x => x.value === payeeKey);

    if (payeeOption === undefined) {
      return compensationModel;
    }

    switch (payeeOption.payeeType) {
      case PayeeType.AcquisitionTeam:
        compensationModel.acquisitionFilePersonId = payeeOption.api_id;
        break;
      case PayeeType.OwnerRepresentative:
        compensationModel.interestHolderId = payeeOption.api_id;
        break;
      case PayeeType.OwnerSolicitor:
        compensationModel.interestHolderId = payeeOption.api_id;
        break;
      case PayeeType.Owner:
        compensationModel.acquisitionOwnerId = payeeOption.api_id;
        break;
      case PayeeType.InterestHolder:
        compensationModel.interestHolderId = payeeOption.api_id;
        break;
    }

    return compensationModel;
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

  public static createOwnerSolicitor(model: Api_InterestHolder): PayeeOption {
    let name = '';
    if (model.person) {
      name = formatApiPersonNames(model.person);
    } else {
      name = model.organization?.name || '';
    }
    return new PayeeOption(
      model.interestHolderId || 0,
      name,
      `Owner's Solicitor`,
      PayeeOption.generateKey(model.interestHolderId, PayeeType.OwnerSolicitor),
      PayeeType.OwnerSolicitor,
    );
  }

  public static createOwnerRepresentative(model: Api_InterestHolder): PayeeOption {
    let name = formatApiPersonNames(model.person);
    return new PayeeOption(
      model.interestHolderId || 0,
      name,
      `Owner's Representative`,
      PayeeOption.generateKey(model.interestHolderId, PayeeType.OwnerRepresentative),
      PayeeType.OwnerRepresentative,
    );
  }

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
    if (model.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR) {
      return this.createOwnerSolicitor(model);
    } else if (model.interestHolderType?.id === InterestHolderType.OWNER_REPRESENTATIVE) {
      return this.createOwnerRepresentative(model);
    }

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
