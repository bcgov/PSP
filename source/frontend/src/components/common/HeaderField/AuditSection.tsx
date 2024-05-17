import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Base_BaseAudit } from '@/models/api/generated/ApiGen_Base_BaseAudit';
import { prettyFormatUTCDate } from '@/utils';

export interface IAuditSectionProps {
  baseAudit: ApiGen_Base_BaseAudit;
  lastUpdatedBy: Api_LastUpdatedBy;
}

export const AuditSection: React.FC<IAuditSectionProps> = ({ baseAudit, lastUpdatedBy }) => {
  return (
    <>
      <Row className="no-gutters">
        <Col className="text-right">
          <StyledSmallText>
            <strong>Created: </strong>
            {prettyFormatUTCDate(baseAudit?.appCreateTimestamp)} by{' '}
            <UserNameTooltip
              userName={baseAudit?.appCreateUserid}
              userGuid={baseAudit?.appCreateUserGuid}
            />
          </StyledSmallText>
        </Col>
      </Row>
      <Row className="no-gutters">
        <Col className="text-right">
          <StyledSmallText>
            <strong>Updated: </strong>
            {prettyFormatUTCDate(lastUpdatedBy?.appLastUpdateTimestamp)} by{' '}
            <UserNameTooltip
              userName={lastUpdatedBy?.appLastUpdateUserid}
              userGuid={lastUpdatedBy?.appLastUpdateUserGuid}
            />
          </StyledSmallText>
        </Col>
      </Row>
    </>
  );
};

export default AuditSection;

const StyledSmallText = styled.span`
  font-size: 0.87em;
  line-height: 1.9;
`;
