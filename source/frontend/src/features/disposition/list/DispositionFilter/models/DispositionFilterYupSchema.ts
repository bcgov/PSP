/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

import { exists } from '@/utils/utils';

export const DispositionFilterYupSchema = yup.object().shape({
  dispositionTeamMemberProfileTypeCode: yup.string(),
  dispositionTeamMember: yup
    .object()
    .nullable()
    .when('dispositionTeamMemberProfileTypeCode', {
      is: (dispositionTeamMemberProfileTypeCode: string) =>
        exists(dispositionTeamMemberProfileTypeCode),
      then: yup.object().nullable().required('Team member is required'),
      otherwise: yup.object().nullable(),
    }),
});
