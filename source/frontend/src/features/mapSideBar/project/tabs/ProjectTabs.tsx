import React from 'react';
import { Tab } from 'react-bootstrap';

import TabView from '@/components/common/TabView';

import { IProjectTabsProps } from './ProjectTabsContainer';

export interface ProjectTabView {
  content: React.ReactNode;
  key: ProjectTabNames;
  name: string;
}

export enum ProjectTabNames {
  projectDetails = 'projectDetails',
  notes = 'notes',
  documents = 'documents',
}

/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const ProjectTabs: React.FC<IProjectTabsProps> = ({
  defaultTabKey,
  tabViews,
  activeTab,
  setActiveTab,
}) => {
  return (
    <TabView
      mountOnEnter
      defaultActiveKey={defaultTabKey}
      activeKey={activeTab}
      onSelect={(eventKey: string | null) => {
        const tab = Object.values(ProjectTabNames).find(value => value === eventKey);
        tab && setActiveTab(tab);
      }}
    >
      {tabViews.map((view: ProjectTabView, index: number) => (
        <Tab eventKey={view.key} title={view.name} key={`project-tab-${index}`}>
          {view.content}
        </Tab>
      ))}
    </TabView>
  );
};
