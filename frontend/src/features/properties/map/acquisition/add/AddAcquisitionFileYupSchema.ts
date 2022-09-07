/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const AddAcquisitionFileYupSchema = Yup.object().shape({
  fileName: Yup.string()
    .required('Acquisition file name is required')
    .max(500, 'Acquisition file name must be at most ${max} characters'),
  acquisitionType: Yup.string().required('Acquisition type is required'),
  region: Yup.string().required('Ministry region is required'),
  team: Yup.array().of(
    Yup.object().shape({
      contact: Yup.object().shape({
        id: Yup.number().required('Select a contact'),
      }),
      contactTypeCode: Yup.string()
        .ensure()
        .required('Select a profile'),
    }),
  ),
});
