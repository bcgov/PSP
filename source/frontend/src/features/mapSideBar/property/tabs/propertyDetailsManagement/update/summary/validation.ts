/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

import { ManagementPurposeModel } from './models';

export const PropertyManagementYupSchema = Yup.object().shape({
  additionalDetails: Yup.string().when('managementPurposes', {
    is: (purposesArray: ManagementPurposeModel[]) =>
      purposesArray?.some(p => p.typeCode === 'OTHER'),
    then: Yup.string()
      .required('Additional details are required when Other purpose is selected')
      .max(4000, 'Additional details must be at most ${max} characters'),
    otherwise: Yup.string()
      .nullable()
      .max(4000, 'Additional details must be at most ${max} characters'),
  }),
});
