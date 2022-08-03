import TabView from 'components/common/TabView';
import * as React from 'react';
import { Tab } from 'react-bootstrap';

export interface TabResearchView {
  content: React.ReactNode;
  key: ResearchTabNames;
  name: string;
}

interface IResearchTabsProps {
  defaultTabKey: ResearchTabNames;
  tabViews: TabResearchView[];
  activeTab: ResearchTabNames;
  setActiveTab: (tab: ResearchTabNames) => void;
}

export enum ResearchTabNames {
  researchDetails = 'researchDetails',
  activities = 'activities',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const ResearchTabs: React.FunctionComponent<IResearchTabsProps> = ({
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
        const tab = Object.values(ResearchTabNames).find(value => value === eventKey);
        tab && setActiveTab(tab);
      }}
    >
      {tabViews.map((view: TabResearchView, index: number) => (
        <Tab eventKey={view.key} title={view.name} key={`inventory-tab-${index}`}>
          {view.content}
        </Tab>
      ))}
    </TabView>
  );
};
