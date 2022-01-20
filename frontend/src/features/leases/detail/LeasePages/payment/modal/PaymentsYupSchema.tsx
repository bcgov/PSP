import * as Yup from 'yup';

export const LeaseTermSchema = Yup.object().shape({
  startDate: Yup.date().required('Required'),
  expiryDate: Yup.date().min(Yup.ref('startDate'), 'Expiry Date must be after Start Date'),
  statusTypeCode: Yup.object().test({
    message: 'Terms with one or more payment must be exercised',
    test: function(value) {
      return this.parent?.payments?.length === 0 || value.id === 'EXER';
    },
  }),
});
