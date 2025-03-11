import React from 'react';

import { Section } from '@/components/common/Section/Section';
import { PropertyInformation } from '@/features/leases';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';

export interface IPropertiesInformationProps {
  lease: ApiGen_Concepts_Lease;
}

/**
 * Formik Field array wrapper around leased properties.
 * @param {IPropertiesInformationProps} param0
 */
export const PropertiesInformation: React.FunctionComponent<
  React.PropsWithChildren<IPropertiesInformationProps>
> = ({ lease }) => {
  const properties: ApiGen_Concepts_PropertyLease[] = React.useMemo(() => {
    return lease?.fileProperties ?? [];
  }, [lease]);

  return properties?.length ? (
    <Section initiallyExpanded={true} isCollapsable={true} header="Property Information">
      {properties.map((property: ApiGen_Concepts_PropertyLease, index) => (
        <PropertyInformation
          property={property}
          hideAddress={false}
          key={`property-${property.id ?? index}`}
        />
      ))}
    </Section>
  ) : (
    <></>
  );
};

export default PropertiesInformation;
