import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
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
    compensation: ApiGen_Concepts_CompensationRequisition | null,
    financialActivities: ApiGen_Concepts_CompensationFinancial[] | [],
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
    } else if (compensation?.acquisitionFileTeam) {
      if (compensation.acquisitionFileTeam.person) {
        this.name = formatNames([
          compensation.acquisitionFileTeam.person.firstName,
          compensation.acquisitionFileTeam.person.surname,
        ]);
      } else {
        this.name = compensation.acquisitionFileTeam.organization?.name ?? '';
      }
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
