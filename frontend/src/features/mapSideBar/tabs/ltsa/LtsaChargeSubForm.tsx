import { Input } from 'components/common/form';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { ChargeOnTitle, LtsaOrders } from 'interfaces/ltsaModels';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

import { SectionFieldWrapper } from '../SectionFieldWrapper';
import { StyledSectionHeader } from '../SectionStyles';
import LtsaChargeOwnerSubForm from './LtsaChargeOwnerSubForm';

export interface ILtsaChargeSubFormProps {
  nameSpace?: string;
}

export const LtsaChargeSubForm: React.FunctionComponent<ILtsaChargeSubFormProps> = ({
  nameSpace,
}) => {
  const { values } = useFormikContext<LtsaOrders>();
  const charges = getIn(values, withNameSpace(nameSpace, 'chargesOnTitle')) ?? [];
  return (
    <>
      <StyledSectionHeader>Charges, Liens and Interests</StyledSectionHeader>
      {charges.length === 0 && 'this title has no charges'}
      <FieldArray
        name={withNameSpace(nameSpace, 'chargesOnTitle')}
        render={({ push, remove, name }) => (
          <React.Fragment key={`charge-row-${name}`}>
            {charges.map((charge: ChargeOnTitle, index: number) => {
              const innerNameSpace = withNameSpace(nameSpace, `chargesOnTitle.${index}.charge`);
              return (
                <>
                  <SectionFieldWrapper label="Nature">
                    <Input field={`${withNameSpace(innerNameSpace, 'transactionType')}`} />
                  </SectionFieldWrapper>
                  <SectionFieldWrapper label="Registration #">
                    <Input field={`${withNameSpace(innerNameSpace, 'chargeNumber')}`} />
                  </SectionFieldWrapper>
                  <SectionFieldWrapper label="Registered date">
                    <Input field={`${withNameSpace(innerNameSpace, 'applicationReceivedDate')}`} />
                  </SectionFieldWrapper>
                  <LtsaChargeOwnerSubForm nameSpace={innerNameSpace} />
                  {index < charges.length - 1 && <hr></hr>}
                </>
              );
            })}
          </React.Fragment>
        )}
      />
    </>
  );
};

export default LtsaChargeSubForm;
