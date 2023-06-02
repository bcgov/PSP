import {
  Api_AcquisitionFileOwner,
  Api_AcquisitionFilePerson,
  Api_AcquisitionFileRepresentative,
  Api_AcquisitionFileSolicitor,
} from 'models/api/AcquisitionFile';
import { Api_Compensation, Api_CompensationPayee } from 'models/api/Compensation';
import { isNullOrWhitespace } from 'utils';
import { booleanToString, stringToBoolean, stringToNull } from 'utils/formUtils';
import { formatApiPersonNames } from 'utils/personUtils';

export class CompensationRequisitionFormModel {
  id: number | null = null;
  acquisitionFileId: number;
  status: string = '' || 'draft' || 'final';
  fiscalYear: string = '';
  agreementDateTime: string = '';
  expropriationNoticeServedDateTime: string = '';
  expropriationVestingDateTime: string = '';
  generationDatetTime: string = '';
  specialInstruction: string = '';
  detailedRemarks: string = '';
  isDisabled: string = '';
  rowVersion: number | null = null;
  payee: string = '';

  constructor(id: number | null, acquisitionFileId: number = 0) {
    this.id = id;
    this.acquisitionFileId = acquisitionFileId;
  }

  toApi(payeeOptions: PayeeOption[]): Api_Compensation {
    const apiPayee: Api_CompensationPayee | null = PayeeOption.toApi(
      this.id,
      this.payee,
      payeeOptions,
    );

    return {
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
      isDraft: this.status === 'draft' ? true : false,
      fiscalYear: stringToNull(this.fiscalYear),
      agreementDate: stringToNull(this.agreementDateTime),
      expropriationNoticeServedDate: stringToNull(this.expropriationNoticeServedDateTime),
      expropriationVestingDate: stringToNull(this.expropriationVestingDateTime),
      generationDate: stringToNull(this.generationDatetTime),
      specialInstruction: stringToNull(this.specialInstruction),
      detailedRemarks: stringToNull(this.detailedRemarks),
      isDisabled: stringToBoolean(this.isDisabled),
      financials: [],
      payees: apiPayee === null ? [] : [apiPayee],
      rowVersion: this.rowVersion ?? undefined,
    };
  }

  static fromApi(apiModel: Api_Compensation): CompensationRequisitionFormModel {
    const compensation = new CompensationRequisitionFormModel(
      apiModel.id,
      apiModel.acquisitionFileId,
    );

    compensation.status =
      apiModel.isDraft === true ? 'draft' : apiModel.isDraft === null ? '' : 'final';
    compensation.fiscalYear = apiModel.fiscalYear || '';
    compensation.agreementDateTime = apiModel.agreementDate || '';
    compensation.expropriationNoticeServedDateTime = apiModel.expropriationNoticeServedDate || '';
    compensation.expropriationVestingDateTime = apiModel.expropriationVestingDate || '';
    compensation.generationDatetTime = apiModel.generationDate || '';
    compensation.specialInstruction = apiModel.specialInstruction || '';
    compensation.detailedRemarks = apiModel.detailedRemarks || '';
    compensation.payee = PayeeOption.fromApi(apiModel.payees);
    compensation.isDisabled = booleanToString(apiModel.isDisabled);
    compensation.rowVersion = apiModel.rowVersion ?? null;

    return compensation;
  }
}

enum PayeeType {
  AcquisitionTeam = 'ACQUISITION_TEAM',
  OwnerRepresentative = 'OWNER_REPRESENTATIVE',
  OwnerSolicitor = 'OWNER_SOLICITOR',
  Owner = 'OWNER',
}

export class PayeeOption {
  public readonly api_id: number;
  public readonly text: string;
  public readonly value: string;
  public readonly payeeType: PayeeType;

  private constructor(api_id: number, text: string, value: string, payeeType: PayeeType) {
    this.api_id = api_id;
    this.text = text;
    this.value = value;
    this.payeeType = payeeType;
  }

  public static fromApi(payeeModels: Api_CompensationPayee[]): string {
    if (payeeModels.length === 1) {
      const apiModel = payeeModels[0];
      if (apiModel.acquisitionOwnerId) {
        return PayeeOption.generateKey(apiModel.acquisitionOwnerId, PayeeType.Owner);
      }
      if (apiModel.ownerRepresentativeId) {
        return PayeeOption.generateKey(
          apiModel.ownerRepresentativeId,
          PayeeType.OwnerRepresentative,
        );
      }
      if (apiModel.motiSolicitorId) {
        return PayeeOption.generateKey(apiModel.motiSolicitorId, PayeeType.AcquisitionTeam);
      }
      if (apiModel.ownerSolicitorId) {
        return PayeeOption.generateKey(apiModel.ownerSolicitorId, PayeeType.OwnerSolicitor);
      }
    }
    return '';
  }

  public static toApi(
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
      id: 0,
      compensationRequisitionId: compensationRequisitionId || 0,
      acquisitionOwnerId: null,
      interestHolderId: null,
      ownerRepresentativeId: null,
      ownerSolicitorId: null,
      motiSolicitorId: null,
      isDisabled: null,
      motiSolicitor: null,
      acquisitionOwner: null,
      compensationRequisition: null,
      interestHolder: null,
      ownerRepresentative: null,
      ownerSolicitor: null,
    };

    switch (payeeOption.payeeType) {
      case PayeeType.AcquisitionTeam:
        payee.motiSolicitorId = payeeOption.api_id;
        break;
      case PayeeType.OwnerRepresentative:
        payee.ownerRepresentativeId = payeeOption.api_id;
        break;
      case PayeeType.OwnerSolicitor:
        payee.ownerSolicitorId = payeeOption.api_id;
        break;
      case PayeeType.Owner:
        payee.acquisitionOwnerId = payeeOption.api_id;
        break;
      default:
        return null;
    }

    return payee;
  }

  public static createOwner(model: Api_AcquisitionFileOwner): PayeeOption {
    let name = model.isOrganization
      ? `${model.lastNameAndCorpName}, Inc. No. ${model.incorporationNumber} (OR Reg. No. ${model.registrationNumber})`
      : [model.givenName, model.lastNameAndCorpName, model.otherName].filter(x => !!x).join(' ');
    return new PayeeOption(
      model.id || 0,
      `${name} (Owner)`,
      PayeeOption.generateKey(model.id, PayeeType.Owner),
      PayeeType.Owner,
    );
  }

  public static createOwnerSolicitor(model: Api_AcquisitionFileSolicitor): PayeeOption {
    let name = formatApiPersonNames(model.person);
    return new PayeeOption(
      model.id || 0,
      `${name} (Owner's Solicitor)`,
      PayeeOption.generateKey(model.id, PayeeType.OwnerSolicitor),
      PayeeType.OwnerSolicitor,
    );
  }

  public static createOwnerRepresentative(model: Api_AcquisitionFileRepresentative): PayeeOption {
    let name = formatApiPersonNames(model.person);
    return new PayeeOption(
      model.id || 0,
      `${name} (Owner's Representative)`,
      PayeeOption.generateKey(model.id, PayeeType.OwnerRepresentative),
      PayeeType.OwnerRepresentative,
    );
  }

  public static createTeamMember(model: Api_AcquisitionFilePerson): PayeeOption {
    let name = formatApiPersonNames(model.person);
    return new PayeeOption(
      model.id || 0,
      `${name} (${model.personProfileType?.description})`,
      PayeeOption.generateKey(model.id, PayeeType.AcquisitionTeam),
      PayeeType.AcquisitionTeam,
    );
  }

  private static generateKey(modelId: number | null | undefined, payeeType: PayeeType) {
    return `${payeeType}-${modelId}`;
  }
}
