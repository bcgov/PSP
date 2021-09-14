import clsx from 'classnames';
import { AdminTools, LeaseAndLicenses } from 'components/layout';
import { useEffect, useState } from 'react';
import { ReactElement } from 'react';
import { useHistory } from 'react-router-dom';
import ReactVisibilitySensor from 'react-visibility-sensor';

import * as Styled from './styles';

export enum SidebarContextType {
  ADMIN = 'admin',
  LEASE = 'lease',
}

interface ISideTrayProps {
  context?: SidebarContextType;
  setContext: (context?: SidebarContextType) => void;
}

export interface ISideTrayPageProps {
  onLinkClick: () => void;
}

export const SideTray = ({ context, setContext }: ISideTrayProps) => {
  const [show, setShow] = useState(false);
  const {
    location: { pathname },
  } = useHistory();

  useEffect(() => {
    setShow(!!context);
  }, [context, setShow]);
  useEffect(() => {
    setShow(false);
  }, [pathname, setContext]);
  return (
    <ReactVisibilitySensor
      onChange={isVisible => {
        !isVisible && setContext(undefined);
      }}
    >
      <Styled.SideTray className={clsx({ show: show })} data-testid="side-tray">
        <Styled.CloseButton
          id="close-tray"
          title="close"
          size={32}
          onClick={() => setShow(false)}
        />
        <Styled.SideTrayPage>{context ? sideTrayPages.get(context) : null}</Styled.SideTrayPage>
      </Styled.SideTray>
    </ReactVisibilitySensor>
  );
};

const sideTrayPages: Map<SidebarContextType, ReactElement> = new Map<
  SidebarContextType,
  ReactElement
>([
  [SidebarContextType.ADMIN, <AdminTools />],
  [SidebarContextType.LEASE, <LeaseAndLicenses />],
]);
