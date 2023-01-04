import { PropertyInformation } from 'features/leases';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { ILease, IProperty } from 'interfaces';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

export interface IPropertiesInformationProps {
  nameSpace?: string;
  disabled?: boolean;
}

/**
 * Formik Field array wrapper around leased properties.
 * @param {IPropertiesInformationProps} param0
 */
export const PropertiesInformation: React.FunctionComponent<
  React.PropsWithChildren<IPropertiesInformationProps>
> = ({ nameSpace, disabled }) => {
  const { values } = useFormikContext<ILease>();
  const properties: IProperty[] = getIn(values, withNameSpace(nameSpace, 'properties')) ?? [];
  return (
    <FieldArray
      name={withNameSpace(nameSpace, 'properties')}
      render={renderProps =>
        properties.map((property: IProperty, index) => (
          <PropertyInformation
            {...renderProps}
            nameSpace={withNameSpace(nameSpace, `properties.${index}`)}
            disabled={disabled}
          />
        ))
      }
    ></FieldArray>
  );
};

export default PropertiesInformation;
