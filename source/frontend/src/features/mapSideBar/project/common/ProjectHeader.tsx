import { Col } from 'react-bootstrap';

import AuditSection from '@/components/common/HeaderField/AuditSection';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import StatusField from '@/components/common/HeaderField/StatusField';
import { StyledFiller, StyledRow } from '@/components/common/HeaderField/styles';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { exists } from '@/utils';

import { StyledLeftHeaderPane } from '../../shared/header/styles';

export interface IProjectHeaderProps {
  project?: ApiGen_Concepts_Project;
}

const ProjectHeader: React.FunctionComponent<React.PropsWithChildren<IProjectHeaderProps>> = ({
  project,
}) => {
  return (
    <StyledRow className="no-gutters">
      <StyledLeftHeaderPane xl="7" xs="12">
        <HeaderField label="Project:" labelWidth={{ xs: 3 }} contentWidth={{ xs: 9 }}>
          {project?.code} {project?.description}
        </HeaderField>
        <HeaderField label="MOTT region:" labelWidth={{ xs: 3 }} contentWidth={{ xs: 9 }}>
          {project?.regionCode?.description}
        </HeaderField>
      </StyledLeftHeaderPane>
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
