import { ApiGen_CodeTypes_SubfileInterestTypes } from '@/models/api/generated/ApiGen_CodeTypes_SubfileInterestTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { formatApiPersonNames } from '@/utils/personUtils';
import { exists, isValidId } from '@/utils/utils';

export class DetailAcquisitionFile {
  isSubFile: boolean;
  fileName?: string;
  legacyFileNumber?: string;
  assignedDate?: string;
  deliveryDate?: string;
  estimatedCompletionDate?: string;
  possessionDate?: string;
  appraisalStatusDescription: string;
  legalSurveyStatusDescription: string;
  expropriationRiskStatusDescription: string;
  acquisitionPhysFileStatusTypeDescription?: string;
  acquisitionTypeDescription?: string;
  subfileInterestTypeDescription: string | null = null;
  regionDescription?: string;
  acquisitionTeam: DetailAcquisitionFileTeam[] = [];

  static fromApi(model?: ApiGen_Concepts_AcquisitionFile): DetailAcquisitionFile {
    const detail = new DetailAcquisitionFile();

    detail.isSubFile =
      exists(model?.parentAcquisitionFileId) && isValidId(model?.parentAcquisitionFileId);
    detail.fileName = model?.fileName ?? undefined;
    detail.legacyFileNumber = model?.legacyFileNumber ?? undefined;
    detail.assignedDate = model?.assignedDate ?? undefined;
    detail.deliveryDate = model?.deliveryDate ?? undefined;
    detail.estimatedCompletionDate = model?.estimatedCompletionDate ?? undefined;
    detail.possessionDate = model?.possessionDate ?? undefined;

    detail.appraisalStatusDescription =
      model.acquisitionFileAppraisalStatusTypeCode?.description ?? '';
    detail.legalSurveyStatusDescription =
      model.acquisitionFileLegalSurveyStatusTypeCode?.description ?? '';
    detail.expropriationRiskStatusDescription =
      model.acquisitionFileExpropiationRiskStatusTypeCode?.description ?? '';
    detail.acquisitionPhysFileStatusTypeDescription =
      model?.acquisitionPhysFileStatusTypeCode?.description ?? undefined;
    detail.acquisitionTypeDescription = model?.acquisitionTypeCode?.description ?? undefined;

    if (detail.isSubFile) {
      if (model?.subfileInterestTypeCode?.id === ApiGen_CodeTypes_SubfileInterestTypes.OTHER) {
        detail.subfileInterestTypeDescription = `Other-${model.otherSubfileInterestType ?? ''}`;
      } else {
        detail.subfileInterestTypeDescription = model?.subfileInterestTypeCode?.description ?? '';
      }
    }
    detail.regionDescription = model?.regionCode?.description ?? undefined;
    detail.acquisitionTeam =
      model?.acquisitionTeam?.map(x => DetailAcquisitionFileTeam.fromApi(x)) || [];

    return detail;
  }
}

export class DetailAcquisitionFileTeam {
  personId?: number;
  organizationId?: number;
  primaryContactId?: number;
  teamName?: string;
  primaryContactName?: string;
  teamProfileTypeCodeDescription?: string;

  static fromApi(model: ApiGen_Concepts_AcquisitionFileTeam): DetailAcquisitionFileTeam {
    const teamDetail = new DetailAcquisitionFileTeam();
    teamDetail.personId = model?.person?.id;
    teamDetail.organizationId = model?.organization?.id;
    teamDetail.primaryContactId = model?.primaryContact?.id;
    teamDetail.teamProfileTypeCodeDescription = model?.teamProfileType?.description ?? undefined;

    teamDetail.teamName = model?.person
      ? formatApiPersonNames(model?.person)
      : model?.organization?.name ?? '';

    teamDetail.primaryContactName = model?.primaryContact
      ? formatApiPersonNames(model.primaryContact)
      : '';

    return teamDetail;
  }
}
