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
  note: Yup.string().max(1000, 'Note must be less than 1000 characters'),
  user: Yup.object().shape({
    position: Yup.string().max(100, 'Note must be less than 100 characters'),
  }),
});

export const UserUpdateSchema = Yup.object().shape({
  email: Yup.string()
    .email()
    .max(100, 'Email must be less than 100 characters'),
  firstName: Yup.string().max(100, 'First Name must be less than 100 characters'),
  middleName: Yup.string().max(100, 'Middle Name must be less than 100 characters'),
  surname: Yup.string().max(100, 'Last Name must be less than 100 characters'),
});

export const OrganizationEditSchema = Yup.object().shape({
  email: Yup.string()
    .email('Please enter a valid email.')
    .max(100, 'Email must be less than 100 characters')
    .when('sendEmail', (sendEmail: boolean, schema: any) =>
      sendEmail ? schema.required('Email address is required') : schema,
    ),
  name: Yup.string()
    .max(100, 'Organization name must be less than 100 characters')
    .required('An organization name is required.'),
  addressTo: Yup.string()
    .max(100, 'Email addressed to must be less than 100 characters')
    .when('sendEmail', (sendEmail: boolean, schema: any) =>
      sendEmail ? schema.required('Email addressed to is required (i.e. Good Morning)') : schema,
    ),
  code: Yup.string().required('An organization code is required.'),
});

export const UserSchema = Yup.object().shape({
  email: Yup.string()
    .email()
    .max(100, 'Email must be less than 100 characters')
    .required('Required'),
  firstName: Yup.string()
    .max(100, 'First Name must be less than 100 characters')
    .required('Required'),
  middleName: Yup.string().max(100, 'Middle Name must be less than 100 characters'),
  surname: Yup.string()
    .max(100, 'Last Name must be less than 100 characters')
    .required('Required'),
  role: Yup.number()
    .min(1, 'Invalid Role')
    .nullable(),
  organization: Yup.number()
    .min(1, 'Invalid Organization')
    .nullable(),
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
