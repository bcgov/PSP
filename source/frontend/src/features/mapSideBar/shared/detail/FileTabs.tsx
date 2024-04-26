import { Tab } from 'react-bootstrap';

import TabView from '@/components/common/TabView';

export interface TabFileView {
  content: React.ReactNode;
  key: FileTabType;
  name: string;
}

interface IFileTabsProps {
  defaultTabKey: FileTabType;
  tabViews: TabFileView[];
  activeTab: FileTabType;
  setActiveTab: (tab: FileTabType) => void;
}

export enum FileTabType {
  FILE_DETAILS = 'fileDetails',
  OFFERS_AND_SALE = 'offersAndSale',
  CHECKLIST = 'checklist',
  DOCUMENTS = 'documents',
  NOTES = 'notes',
  FORMS = 'forms',
  AGREEMENTS = 'agreements',
  COMPENSATIONS = 'compensations',
  STAKEHOLDERS = 'stakeholders',
  EXPROPRIATION = 'expropriation',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const FileTabs: React.FunctionComponent<React.PropsWithChildren<IFileTabsProps>> = ({
  defaultTabKey,
  tabViews,
  activeTab,
  setActiveTab,
}) => {
  return (
    <TabView
      unmountOnExit
      mountOnEnter
      defaultActiveKey={defaultTabKey}
      activeKey={activeTab}
      onSelect={(eventKey: string | null) => {
        const tab = Object.values(FileTabType).find(value => value === eventKey);
        if (tab && tab !== activeTab) {
          setActiveTab(tab);
        }
      }}
    >
      {tabViews.map((view: TabFileView, index: number) => (
        <Tab eventKey={view.key} title={view.name} key={`file-tab-${index}`}>
          {view.content}
        </Tab>
      ))}
    </TabView>
  );
};
