import Col from 'react-bootstrap/Col';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Spinner from 'react-bootstrap/Spinner';
import { Redirect } from 'react-router-dom';

import { Button } from '@/components/common/buttons';
import * as actionTypes from '@/constants/actionTypes';
import { Roles } from '@/constants/roles';
import { useQuery } from '@/hooks/use-query';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useAppSelector } from '@/store/hooks';
import { Logo, useTenant } from '@/tenants';

import { LoginStyled } from './LoginStyled';

/**
 * Login component provides information and links for the user to login.
 * @returns Login component.
 */
const Login = () => {
  const query = useQuery();
  const redirect = query.get('redirect');
  const keyCloakWrapper = useKeycloakWrapper();
  const keycloak = keyCloakWrapper.obj;
  const isIE = usingIE();
  const activated = useAppSelector(state => state.network[actionTypes.ADD_ACTIVATE_USER]);
  const tenant = useTenant();

  if (!keycloak) {
    return <Spinner animation="border"></Spinner>;
  }
  if (keycloak?.authenticated) {
    if (activated?.status === 201 || !keyCloakWrapper?.obj?.userInfo?.client_roles?.length) {
      return <Redirect to={{ pathname: '/access/request' }} />;
    } else if (typeof redirect === 'string' && redirect.length) {
      return <Redirect to={redirect} />;
    } else if (keyCloakWrapper.roles?.length === 1 && keyCloakWrapper.hasRole(Roles.FINANCE)) {
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
      <Container className="unauth" fluid={true}>
        <Logo withText={true} />
        <Row className="sign-in">
          <Col xs={1} md={3} />
          <Col xs={16} md={6} className="block">
            <h1>{tenant.login.title}</h1>
            <h6>{tenant.login.heading}</h6>
            <p>{tenant.login.body}</p>
            <Button variant="primary" onClick={() => keycloak.login({ idpHint: 'idir' })}>
              Sign In
            </Button>
            <p>Sign into PIMS with your government issued IDIR</p>
          </Col>
          <Col xs={1} md={3} />
        </Row>
      </Container>
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
