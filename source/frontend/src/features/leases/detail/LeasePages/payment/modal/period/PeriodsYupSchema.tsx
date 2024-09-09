import * as Yup from 'yup';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';
import { LeasePeriodStatusTypes } from '@/constants/index';

export const LeasePeriodSchema = Yup.object().shape({
  startDate: Yup.date().required('Required'),
  expiryDate: Yup.date().when('isFlexible', (isFlexible, schema) =>
    isFlexible === 'true'
      ? schema.min(Yup.ref('startDate'), 'Expiry Date must be after Start Date')
      : schema
          .min(Yup.ref('startDate'), 'Expiry Date must be after Start Date')
          .required('Required'),
  ),
  paymentAmount: Yup.number().max(MAX_SQL_MONEY_SIZE),
  paymentDueDateStr: Yup.string().max(200),
  paymentNote: Yup.string().max(2000),
  statusTypeCode: Yup.object()
    .test({
      name: 'statusTypeTest',
      test: function (value, context) {
        if (value?.id === LeasePeriodStatusTypes.NOT_EXERCISED && !!this.parent.payments.length) {
          return context.createError({
            path: 'statusTypeCode.id',
            message: 'Periods with one or more payment must be exercised',
          });
        }
        return true;
      },
    })
    .from('statusTypeCode', 'statusTypeCodetest'),
});
