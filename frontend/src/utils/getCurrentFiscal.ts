import { FiscalKeys } from 'constants/fiscalKeys';
import { IFiscal } from 'interfaces';
import _ from 'lodash';
import { getCurrentFiscalYear } from 'utils';

export const getCurrentFiscal = (fiscals: IFiscal[], key: FiscalKeys) => {
  const currentFiscal = getCurrentFiscalYear();
  return _.find(fiscals, { fiscalYear: currentFiscal, key: key });
};
