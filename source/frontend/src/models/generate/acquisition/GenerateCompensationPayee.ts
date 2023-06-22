import { Api_CompensationPayee } from 'models/api/CompensationPayee';
import { formatMoney } from 'utils';
import { formatNames } from 'utils/personUtils';

export class Api_GenerateCompensationPayee {
  name: string;
  pre_tax_amount: string;
  tax_amount: string;
  total_amount: string;
  gst_number: string;

  constructor(payee: Api_CompensationPayee | null) {
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

    this.pre_tax_amount = payee?.cheques?.length ? formatMoney(payee.cheques[0].pretaxAmout) : '';
    this.tax_amount = payee?.cheques?.length ? formatMoney(payee.cheques[0].taxAmount) : '';
    this.total_amount = payee?.cheques?.length ? formatMoney(payee.cheques[0].totalAmount) : '';
    this.gst_number = payee?.cheques?.length ? payee.cheques[0]?.gstNumber ?? '' : '';
  }
}
