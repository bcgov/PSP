import clsx from 'classnames';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import * as React from 'react';
import { Link, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { leasePages } from './LeaseContainer';

export interface ILeaseAndLicenseIndexProps {
  currentPageName?: string;
  leaseId?: number;
}

/**
 * left navigation "index" to allow navigation of lease sub-pages.
 * @param {ILeaseAndLicenseIndexProps} param0
 */
export const LeaseIndex: React.FunctionComponent<
  React.PropsWithChildren<ILeaseAndLicenseIndexProps>
> = ({ currentPageName, leaseId }) => {
  const { url } = useRouteMatch();
  const { hasClaim } = useKeycloakWrapper();
  return (
    <StyledLeaseIndex>
      {Array.from(leasePages.entries()).map(([pageName, page]) => (
        <>
          {(page.claims === undefined || hasClaim(page.claims)) && (
            <StyledLeaseIndexLink
              key={`lease-${pageName}`}
              to={!!leaseId ? `${url}/${pageName}` : '#'}
              className={clsx({ active: currentPageName === pageName, disabled: !leaseId })}
            >
              {page.title}
            </StyledLeaseIndexLink>
          )}
        </>
      ))}
    </StyledLeaseIndex>
  );
};

const StyledLeaseIndexLink = styled(Link)`
  color: ${props => props.theme.css.primaryColor};
  &:hover {
    text-decoration: none;
    color: ${props => props.theme.css.primaryColor};
  }
  &.disabled {
    pointer-events: none;
    color: ${props => props.theme.css.disabledColor};
  }
  &.active {
    font-weight: 700;
    &:before {
      content: '';
      transform: translateX(-100%);
      width: 0;
      height: 0;
      margin-left: -1rem;
      display: inline-block;
      // draw an arrow pointing to the right before the link.
      border-top: 0.5rem solid transparent;
      border-bottom: 0.5rem solid transparent;
      border-left: 0.5rem solid ${props => props.theme.css.primaryColor};
      border-right: 0.5rem solid transparent;
    }
  }
`;

const StyledLeaseIndex = styled.div`
  grid-area: leaseindex;
  display: flex;
  flex-direction: column;
  text-align: left;
  white-space: nowrap;
  margin: 2rem 0 0 3rem;
  border-right: 1px solid grey;
  row-gap: 0.5rem;
`;

export default LeaseIndex;
