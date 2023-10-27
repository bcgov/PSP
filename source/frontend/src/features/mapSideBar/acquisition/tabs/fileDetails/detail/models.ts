import { Api_AcquisitionFile, Api_AcquisitionFileTeam } from '@/models/api/AcquisitionFile';
import { formatApiPersonNames } from '@/utils/personUtils';

export class DetailAcquisitionFile {
  fileName?: string;
  legacyFileNumber?: string;
  assignedDate?: string;
  deliveryDate?: string;
  completionDate?: string;
  acquisitionPhysFileStatusTypeDescription?: string;
  acquisitionTypeDescription?: string;
  regionDescription?: string;
  acquisitionTeam: DetailAcquisitionFileTeam[] = [];

  static fromApi(model?: Api_AcquisitionFile): DetailAcquisitionFile {
    const detail = new DetailAcquisitionFile();
    detail.fileName = model?.fileName;
    detail.legacyFileNumber = model?.legacyFileNumber;
    detail.assignedDate = model?.assignedDate;
    detail.deliveryDate = model?.deliveryDate;
    detail.completionDate = model?.completionDate;
    detail.acquisitionPhysFileStatusTypeDescription =
      model?.acquisitionPhysFileStatusTypeCode?.description;
    detail.acquisitionTypeDescription = model?.acquisitionTypeCode?.description;
    detail.regionDescription = model?.regionCode?.description;
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

  static fromApi(model: Api_AcquisitionFileTeam): DetailAcquisitionFileTeam {
    const teamDetail = new DetailAcquisitionFileTeam();
    teamDetail.personId = model?.person?.id;
    teamDetail.organizationId = model?.organization?.id;
    teamDetail.primaryContactId = model?.primaryContact?.id;
    teamDetail.teamProfileTypeCodeDescription = model?.teamProfileType?.description;

    teamDetail.teamName = model?.person
      ? formatApiPersonNames(model?.person)
      : model?.organization?.name ?? '';

    teamDetail.primaryContactName = model?.primaryContact
      ? formatApiPersonNames(model.primaryContact)
      : '';

    return teamDetail;
  }
}
