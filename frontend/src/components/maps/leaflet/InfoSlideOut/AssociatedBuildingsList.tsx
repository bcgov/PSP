import { Label } from 'components/common/Label';
import { IParcel } from 'interfaces';
import * as React from 'react';
import ListGroup from 'react-bootstrap/ListGroup';

interface IAssociatedBuildings {
  /** the selected property information */
  propertyInfo: IParcel | null;
  /** whether the user has the correct agency/permissions to edit property details */
  canEditDetails: boolean;
}

/**
 * Component that displays the associated buildings of a parcel
 * as a clickable list
 * @param propertyInfo the selected parcel
 * @param addAssociatedBuildingLink link to create a new associated building
 * @param canEditDetails whether the user can edit the parcel details
 */
export const AssociatedBuildingsList: React.FC<IAssociatedBuildings> = ({
  propertyInfo,
  canEditDetails,
}) => {
  return (
    <>
      <ListGroup>
        <Label className="header" style={{ margin: '5px 0px' }}>
          Associated Buildings
        </Label>
        {propertyInfo?.buildings?.length ? (
          propertyInfo?.buildings?.map((building, buildingId) => (
            <ListGroup.Item key={buildingId}>
              <p>{building.name}</p>
            </ListGroup.Item>
          ))
        ) : (
          <ListGroup.Item>This parcel has no associated buildings</ListGroup.Item>
        )}
      </ListGroup>
    </>
  );
};

export default AssociatedBuildingsList;
