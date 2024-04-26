import { Tab } from 'react-bootstrap';
import { generatePath, useHistory, useRouteMatch } from 'react-router-dom';

import TabView from '@/components/common/TabView';

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
}

export enum InventoryTabNames {
  property = 'details',
  title = 'ltsa',
  value = 'bcassessment',
  research = 'research',
  pims = 'pims',
  takes = 'takes',
  management = 'management',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const InventoryTabs: React.FunctionComponent<
  React.PropsWithChildren<IInventoryTabsProps>
> = ({ defaultTabKey, tabViews, activeTab }) => {
  const history = useHistory();
  const match = useRouteMatch<{
    propertyId: string;
    menuIndex: string;
    id: string;
    researchId: string;
  }>();
  return (
    <TabView
      defaultActiveKey={defaultTabKey}
      activeKey={activeTab}
      onSelect={(eventKey: string | null) => {
        const tab = Object.values(InventoryTabNames).find(value => value === eventKey);
        if (match.path.includes('acquisition') || match.path.includes('disposition')) {
          const path = generatePath(match.path, {
            menuIndex: match.params.menuIndex,
            id: match.params.id,
            tab,
          });
          history.push(path);
        } else if (match.path.includes('research')) {
          const path = generatePath(match.path, {
            menuIndex: match.params.menuIndex,
            researchId: match.params.researchId,
            tab,
          });
          history.push(path);
        } else {
          const path = generatePath(match.path, {
            propertyId: match.params.propertyId,
            tab,
          });
          history.push(path);
        }
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
