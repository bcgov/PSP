import { Claims } from 'constants/claims';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import {
  LeaseFileTabNames,
  LeaseFileTabs,
  LeaseTabFileView,
} from 'features/mapSideBar/tabs/LeaseFileTabs';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { ILease } from 'interfaces';
import React, { useState } from 'react';

export interface ILeaseTabsContainerProps {
  lease?: ILease;
}

export const LeaseTabsContainer: React.FC<ILeaseTabsContainerProps> = ({ lease }) => {
  const tabViews: LeaseTabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();

  tabViews.push({
    content: <></>,
    key: LeaseFileTabNames.fileDetails,
    name: 'File details',
  });

  tabViews.push({
    content: <></>,
    key: LeaseFileTabNames.tenant,
    name: 'Tenant',
  });

  tabViews.push({
    content: <></>,
    key: LeaseFileTabNames.improvements,
    name: 'Improvements',
  });

  tabViews.push({
    content: <></>,
    key: LeaseFileTabNames.insurance,
    name: 'Insurance',
  });

  tabViews.push({
    content: <></>,
    key: LeaseFileTabNames.deposit,
    name: 'Deposit',
  });

  tabViews.push({
    content: <></>,
    key: LeaseFileTabNames.payments,
    name: 'Payments',
  });

  tabViews.push({
    content: <></>,
    key: LeaseFileTabNames.surplusDeclaration,
    name: 'Surplus Declaration',
  });

  if (lease?.id && hasClaim(Claims.DOCUMENT_VIEW)) {
    tabViews.push({
      content: (
        <DocumentListContainer
          parentId={lease?.id}
          relationshipType={DocumentRelationshipType.LEASES}
          disableAdd
        />
      ),
      key: LeaseFileTabNames.documents,
      name: 'Documents',
    });
  }

  var defaultTab = LeaseFileTabNames.fileDetails;

  const [activeTab, setActiveTab] = useState<LeaseFileTabNames>(defaultTab);

  return (
    <LeaseFileTabs
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab}
      setActiveTab={setActiveTab}
    />
  );
};

export default LeaseTabsContainer;
