/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

import { MAX_SQL_MONEY_SIZE } from '@/constants/API';

declare module 'yup' {
  interface ArraySchema<T> {
    unique(message: string, mapper?: (value: T, index?: number, list?: T[]) => T[]): ArraySchema<T>;
  }
}

yup.addMethod(yup.array, 'unique', function (message, mapper = (val: unknown) => val) {
  return this.test(
    'unique',
    message,
    (list = []) => list.length === new Set(list.map(mapper)).size,
  );
});

export const AddProjectYupSchema = yup.object().shape({
  projectNumber: yup.string().max(20, 'Project number must be at most ${max} characters'),
  projectName: yup
    .string()
    .required('Name is required.')
    .max(200, 'Project name must be at most ${max} characters'),
  projectStatusType: yup.string().required('Project status is required.'),
  region: yup.string().required('Region is required.'),
  summary: yup.string().max(2000, 'Project summary must be at most ${max} characters'),
  products: yup.array().of(
    yup.object().shape({
      code: yup
        .string()
        .required('Product Code is required')
        .max(20, 'Product code must be at most ${max} characters'),
      description: yup
        .string()
        .required('Product Description is required')
        .max(200, 'Product description must be at most ${max} characters'),
      costEstimate: yup.lazy(value =>
        value === ''
          ? yup.string()
          : yup.number().typeError('Cost estimate must be a number').max(MAX_SQL_MONEY_SIZE),
      ),
      startDate: yup.string(),
      objective: yup.string().max(2000, 'Product objective must be at most ${max} characters'),
      scope: yup.string().max(2000, 'Product scope must be at most ${max} characters'),
    }),
  ),
  projectTeam: yup
    .array()
    .of(
      yup.object().shape({
        contact: yup.object().nullable(),
      }),
    )
    .unique(
      'Each team member can only be added once. Select a new team member.',
      (val: any) => val.contact?.personId ?? 0,
    ),
});
