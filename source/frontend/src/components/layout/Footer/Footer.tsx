import { values } from 'lodash';
import React from 'react';
import { Nav } from 'react-bootstrap';
import { FaBomb } from 'react-icons/fa';
import styled from 'styled-components';

import { ApiVersionInfo } from '@/components/common/ApiVersionInfo';
import { useAppSelector } from '@/store/hooks';
import { IGenericNetworkAction } from '@/store/slices/network/interfaces';

import ErrorModal from '../Header/ErrorModal';

/**
 * Display common links and the api version in the bottom footer.
 * @returns Footer component.
 */
function Footer() {
  const [show, setShow] = React.useState(false);
  const handleShow = () => setShow(true);
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
    <FooterStyled>
      <div className="main-footer">
        <a
          target="_blank"
          rel="noopener noreferrer"
          href="http://www.gov.bc.ca/gov/content/home/disclaimer"
        >
          Disclaimer
        </a>
        <a
          target="_blank"
          rel="noopener noreferrer"
          href="http://www.gov.bc.ca/gov/content/home/privacy"
        >
          Privacy
        </a>
        <a
          target="_blank"
          rel="noopener noreferrer"
          href="http://www.gov.bc.ca/gov/content/home/accessible-government"
        >
          Accessibility
        </a>
        <a
          target="_blank"
          rel="noopener noreferrer"
          href="http://www.gov.bc.ca/gov/content/home/copyright"
        >
          Copyright
        </a>
        <ApiVersionInfo />
      </div>

      <Nav className="other">
        {errors && errors.length > 0 ? (
          <FaBomb size={25} className="errors" onClick={handleShow} />
        ) : null}
      </Nav>
      <ErrorModal errors={errors} show={show} setShow={setShow}></ErrorModal>
    </FooterStyled>
  );
}

/**
 * Determine if the network action resulted in an error.
 * @param action A generic network action.
 * @returns True if the network action resulted in an error.
 */
const isNetworkError = (action: any): action is IGenericNetworkAction =>
  (action as IGenericNetworkAction).type === 'ERROR';

/**
 * Styled footer component.
 */
const FooterStyled = styled('div')`
  display: flex;

  .main-footer {
    width: 95%;
    padding: 1rem 20%;
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: flex-start;
    align-items: stretch;
    align-content: flex-start;
    font-size: 1.2rem;
  }

  a {
    color: #ffffff;
    display: inline;
    &:link,
    &:visited {
      font-size: 1.2rem;
      font-family: 'BCSans', Fallback, sans-serif;
      padding: 0 1rem;
      border-right: 0.1rem solid #666;
    }
    &:hover,
    &:focus {
      text-decoration: underline;
    }
  }

  div.version {
    color: #ffffff;
    font-size: 1.2rem;
    font-family: 'BCSans', Fallback, sans-serif;
    padding: 0 0 0 1rem;
    flex-grow: 1;
    text-align: right;
  }

  .other {
    flex-grow: 1;
    display: flex;
    flex-direction: row-reverse;
    align-items: center;
    padding: 0;
    margin-right: 2rem;
    color: #ffffff;
    :hover {
      color: red;
      cursor: pointer;
    }
  }
`;

export default Footer;
