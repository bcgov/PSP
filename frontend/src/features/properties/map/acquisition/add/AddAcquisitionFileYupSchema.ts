/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

export const AddAcquisitionFileYupSchema = Yup.object().shape({
  name: Yup.string()
    .required('Acquisition file name is required')
    .max(500, 'Acquisition file name must be at most ${max} characters'),
  acquisitionTypeId: Yup.string().required('Acquisition type is required'),
  regionId: Yup.string().required('Ministry region is required'),
});
