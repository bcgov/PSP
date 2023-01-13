import { IAutocompletePrediction } from 'interfaces';
import {
  Api_AcquisitionFile,
  Api_AcquisitionFilePerson,
  Api_AcquisitionFileProperty,
} from 'models/api/AcquisitionFile';
import { fromTypeCode, toTypeCode } from 'utils/formUtils';

import { PropertyForm } from '../../shared/models';
import { AcquisitionTeamFormModel, WithAcquisitionTeam } from '../common/models';

export class AcquisitionForm implements WithAcquisitionTeam {
  id?: number;
  fileName?: string = '';
  assignedDate?: string;
  deliveryDate?: string;
  rowVersion?: number;
  // Code Tables
  acquisitionFileStatusType?: string;
  acquisitionPhysFileStatusType?: string;
  acquisitionType?: string;
  // MOTI region
  region?: string;
  properties: PropertyForm[] = [];
  team: AcquisitionTeamFormModel[] = [];

  project?: IAutocompletePrediction;
  fundingTypeCode?: string;
  fundingTypeOtherDescription: string = '';

  toApi(): Api_AcquisitionFile {
    return {
      id: this.id,
      fileName: this.fileName,
      rowVersion: this.rowVersion,
      assignedDate: this.assignedDate,
      deliveryDate: this.deliveryDate,
      fileStatusTypeCode: toTypeCode(this.acquisitionFileStatusType),
      acquisitionPhysFileStatusTypeCode: toTypeCode(this.acquisitionPhysFileStatusType),
      acquisitionTypeCode: toTypeCode(this.acquisitionType),
      regionCode: toTypeCode(Number(this.region)),
      project: this.project?.id !== undefined ? { id: this.project?.id } : undefined,
      fundingTypeCode: toTypeCode(this.fundingTypeCode),
      fundingOther: this.fundingTypeOtherDescription,
      // ACQ file properties
      fileProperties: this.properties.map<Api_AcquisitionFileProperty>(ap => {
        return {
          id: ap.id,
          propertyName: ap.name,
          isDisabled: ap.isDisabled,
          displayOrder: ap.displayOrder,
          rowVersion: ap.rowVersion,
          property: ap.toApi(),
          acquisitionFile: { id: this.id },
        };
      }),
      acquisitionTeam: this.team
        .filter(x => !!x.contact && !!x.contactTypeCode)
        .map<Api_AcquisitionFilePerson>(x => x.toApi()),
    };
  }

  static fromApi(model: Api_AcquisitionFile): AcquisitionForm {
    const newForm = new AcquisitionForm();
    newForm.id = model.id;
    newForm.fileName = model.fileName || '';
    newForm.rowVersion = model.rowVersion;
    newForm.assignedDate = model.assignedDate;
    newForm.deliveryDate = model.deliveryDate;
    newForm.acquisitionFileStatusType = fromTypeCode(model.fileStatusTypeCode);
    newForm.acquisitionPhysFileStatusType = fromTypeCode(model.acquisitionPhysFileStatusTypeCode);
    newForm.acquisitionType = fromTypeCode(model.acquisitionTypeCode);
    newForm.region = fromTypeCode(model.regionCode)?.toString();
    // ACQ file properties
    newForm.properties = model.fileProperties?.map(x => PropertyForm.fromApi(x)) || [];
    newForm.fundingTypeCode = model.fundingTypeCode?.id;
    newForm.fundingTypeOtherDescription = model.fundingOther || '';
    newForm.project =
      model.project !== undefined
        ? { id: model.project?.id || 0, text: model.project?.description || '' }
        : undefined;

    return newForm;
  }
}
