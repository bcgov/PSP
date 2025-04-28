/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

import { ManagementTeamYupSchema } from './ManagementTeamSubFormYupSchema';

declare module 'yup' {
  interface ArraySchema<T> {
    unique(message: string, mapper?: (value: T, index?: number, list?: T[]) => T[]): ArraySchema<T>;
  }
}

export const AddManagementFormYupSchema = yup
  .object()
  .shape({
    fileName: yup
      .string()
      .max(500, 'Management file name must be at most ${max} characters')
      .nullable()
      .required('File name is required'),
    additionalDetails: yup
      .string()
      .max(2000, 'Management file name must be at most ${max} characters'),
    legacyFileNum: yup.string().max(100, 'Legacy file number must be at most ${max} characters'),
    programTypeCode: yup.string().nullable().required('Purpose is required'),
    properties: yup.array().of(
      yup.object().shape({
        isRetired: yup
          .boolean()
          .isFalse('Selected property is retired and can not be added to the file'),
      }),
    ),
  })
  .concat(ManagementTeamYupSchema);
