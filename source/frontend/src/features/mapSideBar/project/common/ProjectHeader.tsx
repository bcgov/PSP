import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { prettyFormatUTCDate } from '@/utils';

export interface IProjectHeaderProps {
  project?: ApiGen_Concepts_Project;
}

const ProjectHeader: React.FunctionComponent<React.PropsWithChildren<IProjectHeaderProps>> = ({
  project,
}) => {
  return (
    <StyledRow className="no-gutters">
      <Col xs="7">
        <Row className="no-gutters">
          <Col>
            <HeaderField label="Project:" labelWidth="3" contentWidth="9">
              {project?.code} {project?.description}
            </HeaderField>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col>
            <HeaderField label="MoTI Region:" labelWidth="3" contentWidth="9">
              {project?.regionCode?.description}
            </HeaderField>
          </Col>
        </Row>
      </Col>
      <Col xs="5">
        <Row className="no-gutters">
          <Col className="text-right">
            <StyleSmallText>
              Created: <strong>{prettyFormatUTCDate(project?.appCreateTimestamp)}</strong> by{' '}
              <UserNameTooltip
                userName={project?.appCreateUserid}
                userGuid={project?.appCreateUserGuid}
              />
            </StyleSmallText>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col className="text-right">
            <StyleSmallText>
              Last updated: <strong>{prettyFormatUTCDate(project?.appLastUpdateTimestamp)}</strong>{' '}
              by{' '}
              <UserNameTooltip
                userName={project?.appLastUpdateUserid}
                userGuid={project?.appLastUpdateUserGuid}
              />
            </StyleSmallText>
          </Col>
        </Row>
        <Row className="no-gutters">
          <Col>
            <HeaderField className="justify-content-end" label="Status:">
              {project?.projectStatusTypeCode?.description}
            </HeaderField>
          </Col>
        </Row>
      </Col>
    </StyledRow>
  );
};

export default ProjectHeader;

const StyledRow = styled(Row)`
  margin-top: 0.5rem;
  margin-bottom: 1.5rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
`;

const StyleSmallText = styled.span`
  font-size: 0.87em;
  line-height: 1.9;
`;
