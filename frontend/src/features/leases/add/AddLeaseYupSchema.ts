import { MAX_SQL_INT_SIZE } from 'constants/API';
import * as Yup from 'yup';

import { isLeaseCategoryVisible } from './AdministrationSubForm';

export const LeaseSchema = Yup.object().shape({
  statusType: Yup.string().required('Required'),
  startDate: Yup.date().required('Required'),
  expiryDate: Yup.date().min(Yup.ref('startDate'), 'Expiry Date must be after Start Date'),
  paymentReceivableType: Yup.string().required('Payment Receivable Type is required'),
  region: Yup.string().required('MOTI Region Type is required'),
  programType: Yup.string().required('Program Type is required'),
  motiName: Yup.string().max(200, 'MOTI Contact must be at most 200 characters'),
  otherProgramType: Yup.string().when('programType', {
    is: (programType: string) => programType && programType === 'OTHER',
    then: Yup.string()
      .required('Other Description required')
      .max(200, 'Other Description must be at most 200 characters'),
    otherwise: Yup.string().nullable(),
  }),
  type: Yup.string().required('Lease Type is required'),
  hasPhysicalLicense: Yup.string(),
  hasDigitalLicense: Yup.string(),
  otherType: Yup.string().when('type', {
    is: (type: string) => type && type === 'OTHER',
    then: Yup.string()
      .required('Other Description required')
      .max(200, 'Other Description must be at most 200 characters'),
    otherwise: Yup.string().nullable(),
  }),
  categoryType: Yup.string().when('type', {
    is: (type: string) => type && isLeaseCategoryVisible(type),
    then: Yup.string()
      .required('Other Description required')
      .max(200, 'Other Description must be at most 200 characters'),
    otherwise: Yup.string().nullable(),
  }),
  otherCategoryType: Yup.string().when('categoryType', {
    is: (categoryType: string) => categoryType && categoryType === 'OTHER',
    then: Yup.string()
      .required('Other Description required')
      .max(200, 'Other Description must be at most 200 characters'),
    otherwise: Yup.string().nullable(),
  }),
  purposeType: Yup.string().required('Purpose Type is required'),
  otherPurposeType: Yup.string().when('purposeType', {
    is: (purposeType: string) => purposeType && purposeType === 'OTHER',
    then: Yup.string()
      .required('Other Description required')
      .max(200, 'Other Description must be at most 200 characters'),
    otherwise: Yup.string().nullable(),
  }),
  documentationReference: Yup.string().max(
    500,
    'Location of documents must be at most 500 characters',
  ),
  description: Yup.string().max(4000, 'Description must be at most 4000 characters'),
  note: Yup.string().max(4000, 'Notes must be at most 4000 characters'),
  tfaFileNo: Yup.number().max(MAX_SQL_INT_SIZE, `LIS # must be less than ${MAX_SQL_INT_SIZE}`),
  psFileNo: Yup.string().max(50, 'PS # must be at most 50 characters'),
  properties: Yup.array().of(
    Yup.object().shape(
      {
        pid: Yup.string().when('pin', {
          is: (pin: string) => !!pin,
          then: Yup.string().nullable(),
          otherwise: Yup.string()
            .required('valid PID or PIN required')
            .max(10, 'PID must be at most 10 characters'),
        }),
        pin: Yup.string().when('pid', {
          is: (pid: string) => !!pid,
          then: Yup.string().nullable(),
          otherwise: Yup.string()
            .required('valid PID or PIN required')
            .max(10, 'PIN must be at most 10 characters'),
        }),
        landArea: Yup.number().max(
          Number.MAX_VALUE,
          `Land Area must be less than ${Number.MAX_VALUE}`,
        ),
      },
      [
        ['pid', 'pin'],
        ['pin', 'pid'],
      ],
    ),
  ),
});
