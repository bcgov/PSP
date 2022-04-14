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
  console.log(PropertyView);
  const showPropertyInfo = PropertyView !== null;
  console.log(PropertyView);

  return (
    <TabView
      defaultActiveKey={showPropertyInfo ? InventoryTabNames.property : InventoryTabNames.title}
    >
      <Tab eventKey={InventoryTabNames.title} title="Title">
        {LtsaView}
      </Tab>
      <Tab eventKey={InventoryTabNames.value} title="Value"></Tab>
      {showPropertyInfo && (
        <Tab eventKey={InventoryTabNames.property} title="Property Details">
          {PropertyView}
        </Tab>
      )}
    </TabView>
  );
};
