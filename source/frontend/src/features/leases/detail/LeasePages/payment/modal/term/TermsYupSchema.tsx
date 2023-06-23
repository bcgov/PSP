import * as Yup from 'yup';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';
import { LeaseTermStatusTypes } from '@/constants/index';

export const LeaseTermSchema = Yup.object().shape({
  startDate: Yup.date().required('Required'),
  expiryDate: Yup.date().min(Yup.ref('startDate'), 'Expiry Date must be after Start Date'),
  paymentAmount: Yup.number().max(MAX_SQL_MONEY_SIZE),
  paymentDueDate: Yup.string().max(200),
  paymentNote: Yup.string().max(2000),
  statusTypeCode: Yup.object()
    .test({
      name: 'statusTypeTest',
      test: function (value, context) {
        if (value?.id === LeaseTermStatusTypes.NOT_EXERCISED && !!this.parent.payments.length) {
          return context.createError({
            path: 'statusTypeCode.id',
            message: 'Terms with one or more payment must be exercised',
          });
        }
        return true;
      },
    })
    .from('statusTypeCode', 'statusTypeCodetest'),
});
