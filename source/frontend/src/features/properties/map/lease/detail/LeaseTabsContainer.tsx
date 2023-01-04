import { Claims } from 'constants/claims';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import { LeasePageNames, leasePages } from 'features/leases';
import {
  LeaseFileTabNames,
  LeaseFileTabs,
  LeaseTabFileView,
} from 'features/mapSideBar/tabs/LeaseFileTabs';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { ILease } from 'interfaces';
import React, { useState } from 'react';

import { LeaseTab } from './LeaseTab';

export interface ILeaseTabsContainerProps {
  lease?: ILease;
  refreshLease: () => void;
  setLease: (lease: ILease) => void;
}

export const LeaseTabsContainer: React.FC<ILeaseTabsContainerProps> = ({
  lease,
  setLease,
  refreshLease,
}) => {
  const tabViews: LeaseTabFileView[] = [];
  const { hasClaim } = useKeycloakWrapper();

  tabViews.push({
    content: (
      <LeaseTab
        lease={lease}
        setLease={setLease}
        refreshLease={refreshLease}
        leasePage={leasePages.get(LeasePageNames.DETAILS)}
      />
    ),
    key: LeaseFileTabNames.fileDetails,
    name: 'File details',
  });

  tabViews.push({
    content: (
      <LeaseTab
        lease={lease}
        setLease={setLease}
        refreshLease={refreshLease}
        leasePage={leasePages.get(LeasePageNames.TENANT)}
      />
    ),
    key: LeaseFileTabNames.tenant,
    name: 'Tenant',
  });

  tabViews.push({
    content: (
      <LeaseTab
        lease={lease}
        setLease={setLease}
        refreshLease={refreshLease}
        leasePage={leasePages.get(LeasePageNames.IMPROVEMENTS)}
      />
    ),
    key: LeaseFileTabNames.improvements,
    name: 'Improvements',
  });

  tabViews.push({
    content: (
      <LeaseTab
        lease={lease}
        setLease={setLease}
        refreshLease={refreshLease}
        leasePage={leasePages.get(LeasePageNames.INSURANCE)}
      />
    ),
    key: LeaseFileTabNames.insurance,
    name: 'Insurance',
  });

  tabViews.push({
    content: (
      <LeaseTab
        lease={lease}
        setLease={setLease}
        refreshLease={refreshLease}
        leasePage={leasePages.get(LeasePageNames.DEPOSIT)}
      />
    ),
    key: LeaseFileTabNames.deposit,
    name: 'Deposit',
  });

  tabViews.push({
    content: (
      <LeaseTab
        lease={lease}
        setLease={setLease}
        refreshLease={refreshLease}
        leasePage={leasePages.get(LeasePageNames.PAYMENTS)}
      />
    ),
    key: LeaseFileTabNames.payments,
    name: 'Payments',
  });

  tabViews.push({
    content: (
      <LeaseTab
        lease={lease}
        setLease={setLease}
        refreshLease={refreshLease}
        leasePage={leasePages.get(LeasePageNames.SURPLUS)}
      />
    ),
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
