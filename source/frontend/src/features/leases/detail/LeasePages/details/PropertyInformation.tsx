import { InputGroup } from 'components/common/form';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FieldArrayRenderProps, getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import * as React from 'react';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

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
  const formikProps = useFormikContext<IFormLease>();
  const areaUnitType = getIn(formikProps.values, withNameSpace(nameSpace, 'areaUnitType'));
  return (
    <StyledPropertyInfo>
      <SectionField label="PID" labelWidth="3">
        <InputGroup disabled={disabled} field={withNameSpace(nameSpace, 'pid')} />
      </SectionField>
      {!hideAddress ? (
        <SectionField label="Address" labelWidth="3">
          <AddressSubForm nameSpace={withNameSpace(nameSpace, 'address')} disabled={disabled} />
        </SectionField>
      ) : null}
      <SectionField label="Area included" labelWidth="3">
        <InputGroup
          disabled={disabled}
          field={withNameSpace(nameSpace, 'landArea')}
          postText={`${areaUnitType?.description}.` ?? ''}
        />
      </SectionField>
      <hr />
    </StyledPropertyInfo>
  );
};

export default PropertyInformation;
const StyledPropertyInfo = styled.div`
  margin-top: 4rem;
`;
