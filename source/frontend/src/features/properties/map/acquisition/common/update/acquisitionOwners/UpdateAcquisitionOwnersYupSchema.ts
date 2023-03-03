/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const UpdateAcquisitionOwnersYupSchema = Yup.object().shape({
  owners: Yup.array().of(
    Yup.object().shape({
      lastNameOrCorp1: Yup.string().max(
        300,
        'Last name/Corporation name must be at most ${max} characters',
      ),
      lastNameOrCorp2: Yup.string().max(300, 'Other name must be at most ${max} characters'),
      givenName: Yup.string().max(300, 'Given name must be at most ${max} characters'),
      incorporationNumber: Yup.string().max(50, 'Given name must be at most ${max} characters'),
      address: Yup.object().shape({
        streetAddress1: Yup.string().max(200, 'Address (line 1) must be at most ${max} characters'),
        streetAddress2: Yup.string().max(200, 'Address (line 2) be at most ${max} characters'),
        streetAddress3: Yup.string().max(200, 'Address (line 3) be at most ${max} characters'),
        municipality: Yup.string().max(200, 'City must be at most ${max} characters'),
        postal: Yup.string().max(20, 'Postal Code must be at most ${max} characters'),
      }),
    }),
  ),
});
