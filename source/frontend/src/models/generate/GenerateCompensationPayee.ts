import { Api_CompensationPayee } from 'models/api/CompensationPayee';

import { Api_GenerateCompensationPayeeCheque } from './GenerateCompensationPayeeCheque';

export class Api_GenerateCompensationPayee {
  name: string;
  cheques: Api_GenerateCompensationPayeeCheque[];

  constructor(payee: Api_CompensationPayee) {
    this.name = ''; //Todo
    this.cheques = []; //Todo
  }
}
