import TabView from 'components/common/TabView';
import * as React from 'react';
import { Tab } from 'react-bootstrap';

interface IInventoryTabsProps {
  PropertyForm: React.ReactNode;
}

export enum InventoryTabNames {
  property = 'property',
  title = 'title',
  value = 'value',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const InventoryTabs: React.FunctionComponent<IInventoryTabsProps> = ({ PropertyForm }) => {
  return (
    <TabView defaultActiveKey={InventoryTabNames.property}>
      <Tab eventKey={InventoryTabNames.title} title="Title"></Tab>
      <Tab eventKey={InventoryTabNames.value} title="Value"></Tab>
      <Tab eventKey={InventoryTabNames.property} title="Property Details"></Tab>
    </TabView>
  );
};
