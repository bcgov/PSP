import { FaBriefcase, FaFileExcel } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import ProjectExportContainer from '@/features/projects/reports/ProjectExportContainer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import ProjectExportForm from '../../../features/projects/reports/ProjectExportForm';
import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';
import { ExportH3, HalfHeightDiv } from './styles';

export const ProjectTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <HalfHeightDiv>
        <Styled.TrayHeader>
          <span className="mr-2">
            <FaBriefcase size={26} />
          </span>
          Projects
        </Styled.TrayHeader>
        {hasClaim(Claims.PROJECT_VIEW) && (
          <Link className="pl-9 pb-3" onClick={onLinkClick} to="/project/list">
            Manage Projects
          </Link>
        )}
        {hasClaim(Claims.PROJECT_ADD) && (
          <Link className="pl-9 pb-3" onClick={onLinkClick} to="/mapview/sidebar/project/new">
            Create Project
          </Link>
        )}
      </HalfHeightDiv>

      {hasClaim(Claims.PROJECT_VIEW) && (
        <HalfHeightDiv>
          <ExportH3 className="mt-5">
            <span className="mr-4">
              <FaFileExcel />
            </span>
            Exports
          </ExportH3>
          <ProjectExportContainer View={ProjectExportForm} />
        </HalfHeightDiv>
      )}
    </>
  );
};
