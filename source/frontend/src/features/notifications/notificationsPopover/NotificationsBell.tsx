import { FC, useState } from 'react';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Popover from 'react-bootstrap/Popover';
import { FaBell } from 'react-icons/fa';
import styled from 'styled-components';

import variables from '@/assets/scss/_variables.module.scss';
import { exists } from '@/utils/utils';

export interface INotificationsBellProps {
  unreadCount?: number;
  popoverContent?: React.ReactNode;
  onToggle?: (nextShow: boolean) => void;
}

export const NotificationsBell: FC<INotificationsBellProps> = ({
  unreadCount = 0,
  popoverContent,
  onToggle,
}) => {
  const [show, setShow] = useState(false);

  const popover = (
    <StyledPopover id="notifications-popover">
      <Popover.Content>
        {popoverContent}
        {/* <NotificationsListView
          items={items}
          hasMore={hasMore}
          isLoading={loading}
          onLoadMore={handleLoadMore}
          onSelect={handleSelect}
          onToggleRead={handleToggleRead}
        /> */}
      </Popover.Content>
    </StyledPopover>
  );

  return (
    <OverlayTrigger
      trigger="click"
      placement="bottom-end"
      overlay={popover}
      show={show}
      onToggle={(next: boolean) => {
        setShow(next);
        if (exists(onToggle)) {
          onToggle(next);
        }
      }}
      rootClose
      rootCloseEvent="mousedown"
    >
      <StyledBellButton
        type="button"
        aria-label={unreadCount > 0 ? `Notifications (${unreadCount} unread)` : 'Notifications'}
        title="Notifications"
      >
        <FaBell aria-hidden="true" />
        {unreadCount > 0 && <BadgeDot>{unreadCount > 99 ? '99+' : unreadCount}</BadgeDot>}
      </StyledBellButton>
    </OverlayTrigger>
  );
};

export default NotificationsBell;

const StyledBellButton = styled.button`
  position: relative;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  background: transparent;
  border: none;
  color: #ffffff;
  cursor: pointer;
  padding: 0.4rem;
  margin-right: 0.8rem;
  font-size: 2rem;

  &:hover,
  &:focus {
    color: ${variables.pimsBlue10};
    outline: none;
  }
`;

const BadgeDot = styled.span`
  position: absolute;
  top: -0.3rem;
  right: -0.3rem;
  min-width: 1.6rem;
  height: 1.6rem;
  padding: 0 0.4rem;
  border-radius: 50%;
  background-color: ${variables.pimsRed100};
  color: #ffffff;
  font-size: 1rem;
  font-weight: 700;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  line-height: 1;
`;

const StyledPopover = styled(Popover)`
  min-width: 56rem;
  max-width: 56rem;
  box-shadow: 0 0.4rem 1.6rem rgba(0, 0, 0, 0.18);
  border: 1px solid ${variables.borderOutlineColor ?? '#c8d6e0'};
  border-radius: 1rem;
  font-family: inherit;
  font-size: inherit;
  font-weight: normal;

  .popover-body {
    padding: 0;
  }
`;
