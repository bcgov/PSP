import * as Yup from 'yup';

export const addressSchema = Yup.object().shape({
  postal: (Yup.string() as any)
    .optional()
    .matches(/^[a-zA-z][0-9][a-zA-z][\s-]?[0-9][a-zA-z][0-9]$/, 'Invalid Postal Code'),
});

export const propertyFormSchema = Yup.object().shape({
  address: addressSchema,
});
