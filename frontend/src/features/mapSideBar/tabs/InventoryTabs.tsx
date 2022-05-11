import TabView from 'components/common/TabView';
import * as React from 'react';
import { Tab } from 'react-bootstrap';

export interface TabInventoryView {
  content: React.ReactNode;
  key: InventoryTabNames;
  name: string;
}

interface IInventoryTabsProps {
  defaultTabKey: InventoryTabNames;
  tabViews: TabInventoryView[];
}

export enum InventoryTabNames {
  property = 'property',
  title = 'title',
  value = 'value',
  research = 'research',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const InventoryTabs: React.FunctionComponent<IInventoryTabsProps> = ({
  defaultTabKey,
  tabViews,
}) => {
  return (
    <TabView defaultActiveKey={defaultTabKey}>
      {tabViews.map((view: TabInventoryView, index: number) => (
        <Tab eventKey={view.key} title={view.name} key={`inventory-tab-${index}`}>
          {view.content}
        </Tab>
      ))}
    </TabView>
  );
};
