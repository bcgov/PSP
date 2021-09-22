import variables from '_variables.module.scss';
import { PropertyTypes } from 'constants/propertyTypes';
import { IProperty } from 'interfaces';
import * as React from 'react';
import Row from 'react-bootstrap/Row';
import styled from 'styled-components';

const LinkMenu = styled(Row)`
  background-color: ${variables.filterBackgroundColor};
  height: 3.5rem;
  width: 32.2rem;
  margin: 0rem 0rem 0.5rem -1rem;
  font-size: 1.4rem;
  padding: 1rem;
  a {
    padding: 0rem 1rem;
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
  //const location = useLocation();

  return (
    <LinkMenu>
      Actions:
      {/* TODO: disable this functionality until the frontend supports the new database projection <Link to={{ ...location }} onClick={zoomToView}>
        Zoom map
      </Link> */}
    </LinkMenu>
  );
};

export default HeaderActions;
