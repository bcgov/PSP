import * as React from 'react';
import { Tab } from 'react-bootstrap';
import styled from 'styled-components';

import TabView from '@/components/common/TabView';

interface IPropertySelectorTabsViewProps {
  MapSelectorView: React.ReactNode;
  ListSelectorView: React.ReactNode;
  activeTab: SelectorTabNames;
  setActiveTab: (tab: SelectorTabNames) => void;
}

export enum SelectorTabNames {
  map = 'map',
  list = 'list',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const PropertySelectorTabsView: React.FunctionComponent<
  React.PropsWithChildren<IPropertySelectorTabsViewProps>
> = ({ MapSelectorView, ListSelectorView, activeTab, setActiveTab }) => {
  return (
    <StyledTabView
      defaultActiveKey={SelectorTabNames.map}
      activeKey={activeTab}
      onSelect={(eventKey: string | null) => {
        const tab = Object.values(SelectorTabNames).find(value => value === eventKey);
        tab && setActiveTab(tab);
      }}
    >
      <Tab eventKey={SelectorTabNames.map} title="Locate on Map">
        {MapSelectorView}
      </Tab>
      <Tab eventKey={SelectorTabNames.list} title="Search">
        {ListSelectorView}
      </Tab>
    </StyledTabView>
  );
};

const StyledTabView = styled(TabView)`
  height: auto;
  &.tab-content {
    height: auto;
  }
`;
