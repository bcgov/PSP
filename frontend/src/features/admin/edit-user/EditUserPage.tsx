import './EditUserPage.scss';

import { Label } from 'components/common/Label';
import TooltipWrapper from 'components/common/TooltipWrapper';
import * as API from 'constants/API';
import { AUTHORIZATION_URL } from 'constants/strings';
import { Field, Formik } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import ButtonToolbar from 'react-bootstrap/ButtonToolbar';
import Col from 'react-bootstrap/Col';
import Container from 'react-bootstrap/Container';
import Navbar from 'react-bootstrap/Navbar';
import Row from 'react-bootstrap/Row';
import { useHistory } from 'react-router-dom';
import { useAppSelector } from 'store/hooks';
import { ILookupCode } from 'store/slices/lookupCodes';
import { useUsers } from 'store/slices/users';
import { formatApiDateTime } from 'utils';
import { UserUpdateSchema } from 'utils/YupSchema';

import { Form, Input, Select, SelectOption } from '../../../components/common/form';

interface IEditUserPageProps {
  key: string;
  match?: any;
}

const EditUserPage = (props: IEditUserPageProps) => {
  const userId = props?.match?.params?.key || props.key;
  const history = useHistory();
  const { updateUser, fetchUserDetail } = useUsers();

  useEffect(() => {
    fetchUserDetail(userId);
  }, [userId, fetchUserDetail]);

  const { getByType } = useLookupCodeHelpers();
  const agencies = getByType(API.AGENCY_CODE_SET_NAME);
  const roles = getByType(API.ROLE_CODE_SET_NAME);

  const user = useAppSelector(state => state.users.userDetail);
  const mapLookupCode = (code: ILookupCode): SelectOption => ({
    label: code.name,
    value: code.id,
    selected: !!user?.roles?.find(x => x.id === code.id.toString()) ?? [],
  });

  const selectAgencies = agencies.map(c => mapLookupCode(c));
  const selectRoles = roles.map(c => mapLookupCode(c));

  // Arrays below are used to add the role/agency from the dropdown later in code
  let agenciesToUpdate: any[];
  let rolesToUpdate: any[];

  const checkAgencies = (
    <Select
      label="Agency"
      field="agency"
      data-testid="agency"
      required={true}
      options={selectAgencies}
      placeholder={
        user?.agencies?.length && user?.agencies?.length > 0 ? undefined : 'Please Select'
      }
    />
  );

  const checkRoles = (
    <Form.Group className={'check-roles'}>
      <Form.Label>
        Roles{' '}
        <a target="_blank" rel="noopener noreferrer" href={AUTHORIZATION_URL}>
          Role Descriptions
        </a>
      </Form.Label>
      <TooltipWrapper
        toolTipId="select-roles-tip"
        toolTip="To select multiple roles, hold Ctrl and click options."
      >
        <Select
          field="roles"
          data-testid="role"
          multiple={true}
          required={true}
          options={selectRoles}
          placeholder={user?.roles?.length && user.roles.length > 0 ? undefined : 'Please Select'}
        />
      </TooltipWrapper>
    </Form.Group>
  );

  const goBack = () => {
    history.goBack();
  };

  const initialValues = {
    username: user.username ?? '',
    firstName: user.firstName ?? '',
    lastName: user.lastName ?? '',
    email: user.email ?? '',
    displayName: user.displayName ?? '',
    isDisabled: !!user.isDisabled ?? '',
    rowVersion: user.rowVersion ?? '',
    emailVerified: false,
    agencies: user.agencies ?? [],
    roles: user?.roles?.map(x => x.id) ?? [],
    note: user.note ?? '',
    agency: user.agencies && user.agencies.length !== 0 ? user.agencies[0].id : '',
    role: user.roles && user.roles.length !== 0 ? user.roles[0].id : '',
    position: user.position ?? '',
    lastLogin: formatApiDateTime(user.lastLogin),
  };

  return (
    <div>
      <Navbar className="navBar" expand="sm" variant="light" bg="light">
        <Navbar.Brand href="#">User Information</Navbar.Brand>
      </Navbar>
      <Container fluid={true}>
        <Row className="user-edit-form-container">
          <Formik
            enableReinitialize
            initialValues={initialValues}
            validationSchema={UserUpdateSchema}
            onSubmit={async (values, { setSubmitting, setValues }) => {
              if (values.agency !== '') {
                agenciesToUpdate = [{ id: Number(values.agency) }];
              } else {
                agenciesToUpdate = user.agencies ?? [];
              }
              if (values.roles) {
                rolesToUpdate = values.roles.map(r => ({ id: r }));
              } else {
                rolesToUpdate = user.roles ?? [];
              }

              await updateUser({
                id: user.id,
                key: user.key,
                username: user.username,
                displayName: values.displayName,
                firstName: values.firstName,
                lastName: values.lastName,
                email: values.email,
                isDisabled: values.isDisabled,
                rowVersion: values.rowVersion,
                emailVerified: values.emailVerified,
                agencies: agenciesToUpdate,
                roles: rolesToUpdate,
                position: values.position ?? undefined,
                note: values.note,
              });
              setSubmitting(false);
            }}
          >
            {props => (
              <Form className="userInfo">
                <Label>IDIR/BCeID</Label>
                <Input
                  data-testid="username"
                  field="username"
                  value={props.values.username}
                  readOnly={true}
                  type="text"
                />
                <Label>Last Login</Label>
                <Input
                  data-testid="lastLogin"
                  field="lastLogin"
                  value={props.values.lastLogin}
                  readOnly={true}
                  type="text"
                />

                <Row>
                  <Col>
                    <Label>First Name</Label>
                    <Input
                      data-testid="firstName"
                      field="firstName"
                      placeholder={props.values.firstName}
                      type="text"
                    />
                  </Col>
                  <Col>
                    <Label>Last Name</Label>
                    <Input
                      data-testid="lastName"
                      field="lastName"
                      placeholder={props.values.lastName}
                      type="text"
                    />
                  </Col>
                </Row>

                <Label>Email</Label>
                <Input
                  data-testid="email"
                  field="email"
                  placeholder={props.values.email}
                  type="email"
                />

                {checkAgencies}

                <Label>Position</Label>
                <Input
                  field="position"
                  placeholder="e.g) Director, Real Estate and Stakeholder Engagement"
                  type="text"
                  data-testid="position"
                />

                {checkRoles}

                <Label>Notes</Label>
                <Input
                  as="textarea"
                  field="note"
                  placeholder="A note about this user"
                  type="text"
                  data-testid="note"
                />

                <Form.Group className={'is-disabled'}>
                  <Form.Label>Disable Account?&nbsp;</Form.Label>
                  <TooltipWrapper
                    toolTipId="is-disabled-tooltip"
                    toolTip={'Click to change account status then click Save.'}
                  >
                    <Field data-testid="isDisabled" type="checkbox" name="isDisabled" />
                  </TooltipWrapper>
                </Form.Group>

                <Row className="justify-content-md-center">
                  <ButtonToolbar className="cancelSave">
                    <Button className="mr-5" variant="secondary" type="button" onClick={goBack}>
                      Cancel
                    </Button>
                    <Button className="mr-5" type="submit">
                      Save
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

export default EditUserPage;
