import clsx from 'classnames';
import { AdminTools, LeaseAndLicenses, ResearchTray } from 'components/layout';
import { ReactElement, useEffect, useState } from 'react';
import ReactVisibilitySensor from 'react-visibility-sensor';

import { AcquisitionTray } from './AcquisitionTray';
import { ContactTray } from './ContactTray';
import { ProjectTray } from './ProjectTray';
import * as Styled from './styles';

export enum SidebarContextType {
  ADMIN = 'admin',
  PROJECT = 'project',
  LEASE = 'lease',
  RESEARCH = 'research',
  CONTACT = 'contact',
  ACQUISITION = 'acquisition',
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

  const sideTrayPages: Map<SidebarContextType, ReactElement> = new Map<
    SidebarContextType,
    ReactElement
  >([
    [SidebarContextType.ADMIN, <AdminTools onLinkClick={() => setShow(false)} />],
    [SidebarContextType.LEASE, <LeaseAndLicenses onLinkClick={() => setShow(false)} />],
    [SidebarContextType.RESEARCH, <ResearchTray onLinkClick={() => setShow(false)} />],
    [SidebarContextType.CONTACT, <ContactTray onLinkClick={() => setShow(false)} />],
    [SidebarContextType.ACQUISITION, <AcquisitionTray onLinkClick={() => setShow(false)} />],
    [SidebarContextType.PROJECT, <ProjectTray onLinkClick={() => setShow(false)} />],
  ]);

  useEffect(() => {
    setShow(!!context);
  }, [context, setShow]);
  const TrayPage = () => (context ? sideTrayPages.get(context) ?? <></> : <></>);
  return (
    <ReactVisibilitySensor
      onChange={(isVisible: boolean) => {
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
        <Styled.SideTrayPage>
          <TrayPage />
        </Styled.SideTrayPage>
      </Styled.SideTray>
    </ReactVisibilitySensor>
  );
};
