/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

import { ApiGen_CodeTypes_SubfileInterestTypes } from '@/models/api/generated/ApiGen_CodeTypes_SubfileInterestTypes';

import { UpdateAcquisitionOwnersYupSchema } from '../common/update/acquisitionOwners/UpdateAcquisitionOwnersYupSchema';
import { UpdateAcquisitionTeamYupSchema } from '../common/update/acquisitionTeam/UpdateAcquisitionTeamYupSchema';

export const AddAcquisitionFileYupSchema = yup
  .object()
  .shape(
    {
      fileName: yup
        .string()
        .required('Acquisition file name is required')
        .max(500, 'Acquisition file name must be at most ${max} characters'),
      overrideFileNumberSequence: yup.boolean(),
      fileNo: yup
        .string()
        .nullable()
        .when('overrideFileNumberSequence', {
          is: true,
          then: schema =>
            schema.test(
              'one-of-two',
              'Either Legacy file or Historical file number is required',
              function (value) {
                const { legacyFileNumber } = this.parent;
                return !!value || !!legacyFileNumber;
              },
            ),
          otherwise: schema => schema.notRequired(),
        }),
      legacyFileNumber: yup
        .string()
        .max(18, 'Historical file number must be at most ${max} characters')
        .nullable()
        .when('overrideFileNumberSequence', {
          is: true,
          then: schema =>
            schema.test(
              'one-of-two',
              'Either Legacy file or Historical file is required',
              function (value) {
                const { fileNo } = this.parent;
                return !!value || !!fileNo;
              },
            ),
          otherwise: schema => schema.notRequired(),
        }),
      acquisitionType: yup.string().required('Acquisition type is required'),
      parentAcquisitionFileId: yup.number().nullable(),
      subfileInterestTypeCode: yup
        .string()
        .when('parentAcquisitionFileId', {
          is: (parentAcquisitionFileId: number) => parentAcquisitionFileId !== null,
          then: yup.string().required('Subfile interest type is required').nullable(),
          otherwise: yup.string().nullable(),
        })
        .nullable(),
      otherSubfileInterestType: yup.string().when('subfileInterestTypeCode', {
        is: (subfileInterestTypeCode: string) =>
          subfileInterestTypeCode &&
          subfileInterestTypeCode === ApiGen_CodeTypes_SubfileInterestTypes.OTHER,
        then: yup
          .string()
          .required('Other Subfile interest type is required')
          .nullable()
          .max(200, 'Other Subfile interest description must be at most 200 characters'),
        otherwise: yup.string().nullable(),
      }),
      region: yup.string().required('Ministry region is required'),
      physicalFileDetails: yup
        .string()
        .nullable()
        .max(2000, 'Physical file details must be at most ${max} characters'),
      properties: yup.array().of(
        yup.object().shape({
          isRetired: yup
            .boolean()
            .notOneOf([true], 'Selected property is retired and can not be added to the file'),
        }),
      ),
      noticeOfClaim: yup.object().shape({
        comment: yup
          .string()
          .nullable()
          .max(4000, 'Notice of claim comment must be at most ${max} characters'),
      }),
    },
    [['fileNo', 'legacyFileNumber']],
  )
  .test('exclusive-test', function (values) {
    const { overrideFileNumberSequence, fileNo, legacyFileNumber } = values;
    if (!overrideFileNumberSequence) return true;

    const hasA = !!fileNo;
    const hasB = !!legacyFileNumber;

    if (hasA && hasB) {
      return this.createError({
        path: 'fileNo',
        message: 'Please enter Historical file OR Legacy file, not both.',
      });
    }

    return true;
  })
  .concat(UpdateAcquisitionTeamYupSchema)
  .concat(UpdateAcquisitionOwnersYupSchema);
