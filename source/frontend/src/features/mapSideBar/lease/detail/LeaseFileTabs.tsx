import React from 'react';
import { Tab } from 'react-bootstrap';

import TabView from '@/components/common/TabView';

export interface LeaseTabFileView {
  content: React.ReactNode;
  key: LeaseFileTabNames;
  name: string;
}

interface ILeaseFileTabsProps {
  defaultTabKey: LeaseFileTabNames;
  tabViews: LeaseTabFileView[];
  activeTab: LeaseFileTabNames;
  setActiveTab: (tab: LeaseFileTabNames) => void;
}

export enum LeaseFileTabNames {
  fileDetails = 'fileDetails',
  tenant = 'tenant',
  improvements = 'improvements',
  insurance = 'insurance',
  deposit = 'deposit',
  payments = 'payments',
  surplusDeclaration = 'surplusDeclaration',
  documents = 'documents',
  notes = 'notes',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const LeaseFileTabs: React.FC<ILeaseFileTabsProps> = ({
  defaultTabKey,
  tabViews,
  activeTab,
  setActiveTab,
}) => {
  return (
    <TabView
      defaultActiveKey={defaultTabKey}
      mountOnEnter
      unmountOnExit
      activeKey={activeTab}
      onSelect={(eventKey: string | null) => {
        const tab = Object.values(LeaseFileTabNames).find(value => value === eventKey);
        tab && setActiveTab(tab);
      }}
    >
      {tabViews.map((view: LeaseTabFileView, index: number) => (
        <Tab eventKey={view.key} title={view.name} key={`lease-tab-${index}`}>
          {view.content}
        </Tab>
      ))}
    </TabView>
  );
};
