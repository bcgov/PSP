import { TableSelect } from 'components/common/form';
import SaveCancelButtons from 'features/leases/SaveCancelButtons';
import { Formik, FormikProps } from 'formik';
import { defaultFormLease, IContactSearchResult, IFormLease } from 'interfaces';
import * as React from 'react';
import { Link, Prompt } from 'react-router-dom';
import styled from 'styled-components';

import AddLeaseTenantListView from './AddLeastTenantListView';
import columns from './columns';
import SelectedTableHeader from './SelectedTableHeader';
import * as Styled from './styles';
import { FormTenant } from './Tenant';
export interface IAddLeaseTenantFormProps {
  selectedTenants: FormTenant[];
  setSelectedTenants: (selectedTenants: FormTenant[]) => void;
  onCancel: () => void;
  onSubmit: (lease: IFormLease) => Promise<void>;
  initialValues?: IFormLease;
  formikRef: React.Ref<FormikProps<IFormLease>>;
}

export const AddLeaseTenantForm: React.FunctionComponent<IAddLeaseTenantFormProps> = ({
  selectedTenants,
  setSelectedTenants,
  onCancel,
  onSubmit,
  initialValues,
  formikRef,
}) => {
  return (
    <>
      <Styled.TenantH2>Add tenants to this Lease/License</Styled.TenantH2>
      <p>
        If the tenants are not already set up as contacts, you will have to add them first (under{' '}
        {<Link to="/contact/list">Contacts</Link>}) before you can find them here.
      </p>
      <Formik
        onSubmit={values => onSubmit(values)}
        innerRef={formikRef}
        enableReinitialize
        initialValues={{ ...defaultFormLease, ...initialValues }}
      >
        {formikProps => (
          <>
            <Prompt
              when={formikProps.dirty}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
            <StyledFormBody>
              <TableSelect<FormTenant>
                selectedItems={selectedTenants}
                columns={columns}
                field="tenants"
                addLabel="Add selected tenants"
                selectedTableHeader={SelectedTableHeader}
              >
                <AddLeaseTenantListView
                  setSelectedTenants={(selected: IContactSearchResult[]) =>
                    setSelectedTenants(selected.map(s => new FormTenant(undefined, s)))
                  }
                  selectedTenants={selectedTenants.map<IContactSearchResult>(selectedTenant => {
                    return selectedTenant.original ?? { id: selectedTenant?.id?.toString() ?? '' };
                  })}
                />
              </TableSelect>
              <SaveCancelButtons formikProps={formikProps} onCancel={onCancel} />
            </StyledFormBody>
          </>
        )}
      </Formik>
    </>
  );
};

const StyledFormBody = styled.div`
  margin-top: 3rem;
  text-align: left;
`;

export default AddLeaseTenantForm;
