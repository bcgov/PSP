import { Button } from 'components/common/buttons';
import { TableSelect } from 'components/common/form';
import { ContactManagerModal } from 'components/contact/ContactManagerModal';
import { TENANT_TYPES } from 'constants/API';
import SaveCancelButtons from 'features/leases/SaveCancelButtons';
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
import SelectedTableHeader from './SelectedTableHeader';
import * as Styled from './styles';
import { FormTenant } from './Tenant';
export interface IAddLeaseTenantFormProps {
  selectedTenants: FormTenant[];
  setSelectedTenants: (selectedTenants: IContactSearchResult[]) => void;
  onCancel: () => void;
  onSubmit: (lease: IFormLease) => Promise<void>;
  initialValues?: IFormLease;
  formikRef: React.Ref<FormikProps<IFormLease>>;
}

export const AddLeaseTenantForm: React.FunctionComponent<
  React.PropsWithChildren<IAddLeaseTenantFormProps>
> = ({ selectedTenants, setSelectedTenants, onCancel, onSubmit, initialValues, formikRef }) => {
  const lookupCodes = useLookupCodeHelpers();
  const tenantTypes = lookupCodes.getByType(TENANT_TYPES).map(c => mapLookupCode(c));
  const [showContactManager, setShowContactManager] = React.useState<boolean>(false);

  return (
    <>
      <Styled.TenantH2>Add tenants & contacts to this Lease/License</Styled.TenantH2>
      <p>
        If the tenants are not already set up as contacts, you will have to add them first (under{' '}
        {<Link to="/contact/list">Contacts</Link>}) before you can find them here.
      </p>

      <Formik
        validationSchema={AddLeaseTenantYupSchema}
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
                columns={getColumns(tenantTypes)}
                field="tenants"
                disableButton={true}
                addLabel="Selected tenant(s)"
                selectedTableHeader={SelectedTableHeader}
              >
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

                <ContactManagerModal
                  selectedRows={selectedTenants.map<IContactSearchResult>(selectedTenant => {
                    return selectedTenant.original ?? { id: selectedTenant?.id?.toString() ?? '' };
                  })}
                  setSelectedRows={setSelectedTenants}
                  display={showContactManager}
                  setDisplay={setShowContactManager}
                  handleModalOk={() => {
                    formikProps.setSubmitting(true);
                    setShowContactManager(false);
                  }}
                  handleModalCancel={() => {
                    setShowContactManager(false);
                    setSelectedTenants([]);
                  }}
                  showActiveSelector={true}
                ></ContactManagerModal>
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
const StyledButton = styled(Button)`
  float: left;
`;

export default AddLeaseTenantForm;
