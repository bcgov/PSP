import { Input } from 'components/common/form';
import { FieldArray, getIn, useFormikContext } from 'formik';
import { LtsaOrders, TitleTransferDisposition } from 'interfaces/ltsaModels';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

import { SectionFieldWrapper } from '../SectionFieldWrapper';
import { StyledSectionHeader } from '../SectionStyles';

export interface ILtsaTransferSubFormProps {
  nameSpace?: string;
}

export const LtsaTransferSubForm: React.FunctionComponent<ILtsaTransferSubFormProps> = ({
  nameSpace,
}) => {
  const { values } = useFormikContext<LtsaOrders>();
  const transfers = getIn(values, withNameSpace(nameSpace, 'titleTransfersOrDispositions')) ?? [];
  return (
    <>
      <StyledSectionHeader>Transfers</StyledSectionHeader>
      {transfers.length === 0 && 'this title has no transfers'}
      <FieldArray
        name={withNameSpace(nameSpace, 'titleTransfersOrDispositions')}
        render={({ name }) => (
          <React.Fragment key={`transfer-row-${name}`}>
            {transfers.map((transfer: TitleTransferDisposition, index: number) => {
              const innerNameSpace = withNameSpace(
                nameSpace,
                `titleTransfersOrDispositions.${index}`,
              );
              return (
                <>
                  <SectionFieldWrapper label="Nature">
                    <Input field={`${withNameSpace(innerNameSpace, 'disposition')}`} />
                  </SectionFieldWrapper>
                  <SectionFieldWrapper label="Registration #">
                    <Input field={`${withNameSpace(innerNameSpace, 'dispositionDate')}`} />
                  </SectionFieldWrapper>
                  <SectionFieldWrapper label="Registered date">
                    <Input field={`${withNameSpace(innerNameSpace, 'titleNumber')}`} />
                  </SectionFieldWrapper>
                  <SectionFieldWrapper label="Registered date">
                    <Input field={`${withNameSpace(innerNameSpace, 'landLandDistrict')}`} />
                  </SectionFieldWrapper>
                  {index < transfers.length - 1 && <hr></hr>}
                </>
              );
            })}
            {transfers.length !== 0 && (
              <SectionFieldWrapper label="Pending applications">
                <Input
                  disabled
                  field="parcelInfo.orderedProduct.fieldedData.pendingApplicationCount"
                />
              </SectionFieldWrapper>
            )}
          </React.Fragment>
        )}
      />
    </>
  );
};

export default LtsaTransferSubForm;
