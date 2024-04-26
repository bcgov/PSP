import { FieldArray, getIn, useFormikContext } from 'formik';
import React, { Fragment } from 'react';

import { Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { LtsaOrders, TitleTransferDisposition } from '@/interfaces/ltsaModels';
import { withNameSpace } from '@/utils/formUtils';
export interface ILtsaTransferSubFormProps {
  nameSpace?: string;
}

export const LtsaTransferSubForm: React.FunctionComponent<
  React.PropsWithChildren<ILtsaTransferSubFormProps>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<LtsaOrders>();
  const transfers = getIn(values, withNameSpace(nameSpace, 'titleTransfersOrDispositions')) ?? [];
  return (
    <>
      {transfers.length === 0 && 'None'}
      <FieldArray
        name={withNameSpace(nameSpace, 'titleTransfersOrDispositions')}
        render={({ name }) =>
          transfers.map((transfer: TitleTransferDisposition, index: number) => {
            const innerNameSpace = withNameSpace(
              nameSpace,
              `titleTransfersOrDispositions.${index}`,
            );
            return (
              <Fragment key={`transfer-row-${name}-index`}>
                <SectionField label="Nature">
                  <Input field={`${withNameSpace(innerNameSpace, 'disposition')}`} />
                </SectionField>
                <SectionField label="Registration #">
                  <Input field={`${withNameSpace(innerNameSpace, 'dispositionDate')}`} />
                </SectionField>
                <SectionField label="Registered date">
                  <Input field={`${withNameSpace(innerNameSpace, 'titleNumber')}`} />
                </SectionField>
                <SectionField label="Registered date">
                  <Input field={`${withNameSpace(innerNameSpace, 'landLandDistrict')}`} />
                </SectionField>
                {index < transfers.length - 1 && <hr></hr>}
              </Fragment>
            );
          })
        }
      />
      {transfers.length !== 0 && (
        <SectionField label="Pending applications">
          <Input disabled field="parcelInfo.orderedProduct.fieldedData.pendingApplicationCount" />
        </SectionField>
      )}
    </>
  );
};

export default LtsaTransferSubForm;
