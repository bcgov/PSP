import {
  Api_DispositionFile,
  Api_DispositionFileProperty,
  Api_DispositionFileTeam,
} from '@/models/api/DispositionFile';
import { Api_DispositionFilter } from '@/models/api/DispositionFilter';

export class DispositionSearchResultModel {
  id: number | null = null;
  fileNumber: string = '';
  fileName: string = '';
  referenceNumber: string = '';
  region: string = '';
  dispositionType: string = '';
  dispositionStatus: string = '';
  physicalFileStatus: string = '';
  dispositionTeam: Api_DispositionFileTeam[] = [];
  fileProperties?: Api_DispositionFileProperty[] = [];

  static fromApi(base: Api_DispositionFile): DispositionSearchResultModel {
    var newModel = new DispositionSearchResultModel();
    newModel.id = base.id ?? null;
    newModel.fileNumber = base.fileNumber ?? '';
    newModel.fileName = base.fileName ?? '';
    newModel.referenceNumber = base.referenceNumber ?? '';
    newModel.region = base.regionCode?.description ?? '';
    newModel.dispositionType = base.dispositionTypeCode?.description ?? '';
    newModel.dispositionStatus = base.dispositionStatusTypeCode?.description ?? '';
    newModel.physicalFileStatus = base.physicalFileStatusTypeCode?.description ?? '';
    newModel.dispositionTeam = base.dispositionTeam || [];
    newModel.fileProperties = base.fileProperties || [];

    return newModel;
  }
}

export class DispositionFilterModel {
  searchBy: string = 'address';
  pin: string = '';
  pid: string = '';
  address: string = '';
  fileNameOrNumberOrReference: string = '';
  physicalFileStatusCode: string = '';
  dispositionStatusCode: string = '';
  dispositionTypeCode: string = '';

  toApi(): Api_DispositionFilter {
    return {
      searchBy: this.searchBy,
      pin: this.pin,
      pid: this.pid,
      address: this.address,
      fileNameOrNumberOrReference: this.fileNameOrNumberOrReference,
      physicalFileStatusCode: this.physicalFileStatusCode,
      dispositionStatusCode: this.dispositionStatusCode,
      dispositionTypeCode: this.dispositionTypeCode,
      teamMemberPersonId: null, // TODO
      teamMemberOrganizationId: null, // TODO
    };
  }

  static fromApi(base: Api_DispositionFilter): DispositionFilterModel {
    const newModel = new DispositionFilterModel();
    newModel.searchBy = base.searchBy ?? 'address';
    newModel.pin = base.pin ?? '';
    newModel.pid = base.pid ?? '';
    newModel.address = base.address ?? '';
    newModel.fileNameOrNumberOrReference = base.fileNameOrNumberOrReference ?? '';
    newModel.physicalFileStatusCode = base.physicalFileStatusCode ?? '';
    newModel.dispositionStatusCode = base.dispositionStatusCode ?? '';
    newModel.dispositionTypeCode = base.dispositionTypeCode ?? '';

    // TODO: filter by disposition team members

    return newModel;
  }
}
