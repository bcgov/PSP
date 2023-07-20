import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_CompensationPayee } from '@/models/api/CompensationPayee';
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
    payee: Api_CompensationPayee | null,
    financialActivities: Api_CompensationFinancial[] | [],
  ) {
    this.gst_number = payee?.gstNumber ? payee.gstNumber ?? '' : '';

    if (payee?.acquisitionOwner) {
      this.name = formatNames([
        payee.acquisitionOwner.givenName,
        payee.acquisitionOwner.lastNameAndCorpName,
      ]);
    } else if (payee?.interestHolder) {
      if (payee.interestHolder.person) {
        this.name = formatNames([
          payee.interestHolder.person.firstName,
          payee.interestHolder.person.surname,
        ]);
      } else {
        this.name = payee.interestHolder.organization?.name ?? '';
      }
    } else if (payee?.ownerRepresentative) {
      this.name = formatNames([
        payee.ownerRepresentative.person?.firstName,
        payee.ownerRepresentative.person?.surname,
      ]);
    } else if (payee?.ownerSolicitor) {
      if (payee.ownerSolicitor.person) {
        this.name = formatNames([
          payee.ownerSolicitor.person.firstName,
          payee.ownerSolicitor.person.surname,
        ]);
      } else {
        this.name = payee.ownerSolicitor.organization?.name ?? '';
      }
    } else if (payee?.motiSolicitor) {
      this.name = formatNames([payee.motiSolicitor?.firstName, payee.motiSolicitor?.surname]);
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
    this.payment_in_trust = !!payee?.isPaymentInTrust;
  }
}
