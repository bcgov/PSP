import TabView from 'components/common/TabView';
import * as React from 'react';
import { Tab } from 'react-bootstrap';

export interface TabInventoryView {
  content: React.ReactNode;
  key: InventoryTabNames;
  name: string;
}

export interface IInventoryTabsProps {
  loading: boolean;
  defaultTabKey: InventoryTabNames;
  tabViews: TabInventoryView[];
  activeTab: InventoryTabNames;
  setActiveTab: (tab: InventoryTabNames) => void;
}

export enum InventoryTabNames {
  property = 'property',
  title = 'title',
  value = 'value',
  research = 'research',
  pims = 'pims',
  takes = 'takes',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const InventoryTabs: React.FunctionComponent<
  React.PropsWithChildren<IInventoryTabsProps>
> = ({ defaultTabKey, tabViews, activeTab, setActiveTab }) => {
  return (
    <TabView
      defaultActiveKey={defaultTabKey}
      activeKey={activeTab}
      onSelect={(eventKey: string | null) => {
        const tab = Object.values(InventoryTabNames).find(value => value === eventKey);
        tab && setActiveTab(tab);
      }}
    >
      {tabViews.map((view: TabInventoryView, index: number) => (
        <Tab eventKey={view.key} title={view.name} key={`inventory-tab-${index}`}>
          {view.content}
        </Tab>
      ))}
    </TabView>
  );
};
