import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { formatApiPersonNames } from '@/utils/personUtils';

export class DetailAcquisitionFile {
  fileName?: string;
  legacyFileNumber?: string;
  assignedDate?: string;
  deliveryDate?: string;
  acquisitionPhysFileStatusTypeDescription?: string;
  acquisitionTypeDescription?: string;
  regionDescription?: string;
  acquisitionTeam: DetailAcquisitionFileTeam[] = [];

  static fromApi(model?: ApiGen_Concepts_AcquisitionFile): DetailAcquisitionFile {
    const detail = new DetailAcquisitionFile();
    detail.fileName = model?.fileName ?? undefined;
    detail.legacyFileNumber = model?.legacyFileNumber ?? undefined;
    detail.assignedDate = model?.assignedDate ?? undefined;
    detail.deliveryDate = model?.deliveryDate ?? undefined;
    detail.acquisitionPhysFileStatusTypeDescription =
      model?.acquisitionPhysFileStatusTypeCode?.description ?? undefined;
    detail.acquisitionTypeDescription = model?.acquisitionTypeCode?.description ?? undefined;
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
