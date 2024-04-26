import { FieldArray, getIn, useFormikContext } from 'formik';
import { Fragment } from 'react';

import { Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { ChargeOnTitle, LtsaOrders } from '@/interfaces/ltsaModels';
import { withNameSpace } from '@/utils/formUtils';

import LtsaChargeOwnerSubForm from './LtsaChargeOwnerSubForm';

export interface ILtsaChargeSubFormProps {
  nameSpace?: string;
}

export const LtsaChargeSubForm: React.FunctionComponent<
  React.PropsWithChildren<ILtsaChargeSubFormProps>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<LtsaOrders>();
  const charges = getIn(values, withNameSpace(nameSpace, 'chargesOnTitle')) ?? [];
  return (
    <div>
      {charges.length === 0 && 'None'}
      <FieldArray
        name={withNameSpace(nameSpace, 'chargesOnTitle')}
        render={() => (
          <Fragment key={`charge-sub-row-${nameSpace}`}>
            {charges.map((charge: ChargeOnTitle, index: number) => {
              const innerNameSpace = withNameSpace(nameSpace, `chargesOnTitle.${index}.charge`);
              return (
                <Fragment key={`charge-sub-row-${innerNameSpace}`}>
                  <SectionField label="Nature">
                    <Input field={`${withNameSpace(innerNameSpace, 'transactionType')}`} />
                  </SectionField>
                  <SectionField label="Registration #">
                    <Input field={`${withNameSpace(innerNameSpace, 'chargeNumber')}`} />
                  </SectionField>
                  <SectionField label="Registered date">
                    <Input field={`${withNameSpace(innerNameSpace, 'applicationReceivedDate')}`} />
                  </SectionField>
                  <LtsaChargeOwnerSubForm nameSpace={innerNameSpace} />
                  {index < charges.length - 1 && <hr></hr>}
                </Fragment>
              );
            })}
          </Fragment>
        )}
      />
    </div>
  );
};

export default LtsaChargeSubForm;
