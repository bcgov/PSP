import React from 'react';
import Col from 'react-bootstrap/Col';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import { Link } from 'react-router-dom';

export const NotFoundPage = () => {
  return (
    <Container fluid={true}>
      <Row>
        <Col>
          <h1>Page not found</h1>
          <Link to="/mapview">Go back to the map</Link>
        </Col>
      </Row>
    </Container>
  );
};
