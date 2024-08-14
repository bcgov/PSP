import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { stringToBoolean, stringToNull } from '@/utils/formUtils';

import { PayeeOption } from '../../acquisition/models/PayeeOptionModel';

export class CompensationPayeeFormModel {
  payeeKey = '';
  gstNumber = '';
  legacyPayee = '';
  isPaymentInTrust = false;

  pretaxAmount = 0;
  taxAmount = 0;
  totalAmount = 0;

  constructor(readonly compensationRequisitionId: number) {
    this.compensationRequisitionId = compensationRequisitionId;
  }

  static fromApi(apiModel: ApiGen_Concepts_CompensationRequisition): CompensationPayeeFormModel {
    const payeeModel = new CompensationPayeeFormModel(apiModel.id);

    payeeModel.payeeKey = PayeeOption.fromApi(apiModel);
    payeeModel.isPaymentInTrust = apiModel.isPaymentInTrust ?? false;
    payeeModel.gstNumber = apiModel.gstNumber ?? '';
    payeeModel.legacyPayee = apiModel.legacyPayee ?? '';

    return payeeModel;
  }

  toApi(payeeOptions: PayeeOption[]): ApiGen_Concepts_CompensationRequisition {
    const modelWithPayeeInformation: ApiGen_Concepts_CompensationRequisition = PayeeOption.toApi(
      this.compensationRequisitionId,
      this.payeeKey,
      payeeOptions,
    );

    return {
      ...modelWithPayeeInformation,
      legacyPayee: stringToNull(this.legacyPayee),
      isPaymentInTrust: stringToBoolean(this.isPaymentInTrust),
      gstNumber: this.gstNumber,
    };
  }
}
