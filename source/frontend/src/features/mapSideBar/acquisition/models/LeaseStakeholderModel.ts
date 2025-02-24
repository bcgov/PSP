import { ApiGen_CodeTypes_LeaseStakeholderTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStakeholderTypes';
import { ApiGen_CodeTypes_LessorTypes } from '@/models/api/generated/ApiGen_CodeTypes_LessorTypes';
import { ApiGen_Concepts_CompReqLeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_CompReqLeaseStakeholder';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { truncateName } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

export class CompReqLeaseStakeholderModel {
  public compensationRequisitionId: number | null;
  public stakeholderId: number | null;
  public apiId: number;
  public rowVersion: number | null;

  public text: string;
  public fullText: string;

  public static fromApi(
    apiCompReqPayee: ApiGen_Concepts_CompReqLeaseStakeholder,
  ): CompReqLeaseStakeholderModel {
    const compReqPayeeModel = CompReqLeaseStakeholderModel.createFromStakeholder(
      apiCompReqPayee?.leaseStakeholder,
    );

    compReqPayeeModel.apiId = apiCompReqPayee?.compReqLeaseStakeholderId;
    compReqPayeeModel.compensationRequisitionId = apiCompReqPayee?.compensationRequisitionId;

    compReqPayeeModel.rowVersion = apiCompReqPayee?.rowVersion;

    return compReqPayeeModel;
  }

  public static createFromStakeholder(
    apiLeaseStakeholder: ApiGen_Concepts_LeaseStakeholder,
  ): CompReqLeaseStakeholderModel {
    const compReqPayeeModel = new CompReqLeaseStakeholderModel();
    let payeeName = '';
    let payeeDescription = '';

    switch (apiLeaseStakeholder?.lessorType.id) {
      case ApiGen_CodeTypes_LessorTypes.ORG:
        payeeName = `${apiLeaseStakeholder?.organization?.name ?? ''}, Inc. No. ${
          apiLeaseStakeholder?.organization?.incorporationNumber ?? ''
        }`;
        break;
      case ApiGen_CodeTypes_LessorTypes.PER:
        payeeName = formatApiPersonNames(apiLeaseStakeholder.person);
        break;
      default:
        payeeName = ApiGen_CodeTypes_LessorTypes.UNK;
    }

    switch (apiLeaseStakeholder?.stakeholderTypeCode.id) {
      case ApiGen_CodeTypes_LeaseStakeholderTypes.OWNER:
        payeeDescription = 'Owner';
        break;
      case ApiGen_CodeTypes_LeaseStakeholderTypes.OWNREP:
        payeeDescription = `Owner's Representative`;
        break;
      default:
        payeeDescription = apiLeaseStakeholder?.stakeholderTypeCode.description;
        break;
    }
    compReqPayeeModel.fullText = `${payeeName} (${payeeDescription})`;
    compReqPayeeModel.text = `${truncateName(payeeName, 50)} (${payeeDescription})`;
    compReqPayeeModel.stakeholderId = apiLeaseStakeholder?.leaseStakeholderId || null;

    return compReqPayeeModel;
  }

  public toApi(): ApiGen_Concepts_CompReqLeaseStakeholder {
    const compReqPayeeModel: ApiGen_Concepts_CompReqLeaseStakeholder = {
      ...getEmptyBaseAudit(),
      compReqLeaseStakeholderId: this.apiId,
      compensationRequisitionId: this.compensationRequisitionId,
      leaseStakeholderId: this.stakeholderId,
      rowVersion: this.rowVersion,
      leaseStakeholder: null,
    };
    return compReqPayeeModel;
  }
}
