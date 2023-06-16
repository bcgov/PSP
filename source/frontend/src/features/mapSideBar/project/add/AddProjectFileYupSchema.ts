/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';

export const AddProjectYupSchema = Yup.object().shape({
  projectNumber: Yup.string().max(20, 'Project number must be at most ${max} characters'),
  projectName: Yup.string()
    .required('Name is required.')
    .max(200, 'Project name must be at most ${max} characters'),
  projectStatusType: Yup.string().required('Project status is required.'),
  region: Yup.string().required('Region is required.'),
  summary: Yup.string().max(2000, 'Project summary must be at most ${max} characters'),
  products: Yup.array().of(
    Yup.object().shape({
      code: Yup.string()
        .required('Product Code is required')
        .max(20, 'Product code must be at most ${max} characters'),
      description: Yup.string()
        .required('Product Description is required')
        .max(200, 'Product description must be at most ${max} characters'),
      costEstimate: Yup.lazy(value =>
        value === ''
          ? Yup.string()
          : Yup.number().typeError('Cost estimate must be a number').max(MAX_SQL_MONEY_SIZE),
      ),
      startDate: Yup.string(),
      objective: Yup.string().max(2000, 'Product objective must be at most ${max} characters'),
      scope: Yup.string().max(2000, 'Product scope must be at most ${max} characters'),
    }),
  ),
});
