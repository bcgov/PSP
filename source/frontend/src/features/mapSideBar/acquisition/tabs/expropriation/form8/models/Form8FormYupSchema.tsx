import * as yup from 'yup';

import { exists } from '@/utils/utils';
/* eslint-disable no-template-curly-in-string */

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

export const Form8FormModelYupSchema = yup.object().shape({
  payeeKey: yup.string().required('Payee is required'),
  expropriationAuthority: yup.object().shape({
    contact: yup.object().required('Expropriation authority is required').nullable(),
  }),
  description: yup.string().max(2000, 'Description must be at most ${max} characters'),
  paymentItems: yup
    .array()
    .of(
      yup.object().shape({
        paymentItemTypeCode: yup.string().required('Type is required'),
        pretaxAmount: yup.number().transform(value => (isNaN(value) || !exists(value) ? 0 : value)),
      }),
    )
    .unique(
      'Each payment type can only be added once. Select a different payment type.',
      (val: any) => val.paymentItemTypeCode,
    ),
});
