import { InterestHolderType } from '@/constants/interestHolderTypes';
import { ApiGen_CodeTypes_LeaseStakeholderTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStakeholderTypes';
import { ApiGen_CodeTypes_LessorTypes } from '@/models/api/generated/ApiGen_CodeTypes_LessorTypes';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_CompReqLeasePayee } from '@/models/api/generated/ApiGen_Concepts_CompReqLeasePayee';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists, isValidId, isValidString, truncateName } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { ApiGen_Concepts_CompReqAcqPayee } from './../../../../models/api/generated/ApiGen_Concepts_CompReqAcqPayee';
import { PayeeType } from './PayeeTypeModel';

export class PayeeOption {
  public compensationRequisitionId: number | null;
  public payee_api_id: number | null;
  public rowVersion: number | null;
  public api_id: number;
  public originalPayeeName: string;
  public text: string;
  public fullText: string;
  public value: string;
  public payeeType: PayeeType;

  private constructor(
    payee_api_id: number | null,
    api_id: number | null,
    compensationRequisitionId: number | null,
    payeeName: string,
    key: string,
    value: string,
    payeeType: PayeeType,
  ) {
    this.payee_api_id = payee_api_id;
    this.api_id = api_id;
    this.compensationRequisitionId = compensationRequisitionId;
    this.originalPayeeName = payeeName;
    this.fullText = `${payeeName} (${key})`;
    this.text = `${truncateName(payeeName, 50)} (${key})`;
    this.value = value;
    this.payeeType = payeeType;
    this.rowVersion = null;
  }

  public static getKeyFromPayee(apiModel: ApiGen_Concepts_CompReqAcqPayee): string {
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

    if (isValidString(apiModel.legacyPayee)) {
      return PayeeOption.generateKey(apiModel.compReqAcqPayeeId, PayeeType.LegacyPayee);
    }

    return '';
  }

  public static fromApiAcq(compReqPayee: ApiGen_Concepts_CompReqAcqPayee): PayeeOption {
    let payee: PayeeOption = null;
    if (isValidId(compReqPayee.acquisitionOwnerId)) {
      payee = PayeeOption.createOwner(compReqPayee.acquisitionOwner, compReqPayee);
    } else if (isValidId(compReqPayee.acquisitionFileTeamId)) {
      payee = PayeeOption.createTeamMember(compReqPayee.acquisitionFileTeam, compReqPayee);
    } else if (isValidId(compReqPayee.interestHolderId)) {
      payee = PayeeOption.createInterestHolder(compReqPayee.interestHolder, compReqPayee);
    } else if (isValidString(compReqPayee.legacyPayee)) {
      payee = PayeeOption.createLegacyPayee(compReqPayee.legacyPayee, compReqPayee);
    }

    if (exists(payee)) {
      payee.rowVersion = compReqPayee.rowVersion;
    }
    return payee;
  }

  public static fromApiLease(compReqLeasePayee: ApiGen_Concepts_CompReqLeasePayee): PayeeOption {
    let payee: PayeeOption = null;
    if (isValidId(compReqLeasePayee.leaseStakeholderId)) {
      payee = PayeeOption.createLeaseStakeholder(
        compReqLeasePayee.compensationRequisitionId,
        compReqLeasePayee.leaseStakeholder,
        compReqLeasePayee,
      );
    }

    if (exists(payee)) {
      payee.rowVersion = compReqLeasePayee.rowVersion;
    }
    return payee;
  }

  public toAcquisitionApi(): ApiGen_Concepts_CompReqAcqPayee | null {
    const compReqPayeeModel: ApiGen_Concepts_CompReqAcqPayee = {
      ...getEmptyBaseAudit(),
      compensationRequisitionId: this.compensationRequisitionId,
      compensationRequisition: null,
      acquisitionOwnerId: null,
      acquisitionOwner: null,
      interestHolderId: null,
      interestHolder: null,
      acquisitionFileTeamId: null,
      acquisitionFileTeam: null,
      legacyPayee: null,
      compReqAcqPayeeId: this.payee_api_id,
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
      case PayeeType.LegacyPayee:
        compReqPayeeModel.legacyPayee = isValidString(this.originalPayeeName)
          ? this.originalPayeeName
          : null;
        break;
    }

    if (
      isValidId(compReqPayeeModel.acquisitionFileTeamId) ||
      isValidId(compReqPayeeModel.interestHolderId) ||
      isValidId(compReqPayeeModel.acquisitionOwnerId) ||
      isValidString(compReqPayeeModel.legacyPayee)
    ) {
      return compReqPayeeModel;
    } else {
      return null;
    }
  }

  public toLeaseApi(): ApiGen_Concepts_CompReqLeasePayee | null {
    const compReqLeasePayeeModel: ApiGen_Concepts_CompReqLeasePayee = {
      ...getEmptyBaseAudit(),
      compensationRequisitionId: this.compensationRequisitionId,
      leaseStakeholderId: null,
      leaseStakeholder: null,
      compReqLeasePayeeId: null,
      rowVersion: this.rowVersion,
    };

    switch (this.payeeType) {
      case PayeeType.LeaseStakeholder:
        compReqLeasePayeeModel.leaseStakeholderId = this.api_id;
        break;
    }

    if (isValidId(compReqLeasePayeeModel.leaseStakeholderId)) {
      return compReqLeasePayeeModel;
    } else {
      return null;
    }
  }

  public static createOwner(
    model: ApiGen_Concepts_AcquisitionFileOwner,
    compReqPayee?: ApiGen_Concepts_CompReqAcqPayee,
  ): PayeeOption {
    const name = model.isOrganization
      ? `${model.lastNameAndCorpName}, Inc. No. ${model.incorporationNumber} (OR Reg. No. ${model.registrationNumber})`
      : [model.givenName, model.lastNameAndCorpName, model.otherName].filter(x => !!x).join(' ');

    return new PayeeOption(
      compReqPayee?.compReqAcqPayeeId,
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
    compReqPayee?: ApiGen_Concepts_CompReqAcqPayee,
  ): PayeeOption {
    let name = '';
    if (exists(model?.person)) {
      name = formatApiPersonNames(model.person);
    } else {
      name = model?.organization?.name || '';
    }
    return new PayeeOption(
      compReqPayee?.compReqAcqPayeeId,
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
    compReqPayee?: ApiGen_Concepts_CompReqAcqPayee,
  ): PayeeOption {
    const name = formatApiPersonNames(model.person);
    return new PayeeOption(
      compReqPayee?.compReqAcqPayeeId,
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
    compReqPayee?: ApiGen_Concepts_CompReqAcqPayee,
  ): PayeeOption {
    let name = '';
    if (exists(model?.person)) {
      name = formatApiPersonNames(model.person);
    } else {
      name = model?.organization?.name || '';
    }
    return new PayeeOption(
      compReqPayee?.compReqAcqPayeeId,
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
    compReqPayee?: ApiGen_Concepts_CompReqAcqPayee,
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
      compReqPayee?.compReqAcqPayeeId,
      model.interestHolderId || 0,
      compReqPayee?.compensationRequisitionId,
      name,
      `${typeDescription}`,
      PayeeOption.generateKey(model.interestHolderId, PayeeType.InterestHolder),
      PayeeType.InterestHolder,
    );
  }

  public static createLeaseStakeholder(
    compensationRequisitionId: number,
    model: ApiGen_Concepts_LeaseStakeholder,
    compReqLeasePayee?: ApiGen_Concepts_CompReqLeasePayee,
  ): PayeeOption {
    let payeeName: string;
    let payeeDescription: string;

    switch (model?.lessorType?.id) {
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

    switch (model?.stakeholderTypeCode?.id) {
      case ApiGen_CodeTypes_LeaseStakeholderTypes.OWNER:
        payeeDescription = 'Owner';
        break;
      case ApiGen_CodeTypes_LeaseStakeholderTypes.OWNREP:
        payeeDescription = `Owner's Representative`;
        break;
      default:
        payeeDescription = model?.stakeholderTypeCode?.description;
        break;
    }

    return new PayeeOption(
      compReqLeasePayee?.compReqLeasePayeeId,
      model?.leaseStakeholderId || 0,
      compensationRequisitionId,
      payeeName,
      payeeDescription,
      PayeeOption.generateKey(model?.leaseStakeholderId, PayeeType.LeaseStakeholder),
      PayeeType.LeaseStakeholder,
    );
  }

  public static createLegacyPayee(
    legacyPayee: string,
    compReqPayee?: ApiGen_Concepts_CompReqAcqPayee,
  ): PayeeOption {
    return new PayeeOption(
      compReqPayee?.compReqAcqPayeeId,
      null,
      compReqPayee?.compensationRequisitionId,
      legacyPayee || '',
      'Legacy free-text value',
      PayeeOption.generateKey(compReqPayee?.compReqAcqPayeeId ?? 0, PayeeType.LegacyPayee),
      PayeeType.LegacyPayee,
    );
  }

  public static generateKey(modelId: number | null | undefined, payeeType: PayeeType) {
    return `${payeeType}-${modelId}`;
  }
}
