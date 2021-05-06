import './Header.scss';

import React from 'react';
import { Navbar, Nav } from 'react-bootstrap';
import { useHistory } from 'react-router-dom';
import { IGenericNetworkAction } from 'actions/genericActions';
import { RootState } from 'reducers/rootReducer';
import { useSelector } from 'react-redux';
import { FaBomb } from 'react-icons/fa';
import _ from 'lodash';
import { UserProfile } from './UserProfile';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import styled from 'styled-components';
import { tenant } from 'tenants';
import { ErrorModal } from './ErrorModal';
import { Logo } from 'tenants';
import { BCGovLogo } from 'components/common/BCGovLogo';

/**
 * A header component that includes the navigation bar.
 * @returns Header component.
 */
export const Header = () => {
  const history = useHistory();
  const keycloak = useKeycloakWrapper();
  if (history.location.pathname === '/') {
    history.replace('/mapview');
  }
  const [show, setShow] = React.useState(false);
  const handleShow = () => setShow(true);

  const errors = useSelector<RootState, IGenericNetworkAction[]>(state => {
    const errors: IGenericNetworkAction[] = [];
    _.values(state).forEach(reducer => {
      _.values(reducer)
        .filter(x => x instanceof Object)
        .forEach(action => {
          if (isNetworkError(action)) {
            errors.push(action);
          }
        });
    });
    return errors;
  });
  return (
    <Navbar expand className="App-header">
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
      {keycloak.obj.authenticated && <UserProfile />}
      <Nav className="other">
        {errors && errors.length ? (
          <FaBomb size={30} className="errors" onClick={handleShow} />
        ) : null}
      </Nav>
      <ErrorModal errors={errors} show={show} setShow={setShow}></ErrorModal>
    </Navbar>
  );
};

/**
 * Determine if the network action resulted in an error.
 * @param action A generic network action.
 * @returns True if the network action resulted in an error.
 */
const isNetworkError = (action: any): action is IGenericNetworkAction =>
  (action as IGenericNetworkAction).type === 'ERROR';

/**
 * Styled component returns a vertical bar.
 */
const VerticalBar = styled.span`
  border-left: 2px solid white;
  font-size: 34px;
  margin: 0 15px 0 25px;
  vertical-align: top;
`;

export default Header;
