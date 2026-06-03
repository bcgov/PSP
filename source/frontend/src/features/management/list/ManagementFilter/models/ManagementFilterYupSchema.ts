/* eslint-disable no-template-curly-in-string */
import * as yup from 'yup';

import { exists } from '@/utils/utils';

export const ManagementFilterYupSchema = yup.object().shape({
  managementTeamMemberProfileTypeCode: yup.string(),
  managementTeamMember: yup
    .object()
    .nullable()
    .when('managementTeamMemberProfileTypeCode', {
      is: (managementTeamMemberProfileTypeCode: string) =>
        exists(managementTeamMemberProfileTypeCode),
      then: yup.object().nullable().required('Team member is required'),
      otherwise: yup.object().nullable(),
    }),
});
