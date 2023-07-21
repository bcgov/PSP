/* eslint-disable no-template-curly-in-string */
import * as Yup from 'yup';

import { UpdateAcquisitionOwnersYupSchema } from '../../../common/update/acquisitionOwners/UpdateAcquisitionOwnersYupSchema';
import { UpdateAcquisitionTeamYupSchema } from '../../../common/update/acquisitionTeam/UpdateAcquisitionTeamYupSchema';

export const UpdateAcquisitionFileYupSchema = Yup.object()
  .shape({
    fileStatusTypeCode: Yup.string().required('Status is required'),
    fileName: Yup.string()
      .required('Acquisition file name is required')
      .max(500, 'Acquisition file name must be at most ${max} characters'),
    acquisitionType: Yup.string().required('Acquisition type is required'),
    region: Yup.string().required('Ministry region is required'),
    legacyFileNumber: Yup.string().max(18, 'Legacy file number must be at most ${max} characters'),
    completionDate: Yup.date().when('fileStatusTypeCode', {
      is: (fileStatusTypeCode: string) => fileStatusTypeCode && fileStatusTypeCode === 'COMPLT',
      then: Yup.date().required(
        `Acquisition completed date is required when file status is set to "Complete"`,
      ),
      otherwise: Yup.date().nullable(),
    }),
  })
  .concat(UpdateAcquisitionTeamYupSchema)
  .concat(UpdateAcquisitionOwnersYupSchema);
