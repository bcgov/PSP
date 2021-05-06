import './Header.scss';

import React from 'react';
import { Navbar, Nav } from 'react-bootstrap';
import styled from 'styled-components';
import { tenant, Logo } from 'tenants';
import { BCGovLogo } from 'components/common/BCGovLogo';

/**
 * Display an "empty" header bar with limited functionality as a placeholder
 */
const EmptyHeader = () => {
  return (
    <Navbar expand className="App-header">
      <Navbar.Brand className="brand-box">
        <a target="_blank" rel="noopener noreferrer" href="https://www2.gov.bc.ca/gov/content/home">
          <BCGovLogo />
        </a>
        <VerticalBar />
        <Logo />
      </Navbar.Brand>
      <Nav className="title mr-auto">
        <Nav.Item>
          <h1 className="longAppName">{tenant.title}</h1>
          <h1 className="shortAppName">{tenant.shortName}</h1>
        </Nav.Item>
      </Nav>
    </Navbar>
  );
};

/**
 * Styled VerticalBar component.
 */
const VerticalBar = styled.span`
  border-left: 2px solid white;
  font-size: 34px;
  margin: 0 15px 0 25px;
  vertical-align: top;
`;

export default EmptyHeader;
