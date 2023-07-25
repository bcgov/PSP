import values from 'lodash/values';
import React from 'react';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import { FaBomb } from 'react-icons/fa';

import { BCGovLogo } from '@/components/common/BCGovLogo';
import { VerticalBar } from '@/components/common/VerticalBar';
import HelpContainer from '@/features/help/containers/HelpContainer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useAppSelector } from '@/store/hooks';
import { IGenericNetworkAction } from '@/store/slices/network/interfaces';
import { useTenant } from '@/tenants';
import { Logo } from '@/tenants';

import { ErrorModal } from './ErrorModal';
import { HeaderStyled } from './HeaderStyled';
import { UserProfile } from './UserProfile';

/**
 * A header component that includes the navigation bar.
 * @returns Header component.
 */
export const Header = () => {
  const keycloak = useKeycloakWrapper();
  const [show, setShow] = React.useState(false);
  const handleShow = () => setShow(true);
  const tenant = useTenant();

  const errors = useAppSelector(state => {
    const networkErrors: IGenericNetworkAction[] = [];
    values(state).forEach(reducer => {
      values(reducer)
        .filter(x => x instanceof Object)
        .forEach(action => {
          if (isNetworkError(action)) {
            networkErrors.push(action);
          }
        });
    });
    return networkErrors;
  });
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
      <HelpContainer />
      {keycloak.obj.authenticated && <UserProfile />}
      <Nav className="other">
        {errors && errors.length ? (
          <FaBomb size={30} className="errors" onClick={handleShow} />
        ) : null}
      </Nav>
      <ErrorModal errors={errors} show={show} setShow={setShow}></ErrorModal>
    </HeaderStyled>
  );
};

/**
 * Determine if the network action resulted in an error.
 * @param action A generic network action.
 * @returns True if the network action resulted in an error.
 */
const isNetworkError = (action: any): action is IGenericNetworkAction =>
  (action as IGenericNetworkAction).type === 'ERROR';

export default Header;
