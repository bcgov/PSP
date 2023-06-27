import { Formik, FormikProps } from 'formik';
import { Col, Row } from 'react-bootstrap';
import { Prompt } from 'react-router';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { TableSelect } from '@/components/common/form';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { ContactManagerModal } from '@/components/contact/ContactManagerModal';
import { TENANT_TYPES } from '@/constants/API';
import { LeaseFormModel } from '@/features/leases/models';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { IContactSearchResult } from '@/interfaces';
import { mapLookupCode } from '@/utils';

import { AddLeaseTenantYupSchema } from './AddLeaseTenantYupSchema';
import getColumns from './columns';
import { FormTenant } from './models';
import PrimaryContactWarningModal, {
  IPrimaryContactWarningModalProps,
} from './PrimaryContactWarningModal';
import SelectedTableHeader from './SelectedTableHeader';
import * as Styled from './styles';

export interface IAddLeaseTenantFormProps {
  selectedContacts: IContactSearchResult[];
  setSelectedContacts: (selectedContacts: IContactSearchResult[]) => void;
  setSelectedTenants: (selectedContacts: IContactSearchResult[]) => void;
  selectedTenants: FormTenant[];
  onSubmit: (lease: LeaseFormModel) => Promise<void>;
  initialValues?: LeaseFormModel;
  formikRef: React.Ref<FormikProps<LeaseFormModel>>;
  showContactManager: boolean;
  setShowContactManager: (showContactManager: boolean) => void;
  loading?: boolean;
}

export const AddLeaseTenantForm: React.FunctionComponent<
  React.PropsWithChildren<IAddLeaseTenantFormProps & IPrimaryContactWarningModalProps>
> = ({
  selectedContacts,
  setSelectedContacts,
  setSelectedTenants,
  selectedTenants,
  onSubmit,
  initialValues,
  formikRef,
  children,
  showContactManager,
  setShowContactManager,
  saveCallback,
  onCancel,
  loading,
}) => {
  const lookupCodes = useLookupCodeHelpers();
  const tenantTypes = lookupCodes.getByType(TENANT_TYPES).map(c => mapLookupCode(c));
  const onRemove = (remainingTenants: FormTenant[]) => {
    const remainingContacts = remainingTenants.map(t => FormTenant.toContactSearchResult(t));
    setSelectedTenants(remainingContacts);
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
        initialValues={{ ...new LeaseFormModel(), ...initialValues, tenants: selectedTenants }}
      >
        {formikProps => (
          <>
            <LoadingBackdrop show={loading} parentScreen />
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
                selectedItems={selectedTenants}
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
                  setSelectedTenants(selectedContacts);
                }}
                handleModalCancel={() => {
                  setShowContactManager(false);
                  setSelectedContacts(
                    selectedTenants.map(t => FormTenant.toContactSearchResult(t)),
                  );
                }}
                showActiveSelector={true}
                isSummary={true}
              ></ContactManagerModal>
            </StyledFormBody>
            {children}
          </>
        )}
      </Formik>
      <PrimaryContactWarningModal
        saveCallback={saveCallback}
        onCancel={onCancel}
        selectedTenants={selectedTenants}
      />
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
