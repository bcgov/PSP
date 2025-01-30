import { FieldArray, getIn, useFormikContext } from 'formik';
import React from 'react';

import { Section } from '@/components/common/Section/Section';
import { PropertyInformation } from '@/features/leases';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
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
  const { values } = useFormikContext<ApiGen_Concepts_Lease>();

  const properties: ApiGen_Concepts_PropertyLease[] = React.useMemo(() => {
    return getIn(values, withNameSpace(nameSpace, 'fileProperties')) ?? [];
  }, [values, nameSpace]);

  return properties?.length ? (
    <Section initiallyExpanded={true} isCollapsable={true} header="Property Information">
      <FieldArray
        name={withNameSpace(nameSpace, 'fileProperties')}
        render={renderProps =>
          properties.map((property: ApiGen_Concepts_PropertyLease, index) => (
            <PropertyInformation
              {...renderProps}
              nameSpace={withNameSpace(nameSpace, `fileProperties.${index}`)}
              disabled={disabled}
              hideAddress={hideAddress}
              key={`property-${property.id ?? index}`}
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
