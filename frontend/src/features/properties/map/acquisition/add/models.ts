import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { fromTypeCode, toTypeCode } from 'utils/formUtils';

export class AcquisitionForm {
  id?: number;
  name?: string = '';
  assignedDate?: string;
  deliveryDate?: string;
  rowVersion?: number;
  // Code Tables
  acquisitionFileStatusTypeId?: string;
  acquisitionPhysFileStatusTypeId?: string;
  acquisitionTypeId?: string;
  // MOTI region
  regionId?: number;

  toApi(): Api_AcquisitionFile {
    return {
      id: this.id,
      name: this.name,
      rowVersion: this.rowVersion,
      assignedDate: this.assignedDate,
      deliveryDate: this.deliveryDate,
      acquisitionFileStatusTypeCode: toTypeCode(this.acquisitionFileStatusTypeId),
      acquisitionPhysFileStatusTypeCode: toTypeCode(this.acquisitionPhysFileStatusTypeId),
      acquisitionTypeCode: toTypeCode(this.acquisitionTypeId),
      regionCode: toTypeCode(this.regionId),
    };
  }

  static fromApi(model: Api_AcquisitionFile): AcquisitionForm {
    const newForm = new AcquisitionForm();
    newForm.id = model.id;
    newForm.name = model.name || '';
    newForm.rowVersion = model.rowVersion;
    newForm.assignedDate = model.assignedDate;
    newForm.deliveryDate = model.deliveryDate;
    newForm.acquisitionFileStatusTypeId = fromTypeCode(model.acquisitionFileStatusTypeCode);
    newForm.acquisitionPhysFileStatusTypeId = fromTypeCode(model.acquisitionPhysFileStatusTypeCode);
    newForm.acquisitionTypeId = fromTypeCode(model.acquisitionTypeCode);
    newForm.regionId = fromTypeCode(model.regionCode);

    return newForm;
  }
}
