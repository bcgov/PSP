import clsx from 'classnames';
import { useContext, useState } from 'react';
import { FaBriefcase } from 'react-icons/fa';
import { MdContactMail, MdFence, MdHome } from 'react-icons/md';
import { TbArrowBounce } from 'react-icons/tb';
import { useHistory } from 'react-router-dom';

import AdminPanelSettings from '@/assets/images/admin-panel-settings.svg?react';
import RealEstateAgent from '@/assets/images/real-estate-agent.svg?react';
import Source from '@/assets/images/source.svg?react';
import ConsolidateSubdivideIcon from '@/assets/images/subdivisionconsolidation.svg?react';
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
  return (
    <Styled.ZIndexWrapper>
      <Styled.SideNavBar className={clsx({ expanded: expanded })}>
        <NavIcon
          onClick={() => history.push('/mapview')}
          icon={<MdHome size={24} />}
          text="Home"
          showText={expanded}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.PROJECT)}
          icon={<FaBriefcase size={24} />}
          text="Project"
          showText={expanded}
          claims={[Claims.PROJECT_VIEW]}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.RESEARCH)}
          icon={<Source />}
          text="Research"
          showText={expanded}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.ACQUISITION)}
          icon={<RealEstateAgent />}
          text="Acquisition"
          showText={expanded}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.LEASE)}
          icon={<MdFence size={24} />}
          text="Leases & Licences"
          showText={expanded}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.DISPOSITION)}
          icon={<TbArrowBounce size={24} color="white" fillOpacity={0} />}
          text="Disposition"
          showText={expanded}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.SUBDCONS)}
          icon={<ConsolidateSubdivideIcon className="mr-1" />}
          text="Subdivision & Consolidation"
          showText={expanded}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.CONTACT)}
          icon={<MdContactMail size={24} />}
          text="Contacts"
          showText={expanded}
          claims={[Claims.CONTACT_VIEW]}
        />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.ADMIN)}
          icon={<AdminPanelSettings />}
          text="Admin Tools"
          showText={expanded}
          roles={[Roles.SYSTEM_ADMINISTRATOR]}
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
