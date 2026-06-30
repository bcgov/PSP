import { MultiSelectOption } from '@/interfaces/MultiSelectOption';
import { Api_ManagementActivityFilter } from '@/models/api/ManagementActivityFilter';

export class ManagementActivityFilterModel {
  searchBy = 'address';
  pin = '';
  pid = '';
  regionCodes = [];
  address = '';
  fileNameOrNumberOrReference = '';
  activityStatusCode = '';
  activityTypeCode = '';
  projectNameOrNumber = '';
  managementFileStatusCode = '';
  managementFilePurposeCode = '';

  constructor(initialRegions: MultiSelectOption[] = []) {
    this.regionCodes = initialRegions;
  }

  toApi(): Api_ManagementActivityFilter {
    return {
      searchBy: this.searchBy,
      pin: this.pin,
      pid: this.pid,
      regionCodes: this.regionCodes?.map(x => x.id) ?? [],
      address: this.address,
      fileNameOrNumberOrReference: this.fileNameOrNumberOrReference,
      activityStatusCode: this.activityStatusCode,
      activityTypeCode: this.activityTypeCode,
      projectNameOrNumber: this.projectNameOrNumber,
      managementFileStatusCode: this.managementFileStatusCode,
      managementFilePurposeCode: this.managementFilePurposeCode,
    };
  }

  static fromApi(
    base: Api_ManagementActivityFilter,
    userRegions: MultiSelectOption[],
  ): ManagementActivityFilterModel {
    const newModel = new ManagementActivityFilterModel();
    newModel.searchBy = base.searchBy ?? 'address';
    newModel.pin = base.pin ?? '';
    newModel.pid = base.pid ?? '';
    newModel.regionCodes = userRegions ?? [];
    newModel.address = base.address ?? '';
    newModel.fileNameOrNumberOrReference = base.fileNameOrNumberOrReference ?? '';
    newModel.activityStatusCode = base.activityStatusCode ?? '';
    newModel.activityTypeCode = base.activityTypeCode ?? '';
    newModel.projectNameOrNumber = base.projectNameOrNumber ?? '';
    newModel.managementFileStatusCode = base.managementFileStatusCode ?? '';
    newModel.managementFilePurposeCode = base.managementFilePurposeCode ?? '';

    return newModel;
  }
}
