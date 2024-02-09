/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

import { DispositionTeamYupSchema } from './DispositionTeamSubFormYupSchema';

declare module 'yup' {
  interface ArraySchema<T> {
    unique(message: string, mapper?: (value: T, index?: number, list?: T[]) => T[]): ArraySchema<T>;
  }
}

export const AddDispositionFormYupSchema = yup
  .object()
  .shape({
    fileName: yup.string().max(200, 'Disposition file name must be at most ${max} characters'),
    referenceNumber: yup
      .string()
      .nullable()
      .max(200, 'Disposition reference number must be at most ${max} characters'),
    dispositionTypeCode: yup.string().nullable().required('Disposition type is required'),
    dispositionTypeOther: yup.string().when('dispositionTypeCode', {
      is: (dispositionTypeCode: string) => dispositionTypeCode && dispositionTypeCode === 'OTHER',
      then: yup
        .string()
        .nullable()
        .required('Other Disposition type is required')
        .max(200, 'Other Disposition type must be at most ${max} characters'),
      otherwise: yup.string().nullable(),
    }),
    initiatingDocumentTypeCode: yup.string().nullable(),
    initiatingDocumentTypeOther: yup.string().when('initiatingDocumentTypeCode', {
      is: (initiatingDocumentTypeCode: string) =>
        initiatingDocumentTypeCode && initiatingDocumentTypeCode === 'OTHER',
      then: yup
        .string()
        .required('Other Iniating Document type is required')
        .max(200, 'Other Iniating Document type must be at most ${max} characters'),
      otherwise: yup.string().nullable(),
    }),
    regionCode: yup.string().nullable().required('Ministry region is required'),
  })
  .concat(DispositionTeamYupSchema);

export const UpdateDispositionFormYupSchema = yup
  .object()
  .shape({
    dispositionStatusTypeCode: yup.string().required('Disposition status is required'),
  })
  .concat(AddDispositionFormYupSchema);
