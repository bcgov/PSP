import './AccessRequestPage.scss';

import { ISnackbarState, Snackbar } from 'components/common/Snackbar';
import { AccessRequestStatus } from 'constants/accessStatus';
import * as API from 'constants/API';
import { DISCLAIMER_URL, PRIVACY_POLICY_URL } from 'constants/strings';
import { AUTHORIZATION_URL } from 'constants/strings';
import { Formik } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IAccessRequest, IUserInfo } from 'interfaces';
import React, { useEffect } from 'react';
import Alert from 'react-bootstrap/Alert';
import Button from 'react-bootstrap/Button';
import ButtonToolbar from 'react-bootstrap/ButtonToolbar';
import Col from 'react-bootstrap/Col';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import { useAppSelector } from 'store/hooks';
import { toAccessRequest } from 'store/slices/accessRequests/accessRequestsSlice';
import { useAccessRequests } from 'store/slices/accessRequests/useAccessRequests';
import { mapLookupCode } from 'utils';
import { AccessRequestSchema } from 'utils/YupSchema';

import { Form, Input, Select, TextArea } from '../../../components/common/form';

/**
 * The AccessRequestPage provides a way to new authenticated users to submit a request
 * that associates them with a specific organization and a role within the organization.
 * If they have an active access request already submitted, it will allow them to update it until it has been approved or disabled.
 * If their prior request was disabled they will then be able to submit a new request.
 */
const AccessRequestPage = () => {
  const keycloakWrapper = useKeycloakWrapper();
  const keycloak = keycloakWrapper.obj;
  const userInfo = keycloak?.userInfo as IUserInfo;
  const { fetchCurrentAccessRequest, addAccessRequest } = useAccessRequests();
  const [alert, setAlert] = React.useState<ISnackbarState>({});

  useEffect(() => {
    fetchCurrentAccessRequest();
  }, [fetchCurrentAccessRequest]);

  const data = useAppSelector(state => state.accessRequests);

  const { getPublicByType } = useLookupCodeHelpers();
  const roles = getPublicByType(API.ROLE_CODE_SET_NAME);
  const organizations = getPublicByType(API.ORGANIZATION_CODE_SET_NAME);

  const accessRequest = data?.accessRequest;
  const initialValues: Partial<IAccessRequest> = {
    id: accessRequest?.id,
    userId: userInfo?.id,
    user: {
      id: userInfo?.id,
      keycloakUserId: userInfo?.keycloakUserId,
      businessIdentifier: userInfo?.businessIdentifier,
      displayName: userInfo?.name,
      firstName: userInfo?.firstName,
      surname: userInfo?.surname,
      email: userInfo?.email,
      position: accessRequest?.user?.position ?? userInfo?.position ?? '',
    },
    status: AccessRequestStatus.Received,
    note: accessRequest?.note ?? '',
    organizationId: +(organizations?.find(a => a.code === 'MOTI2')?.id ?? 0), // Select TRAN as the default organization for all access requests.
    roleId: accessRequest?.role?.id,
    rowVersion: accessRequest?.rowVersion,
  };

  const selectRoles = roles.map(c => mapLookupCode(c, initialValues?.role?.id));

  const checkRoles = (
    <Form.Group className="check-roles">
      <Form.Label>
        Role{' '}
        <a target="_blank" rel="noopener noreferrer" href={AUTHORIZATION_URL}>
          Role Descriptions
        </a>
      </Form.Label>
      <Select field="roleId" required={true} options={selectRoles} placeholder="Please Select" />
    </Form.Group>
  );

  const button = initialValues.id === undefined ? 'Submit' : 'Update';
  const inProgress = React.useMemo(
    () =>
      initialValues.id !== 0 ? (
        <Alert key={initialValues.id} variant="primary">
          You will receive an email when your request is reviewed.
        </Alert>
      ) : null,
    [initialValues.id],
  );

  return (
    <div className="accessRequestPage">
      <div>
        <h3>Access Request</h3>
      </div>
      <hr />
      <Container fluid={true}>
        <Row className="justify-content-md-center">
          <Formik
            enableReinitialize={true}
            initialValues={initialValues}
            validationSchema={AccessRequestSchema}
            onSubmit={async (values, { setSubmitting }) => {
              try {
                await addAccessRequest(toAccessRequest(values));
                setAlert({
                  variant: 'success',
                  message: 'Your request has been submitted.',
                  show: true,
                });
              } catch (error) {
                setAlert({
                  variant: 'danger',
                  message: 'Failed to submit your access request.',
                  show: true,
                });
              }
              setSubmitting(false);
            }}
          >
            {props => (
              <Form className="userInfo">
                {inProgress}

                <Input
                  label="IDIR/BCeID"
                  field="user.businessIdentifier"
                  placeholder={initialValues?.user?.businessIdentifier}
                  readOnly={true}
                  type="text"
                />

                <Row>
                  <Col>
                    <Input
                      label="First Name"
                      field="user.firstName"
                      placeholder={initialValues?.user?.firstName}
                      readOnly={true}
                      type="text"
                    />
                  </Col>
                  <Col>
                    <Input
                      label="Last Name"
                      field="user.surname"
                      placeholder={initialValues?.user?.surname}
                      readOnly={true}
                      type="text"
                    />
                  </Col>
                </Row>

                <Input
                  label="Email"
                  field="user.email"
                  placeholder={initialValues?.user?.email}
                  readOnly={true}
                  type="email"
                />

                <Input
                  label="Position"
                  field="user.position"
                  placeholder="e.g) Director, Real Estate and Stakeholder Engagement"
                  type="text"
                />

                {checkRoles}

                <TextArea
                  label="Notes"
                  field="note"
                  placeholder="Please specify why you need access to PIMS and include your manager's name."
                  required={true}
                />

                <p>
                  By clicking request, you agree to our{' '}
                  <a href={DISCLAIMER_URL}>Terms and Conditions</a> and that you have read our{' '}
                  <a href={PRIVACY_POLICY_URL}>Privacy Policy</a>.
                </p>
                {alert.show && (
                  <Snackbar
                    show={alert.show}
                    message={alert.message}
                    variant={alert.variant}
                    onClose={() => setAlert({})}
                  />
                )}
                <Row className="justify-content-md-center">
                  <ButtonToolbar className="cancelSave">
                    <Button className="mr-5" type="submit">
                      {button}
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

export default AccessRequestPage;
