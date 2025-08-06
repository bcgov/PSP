/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

import { ApiGen_CodeTypes_LeasePurposeTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePurposeTypes';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { exists, isValidId } from '@/utils';

import { LeasePurposeModel } from '../models/LeasePurposeModel';

export const AddLeaseYupSchema = Yup.object().shape({
  statusTypeCode: Yup.string().required('Required'),
  startDate: Yup.date().when('statusTypeCode', {
    is: (statusTypeCode: string) =>
      statusTypeCode && statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE.toString(),
    then: Yup.date().required('Required'),
    otherwise: Yup.date().nullable(),
  }),
  expiryDate: Yup.date().when('startDate', {
    is: (startDate: Date) => exists(startDate),
    then: Yup.date().min(Yup.ref('startDate'), 'Expiry Date must be after Start Date'),
    otherwise: Yup.date().nullable(),
  }),
  paymentReceivableTypeCode: Yup.string().required('Payment Receivable Type is required'),
  regionId: Yup.string().required('MOTT Region Type is required'),
  programTypeCode: Yup.string().required('Program Type is required'),
  motiName: Yup.string().max(200, 'MOTT Contact must be at most ${max} characters'),
  otherProgramTypeDescription: Yup.string().when('programTypeCode', {
    is: (programTypeCode: string) => programTypeCode && programTypeCode === 'OTHER',
    then: Yup.string()
      .required('Other Description required')
      .max(200, 'Other Description must be at most ${max} characters'),
    otherwise: Yup.string().nullable(),
  }),
  leaseTypeCode: Yup.string().required('Lease Type is required'),
  purposes: Yup.array().min(1, 'Purpose Type is required'),
  purposeOtherDescription: Yup.string()
    .nullable()
    .when('purposes', {
      is: (purposesArray: LeasePurposeModel[]) =>
        purposesArray?.some(
          obj => obj.purposeTypeCode === ApiGen_CodeTypes_LeasePurposeTypes.OTHER,
        ),
      then: Yup.string()
        .nullable()
        .required('Other purpose description is required')
        .max(200, 'Other purpose description must be at most ${max} characters'),
      otherwise: Yup.string().nullable(),
    }),
  hasPhysicalLicense: Yup.string().nullable(),
  hasDigitalLicense: Yup.string().nullable(),
  otherLeaseTypeDescription: Yup.string().when('leaseTypeCode', {
    is: (leaseTypeCode: string) => leaseTypeCode && leaseTypeCode === 'OTHER',
    then: Yup.string()
      .required('Other Description required')
      .max(200, 'Other Description must be at most ${max} characters'),
    otherwise: Yup.string().nullable(),
  }),
  documentationReference: Yup.string().max(
    500,
    'Location of documents must be at most ${max} characters',
  ),
  description: Yup.string().max(4000, 'Description must be at most ${max} characters'),
  note: Yup.string().max(4000, 'Notes must be at most ${max} characters'),
  tfaFileNumber: Yup.string().max(50, 'LIS # must be at most ${max} characters'),
  psFileNo: Yup.string().max(50, 'PS # must be at most ${max} characters'),
  properties: Yup.array().of(
    Yup.object().shape({
      name: Yup.string().max(250, 'Property name must be at most ${max} characters'),
      property: Yup.object().shape({
        isRetired: Yup.boolean().when('id', {
          is: (id: number) => !isValidId(id),
          then: Yup.boolean().notOneOf(
            [true],
            'Selected property is retired and can not be added to the file',
          ),
          otherwise: Yup.boolean().nullable(),
        }),
        isDisposed: Yup.boolean().notOneOf(
          [true],
          'Selected property is disposed and can not be added to the file',
        ),
      }),
    }),
  ),
  consultations: Yup.array().of(
    Yup.object().shape({
      consultationTypeOtherDescription: Yup.string()
        .when(['consultationType', 'consultationStatusType'], {
          is: (consultationType: string, consultationStatusType: string) =>
            consultationType &&
            consultationType === 'OTHER' &&
            consultationStatusType &&
            consultationStatusType !== 'UNKNOWN',
          then: Yup.string().required('Other Description required'),
          otherwise: Yup.string().nullable(),
        })
        .max(2000, 'Other Description must be at most ${max} characters'),
    }),
  ),
  cancellationReason: Yup.string().when('statusTypeCode', {
    is: (statusTypeCode: string) =>
      statusTypeCode && statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD.toString(),
    then: Yup.string()
      .required('Cancellation reason is required.')
      .max(500, 'Cancellation reason must be at most ${max} characters'),
    otherwise: Yup.string().nullable(),
  }),
  terminationReason: Yup.string().when('statusTypeCode', {
    is: (statusTypeCode: string) =>
      statusTypeCode && statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED.toString(),
    then: Yup.string()
      .required('Termination reason is required.')
      .max(500, 'Termination reason must be at most ${max} characters'),
    otherwise: Yup.string().nullable(),
  }),
  primaryArbitrationCity: Yup.string()
    .nullable()
    .max(200, 'Primary arbitration city must be at most ${max} characters'),
  feeDeterminationNote: Yup.string()
    .nullable()
    .max(1000, 'Fee determination comments must be at most ${max} characters'),
  renewals: Yup.array().of(
    Yup.object().shape({
      renewalNote: Yup.string().max(2000, 'Renewal note must be at most ${max} characters'),
    }),
  ),
});
