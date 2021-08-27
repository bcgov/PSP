import { IPropertyEvaluation } from 'interfaces';
import _ from 'lodash';
import { getCurrentFiscalYear } from 'utils';

export const getCurrentFiscal = (fiscals: IPropertyEvaluation[], key: number) => {
  const currentFiscal = getCurrentFiscalYear();
  return _.find(fiscals, { evaluatedOn: new Date(`${currentFiscal}-01-01`), key: key });
};
