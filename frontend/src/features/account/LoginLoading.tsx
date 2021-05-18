import React from 'react';
import { Container, Row, Col, Button } from 'react-bootstrap';
import FilterBackdrop from 'components/maps/leaflet/FilterBackdrop';
import { LoginStyled } from './LoginStyled';
import { useTenant, Logo } from 'tenants';

/**
 * Display a placeholder of the PIMS login screen when keycloak is being initialized.
 */
const LoginLoading = () => {
  const tenant = useTenant();
  return (
    <LoginStyled className="login" fluid={true}>
      <FilterBackdrop show={true}></FilterBackdrop>
      <Container className="unauth" fluid={true}>
        <Logo withText={true} />
        <Row className="sign-in">
          <Col xs={1} md={3} />
          <Col xs={16} md={6} className="block">
            <h1>{tenant.login.title}</h1>
            <h6>{tenant.login.heading}</h6>
            <p>{tenant.login.body}</p>
            <Button variant="primary" disabled={true}>
              Sign In
            </Button>
            <p>Sign into PIMS with your government issued IDIR or with your Business BCeID.</p>
            <Row></Row>
          </Col>
          <Col xs={1} md={3} />
        </Row>
      </Container>
    </LoginStyled>
  );
};

export default LoginLoading;
