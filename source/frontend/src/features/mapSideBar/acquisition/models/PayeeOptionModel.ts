import { InterestHolderType } from '@/constants/interestHolderTypes';
import { ApiGen_CodeTypes_LeaseStakeholderTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStakeholderTypes';
import { ApiGen_CodeTypes_LessorTypes } from '@/models/api/generated/ApiGen_CodeTypes_LessorTypes';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqPayee } from '@/models/api/generated/ApiGen_Concepts_CompReqPayee';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists, isValidId, truncateName } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { PayeeType } from './PayeeTypeModel';

export class PayeeOption {
  public compensationRequisitionId: number | null;
  public payee_api_id: number | null;
  public rowVersion: number | null;
  public api_id: number;
  public text: string;
  public fullText: string;
  public value: string;
  public payeeType: PayeeType;

  private constructor(
    payee_api_id: number | null,
    api_id: number,
    compensationRequisitionId: number | null,
    name: string,
    key: string,
    value: string,
    payeeType: PayeeType,
  ) {
    this.payee_api_id = payee_api_id;
    this.api_id = api_id;
    this.compensationRequisitionId = compensationRequisitionId;
    this.fullText = `${name} (${key})`;
    this.text = `${truncateName(name, 50)} (${key})`;
    this.value = value;
    this.payeeType = payeeType;
    this.rowVersion = null;
  }

  public static getKeyFromPayee(apiModel: ApiGen_Concepts_CompReqPayee): string {
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

    return '';
  }

  public static getKeyFromLegacyPayee(compReqId: number): string {
    return PayeeOption.generateKey(compReqId, PayeeType.LegacyPayee);
  }

  public static fromApi(compReqPayee: ApiGen_Concepts_CompReqPayee): PayeeOption {
    let payee: PayeeOption = null;
    if (isValidId(compReqPayee.acquisitionOwnerId)) {
      payee = PayeeOption.createOwner(compReqPayee.acquisitionOwner, compReqPayee);
    } else if (isValidId(compReqPayee.acquisitionFileTeamId)) {
      payee = PayeeOption.createTeamMember(compReqPayee.acquisitionFileTeam, compReqPayee);
    } else if (isValidId(compReqPayee.interestHolderId)) {
      payee = PayeeOption.createInterestHolder(compReqPayee.interestHolder, compReqPayee);
    }
    if (exists(payee)) {
      payee.rowVersion = compReqPayee.rowVersion;
    }
    return payee;
  }

  public toApi(): ApiGen_Concepts_CompReqPayee | null {
    const compReqPayeeModel: ApiGen_Concepts_CompReqPayee = {
      ...getEmptyBaseAudit(),
      compensationRequisitionId: this.compensationRequisitionId,
      compensationRequisition: null,
      acquisitionOwnerId: null,
      acquisitionOwner: null,
      interestHolderId: null,
      interestHolder: null,
      acquisitionFileTeamId: null,
      acquisitionFileTeam: null,
      compReqPayeeId: this.payee_api_id,
      rowVersion: this.rowVersion,
    };

    switch (this.payeeType) {
      case PayeeType.AcquisitionTeam:
        compReqPayeeModel.acquisitionFileTeamId = this.api_id;
        break;
      case PayeeType.OwnerRepresentative:
      case PayeeType.OwnerSolicitor:
      case PayeeType.InterestHolder:
        compReqPayeeModel.interestHolderId = this.api_id;
        break;
      case PayeeType.Owner:
        compReqPayeeModel.acquisitionOwnerId = this.api_id;
        break;
    }

    if (
      isValidId(compReqPayeeModel.acquisitionFileTeamId) ||
      isValidId(compReqPayeeModel.interestHolderId) ||
      isValidId(compReqPayeeModel.acquisitionOwnerId)
    ) {
      return compReqPayeeModel;
    } else {
      return null;
    }
  }

  public static createOwner(
    model: ApiGen_Concepts_AcquisitionFileOwner,
    compReqPayee?: ApiGen_Concepts_CompReqPayee,
  ): PayeeOption {
    const name = model.isOrganization
      ? `${model.lastNameAndCorpName}, Inc. No. ${model.incorporationNumber} (OR Reg. No. ${model.registrationNumber})`
      : [model.givenName, model.lastNameAndCorpName, model.otherName].filter(x => !!x).join(' ');

    return new PayeeOption(
      compReqPayee?.compReqPayeeId,
      model?.id || 0,
      compReqPayee?.compensationRequisitionId,
      name,
      'Owner',
      PayeeOption.generateKey(model?.id, PayeeType.Owner),
      PayeeType.Owner,
    );
  }

  public static createOwnerSolicitor(
    model: ApiGen_Concepts_InterestHolder,
    compReqPayee?: ApiGen_Concepts_CompReqPayee,
  ): PayeeOption {
    let name = '';
    if (exists(model?.person)) {
      name = formatApiPersonNames(model.person);
    } else {
      name = model?.organization?.name || '';
    }
    return new PayeeOption(
      compReqPayee?.compReqPayeeId,
      model.interestHolderId || 0,
      compReqPayee?.compensationRequisitionId,
      name,
      `Owner's Solicitor`,
      PayeeOption.generateKey(model.interestHolderId, PayeeType.OwnerSolicitor),
      PayeeType.OwnerSolicitor,
    );
  }

  public static createOwnerRepresentative(
    model: ApiGen_Concepts_InterestHolder,
    compReqPayee?: ApiGen_Concepts_CompReqPayee,
  ): PayeeOption {
    const name = formatApiPersonNames(model.person);
    return new PayeeOption(
      compReqPayee?.compReqPayeeId,
      model.interestHolderId || 0,
      compReqPayee?.compensationRequisitionId,
      name,
      `Owner's Representative`,
      PayeeOption.generateKey(model.interestHolderId, PayeeType.OwnerRepresentative),
      PayeeType.OwnerRepresentative,
    );
  }

  public static createTeamMember(
    model: ApiGen_Concepts_AcquisitionFileTeam,
    compReqPayee?: ApiGen_Concepts_CompReqPayee,
  ): PayeeOption {
    let name = '';
    if (exists(model?.person)) {
      name = formatApiPersonNames(model.person);
    } else {
      name = model?.organization?.name || '';
    }
    return new PayeeOption(
      compReqPayee?.compReqPayeeId,
      model?.id || 0,
      compReqPayee?.compensationRequisitionId,
      name,
      model?.teamProfileType?.description ?? '',
      PayeeOption.generateKey(model?.id, PayeeType.AcquisitionTeam),
      PayeeType.AcquisitionTeam,
    );
  }

  public static createInterestHolder(
    model: ApiGen_Concepts_InterestHolder,
    compReqPayee?: ApiGen_Concepts_CompReqPayee,
  ): PayeeOption {
    if (model.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR) {
      return this.createOwnerSolicitor(model, compReqPayee);
    } else if (model.interestHolderType?.id === InterestHolderType.OWNER_REPRESENTATIVE) {
      return this.createOwnerRepresentative(model, compReqPayee);
    }

    let name = '';
    if (exists(model?.person)) {
      name = formatApiPersonNames(model.person);
    } else {
      name = model?.organization?.name || '';
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
      compReqPayee?.compReqPayeeId,
      model.interestHolderId || 0,
      compReqPayee?.compensationRequisitionId,
      name,
      `${typeDescription}`,
      PayeeOption.generateKey(model.interestHolderId, PayeeType.InterestHolder),
      PayeeType.InterestHolder,
    );
  }

  public static createLeaseStakeholder(
    compReqId: number,
    model: ApiGen_Concepts_LeaseStakeholder,
  ): PayeeOption {
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
      null,
      model.leaseStakeholderId || 0,
      compReqId,
      payeeName,
      payeeDescription,
      PayeeOption.generateKey(model.leaseStakeholderId, PayeeType.LeaseStakeholder),
      PayeeType.LeaseStakeholder,
    );
  }

  public static createLegacyPayee(
    model: ApiGen_Concepts_CompensationRequisition,
    compReqPayeeId: number | null,
    compReqId: number | null,
  ): PayeeOption {
    return new PayeeOption(
      compReqPayeeId,
      model?.id || 0,
      compReqId,
      model.legacyPayee || '',
      'Legacy free-text value',
      PayeeOption.generateKey(model?.id, PayeeType.LegacyPayee),
      PayeeType.LegacyPayee,
    );
  }

  public static generateKey(modelId: number | null | undefined, payeeType: PayeeType) {
    return `${payeeType}-${modelId}`;
  }
}
