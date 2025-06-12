import { Api_ManagementActivityFilter } from '@/models/api/ManagementActivityFilter';

export class ManagementActivityFilterModel {
  searchBy = 'address';
  pin = '';
  pid = '';
  address = '';
  fileNameOrNumberOrReference = '';
  activityStatusCode = '';
  activityTypeCode = '';
  projectNameOrNumber = '';

  toApi(): Api_ManagementActivityFilter {
    return {
      searchBy: this.searchBy,
      pin: this.pin,
      pid: this.pid,
      address: this.address,
      fileNameOrNumberOrReference: this.fileNameOrNumberOrReference,
      activityStatusCode: this.activityStatusCode,
      activityTypeCode: this.activityTypeCode,
      projectNameOrNumber: this.projectNameOrNumber,
    };
  }

  static fromApi(base: Api_ManagementActivityFilter): ManagementActivityFilterModel {
    const newModel = new ManagementActivityFilterModel();
    newModel.searchBy = base.searchBy ?? 'address';
    newModel.pin = base.pin ?? '';
    newModel.pid = base.pid ?? '';
    newModel.address = base.address ?? '';
    newModel.fileNameOrNumberOrReference = base.fileNameOrNumberOrReference ?? '';
    newModel.activityStatusCode = base.activityStatusCode ?? '';
    newModel.activityTypeCode = base.activityTypeCode ?? '';
    newModel.projectNameOrNumber = base.projectNameOrNumber ?? '';

    return newModel;
  }
}
