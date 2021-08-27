import { Label } from 'components/common/Label';
import { IProperty } from 'interfaces';
import * as React from 'react';
import ListGroup from 'react-bootstrap/ListGroup';

interface IAssociatedBuildings {
  /** the list of buildings */
  buildings: IProperty[];
}

/**
 * Component that displays the associated buildings of a parcel
 * as a clickable list
 * @param buildings the list of buildings
 */
export const AssociatedBuildingsList: React.FC<IAssociatedBuildings> = ({ buildings }) => {
  return (
    <>
      <ListGroup>
        <Label className="header" style={{ margin: '5px 0px' }}>
          Associated Buildings
        </Label>
        {buildings.length ? (
          buildings.map((property, propertyId) => (
            <ListGroup.Item key={propertyId}>
              <p>{property.pid ? property.pid : property.pin}</p>
            </ListGroup.Item>
          ))
        ) : (
          <ListGroup.Item>This parcel has no associated buildings.</ListGroup.Item>
        )}
      </ListGroup>
    </>
  );
};

export default AssociatedBuildingsList;
