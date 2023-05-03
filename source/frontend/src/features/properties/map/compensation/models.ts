import { Api_Compensation } from 'models/api/Compensation';
import { booleanToString, stringToBoolean, stringToNull } from 'utils/formUtils';

export class CompensationRequisitionFormModel {
  id: number | null = null;
  acquisitionFileId: number;
  isDraft: string = '';
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

  static fromApi(apiModel: Api_Compensation): CompensationRequisitionFormModel {
    const compensation = new CompensationRequisitionFormModel(
      apiModel.id,
      apiModel.acquisitionFileId,
    );

    compensation.isDraft = booleanToString(apiModel.isDraft);
    compensation.fiscalYear = apiModel.fiscalYear || '';
    compensation.agreementDateTime = apiModel.agreementDateTime || '';
    compensation.expropriationNoticeServedDateTime =
      apiModel.expropriationNoticeServedDateTime || '';
    compensation.expropriationVestingDateTime = apiModel.expropriationVestingDateTime || '';
    compensation.generationDatetTime = apiModel.generationDatetTime || '';
    compensation.specialInstruction = apiModel.specialInstruction || '';
    compensation.detailedRemarks = apiModel.detailedRemarks || '';
    compensation.isDisabled = booleanToString(apiModel.isDisabled);

    return compensation;
  }

  public toApi(): Api_Compensation {
    return {
      id: this.id,
      acquisitionFileId: this.acquisitionFileId,
      isDraft: stringToBoolean(this.isDraft),
      fiscalYear: stringToNull(this.fiscalYear),
      agreementDateTime: stringToNull(this.agreementDateTime),
      expropriationNoticeServedDateTime: stringToNull(this.expropriationNoticeServedDateTime),
      expropriationVestingDateTime: stringToNull(this.expropriationVestingDateTime),
      generationDatetTime: stringToNull(this.generationDatetTime),
      specialInstruction: stringToNull(this.specialInstruction),
      detailedRemarks: stringToNull(this.detailedRemarks),
      isDisabled: stringToBoolean(this.isDisabled),
      financials: [],
      rowVersion: this.rowVersion ?? undefined,
    };
  }
}
