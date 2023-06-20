import { FieldArrayRenderProps, getIn, useFormikContext } from 'formik';
import * as React from 'react';
import styled from 'styled-components';

import { Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { LeaseFormModel } from '@/features/leases/models';
import { formatNumber, pidFormatter } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';

import AddressSubForm from '../AddressSubForm';

export interface IPropertyInformationProps {
  nameSpace: string;
  hideAddress?: boolean;
  disabled?: boolean;
}

/**
 * Sub-form displaying a property associated to the current lease.
 * @param {IPropertyInformationProps} param0
 */
export const PropertyInformation: React.FunctionComponent<
  React.PropsWithChildren<IPropertyInformationProps & Partial<FieldArrayRenderProps>>
> = ({ nameSpace, disabled, hideAddress }) => {
  const formikProps = useFormikContext<LeaseFormModel>();
  const landArea = getIn(formikProps.values, withNameSpace(nameSpace, 'landArea'));
  const areaUnitType = getIn(formikProps.values, withNameSpace(nameSpace, 'areaUnitType'));
  const legalDescription = getIn(formikProps.values, withNameSpace(nameSpace, 'legalDescription'));
  const pid = getIn(formikProps.values, withNameSpace(nameSpace, 'pid'));
  const pidText = pid ? `PID: ${pidFormatter(pid)}` : '';
  const legalPidText = [legalDescription, pidText].filter(x => x).join(' ');
  return (
    <StyledPropertyInfo>
      <SectionField label="Descriptive name" labelWidth="3">
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'propertyName')} />
      </SectionField>
      <SectionField label="Area included" labelWidth="3">
        {formatNumber(landArea, 2, 2)}{' '}
        {areaUnitType?.description ? `${areaUnitType?.description}.` : ''}
      </SectionField>
      {!hideAddress ? (
        <SectionField label="Address" labelWidth="3">
          <AddressSubForm nameSpace={withNameSpace(nameSpace, 'address')} disabled={disabled} />
        </SectionField>
      ) : null}
      <SectionField label="Legal description" labelWidth="3">
        {legalPidText}
      </SectionField>

      <hr />
    </StyledPropertyInfo>
  );
};

export default PropertyInformation;
const StyledPropertyInfo = styled.div`
  margin-top: 4rem;
`;
