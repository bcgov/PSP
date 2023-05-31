import { Api_CompensationPayee } from 'models/api/Compensation';

import { Api_GenerateCompensationPayeeCheque } from './GenerateCompensationPayeeCheque';

export class Api_GenerateCompensationPayee {
  payee_name: string;
  payee_cheques: Api_GenerateCompensationPayeeCheque[];

  constructor(payee: Api_CompensationPayee) {
    this.payee_name =
      (payee.owner.isOrganization
        ? payee.owner.lastNameAndCorpName
        : payee.owner.givenName
        ? payee.owner.givenName + ' ' + payee.owner.lastNameAndCorpName
        : payee.owner.lastNameAndCorpName) ?? '';
    this.payee_cheques =
      payee.payeeCheques.map(cheque => new Api_GenerateCompensationPayeeCheque(cheque)) ?? [];
  }
}
