import variables from '_variables.module.scss';
import { PropertyTypes } from 'constants/propertyTypes';
import { IProperty } from 'interfaces';
import * as React from 'react';
import Row from 'react-bootstrap/Row';
import { Link, useLocation } from 'react-router-dom';
import styled from 'styled-components';

const LinkMenu = styled(Row)`
  background-color: ${variables.filterBackgroundColor};
  height: 35px;
  width: 322px;
  margin: 0px 0px 5px -10px;
  font-size: 14px;
  padding: 10px;
  a {
    padding: 0px 10px;
    color: ${variables.slideOutBlue};
  }
`;

interface IHeaderActions {
  /** The selected property */
  propertyInfo: IProperty | null;
  /** the selected property type */
  propertyTypeId: PropertyTypes | null;
  jumpToView: () => void;
  zoomToView: () => void;
  /** additional action to be taken when a link in the menu is clicked */
  onLinkClick?: () => void;
  /** whether the user has the correct organization/permissions to view all the details */
  canViewDetails: boolean;
  /** whether the user has the correct organization/permissions to edit property details */
  canEditDetails: boolean;
}

/**
 * Actions that can be done on the property info slide out
 * @param propertyInfo the selected property information
 * @param propertyTypeId the selected property type
 * @param onLinkClick additional action on menu item click
 * @param canViewDetails user can view all property details
 * @param canEditDetails user can edit property details
 */
const HeaderActions: React.FC<IHeaderActions> = ({
  propertyInfo,
  propertyTypeId,
  onLinkClick,
  jumpToView,
  zoomToView,
  canViewDetails,
  canEditDetails,
}) => {
  const location = useLocation();

  return (
    <LinkMenu>
      Actions:
      <Link to={{ ...location }} onClick={zoomToView}>
        Zoom map
      </Link>
    </LinkMenu>
  );
};

export default HeaderActions;
