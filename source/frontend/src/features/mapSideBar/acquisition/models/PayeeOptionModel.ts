import { InterestHolderType } from '@/constants/interestHolderTypes';
import { Api_AcquisitionFileOwner, Api_AcquisitionFilePerson } from '@/models/api/AcquisitionFile';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_InterestHolder } from '@/models/api/InterestHolder';
import { isNullOrWhitespace } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { PayeeType } from './PayeeTypeModel';

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

    if (!!apiModel.legacyPayee) {
      return PayeeOption.generateKey(apiModel.id, PayeeType.LegacyPayee);
    }

    return '';
  }

  public static toApi(payeeKey: string, options: PayeeOption[]): Api_CompensationRequisition {
    const compensationModel: Api_CompensationRequisition = {
      isPaymentInTrust: null,
      gstNumber: null,
      acquisitionOwnerId: null,
      alternateProject: null,
      alternateProjectId: null,
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
      advancedPaymentServedDate: null,
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
      case PayeeType.OwnerSolicitor:
      case PayeeType.InterestHolder:
        compensationModel.interestHolderId = payeeOption.api_id;
        break;
      case PayeeType.Owner:
        compensationModel.acquisitionOwnerId = payeeOption.api_id;
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

  public static createLegacyPayee(model: Api_CompensationRequisition): PayeeOption {
    return new PayeeOption(
      model.id || 0,
      model.legacyPayee || '',
      'Legacy free-text value',
      PayeeOption.generateKey(model.id, PayeeType.LegacyPayee),
      PayeeType.LegacyPayee,
    );
  }

  public static generateKey(modelId: number | null | undefined, payeeType: PayeeType) {
    return `${payeeType}-${modelId}`;
  }
}
