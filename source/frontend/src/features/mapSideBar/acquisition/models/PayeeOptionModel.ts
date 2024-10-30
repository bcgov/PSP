import { InterestHolderType } from '@/constants/interestHolderTypes';
import { ApiGen_CodeTypes_LeaseStakeholderTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStakeholderTypes';
import { ApiGen_CodeTypes_LessorTypes } from '@/models/api/generated/ApiGen_CodeTypes_LessorTypes';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqLeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_CompReqLeaseStakeholder';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists, isNullOrWhitespace } from '@/utils';
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

  public static fromApi(apiModel: ApiGen_Concepts_CompensationRequisition): string {
    if (apiModel.acquisitionOwnerId) {
      return PayeeOption.generateKey(apiModel.acquisitionOwnerId, PayeeType.Owner);
    }

    if (apiModel.acquisitionFileTeamId) {
      return PayeeOption.generateKey(apiModel.acquisitionFileTeamId, PayeeType.AcquisitionTeam);
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

    if (apiModel.compReqLeaseStakeholder?.length > 0) {
      return PayeeOption.generateKey(
        apiModel.compReqLeaseStakeholder[0].leaseStakeholderId,
        PayeeType.LeaseStakeholder,
      );
    }

    if (apiModel.legacyPayee) {
      return PayeeOption.generateKey(apiModel.id, PayeeType.LegacyPayee);
    }

    return '';
  }

  public static toApi(
    compensationRequisitionId: number,
    payeeKey: string,
    options: PayeeOption[],
  ): ApiGen_Concepts_CompensationRequisition {
    const compensationModel: ApiGen_Concepts_CompensationRequisition = {
      acquisitionFileId: null,
      leaseId: null,
      isPaymentInTrust: null,
      gstNumber: null,
      acquisitionOwnerId: null,
      alternateProject: null,
      alternateProjectId: null,
      interestHolderId: null,
      acquisitionFileTeam: null,
      acquisitionOwner: null,
      interestHolder: null,
      acquisitionFileTeamId: null,
      id: null,
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
      compReqLeaseStakeholder: [],
      compReqAcquisitionProperties: [],
      compReqLeaseProperties: [],
      legacyPayee: null,
      finalizedDate: null,
      specialInstruction: null,
      detailedRemarks: null,
      ...getEmptyBaseAudit(),
    };

    if (isNullOrWhitespace(payeeKey)) {
      return compensationModel;
    }

    const payeeOption = options?.find(x => x.value === payeeKey) ?? null;

    if (payeeOption === null) {
      return compensationModel;
    }

    switch (payeeOption.payeeType) {
      case PayeeType.AcquisitionTeam:
        compensationModel.acquisitionFileTeamId = payeeOption.api_id;
        break;
      case PayeeType.OwnerRepresentative:
      case PayeeType.OwnerSolicitor:
      case PayeeType.InterestHolder:
        compensationModel.interestHolderId = payeeOption.api_id;
        break;
      case PayeeType.Owner:
        compensationModel.acquisitionOwnerId = payeeOption.api_id;
        break;
      case PayeeType.LeaseStakeholder:
        {
          const leaseStakeHolderPayee: ApiGen_Concepts_CompReqLeaseStakeholder = {
            compReqLeaseStakeholderId: null,
            compensationRequisitionId: compensationRequisitionId,
            leaseStakeholderId: payeeOption.api_id,
            leaseStakeholder: null,
          } as ApiGen_Concepts_CompReqLeaseStakeholder;

          compensationModel.compReqLeaseStakeholder = [leaseStakeHolderPayee];
        }
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

  public static createOwner(model: ApiGen_Concepts_AcquisitionFileOwner): PayeeOption {
    const name = model.isOrganization
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

  public static createOwnerSolicitor(model: ApiGen_Concepts_InterestHolder): PayeeOption {
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

  public static createOwnerRepresentative(model: ApiGen_Concepts_InterestHolder): PayeeOption {
    const name = formatApiPersonNames(model.person);
    return new PayeeOption(
      model.interestHolderId || 0,
      name,
      `Owner's Representative`,
      PayeeOption.generateKey(model.interestHolderId, PayeeType.OwnerRepresentative),
      PayeeType.OwnerRepresentative,
    );
  }

  public static createTeamMember(model: ApiGen_Concepts_AcquisitionFileTeam): PayeeOption {
    let name = '';
    if (model.person) {
      name = formatApiPersonNames(model.person);
    } else {
      name = model.organization?.name || '';
    }
    return new PayeeOption(
      model.id || 0,
      name,
      model.teamProfileType?.description ?? '',
      PayeeOption.generateKey(model.id, PayeeType.AcquisitionTeam),
      PayeeType.AcquisitionTeam,
    );
  }

  public static createInterestHolder(model: ApiGen_Concepts_InterestHolder): PayeeOption {
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
      exists(model.interestHolderProperties) && model.interestHolderProperties.length > 0
        ? exists(model.interestHolderProperties[0].propertyInterestTypes) &&
          model.interestHolderProperties[0].propertyInterestTypes.length > 0
          ? model.interestHolderProperties[0].propertyInterestTypes[0].description
          : 'ERROR: Missing interest type'
        : 'ERROR: Missing interest holder';

    return new PayeeOption(
      model.interestHolderId || 0,
      name,
      `${typeDescription}`,
      PayeeOption.generateKey(model.interestHolderId, PayeeType.InterestHolder),
      PayeeType.InterestHolder,
    );
  }

  public static createLeaseStakeholder(model: ApiGen_Concepts_LeaseStakeholder): PayeeOption {
    let payeeName: string;
    let payeeDescription: string;

    switch (model.lessorType.id) {
      case ApiGen_CodeTypes_LessorTypes.ORG:
        payeeName = `${model?.organization?.name ?? ''}, Inc. No. ${
          model?.organization?.incorporationNumber ?? ''
        }`;
        break;
      case ApiGen_CodeTypes_LessorTypes.PER:
        payeeName = formatApiPersonNames(model.person);
        break;
      default:
        payeeName = ApiGen_CodeTypes_LessorTypes.UNK;
    }

    switch (model.stakeholderTypeCode.id) {
      case ApiGen_CodeTypes_LeaseStakeholderTypes.OWNER:
        payeeDescription = 'Owner';
        break;
      case ApiGen_CodeTypes_LeaseStakeholderTypes.OWNREP:
        payeeDescription = `Owner's Representative`;
        break;
      default:
        payeeDescription = model.stakeholderTypeCode.description;
        break;
    }

    return new PayeeOption(
      model.leaseStakeholderId || 0,
      payeeName,
      payeeDescription,
      PayeeOption.generateKey(model.leaseStakeholderId, PayeeType.LeaseStakeholder),
      PayeeType.LeaseStakeholder,
    );
  }

  public static createLegacyPayee(model: ApiGen_Concepts_CompensationRequisition): PayeeOption {
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
