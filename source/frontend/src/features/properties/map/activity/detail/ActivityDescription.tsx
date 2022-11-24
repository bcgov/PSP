import { TextArea } from 'components/common/form';
import { Claims } from 'constants/claims';
import { getIn, useFormikContext } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

import { ActivityModel } from './models';

export interface IActivityDescriptionProps {
  editMode?: boolean;
  isEditable?: boolean;
  nameSpace?: string;
}

export const ActivityDescription: React.FunctionComponent<IActivityDescriptionProps> = ({
  editMode,
  isEditable,
  nameSpace,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  const { values } = useFormikContext<ActivityModel>();
  const fieldNameSpace = withNameSpace(nameSpace, 'description');
  const description = getIn(values, fieldNameSpace);

  return isEditable && editMode && hasClaim(Claims.ACTIVITY_EDIT) ? (
    <>
      <TextArea field={fieldNameSpace} />
    </>
  ) : (
    <p>{description}</p>
  );
};

export default ActivityDescription;
