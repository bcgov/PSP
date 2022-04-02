import TabView from 'components/common/TabView';
import * as React from 'react';
import { Tab } from 'react-bootstrap';

interface IPropertySelectorTabsViewProps {
  MapSelectorView: React.ReactNode;
  ListSelectorView: React.ReactNode;
}

export enum SelectorTabNames {
  map = 'map',
  list = 'list',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const PropertySelectorTabsView: React.FunctionComponent<IPropertySelectorTabsViewProps> = ({
  MapSelectorView,
  ListSelectorView,
}) => {
  return (
    <TabView defaultActiveKey={SelectorTabNames.map}>
      <Tab eventKey={SelectorTabNames.map} title="Locate on Map">
        {MapSelectorView}
      </Tab>
      <Tab eventKey={SelectorTabNames.list} title="Search"></Tab>
    </TabView>
  );
};
