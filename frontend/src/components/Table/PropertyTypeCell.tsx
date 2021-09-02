import { ReactComponent as BuildingSvg } from 'assets/images/icon-business.svg';
import { ReactComponent as LandSvg } from 'assets/images/icon-lot.svg';
import { ReactComponent as SubdivisionSvg } from 'assets/images/project-diagram-solid.svg';
import { PropertyTypes } from 'constants/propertyTypes';
import { IProperty } from 'interfaces';
import React from 'react';
import { CellProps } from 'react-table';

/**
 * Display an icon based on the property type.
 * @param {CellProps<IProperty, number>} param0
 */
export const PropertyTypeCell = ({ cell: { value } }: CellProps<IProperty, PropertyTypes>) => {
  switch (value) {
    case PropertyTypes.Subdivision:
      return <SubdivisionSvg className="svg" />;

    case PropertyTypes.Building:
      return <BuildingSvg className="svg" />;
    default:
      return <LandSvg className="svg" />;
  }
};
