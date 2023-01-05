import { ContactManagerModal } from 'components/contact/ContactManagerModal';
import { IContactSearchResult } from 'interfaces';
import * as React from 'react';
import { Button } from 'react-bootstrap';

interface IAddLeaseTenantListViewProps {
  selectedTenants: IContactSearchResult[];
  setSelectedTenants: (selectedTenants: IContactSearchResult[]) => void;
}

export const AddLeaseTenantListView: React.FunctionComponent<
  React.PropsWithChildren<IAddLeaseTenantListViewProps>
> = ({ selectedTenants, setSelectedTenants }) => {
  const [showContactManager, setShowContactManager] = React.useState<boolean>(false);
  const handleContactManagerOk = () => {
    setShowContactManager(false);
  };
  return (
    <>
      <div>
        {' '}
        <Button
          variant="secondary"
          onClick={() => {
            setShowContactManager(true);
          }}
        >
          Select Tenant(s)
        </Button>
      </div>
      <ContactManagerModal
        selectedRows={selectedTenants}
        setSelectedRows={setSelectedTenants}
        display={showContactManager}
        setDisplay={setShowContactManager}
        handleModalOk={handleContactManagerOk}
        handleModalCancel={() => {
          setShowContactManager(false);
          setSelectedTenants([]);
        }}
        showActiveSelector={true}
      ></ContactManagerModal>
    </>
  );
};

export default AddLeaseTenantListView;
