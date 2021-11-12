import { ReactComponent as AdminPanelSettings } from 'assets/images/admin-panel-settings.svg';
import { ReactComponent as Stops } from 'assets/images/airline-stops.svg';
import { ReactComponent as Forms } from 'assets/images/dynamic-form.svg';
import { ReactComponent as Fence } from 'assets/images/fence.svg';
import { ReactComponent as Insights } from 'assets/images/insights.svg';
import { ReactComponent as Inventory } from 'assets/images/inventory.svg';
import { ReactComponent as RealEstateAgent } from 'assets/images/real-estate-agent.svg';
import { ReactComponent as Source } from 'assets/images/source.svg';
import clsx from 'classnames';
import { NavIcon } from 'components/layout';
import { Claims, Roles } from 'constants/index';
import noop from 'lodash/noop';
import { useContext } from 'react';
import { useState } from 'react';
import { MdChevronLeft, MdChevronRight, MdContactMail, MdHome } from 'react-icons/md';
import { useHistory } from 'react-router-dom';

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
        <NavIcon onClick={noop} icon={<Source />} text="Research" showText={expanded} />
        <NavIcon onClick={noop} icon={<RealEstateAgent />} text="Acquisition" showText={expanded} />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.LEASE)}
          icon={<Fence />}
          text="Management"
          showText={expanded}
        />
        <NavIcon onClick={noop} icon={<Stops />} text="Disposition" showText={expanded} />
        <NavIcon
          onClick={() => history.push('/contact/list')}
          icon={<MdContactMail size={24} />}
          text="Contacts"
          showText={expanded}
          claims={[Claims.CONTACT_VIEW]}
        />
        <NavIcon onClick={noop} icon={<Inventory />} text="Documents" showText={expanded} />
        <NavIcon onClick={noop} icon={<Forms />} text="Forms" showText={expanded} />
        <NavIcon onClick={noop} icon={<Insights />} text="Reporting" showText={expanded} />
        <NavIcon
          onClick={() => setTrayPage(SidebarContextType.ADMIN)}
          icon={<AdminPanelSettings />}
          text="Admin Tools"
          showText={expanded}
          roles={[Roles.SYSTEM_ADMINISTRATOR]}
        />
        {expanded ? (
          <MdChevronLeft
            title="collapse"
            className="chevron"
            size={24}
            onClick={() => setExpanded(false)}
          />
        ) : (
          <MdChevronRight
            title="expand"
            className="chevron"
            size={24}
            onClick={() => setExpanded(true)}
          />
        )}
      </Styled.SideNavBar>
      <SideTray context={trayPage} setContext={setTrayPage} />
    </Styled.ZIndexWrapper>
  );
};
