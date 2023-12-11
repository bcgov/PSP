import { FaFileExcel } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import ProjectExportContainer from '@/features/projects/reports/ProjectExportContainer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import ProjectExportForm from '../../../features/projects/reports/ProjectExportForm';
import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';
import { ExportH3 } from './styles';

export const ProjectTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <Styled.TrayHeader>Projects</Styled.TrayHeader>
      {hasClaim(Claims.PROJECT_VIEW) && (
        <Link onClick={onLinkClick} to="/project/list">
          Manage Projects
        </Link>
      )}
      {hasClaim(Claims.PROJECT_ADD) && (
        <Link onClick={onLinkClick} to="/mapview/sidebar/project/new">
          Create Project
        </Link>
      )}
      {hasClaim(Claims.PROJECT_VIEW) && (
        <>
          <ExportH3 className="mt-5">
            <FaFileExcel />
            Exports
          </ExportH3>
          <ProjectExportContainer View={ProjectExportForm} />
        </>
      )}
    </>
  );
};
