import * as Yup from 'yup';

Yup.addMethod(Yup.string, 'optional', function optional() {
  return this.transform(value => {
    return (typeof value == 'string' && !value) ||
      (value instanceof Array && !value.length) ||
      value === null // allow to skip "nullable"
      ? undefined
      : value;
  });
});

export const AccessRequestSchema = Yup.object().shape({
  showOrganization: Yup.boolean(),
  roleId: Yup.number()
    .min(0, 'Invalid Role')
    .required('Required'),
  regionCodeId: Yup.number()
    .min(0, 'Invalid Region')
    .required('Required'),
  note: Yup.string().max(4000, 'Note must be less than 4000 characters'),
  user: Yup.object().shape({
    position: Yup.string().max(100, 'Note must be less than 100 characters'),
  }),
});

export const UserUpdateSchema = Yup.object().shape({
  email: Yup.string()
    .email()
    .max(100, 'Email must be less than 100 characters'),
  firstName: Yup.string().max(100, 'First Name must be less than 100 characters'),
  middleName: Yup.string().max(400, 'Middle Name must be less than 400 characters'),
  surname: Yup.string().max(100, 'Last Name must be less than 100 characters'),
  note: Yup.string().max(4000, 'Note must be less than 4000 characters'),
  position: Yup.string().max(100, 'Note must be less than 100 characters'),
});

export const FilterBarSchema = Yup.object().shape({
  minLotSize: Yup.number()
    .typeError('Invalid')
    .positive('Must be greater than 0')
    .max(200000, 'Invalid'),
  maxLotSize: Yup.number()
    .typeError('Invalid')
    .positive('Must be greater than 0')
    .max(200000, 'Invalid')
    /* Reference minLotSize field in validating maxLotSize value */
    .moreThan(Yup.ref('minLotSize'), 'Must be greater than Min Lot Size'),
});
