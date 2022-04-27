import { LinkButton } from 'components/common/buttons';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { IFormLease, IProperty } from 'interfaces';
import React from 'react';
import { withNameSpace } from 'utils/formUtils';

import PropertyRow from './PropertyRow';

export interface IPropertyRowsProps {
  nameSpace?: string;
}

/**
 * Formik Field array wrapper around leased properties.
 * @param {IPropertyRowsProps} param0
 */
export const PropertyRows: React.FunctionComponent<IPropertyRowsProps> = ({ nameSpace }) => {
  const { values } = useFormikContext<IFormLease>();
  const properties: IProperty[] = getIn(values, withNameSpace(nameSpace, 'properties')) ?? [];
  return (
    <FieldArray
      name={withNameSpace(nameSpace, 'properties')}
      render={({ push, remove, name }) => (
        <React.Fragment key={`property-row-${name}`}>
          {properties.map((property: IProperty, index) => (
            <PropertyRow
              nameSpace={withNameSpace(nameSpace, `properties.${index}`)}
              onRemove={() => remove(index)}
              key={`property-row-${index}`}
            />
          ))}
          <LinkButton
            onClick={() => {
              push({ pid: '', pin: '', areaUnitId: '' });
            }}
          >
            + Add another property
          </LinkButton>
        </React.Fragment>
      )}
    />
  );
};

export default PropertyRows;
