import TabView from 'components/common/TabView';
import * as React from 'react';
import { Tab } from 'react-bootstrap';

interface IInventoryTabsProps {
  PropertyView: React.ReactNode;
  LtsaView: React.ReactNode;
}

export enum InventoryTabNames {
  property = 'property',
  title = 'title',
  value = 'value',
  styles = 'styles',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const InventoryTabs: React.FunctionComponent<IInventoryTabsProps> = ({
  PropertyView,
  LtsaView,
}) => {
  return (
    <TabView defaultActiveKey={InventoryTabNames.property}>
      <Tab eventKey={InventoryTabNames.title} title="Title">
        {LtsaView}
      </Tab>
      <Tab eventKey={InventoryTabNames.value} title="Value"></Tab>
      <Tab eventKey={InventoryTabNames.property} title="Property Details">
        {PropertyView}
      </Tab>
    </TabView>
  );
};
