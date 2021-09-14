import { ReactComponent as AdminPanelSettings } from 'assets/images/admin-panel-settings.svg';
import { ReactComponent as Fence } from 'assets/images/fence.svg';
import clsx from 'classnames';
import { NavIcon } from 'components/layout';
import { Roles } from 'constants/index';
import noop from 'lodash/noop';
import { useState } from 'react';
import { MdChevronLeft, MdChevronRight } from 'react-icons/md';

import * as Styled from './styles';
/**
 * Optional Collapsible side navigation bar that displays to the left of the page content.
 * Contains a list of navigation links that open a try of sub-navigation options.
 */
export const SideNavBar = () => {
  const [expanded, setExpanded] = useState(false);
  return (
    <Styled.SideNavBar className={clsx({ expanded: expanded }, 'd-flex flex-column')}>
      <NavIcon onClick={noop} icon={<Fence />} text="Management" showText={expanded} />
      <NavIcon
        onClick={noop}
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
  );
};
