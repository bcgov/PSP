import { Claims } from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Link } from 'react-router-dom';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

export const ProjectTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <Styled.TrayHeader>Projects</Styled.TrayHeader>
      {true && (
        <Link onClick={onLinkClick} to="/project/list">
          Manage Projects
        </Link>
      )}
      {/* {hasClaim(Claims.PROJECT_VIEW) && (
        <Link onClick={onLinkClick} to="/project/list">
          Manage Projects
        </Link>
      )} */}
    </>
  );
};
