import * as React from 'react';
import { Tab, Tabs } from 'react-bootstrap';

interface IInventoryTabsProps {
  PropertyForm: React.ReactNode;
}

export enum InventoryTabNames {
  property = 'property',
  title = 'title',
  owner = 'owner',
  value = 'value',
  history = 'history',
  notes = 'notes',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
const InventoryTabs: React.FunctionComponent<IInventoryTabsProps> = ({ PropertyForm }) => {
  return (
    <Tabs defaultActiveKey={InventoryTabNames.property}>
      <Tab eventKey={InventoryTabNames.property} title="Property">
        {PropertyForm}
      </Tab>
      <Tab eventKey={InventoryTabNames.title} title="Title"></Tab>
      <Tab eventKey={InventoryTabNames.owner} title="Owner"></Tab>
      <Tab eventKey={InventoryTabNames.value} title="Value"></Tab>
      <Tab eventKey={InventoryTabNames.history} title="History"></Tab>
      <Tab eventKey={InventoryTabNames.notes} title="Notes"></Tab>
    </Tabs>
  );
};

export default InventoryTabs;
