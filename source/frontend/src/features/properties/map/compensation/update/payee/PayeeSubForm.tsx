import { Check, FastCurrencyInput, Input, Select, SelectOption } from 'components/common/form';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FormikProps } from 'formik';
import { withNameSpace } from 'utils/formUtils';

import { CompensationRequisitionFormModel } from '../../models';

export interface IPayeeSubFormProps {
  nameSpace: string;
  formikProps: FormikProps<CompensationRequisitionFormModel>;
  payeeListOptions: SelectOption[];
}

export const PayeeSubForm: React.FunctionComponent<IPayeeSubFormProps> = ({
  nameSpace,
  formikProps,
  payeeListOptions,
}) => {
  return (
    <>
      <SectionField label="Payee">
        <Select options={payeeListOptions} field={withNameSpace(nameSpace, 'acquisitionOwnerId')} />
      </SectionField>
      <SectionField label="Payment in trust">
        <Check field={withNameSpace(nameSpace, 'cheques[0].isPaymentInTrust')} />
      </SectionField>
      <SectionField label={'GstNumber'}>
        <Input field={withNameSpace(nameSpace, 'cheques[0].gstNumber')}></Input>
      </SectionField>
      <SectionField label="Amount (before tax)">
        <FastCurrencyInput
          field={withNameSpace(nameSpace, 'cheques[0].pretaxAmout')}
          formikProps={formikProps}
          disabled
        />
      </SectionField>
      <SectionField label="GST amount">
        <FastCurrencyInput
          field={withNameSpace(nameSpace, 'cheques[0].taxAmount')}
          formikProps={formikProps}
          disabled
        />
      </SectionField>
      <SectionField label="Total amount">
        <FastCurrencyInput
          field={withNameSpace(nameSpace, 'cheques[0].totalAmount')}
          formikProps={formikProps}
          disabled
        />
      </SectionField>
    </>
  );
};

export default PayeeSubForm;
