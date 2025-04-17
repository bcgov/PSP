import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Base_BaseAudit } from '@/models/api/generated/ApiGen_Base_BaseAudit';
import { prettyFormatUTCDate } from '@/utils';

export interface IAuditSectionProps {
  baseAudit: ApiGen_Base_BaseAudit;
  lastUpdatedBy?: Api_LastUpdatedBy;
}

export const AuditSection: React.FC<IAuditSectionProps> = ({ baseAudit, lastUpdatedBy }) => {
  return (
    <>
      <Row className="no-gutters">
        <StyledCol>
          <StyledSmallText>
            <strong>Created: </strong>
            {prettyFormatUTCDate(baseAudit?.appCreateTimestamp)} by{' '}
            <UserNameTooltip
              userName={baseAudit?.appCreateUserid}
              userGuid={baseAudit?.appCreateUserGuid}
            />
          </StyledSmallText>
        </StyledCol>
      </Row>
      <Row className="no-gutters">
        <StyledCol>
          <StyledSmallText>
            <strong>Updated: </strong>
            {prettyFormatUTCDate(
              lastUpdatedBy?.appLastUpdateTimestamp ?? baseAudit?.appLastUpdateTimestamp,
            )}{' '}
            by{' '}
            <UserNameTooltip
              userName={lastUpdatedBy?.appLastUpdateUserid ?? baseAudit?.appLastUpdateUserid}
              userGuid={lastUpdatedBy?.appLastUpdateUserGuid ?? baseAudit?.appLastUpdateUserGuid}
            />
          </StyledSmallText>
        </StyledCol>
      </Row>
    </>
  );
};

export default AuditSection;

export const StyledSmallText = styled.span`
  font-size: 0.87em;
  line-height: 1.9;
`;

const StyledCol = styled(Col)`
  text-align: right;
  @media only screen and (max-width: 1199px) {
    text-align: left;
  }
`;
