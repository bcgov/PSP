import { InterestHolderType } from '@/constants/interestHolderTypes';
import { Api_AcquisitionFileOwner, Api_AcquisitionFilePerson } from '@/models/api/AcquisitionFile';
import { Api_CompensationPayee } from '@/models/api/CompensationPayee';
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
    this.fullText = `${name} (${key})`;
    this.text = `${PayeeOption.truncateName(name)} (${key})`;
    this.value = value;
    this.payeeType = payeeType;
  }

  public static fromApi(payeeModels: Api_CompensationPayee[]): string {
    if (payeeModels.length === 1) {
      const apiModel = payeeModels[0];
      if (apiModel.acquisitionOwnerId) {
        return PayeeOption.generateKey(apiModel.acquisitionOwnerId, PayeeType.Owner);
      }

      if (apiModel.motiSolicitorId) {
        return PayeeOption.generateKey(apiModel.motiSolicitorId, PayeeType.AcquisitionTeam);
      }

      if (apiModel.interestHolderId) {
        if (
          apiModel.interestHolder?.interestHolderType?.id ===
          InterestHolderType.OWNER_REPRESENTATIVE
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
      case PayeeType.OwnerRepresentative:
        payee.interestHolderId = payeeOption.api_id;
        break;
      case PayeeType.OwnerSolicitor:
        payee.interestHolderId = payeeOption.api_id;
        break;
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
