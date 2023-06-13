import { FieldArray, getIn, useFormikContext } from 'formik';
import * as React from 'react';

import { Section } from '@/components/common/Section/Section';
import { PropertyInformation } from '@/features/leases';
import { ILease, IProperty } from '@/interfaces';
import { withNameSpace } from '@/utils/formUtils';

export interface IPropertiesInformationProps {
  nameSpace?: string;
  disabled?: boolean;
  hideAddress?: boolean;
}

/**
 * Formik Field array wrapper around leased properties.
 * @param {IPropertiesInformationProps} param0
 */
export const PropertiesInformation: React.FunctionComponent<
  React.PropsWithChildren<IPropertiesInformationProps>
> = ({ nameSpace, disabled, hideAddress }) => {
  const { values } = useFormikContext<ILease>();
  const properties: IProperty[] = getIn(values, withNameSpace(nameSpace, 'properties')) ?? [];
  return properties?.length ? (
    <Section initiallyExpanded={true} isCollapsable={true} header="Property Information">
      <FieldArray
        name={withNameSpace(nameSpace, 'properties')}
        render={renderProps =>
          properties.map((property: IProperty, index) => (
            <PropertyInformation
              {...renderProps}
              nameSpace={withNameSpace(nameSpace, `properties.${index}`)}
              disabled={disabled}
              hideAddress={hideAddress}
            />
          ))
        }
      ></FieldArray>
    </Section>
  ) : (
    <></>
  );
};

export default PropertiesInformation;
