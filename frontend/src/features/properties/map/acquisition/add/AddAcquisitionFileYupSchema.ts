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
        contactTypeCode: Yup.string().when('contact', {
          is: (contact: object) => !!contact,
          then: Yup.string().required('Select a profile'),
        }),
        contact: Yup.object().when('contactTypeCode', {
          is: (contactTypeCode: string) => !!contactTypeCode,
          then: Yup.object().required('Select a team member'),
        }),
      },
      [
        ['contactTypeCode', 'contact'],
        ['contact', 'contactTypeCode'],
      ],
    ),
  ),
});
