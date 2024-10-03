import Col from 'react-bootstrap/Col';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import { MdLocationPin } from 'react-icons/md';

import { Button } from '@/components/common/buttons/Button';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Logo, useTenant } from '@/tenants';

import { LoginStyled } from './LoginStyled';

/**
 * Display a placeholder of the PIMS login screen when keycloak is being initialized.
 */
const LoginLoading = () => {
  const tenant = useTenant();
  return (
    <LoginStyled className="login" fluid={true}>
      <LoadingBackdrop show={true}></LoadingBackdrop>
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
                <Button variant="primary" disabled={true}>
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
    </LoginStyled>
  );
};

export default LoginLoading;
