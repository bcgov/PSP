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
            <FaBriefcase title="Project Icon" fill="currentColor" />
          </span>
          Projects
        </Styled.TrayHeader>
        {hasClaim(Claims.PROJECT_VIEW) && (
          <Link onClick={onLinkClick} to="/project/list" className="pl-9 pb-3 nav-item">
            Manage Projects
          </Link>
        )}
        {hasClaim(Claims.PROJECT_ADD) && (
          <Link
            onClick={onLinkClick}
            to="/mapview/sidebar/project/new"
            className="pl-9 pb-3 nav-item"
          >
            Create Project
          </Link>
        )}
      </HalfHeightDiv>
      <HalfHeightDiv>
        {hasClaim(Claims.PROJECT_VIEW) && (
          <>
            <ExportH3 className="mt-5">
              <span className="mr-2">
                <FaFileExcel />
              </span>
              Exports
            </ExportH3>
            <ProjectExportContainer View={ProjectExportForm} />
          </>
        )}
      </HalfHeightDiv>
    </>
  );
};
