/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

import { ApiGen_CodeTypes_SubfileInterestTypes } from '@/models/api/generated/ApiGen_CodeTypes_SubfileInterestTypes';

import { UpdateAcquisitionOwnersYupSchema } from '../../../common/update/acquisitionOwners/UpdateAcquisitionOwnersYupSchema';
import { UpdateAcquisitionTeamYupSchema } from '../../../common/update/acquisitionTeam/UpdateAcquisitionTeamYupSchema';

export const UpdateAcquisitionFileYupSchema = yup
  .object()
  .shape({
    fileStatusTypeCode: yup.string().required('Status is required'),
    fileName: yup
      .string()
      .required('Acquisition file name is required')
      .max(500, 'Acquisition file name must be at most ${max} characters'),
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
    acquisitionType: yup.string().required('Acquisition type is required'),
    region: yup.string().required('Ministry region is required'),
    legacyFileNumber: yup.string().max(18, 'Legacy file number must be at most ${max} characters'),
  })
  .concat(UpdateAcquisitionTeamYupSchema)
  .concat(UpdateAcquisitionOwnersYupSchema);
