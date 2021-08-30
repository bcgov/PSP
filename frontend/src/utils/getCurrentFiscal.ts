import { IPropertyEvaluation } from 'interfaces';
import _ from 'lodash';
import moment from 'moment';
import { getCurrentFiscalYear } from 'utils';

export const getCurrentFiscal = (fiscals: IPropertyEvaluation[], key: number) => {
  const currentFiscal = getCurrentFiscalYear();
  return _.find(fiscals, (fiscal: IPropertyEvaluation) => {
    const evaluatedOn =
      fiscal.evaluatedOn instanceof Date
        ? moment(fiscal.evaluatedOn.toISOString(), 'YYYY-MM-DD')
        : moment(fiscal.evaluatedOn);
    return evaluatedOn.isSame(moment(`${currentFiscal}-01-01`), 'day') && fiscal.key === key;
  });
};
