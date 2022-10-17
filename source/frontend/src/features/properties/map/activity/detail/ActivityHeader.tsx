import { Select } from 'components/common/form';
import { StyledDivider } from 'components/common/styles';
import { UserNameTooltip } from 'components/common/UserNameTooltip';
import * as API from 'constants/API';
import { Claims } from 'constants/claims';
import { HeaderField } from 'features/mapSideBar/tabs/HeaderField';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { useFormikContext } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_Activity } from 'models/api/Activity';
import { Api_File } from 'models/api/File';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { prettyFormatDate } from 'utils';

import { ActivityModel } from './models';

export interface IActivityHeaderProps {
  editMode: boolean;
  file: Api_File;
  activity: Api_Activity;
}

export const ActivityHeader: React.FunctionComponent<IActivityHeaderProps> = ({
  editMode,
  file,
  activity,
}) => {
  const leftColumnWidth = '5';
  const leftColumnLabel = '3';
  const { hasClaim } = useKeycloakWrapper();
  const { getOptionsByType } = useLookupCodeHelpers();
  const activityStatusTypeOptions = getOptionsByType(API.ACTIVITY_STATUS_TYPE);
  const formikProps = useFormikContext<ActivityModel>();
  const handleStatusChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const selected = activityStatusTypeOptions.find(
      x => x.value === event.target.selectedOptions[0].value,
    );
    formikProps.setFieldValue('activityStatusTypeCode', {
      id: selected?.value,
      description: selected?.label,
    });
  };

  return (
    <Row className="no-gutters">
      <Col>
        <Row className="no-gutters">
          <Col xs={leftColumnWidth}>
            <HeaderField label="File:" labelWidth={leftColumnLabel} className="flex-nowrap">
              {file?.fileNumber} - {file?.fileName}
            </HeaderField>
          </Col>
          <Col className="text-right">
            Created: <strong>{prettyFormatDate(activity?.appCreateTimestamp)}</strong> by{' '}
            <UserNameTooltip
              userName={activity?.appCreateUserid}
              userGuid={activity?.appCreateUserGuid}
            />
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col xs={leftColumnWidth}></Col>
          <Col className="text-right">
            Last updated: <strong>{prettyFormatDate(activity?.appLastUpdateTimestamp)}</strong> by{' '}
            <UserNameTooltip
              userName={activity?.appLastUpdateUserid}
              userGuid={activity?.appLastUpdateUserGuid}
            />
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col xs={leftColumnWidth}></Col>
          <Col>
            {!editMode && (
              <HeaderField className="justify-content-end" label="Status:">
                {activity?.activityStatusTypeCode?.description}
              </HeaderField>
            )}
            {hasClaim(Claims.ACTIVITY_EDIT) && editMode && (
              <SectionField label="Status">
                <Select
                  disabled={!editMode}
                  field="activityStatusTypeCode.id"
                  options={activityStatusTypeOptions}
                  onChange={handleStatusChange}
                />
              </SectionField>
            )}
          </Col>
        </Row>
        <StyledDivider />
      </Col>
    </Row>
  );
};

export default ActivityHeader;
