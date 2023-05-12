import { Api_Compensation } from 'models/api/Compensation';
import { booleanToString, stringToBoolean, stringToNull } from 'utils/formUtils';

export class CompensationRequisitionFormModel {
  id: number | null = null;
  acquisitionFileId: number;
  status: string = '' || 'draft' || 'final';
  fiscalYear: string = '';
  agreementDateTime: string = '';
  expropriationNoticeServedDateTime: string = '';
  expropriationVestingDateTime: string = '';
  generationDatetTime: string = '';
  specialInstruction: string = '';
  detailedRemarks: string = '';
  isDisabled: string = '';
  rowVersion: number | null = null;

  constructor(id: number | null, acquisitionFileId: number = 0) {
    this.id = id;
    this.acquisitionFileId = acquisitionFileId;
  }

  toApi(): Api_Compensation {
    return {
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
      isDraft: this.status === 'draft' ? true : false,
      fiscalYear: stringToNull(this.fiscalYear),
      agreementDate: stringToNull(this.agreementDateTime),
      expropriationNoticeServedDate: stringToNull(this.expropriationNoticeServedDateTime),
      expropriationVestingDate: stringToNull(this.expropriationVestingDateTime),
      generationDate: stringToNull(this.generationDatetTime),
      specialInstruction: stringToNull(this.specialInstruction),
      detailedRemarks: stringToNull(this.detailedRemarks),
      isDisabled: stringToBoolean(this.isDisabled),
      financials: [],
      rowVersion: this.rowVersion ?? undefined,
    };
  }

  static fromApi(apiModel: Api_Compensation): CompensationRequisitionFormModel {
    const compensation = new CompensationRequisitionFormModel(
      apiModel.id,
      apiModel.acquisitionFileId,
    );

    compensation.status =
      apiModel.isDraft === true ? 'draft' : apiModel.isDraft === null ? '' : 'final';
    compensation.fiscalYear = apiModel.fiscalYear || '';
    compensation.agreementDateTime = apiModel.agreementDate || '';
    compensation.expropriationNoticeServedDateTime = apiModel.expropriationNoticeServedDate || '';
    compensation.expropriationVestingDateTime = apiModel.expropriationVestingDate || '';
    compensation.generationDatetTime = apiModel.generationDate || '';
    compensation.specialInstruction = apiModel.specialInstruction || '';
    compensation.detailedRemarks = apiModel.detailedRemarks || '';
    compensation.isDisabled = booleanToString(apiModel.isDisabled);
    compensation.rowVersion = apiModel.rowVersion ?? null;

    return compensation;
  }
}
