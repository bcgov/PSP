import clsx from 'classnames';
import { useContext, useMemo, useState } from 'react';
import { matchPath, useHistory, useLocation } from 'react-router-dom';

import AcquisitionFileIcon from '@/assets/images/acquisition-icon.svg?react';
import AdminIcon from '@/assets/images/admin-icon.svg?react';
import ContactIcon from '@/assets/images/contact-icon.svg?react';
import DispositionIcon from '@/assets/images/disposition-icon.svg?react';
import HomeIcon from '@/assets/images/home-icon.svg?react';
import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import ProjectsIcon from '@/assets/images/projects-icon.svg?react';
import ResearchIcon from '@/assets/images/research-icon.svg?react';
import SubdivisionIcon from '@/assets/images/subdivision-icon.svg?react';
import { ExpandCollapseButton } from '@/components/common/buttons/ExpandCollapseButton';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { NavIcon } from '@/components/layout';
import { Claims, Roles } from '@/constants/index';

import { SidebarStateContext } from './SideNavbarContext';
import { SidebarContextType, SideTray } from './SideTray';
import * as Styled from './styles';

/**
 * Optional Collapsible side navigation bar that displays to the left of the page content.
 * Contains a list of navigation links that open a try of sub-navigation options.
 */
export const SideNavBar = () => {
  const [expanded, setExpanded] = useState(false);
  const { setTrayPage, trayPage } = useContext(SidebarStateContext);
  const history = useHistory();
  const location = useLocation();

  const isHomeMap = useMemo(
    () =>
      matchPath(location.pathname, {
        path: ['/mapview/sidebar/property/*', '/mapview'],
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isProject = useMemo(
    () =>
      matchPath(location.pathname, {
        path: ['/project/*', '/mapview/sidebar/project/*'],
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isResearch = useMemo(
    () =>
      matchPath(location.pathname, {
        path: ['/research/*', '/mapview/sidebar/research/*'],
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isAcquisition = useMemo(
    () =>
      matchPath(location.pathname, {
        path: ['/acquisition/*', '/mapview/sidebar/acquisition/*'],
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isLease = useMemo(
    () =>
      matchPath(location.pathname, {
        path: ['/lease/*', '/mapview/sidebar/lease/*'],
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isDisposition = useMemo(
    () =>
      matchPath(location.pathname, {
        path: ['/disposition/*', '/mapview/sidebar/disposition/*'],
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isConsSub = useMemo(
    () =>
      matchPath(location.pathname, {
        path: ['/mapview/sidebar/subdivision/*', '/mapview/sidebar/consolidation/*'],
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isContacts = useMemo(
    () =>
      matchPath(location.pathname, {
        path: '/contact/*',
        exact: true,
        strict: true,
      }),
    [location],
  );

  const isAdminTools = useMemo(
    () =>
      matchPath(location.pathname, {
        path: '/admin/*',
        exact: true,
        strict: true,
      }),
    [location],
  );
  return (
    <Styled.ZIndexWrapper>
      <Styled.SideNavBar className={clsx({ expanded: expanded })}>
        <NavIcon
          onClick={() => {
            history.push('/mapview');
          }}
          icon={<HomeIcon />}
          text="Map View"
          showText={expanded}
          isNavActive={isHomeMap != null}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.PROJECT)}
          icon={<ProjectsIcon />}
          text="Project"
          showText={expanded}
          claims={[Claims.PROJECT_VIEW]}
          isNavActive={isProject != null}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.RESEARCH)}
          icon={<ResearchIcon />}
          text="Research"
          showText={expanded}
          isNavActive={isResearch != null}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.ACQUISITION)}
          icon={<AcquisitionFileIcon />}
          text="Acquisition"
          showText={expanded}
          isNavActive={isAcquisition != null}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.LEASE)}
          icon={<LeaseIcon />}
          text="Leases & Licences"
          showText={expanded}
          isNavActive={isLease != null}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.DISPOSITION)}
          icon={<DispositionIcon />}
          text="Disposition"
          showText={expanded}
          isNavActive={isDisposition != null}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.SUBDCONS)}
          icon={<SubdivisionIcon />}
          text="Subdivision & Consolidation"
          showText={expanded}
          isNavActive={isConsSub != null}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.CONTACT)}
          icon={<ContactIcon />}
          text="Contacts"
          showText={expanded}
          claims={[Claims.CONTACT_VIEW]}
          isNavActive={isContacts != null}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.ADMIN)}
          icon={<AdminIcon />}
          text="Admin Tools"
          showText={expanded}
          roles={[Roles.SYSTEM_ADMINISTRATOR]}
          isNavActive={isAdminTools != null}
        />
        <TooltipWrapper
          tooltipId="expand-navbar"
          tooltip={expanded ? 'Collapse Menu' : 'Expand Menu'}
        >
          <span className="to-bottom">
            <ExpandCollapseButton
              expanded={expanded}
              toggleExpanded={() => setExpanded(!expanded)}
            />
          </span>
        </TooltipWrapper>
      </Styled.SideNavBar>
      <SideTray context={trayPage} setContext={setTrayPage} />
    </Styled.ZIndexWrapper>
  );
};
