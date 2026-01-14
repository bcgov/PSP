/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';

export const PropertyNetBookYupSchema = yup.object().shape({
  netBookAmount: yup.lazy(value =>
    value === ''
      ? yup.string().nullable()
      : yup
          .number()
          .nullable()
          .min(0, 'Net Book Amount must be at least ${min}')
          .max(MAX_SQL_MONEY_SIZE, 'Net Book Amount must be less than ${max}'),
  ),
  netBookNote: yup.string().nullable().max(4000, 'Net Book Note must be at most ${max} characters'),
});
