/* eslint-disable react/jsx-key */
import clsx from 'classnames';
import { ReactElement, useEffect, useState } from 'react';
import { FaBriefcase } from 'react-icons/fa';
import ReactVisibilitySensor from 'react-visibility-sensor';

import AcquisitionFileIcon from '@/assets/images/acquisition-icon.svg?react';
import AdminIcon from '@/assets/images/admin-icon.svg?react';
import ContactIcon from '@/assets/images/contact-icon.svg?react';
import DispositionFileIcon from '@/assets/images/disposition-icon.svg?react';
import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import ResearchFileIcon from '@/assets/images/research-icon.svg?react';
import SubdivisionIcon from '@/assets/images/subdivision-icon.svg?react';
import { AdminTools, LeaseAndLicenses, ResearchTray } from '@/components/layout';

import { AcquisitionTray } from './AcquisitionTray';
import { ContactTray } from './ContactTray';
import { DispositionTray } from './DispositionTray';
import { ProjectTray } from './ProjectTray';
import { SideTrayLayout } from './SideTrayLayout';
import * as Styled from './styles';
import { SubdivisionConsolidationTray } from './SubdCons';

export enum SidebarContextType {
  ADMIN = 'admin',
  PROJECT = 'project',
  LEASE = 'lease',
  RESEARCH = 'research',
  CONTACT = 'contact',
  ACQUISITION = 'acquisition',
  DISPOSITION = 'disposition',
  SUBDCONS = 'subdivision-consolidation',
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

  const onClose = () => {
    setShow(false);
  };

  const sideTrayPages: Map<SidebarContextType, ReactElement> = new Map<
    SidebarContextType,
    ReactElement
  >([
    [
      SidebarContextType.ADMIN,
      <SideTrayLayout
        icon={
          <AdminIcon title="Admin Tools icon" width="2.6rem" height="2.6rem" fill="currentColor" />
        }
        title="Admin Tools"
        onClose={onClose}
      >
        <AdminTools onLinkClick={onClose} />
      </SideTrayLayout>,
    ],
    [
      SidebarContextType.LEASE,
      <SideTrayLayout
        icon={<LeaseIcon title="Lease and Licence icon" fill="currentColor" />}
        title="Leases & Licences"
        onClose={onClose}
      >
        <LeaseAndLicenses onLinkClick={onClose} />
      </SideTrayLayout>,
    ],
    [
      SidebarContextType.RESEARCH,
      <SideTrayLayout
        icon={<ResearchFileIcon title="Research file icon" fill="currentColor" />}
        title="Research Files"
        onClose={onClose}
      >
        <ResearchTray onLinkClick={onClose} />
      </SideTrayLayout>,
    ],
    [
      SidebarContextType.CONTACT,
      <SideTrayLayout
        icon={
          <ContactIcon
            title="Contact manager icon"
            width="2.6rem"
            height="2.6rem"
            fill="currentColor"
          />
        }
        title="Contacts"
        onClose={onClose}
      >
        <ContactTray onLinkClick={onClose} />
      </SideTrayLayout>,
    ],
    [
      SidebarContextType.ACQUISITION,
      <SideTrayLayout
        icon={
          <AcquisitionFileIcon
            title="Acquisition file icon"
            width="2.6rem"
            height="2.6rem"
            fill="currentColor"
          />
        }
        title="Acquisition Files"
        onClose={onClose}
      >
        <AcquisitionTray onLinkClick={onClose} />
      </SideTrayLayout>,
    ],
    [
      SidebarContextType.PROJECT,
      <SideTrayLayout
        icon={<FaBriefcase size={26} title="Project icon" fill="currentColor" />}
        title="Projects"
        onClose={onClose}
      >
        <ProjectTray onLinkClick={onClose} />
      </SideTrayLayout>,
    ],
    [
      SidebarContextType.DISPOSITION,
      <SideTrayLayout
        icon={
          <DispositionFileIcon
            title="Disposition file Icon"
            width="2.6rem"
            height="2.6rem"
            fill="currentColor"
          />
        }
        title="Disposition Files"
        onClose={onClose}
      >
        <DispositionTray onLinkClick={onClose} />
      </SideTrayLayout>,
    ],
    [
      SidebarContextType.SUBDCONS,
      <SideTrayLayout
        icon={
          <SubdivisionIcon
            width="2.6rem"
            height="2.6rem"
            title="Subdivision Cons Icon"
            fill="currentColor"
          />
        }
        title="Subdivision & Consolidation"
        onClose={onClose}
      >
        <SubdivisionConsolidationTray onLinkClick={onClose} />
      </SideTrayLayout>,
    ],
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
        <Styled.SideTrayPage>
          <TrayPage />
        </Styled.SideTrayPage>
      </Styled.SideTray>
    </ReactVisibilitySensor>
  );
};
