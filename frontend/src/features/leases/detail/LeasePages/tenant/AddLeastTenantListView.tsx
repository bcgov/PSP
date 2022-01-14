import { IContactSearchResult } from 'interfaces';
import * as React from 'react';

import * as Styled from './styles';

interface IAddLeaseTenantListViewProps {
  selectedTenants: IContactSearchResult[];
  setSelectedTenants: (selectedTenants: IContactSearchResult[]) => void;
}

export const AddLeaseTenantListView: React.FunctionComponent<IAddLeaseTenantListViewProps> = ({
  selectedTenants,
  setSelectedTenants,
}) => {
  return (
    <>
      <Styled.TenantH2>Step 1 - Select Tenant(s)</Styled.TenantH2>
      <ul>
        <li>Search the Contacts list for the tenant(s) who are listed on the lease.</li>
        <li>Select the checkbox next to their name.</li>
        <li>Click the "add selected tenants" button.</li>
      </ul>
      <p>
        Repeat these steps for any additional tenants. Your selections will not be saved to the
        lease/license until you click the final "Save" button.
      </p>
      <Styled.ContactListViewWrapper
        setSelectedRows={setSelectedTenants}
        selectedRows={selectedTenants}
        showSelectedRowCount
        hideAddButton
      />
    </>
  );
};

export default AddLeaseTenantListView;
