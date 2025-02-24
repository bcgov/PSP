import { ApiGen_CodeTypes_LessorTypes } from '@/models/api/generated/ApiGen_CodeTypes_LessorTypes';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqPayee } from '@/models/api/generated/ApiGen_Concepts_CompReqPayee';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
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
    compReqPayees: ApiGen_Concepts_CompReqPayee[],
    financialActivities: ApiGen_Concepts_CompensationFinancial[] | [],
  ) {
    this.gst_number = compensation?.gstNumber ?? '';
    const names: string[] = [];

    compReqPayees.forEach((payee: ApiGen_Concepts_CompReqPayee) => {
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

    if (compensation?.compReqLeaseStakeholders?.length > 0) {
      const stakeHolder: ApiGen_Concepts_LeaseStakeholder =
        compensation?.compReqLeaseStakeholders[0].leaseStakeholder;
      if (stakeHolder.lessorType.id === ApiGen_CodeTypes_LessorTypes.ORG) {
        names.push(stakeHolder.organization?.name ?? '');
      } else if (stakeHolder.lessorType.id === ApiGen_CodeTypes_LessorTypes.PER) {
        names.push(formatApiPersonNames(stakeHolder.person));
      }
    }

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
