import { LeaseTermStatusTypes } from 'constants/index';
import * as Yup from 'yup';

export const LeaseTermSchema = Yup.object().shape({
  startDate: Yup.date().required('Required'),
  expiryDate: Yup.date().min(Yup.ref('startDate'), 'Expiry Date must be after Start Date'),
  statusTypeCode: Yup.object()
    .test({
      name: 'statusTypeTest',
      test: function(value, context) {
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
