import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { formatMoney } from '@/utils';
import { formatNames } from '@/utils/personUtils';

export class Api_GenerateCompensationPayee {
  name: string;
  pre_tax_amount: string;
  tax_amount: string;
  total_amount: string;
  payment_in_trust: boolean;
  gst_number: string;

  constructor(
    compensation: Api_CompensationRequisition | null,
    financialActivities: Api_CompensationFinancial[] | [],
  ) {
    this.gst_number = compensation?.gstNumber ? compensation.gstNumber ?? '' : '';

    if (compensation?.acquisitionOwner) {
      this.name = formatNames([
        compensation.acquisitionOwner.givenName,
        compensation.acquisitionOwner.lastNameAndCorpName,
      ]);
    } else if (compensation?.interestHolder) {
      if (compensation.interestHolder.person) {
        this.name = formatNames([
          compensation.interestHolder.person.firstName,
          compensation.interestHolder.person.surname,
        ]);
      } else {
        this.name = compensation.interestHolder.organization?.name ?? '';
      }
    } else if (compensation?.acquisitionFilePerson) {
      this.name = formatNames([
        compensation.acquisitionFilePerson?.person?.firstName,
        compensation.acquisitionFilePerson?.person?.surname,
      ]);
    } else if (compensation?.legacyPayee) {
      this.name = compensation.legacyPayee ?? '';
    } else {
      this.name = '';
    }

    const preTaxAmount = financialActivities
      .map(f => f.pretaxAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    const taxAmount = financialActivities
      .map(f => f.taxAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    const totalAmount = financialActivities
      .map(f => f.totalAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    this.pre_tax_amount = formatMoney(preTaxAmount) ?? '';
    this.tax_amount = formatMoney(taxAmount) ?? '';
    this.total_amount = formatMoney(totalAmount) ?? '';
    this.payment_in_trust = !!compensation?.isPaymentInTrust;
  }
}
