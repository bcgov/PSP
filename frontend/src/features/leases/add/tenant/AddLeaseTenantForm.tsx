import { useFormikContext } from 'formik';
import { IContactSearchResult, IFormLease } from 'interfaces';
import * as React from 'react';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { TableSelect } from '../../../../components/common/form/TableSelect';
import AddLeaseFormButtons from '../AddLeaseFormButtons';
import AddLeaseTenantListView from './AddLeastTenantListView';
import columns from './columns';
import SelectedTableHeader from './SelectedTableHeader';
import * as Styled from './styles';
export interface IAddLeaseTenantFormProps {
  selectedTenants: IContactSearchResult[];
  setSelectedTenants: (selectedTenants: IContactSearchResult[]) => void;
  onCancel: () => void;
}

export const AddLeaseTenantForm: React.FunctionComponent<IAddLeaseTenantFormProps> = ({
  selectedTenants,
  setSelectedTenants,
  onCancel,
}) => {
  const formikProps = useFormikContext<IFormLease>();
  return (
    <>
      <Styled.TenantH2>Add tenants to this Lease/License</Styled.TenantH2>
      <p>
        If the tenants are not already set up as contacts, you will have to add them first (under{' '}
        {<Link to="/contact/list">Contacts</Link>}) before you can find them here.
      </p>
      <StyledFormBody>
        <TableSelect<IContactSearchResult>
          selectedItems={selectedTenants}
          columns={columns}
          field="tenants"
          addLabel="Add selected tenants"
          selectedTableHeader={SelectedTableHeader}
        >
          <AddLeaseTenantListView
            setSelectedTenants={setSelectedTenants}
            selectedTenants={selectedTenants}
          />
        </TableSelect>
        <AddLeaseFormButtons formikProps={formikProps} onCancel={onCancel} />
      </StyledFormBody>
    </>
  );
};

const StyledFormBody = styled.div`
  margin-top: 3rem;
`;

export default AddLeaseTenantForm;
