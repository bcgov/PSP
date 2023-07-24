import { FieldArray, getIn, useFormikContext } from 'formik';
import { LatLngLiteral } from 'leaflet';
import * as React from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Section } from '@/components/common/Section/Section';
import { PropertyInformation } from '@/features/leases';
import { FormLeaseProperty, LeaseFormModel } from '@/features/leases/models';
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
  const { values } = useFormikContext<LeaseFormModel>();

  const properties: FormLeaseProperty[] = React.useMemo(() => {
    return getIn(values, withNameSpace(nameSpace, 'properties')) ?? [];
  }, [values, nameSpace]);

  const { setFilePropertyLocations } = useMapStateMachine();

  const locations: LatLngLiteral[] = React.useMemo(() => {
    return (
      properties
        .map<LatLngLiteral | undefined>(x => {
          if (x.property?.latitude !== undefined && x.property?.longitude !== undefined) {
            return { lat: x.property?.latitude, lng: x.property?.longitude };
          } else {
            return undefined;
          }
        })
        .filter((x): x is LatLngLiteral => x !== undefined) || []
    );
  }, [properties]);

  React.useEffect(() => {
    setFilePropertyLocations(locations);
  }, [setFilePropertyLocations, locations]);

  return properties?.length ? (
    <Section initiallyExpanded={true} isCollapsable={true} header="Property Information">
      <FieldArray
        name={withNameSpace(nameSpace, 'properties')}
        render={renderProps =>
          properties.map((property: FormLeaseProperty, index) => (
            <PropertyInformation
              {...renderProps}
              nameSpace={withNameSpace(nameSpace, `properties.${index}.property`)}
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
