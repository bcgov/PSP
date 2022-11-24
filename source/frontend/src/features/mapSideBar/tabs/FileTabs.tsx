import TabView from 'components/common/TabView';
import * as React from 'react';
import { Tab } from 'react-bootstrap';

export interface TabFileView {
  content: React.ReactNode;
  key: FileTabNames;
  name: string;
}

interface IFileTabsProps {
  defaultTabKey: FileTabNames;
  tabViews: TabFileView[];
  activeTab: FileTabNames;
  setActiveTab: (tab: FileTabNames) => void;
}

export enum FileTabNames {
  fileDetails = 'fileDetails',
  activities = 'activities',
  documents = 'documents',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const FileTabs: React.FunctionComponent<IFileTabsProps> = ({
  defaultTabKey,
  tabViews,
  activeTab,
  setActiveTab,
}) => {
  return (
    <TabView
      defaultActiveKey={defaultTabKey}
      activeKey={activeTab}
      onSelect={(eventKey: string | null) => {
        const tab = Object.values(FileTabNames).find(value => value === eventKey);
        tab && setActiveTab(tab);
      }}
    >
      {tabViews.map((view: TabFileView, index: number) => (
        <Tab eventKey={view.key} title={view.name} key={`file-tab-${index}`}>
          {view.content}
        </Tab>
      ))}
    </TabView>
  );
};
