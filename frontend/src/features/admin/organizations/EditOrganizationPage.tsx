import './EditOrganizationPage.scss';

import GenericModal from 'components/common/GenericModal';
import TooltipWrapper from 'components/common/TooltipWrapper';
import * as API from 'constants/API';
import { OrganizationIdentifierTypes, OrganizationTypes } from 'constants/index';
import service from 'features/properties/service';
import { Formik } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IOrganization } from 'interfaces';
import React, { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import ButtonToolbar from 'react-bootstrap/ButtonToolbar';
import Container from 'react-bootstrap/Container';
import Navbar from 'react-bootstrap/Navbar';
import Row from 'react-bootstrap/Row';
import { FaArrowAltCircleLeft } from 'react-icons/fa';
import { useDispatch } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { useAppSelector } from 'store/hooks';
import { ILookupCode } from 'store/slices/lookupCodes';
import { useOrganizations } from 'store/slices/organizations/useOrganizations';
import { OrganizationEditSchema } from 'utils/YupSchema';

import { Check, Form, Input, Select, SelectOption } from '../../../components/common/form';

interface IEditOrganizationPageProps {
  /** id prop to identify which organization to edit */
  id: number;
  match?: any;
}

/** This page is used to either add a new organization or edit the and organization's details */
const EditOrganizationPage = (props: IEditOrganizationPageProps) => {
  const organizationId = props?.match?.params?.id || props.id;
  const history = useHistory();
  const dispatch = useDispatch();
  const newOrganization = history.location.pathname.includes('/new');
  const [showDelete, setShowDelete] = useState(false);
  const [showFailed, setShowFailed] = useState(false);
  const { fetchOrganizationDetail, addOrganization, updateOrganization } = useOrganizations();
  useEffect(() => {
    if (!newOrganization) {
      fetchOrganizationDetail(organizationId);
    }
  }, [fetchOrganizationDetail, organizationId, newOrganization]);

  const { getByType } = useLookupCodeHelpers();
  const organizations = getByType(API.ORGANIZATION_CODE_SET_NAME).filter(x => !x.parentId);

  const organization = useAppSelector(state => state.organizations.organizationDetail);
  const mapLookupCode = (code: ILookupCode): SelectOption => ({
    label: code.name,
    value: code.id.toString(),
    selected: !!organization.parentId,
  });
  //
  const selectOrganizations = organizations.map(c => mapLookupCode(c));
  const checkOrganizations = (
    <Select
      label="Parent Organization - If applicable"
      field="parentId"
      options={selectOrganizations}
      disabled={!organization.parentId && !newOrganization}
      placeholder={newOrganization ? 'Please select if applicable' : 'No parent'}
    />
  );

  const goBack = () => {
    history.push('/admin/organizations');
  };

  const newValues: any = {
    code: '',
    name: '',
    description: '',
    isDisabled: false,
    sendEmail: true,
    email: '',
    addressTo: '',
    rowVersion: '',
  };

  const initialValues: IOrganization = {
    ...newValues,
    ...organization,
  };

  return (
    <div>
      {showDelete && (
        <DeleteModal {...{ showDelete, setShowDelete, history, dispatch, organization }} />
      )}
      {showFailed && <FailedDeleteModal {...{ showFailed, setShowFailed }} />}
      <Navbar className="navBar" expand="sm" variant="light" bg="light">
        <Navbar.Brand>
          {' '}
          <TooltipWrapper toolTipId="back" toolTip="Back to Organization list">
            <FaArrowAltCircleLeft onClick={goBack} size={20} />
          </TooltipWrapper>
        </Navbar.Brand>
        <h4>Organization Information</h4>
      </Navbar>
      <Container fluid={true}>
        <Row className="organization-edit-form-container">
          <Formik
            enableReinitialize
            initialValues={newOrganization ? newValues : initialValues}
            validationSchema={OrganizationEditSchema}
            onSubmit={async (values, { setSubmitting, setStatus, setValues }) => {
              try {
                if (!newOrganization) {
                  const data = await updateOrganization({
                    id: organization.id,
                    parentId: values.parentId ? Number(values.parentId) : undefined,
                    name: values.name,
                    organizationTypeId: OrganizationTypes.BCMinistry, // TODO: Needs to be implemented on the form.
                    identifierTypeId: OrganizationIdentifierTypes.Government, // TODO: Needs to be implemented on the form.
                    identifier: 'fake value', // TODO: Needs to be implemented on the form.
                    isDisabled: values.isDisabled,
                    rowVersion: values.rowVersion,
                  });
                  setValues(data);
                } else {
                  const data = await addOrganization({
                    parentId: Number(values.parentId),
                    name: values.name,
                    organizationTypeId: OrganizationTypes.BCMinistry, // TODO: Needs to be implemented on the form.
                    identifierTypeId: OrganizationIdentifierTypes.Government, // TODO: Needs to be implemented on the form.
                    identifier: 'fake value', // TODO: Needs to be implemented on the form.
                    isDisabled: values.isDisabled,
                  });
                  setValues(data);
                  history.replace(`/admin/organization/${data.id}`);
                }
              } catch (error) {
                const msg: string =
                  error?.response?.data?.error ?? 'Error saving property data, please try again.';
                setStatus({ msg });
              } finally {
                setSubmitting(false);
              }
            }}
          >
            {props => (
              <Form className="organizationInfo">
                <Input
                  label="Organization"
                  required
                  field="name"
                  value={props.values.name}
                  type="text"
                />
                <Input
                  label="Short Name (Code)"
                  field="code"
                  value={props.values.code}
                  type="text"
                  required
                />
                {checkOrganizations}
                <Input label="Description" field="description" type="text" />
                <Input label="Organization e-mail address" field="email" type="text" />
                <Input label="Email Addressed To" field="addressTo" type="text" />

                <Form.Group className="checkboxes">
                  <TooltipWrapper
                    toolTip="Click to change Organization status then click Save Changes."
                    toolTipId="is-disabled-tooltip"
                  >
                    <Check field="isDisabled" label="Disable Organization?" />
                  </TooltipWrapper>
                  <TooltipWrapper
                    toolTip="Click to enable to email notifications for Organization then click Save Changes."
                    toolTipId="email-tooltip"
                  >
                    <Check field="sendEmail" label="Email Notifications?" />
                  </TooltipWrapper>
                </Form.Group>

                <hr></hr>
                <Row className="buttons">
                  <ButtonToolbar className="cancelSave">
                    {!newOrganization ? (
                      <Button
                        className="bg-danger mr-5"
                        type="button"
                        onClick={async () => {
                          const data = await service.getPropertyList({
                            page: 1,
                            quantity: 10,
                            organizations: organizationId,
                          });
                          if (data.total === 0) {
                            setShowDelete(true);
                          } else {
                            setShowFailed(true);
                          }
                        }}
                      >
                        Delete Organization
                      </Button>
                    ) : (
                      <Button className="bg-danger mr-5" type="button" onClick={() => goBack()}>
                        Cancel
                      </Button>
                    )}

                    <Button className="mr-5" type="submit">
                      {newOrganization ? 'Submit Organization' : 'Save Changes'}
                    </Button>
                  </ButtonToolbar>
                </Row>
              </Form>
            )}
          </Formik>
        </Row>
      </Container>
    </div>
  );
};

export default EditOrganizationPage;

const DeleteModal = ({ showDelete, setShowDelete, history, dispatch, organization }: any) => {
  const { removeOrganization } = useOrganizations();
  return (
    <GenericModal
      message="Are you sure you want to permanently delete the organization?"
      cancelButtonText="Cancel"
      okButtonText="Delete"
      display={showDelete}
      handleOk={() => {
        removeOrganization(organization).then(() => {
          history.push('/admin/organizations');
        });
      }}
      handleCancel={() => {
        setShowDelete(false);
      }}
    />
  );
};

const FailedDeleteModal = ({ showFailed, setShowFailed }: any) => (
  <GenericModal
    message="You are not able to delete this organization as there are properties currently associated with it."
    okButtonText="Ok"
    display={showFailed}
    handleOk={() => {
      setShowFailed(false);
    }}
  />
);
