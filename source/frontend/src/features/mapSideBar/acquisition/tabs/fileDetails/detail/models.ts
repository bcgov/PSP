import { Api_AcquisitionFile, Api_AcquisitionFilePerson } from '@/models/api/AcquisitionFile';
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
  acquisitionTeam: DetailAcquisitionFilePerson[] = [];

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
      model?.acquisitionTeam?.map(x => DetailAcquisitionFilePerson.fromApi(x)) || [];

    return detail;
  }
}

export class DetailAcquisitionFilePerson {
  personId?: number;
  personName?: string;
  personProfileTypeCodeDescription?: string;

  static fromApi(model: Api_AcquisitionFilePerson): DetailAcquisitionFilePerson {
    const personDetail = new DetailAcquisitionFilePerson();
    personDetail.personId = model?.person?.id;
    personDetail.personName = formatApiPersonNames(model?.person);
    personDetail.personProfileTypeCodeDescription = model?.personProfileType?.description;

    return personDetail;
  }
}
