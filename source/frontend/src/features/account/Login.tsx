import Col from 'react-bootstrap/Col';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Spinner from 'react-bootstrap/Spinner';
import { MdLocationPin } from 'react-icons/md';
import { Redirect } from 'react-router-dom';

import { Button } from '@/components/common/buttons';
import * as actionTypes from '@/constants/actionTypes';
import { Roles } from '@/constants/roles';
import { useQuery } from '@/hooks/use-query';
import useKeycloakWrapper, { IUserInfo } from '@/hooks/useKeycloakWrapper';
import { useAppSelector } from '@/store/hooks';
import { Logo, useTenant } from '@/tenants';

import { LoginStyled, VertialAlign } from './LoginStyled';

/**
 * Login component provides information and links for the user to login.
 * @returns Login component.
 */
const Login = () => {
  const query = useQuery();
  const redirect = query.get('redirect');
  const keyCloakWrapper = useKeycloakWrapper();
  const keycloak = keyCloakWrapper?.obj;
  const userInfo = keycloak?.userInfo as IUserInfo;
  const isIE = usingIE();
  const activated = useAppSelector(state => state.network[actionTypes.ADD_ACTIVATE_USER]);
  const tenant = useTenant();

  if (!keycloak) {
    return <Spinner animation="border"></Spinner>;
  }
  if (keycloak?.authenticated) {
    if (activated?.status === 201 || !userInfo?.client_roles?.length) {
      return <Redirect to={{ pathname: '/access/request' }} />;
    } else if (typeof redirect === 'string' && redirect.length) {
      return <Redirect to={redirect} />;
    } else if (
      keyCloakWrapper.roles?.length === 1 &&
      (keyCloakWrapper.hasRole(Roles.LEASE_FUNCTIONAL) ||
        keyCloakWrapper.hasRole(Roles.LEASE_READ_ONLY))
    ) {
      return <Redirect to="/lease/list" />;
    } else {
      return <Redirect to={'/mapview'} />;
    }
  }
  if (isIE) {
    return <Redirect to={{ pathname: '/ienotsupported' }} />;
  }

  return (
    <LoginStyled className="login" fluid={true}>
      <VertialAlign>
        <Container className="unauth" fluid={true}>
          <Row>
            <Col xs={1} />
            <Col xs={1} md={3}>
              <Logo withText={false} />
            </Col>
            <Col xs={16} md={6} className="logo-title">
              <h1>{tenant.login.title}</h1>
            </Col>
          </Row>

          <Row className="sign-in">
            <div className="message-container">
              <div className="message-header">
                <div className="header-icon">
                  <MdLocationPin size={27} />
                </div>
                <p className="message-title">Welcome!</p>
              </div>
              <div className="message-body">
                <p>{tenant.login.heading}</p>
                <hr className="spacer" />
                <p>{tenant.login.body}</p>
              </div>
              <Row className="message-footer">
                <Col xs={16} md={6} />
                <Col xs={16} md={6}>
                  <Button variant="primary" onClick={() => keycloak.login({ idpHint: 'idir' })}>
                    Sign In
                  </Button>
                </Col>
              </Row>
            </div>
          </Row>

          <Row>
            <Col xs={1} md={3} />
            <Col xs={16} md={6} className="foot-note">
              <p>Sign into PIMS with your government issued IDIR</p>
            </Col>
            <Col xs={1} md={3} />
          </Row>
        </Container>
      </VertialAlign>
    </LoginStyled>
  );
};

//check to see if user is using Internet Explorer
//as their browser
const usingIE = () => {
  const userAgent = window.navigator.userAgent;
  const isOldIE = userAgent.indexOf('MSIE '); //tag used for IE 10 or older
  const isIE11 = userAgent.indexOf('Trident/'); //tag used for IE11
  if (isOldIE > 0 || isIE11 > 0) return true;
  return false;
};

export default Login;
