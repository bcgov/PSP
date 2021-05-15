import React from 'react';
import { Navbar, Nav } from 'react-bootstrap';
import { useTenant, Logo } from 'tenants';
import { BCGovLogo } from 'components/common/BCGovLogo';
import { HeaderStyled } from './HeaderStyled';
import { VerticalBar } from 'components/common/VerticalBar';

/**
 * Display an "empty" header bar with limited functionality as a placeholder
 */
const EmptyHeader = () => {
  const tenant = useTenant();
  return (
    <HeaderStyled expand className="App-header">
      <Navbar.Brand className="brand-box">
        <a target="_blank" rel="noopener noreferrer" href="https://www2.gov.bc.ca/gov/content/home">
          <BCGovLogo />
        </a>
        <VerticalBar />
        <Logo height={50} />
      </Navbar.Brand>
      <Nav className="title mr-auto">
        <Nav.Item>
          <h1 className="longAppName">{tenant.title}</h1>
          <h1 className="shortAppName">{tenant.shortName}</h1>
        </Nav.Item>
      </Nav>
    </HeaderStyled>
  );
};

export default EmptyHeader;
