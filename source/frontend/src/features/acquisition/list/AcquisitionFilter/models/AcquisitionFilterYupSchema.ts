/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

import { exists } from '@/utils/utils';

export const AcquisitionFilterYupSchema = yup.object().shape({
  acquisitionTeamMemberProfileTypeCode: yup.string(),
  acquisitionTeamMembers: yup.array().when('acquisitionTeamMemberProfileTypeCode', {
    is: (acquisitionTeamMemberProfileTypeCode: string) =>
      exists(acquisitionTeamMemberProfileTypeCode),
    then: yup.array().min(1, 'Team member is required').required(''),
    otherwise: yup.array(),
  }),
});
