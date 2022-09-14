/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const AddAcquisitionFileYupSchema = Yup.object().shape({
  fileName: Yup.string()
    .required('Acquisition file name is required')
    .max(500, 'Acquisition file name must be at most ${max} characters'),
  acquisitionType: Yup.string().required('Acquisition type is required'),
  region: Yup.string().required('Ministry region is required'),
  team: Yup.array().of(
    Yup.object().shape(
      {
        contact: Yup.object()
          .nullable()
          .when('contactTypeCode', {
            is: (contactTypeCode: string) => Boolean(contactTypeCode),
            then: Yup.string().required('Select a team member'),
          }),
        contactTypeCode: Yup.string()
          .trim()
          .when('contact', {
            is: (contact: object) => Boolean(contact),
            then: Yup.string().required('Select a profile'),
          }),
      },
      [
        ['contact', 'contactTypeCode'],
        ['contactTypeCode', 'contact'],
      ],
    ),
  ),
});
