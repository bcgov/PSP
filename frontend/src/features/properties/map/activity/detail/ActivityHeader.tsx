import { StyledDivider } from 'components/common/styles';
import { UserNameTooltip } from 'components/common/UserNameTooltip';
import { HeaderField } from 'features/mapSideBar/tabs/HeaderField';
import { Api_Activity } from 'models/api/Activity';
import { Api_File } from 'models/api/File';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { prettyFormatDate } from 'utils';

export interface IActivityHeaderProps {
  file: Api_File;
  activity: Api_Activity;
}

export const ActivityHeader: React.FunctionComponent<IActivityHeaderProps> = ({
  file,
  activity,
}) => {
  const leftColumnWidth = '5';
  const leftColumnLabel = '3';
  return (
    <Row className="no-gutters pl-4 pr-4">
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
            <HeaderField className="justify-content-end" label="Status:">
              {activity?.activityStatusTypeCode?.description}
            </HeaderField>
          </Col>
        </Row>
        <StyledDivider />
      </Col>
    </Row>
  );
};

export default ActivityHeader;
