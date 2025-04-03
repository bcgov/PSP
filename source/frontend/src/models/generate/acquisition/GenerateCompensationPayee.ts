import { ApiGen_CodeTypes_LessorTypes } from '@/models/api/generated/ApiGen_CodeTypes_LessorTypes';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqAcqPayee } from '@/models/api/generated/ApiGen_Concepts_CompReqAcqPayee';
import { ApiGen_Concepts_CompReqLeasePayee } from '@/models/api/generated/ApiGen_Concepts_CompReqLeasePayee';
import { exists, formatMoney, isValidString } from '@/utils';
import { formatApiPersonNames, formatNames } from '@/utils/personUtils';

export class Api_GenerateCompensationPayee {
  name: string;
  pre_tax_amount: string;
  tax_amount: string;
  total_amount: string;
  payment_in_trust: boolean;
  gst_number: string;

  constructor(
    compensation: ApiGen_Concepts_CompensationRequisition | null,
    compReqAcqPayees: ApiGen_Concepts_CompReqAcqPayee[],
    compReqLeasePayees: ApiGen_Concepts_CompReqLeasePayee[],
    financialActivities: ApiGen_Concepts_CompensationFinancial[] | [],
  ) {
    this.gst_number = compensation?.gstNumber ?? '';
    const names: string[] = [];

    compReqAcqPayees.forEach((payee: ApiGen_Concepts_CompReqAcqPayee) => {
      if (exists(payee?.acquisitionOwner)) {
        names.push(
          formatNames([
            payee?.acquisitionOwner.givenName,
            payee?.acquisitionOwner.lastNameAndCorpName,
          ]),
        );
      } else if (exists(payee?.interestHolder)) {
        if (exists(payee?.interestHolder.person)) {
          names.push(
            formatNames([
              payee?.interestHolder.person.firstName,
              payee?.interestHolder.person.surname,
            ]),
          );
        } else {
          names.push(payee?.interestHolder.organization?.name ?? '');
        }
      } else if (exists(payee?.acquisitionFileTeam)) {
        if (exists(payee?.acquisitionFileTeam.person)) {
          names.push(
            formatNames([
              payee?.acquisitionFileTeam.person.firstName,
              payee?.acquisitionFileTeam.person.surname,
            ]),
          );
        } else {
          names.push(payee?.acquisitionFileTeam?.organization?.name ?? '');
        }
      } else if (isValidString(payee?.legacyPayee)) {
        names.push(payee?.legacyPayee ?? '');
      }
    });

    compReqLeasePayees.forEach((payee: ApiGen_Concepts_CompReqLeasePayee) => {
      if (exists(payee?.leaseStakeholder)) {
        if (payee?.leaseStakeholder?.lessorType?.id === ApiGen_CodeTypes_LessorTypes.ORG) {
          names.push(payee?.leaseStakeholder.organization?.name ?? '');
        } else if (payee?.leaseStakeholder?.lessorType?.id === ApiGen_CodeTypes_LessorTypes.PER) {
          names.push(formatApiPersonNames(payee?.leaseStakeholder.person));
        }
      }
    });

    this.name = names.join(', ');

    const preTaxAmount: number = financialActivities
      .map(f => f.pretaxAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    const taxAmount: number = financialActivities
      .map(f => f.taxAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    const totalAmount: number = financialActivities
      .map(f => f.totalAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    this.pre_tax_amount = formatMoney(preTaxAmount) ?? '';
    this.tax_amount = formatMoney(taxAmount) ?? '';
    this.total_amount = formatMoney(totalAmount) ?? '';
    this.payment_in_trust = !!compensation?.isPaymentInTrust;
  }
}
