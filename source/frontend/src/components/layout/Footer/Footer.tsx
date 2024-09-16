import { values } from 'lodash';
import React from 'react';
import { Nav } from 'react-bootstrap';
import { FaBomb } from 'react-icons/fa';

import { ApiVersionInfo } from '@/components/common/ApiVersionInfo';
import { useAppSelector } from '@/store/hooks';
import { IGenericNetworkAction } from '@/store/slices/network/interfaces';

import ErrorModal from '../Header/ErrorModal';
import FooterStyled from './FooterStyled';

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

export default Footer;
