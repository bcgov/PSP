import { Col } from 'react-bootstrap';

import AuditSection from '@/components/common/HeaderField/AuditSection';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import StatusField from '@/components/common/HeaderField/StatusField';
import { StyledFiller, StyledRow } from '@/components/common/HeaderField/styles';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { exists } from '@/utils';

export interface IProjectHeaderProps {
  project?: ApiGen_Concepts_Project;
}

const ProjectHeader: React.FunctionComponent<React.PropsWithChildren<IProjectHeaderProps>> = ({
  project,
}) => {
  return (
    <StyledRow className="no-gutters">
      <Col xs="7">
        <HeaderField label="Project:" labelWidth="3" contentWidth="9">
          {project?.code} {project?.description}
        </HeaderField>
        <HeaderField label="MoTI region:" labelWidth="3" contentWidth="9">
          {project?.regionCode?.description}
        </HeaderField>
      </Col>
      <Col>
        <StyledFiller>
          <AuditSection baseAudit={project} />
          {exists(project?.projectStatusTypeCode) && (
            <StatusField statusCodeType={project?.projectStatusTypeCode} />
          )}
        </StyledFiller>
      </Col>
    </StyledRow>
  );
};

export default ProjectHeader;
