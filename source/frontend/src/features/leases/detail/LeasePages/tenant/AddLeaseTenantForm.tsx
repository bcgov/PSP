import { Button } from 'components/common/buttons';
import { TableSelect } from 'components/common/form';
import { ContactManagerModal } from 'components/contact/ContactManagerModal';
import { TENANT_TYPES } from 'constants/API';
import { Formik, FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { defaultFormLease, IContactSearchResult, IFormLease } from 'interfaces';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { Link, Prompt } from 'react-router-dom';
import styled from 'styled-components';
import { mapLookupCode } from 'utils';

import { AddLeaseTenantYupSchema } from './AddLeaseTenantYupSchema';
import getColumns from './columns';
import PrimaryContactWarningModal, {
  IPrimaryContactWarningModalProps,
} from './PrimaryContactWarningModal';
import SelectedTableHeader from './SelectedTableHeader';
import * as Styled from './styles';
import { FormTenant } from './ViewTenantForm';
export interface IAddLeaseTenantFormProps {
  selectedContacts: IContactSearchResult[];
  setSelectedContacts: (selectedContacts: IContactSearchResult[]) => void;
  setTenants: (selectedContacts: IContactSearchResult[]) => void;
  tenants: FormTenant[];
  onSubmit: (lease: IFormLease) => Promise<void>;
  initialValues?: IFormLease;
  formikRef: React.Ref<FormikProps<IFormLease>>;
  showContactManager: boolean;
  setShowContactManager: (showContactManager: boolean) => void;
}

export const AddLeaseTenantForm: React.FunctionComponent<
  React.PropsWithChildren<IAddLeaseTenantFormProps & IPrimaryContactWarningModalProps>
> = ({
  selectedContacts,
  setSelectedContacts,
  setTenants,
  tenants,
  onSubmit,
  initialValues,
  formikRef,
  children,
  showContactManager,
  setShowContactManager,
  saveCallback,
  lease,
  onCancel,
}) => {
  const lookupCodes = useLookupCodeHelpers();
  const tenantTypes = lookupCodes.getByType(TENANT_TYPES).map(c => mapLookupCode(c));
  const onRemove = (remainingTenants: FormTenant[]) => {
    const remainingContacts = remainingTenants.map(t => t.toContactSearchResult());
    setTenants(remainingContacts);
    setSelectedContacts(remainingContacts);
  };

  return (
    <>
      <Styled.TenantH2>Add tenants & contacts to this Lease/License</Styled.TenantH2>
      <p>
        If the tenants are not already set up as contacts, you will have to add them first (under{' '}
        {<Link to="/contact/list">Contacts</Link>}) before you can find them here.
      </p>

      <Formik
        validationSchema={AddLeaseTenantYupSchema}
        onSubmit={values => {
          onSubmit(values);
        }}
        innerRef={formikRef}
        enableReinitialize
        initialValues={{ ...defaultFormLease, ...initialValues, tenants: tenants }}
      >
        {formikProps => (
          <>
            <Prompt
              when={formikProps.dirty && !formikProps.isSubmitting}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
            <StyledFormBody>
              <Row>
                <Col>
                  <StyledButton
                    className="ml-auto"
                    variant="secondary"
                    onClick={() => {
                      setShowContactManager(true);
                    }}
                  >
                    Select Tenant(s)
                  </StyledButton>
                </Col>
              </Row>
              <TableSelect<FormTenant>
                selectedItems={tenants}
                columns={getColumns(tenantTypes)}
                field="tenants"
                selectedTableHeader={SelectedTableHeader}
                onRemove={onRemove}
              ></TableSelect>
              <ContactManagerModal
                selectedRows={selectedContacts}
                setSelectedRows={(s: IContactSearchResult[]) => {
                  setSelectedContacts(s);
                }}
                display={showContactManager}
                setDisplay={setShowContactManager}
                handleModalOk={() => {
                  setShowContactManager(false);
                  setTenants(selectedContacts);
                }}
                handleModalCancel={() => {
                  setShowContactManager(false);
                  setSelectedContacts(tenants.map(t => t.toContactSearchResult()));
                }}
                showActiveSelector={true}
                isSummary={true}
              ></ContactManagerModal>
            </StyledFormBody>
            {children}
          </>
        )}
      </Formik>
      <PrimaryContactWarningModal saveCallback={saveCallback} onCancel={onCancel} lease={lease} />
    </>
  );
};

const StyledFormBody = styled.div`
  margin-top: 3rem;
  text-align: left;
`;
const StyledButton = styled(Button)`
  float: left;
`;

export default AddLeaseTenantForm;
