import { FastCurrencyInput, FastDatePicker, Input, TextArea } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { getIn, useFormikContext } from 'formik';
import ITypeCode from 'interfaces/ITypeCode';
import { withNameSpace } from 'utils/formUtils';

import { FormInsurance } from './models';

interface IInsuranceRowProps {
  nameSpace: string;
}

const InsuranceForm: React.FunctionComponent<React.PropsWithChildren<IInsuranceRowProps>> = ({
  nameSpace,
}) => {
  const formikProps = useFormikContext<FormInsurance>();
  const insuranceType: ITypeCode<string> = getIn(
    formikProps.values,
    `${withNameSpace(nameSpace, 'insuranceType')}`,
  );

  return (
    <div data-testid="insurance-form">
      <Section header={insuranceType.description}>
        {insuranceType.id === 'OTHER' && (
          <SectionField label="Insurance Type">
            <Input field={withNameSpace(nameSpace, 'otherInsuranceType')} />
          </SectionField>
        )}
        <SectionField label="Insurance In Place">
          <RadioGroup
            field={withNameSpace(nameSpace, 'isInsuranceInPlaceRadio')}
            flexDirection="row"
            radioValues={[
              {
                radioValue: 'yes',
                radioLabel: 'Yes',
              },
              {
                radioValue: 'no',
                radioLabel: 'No',
              },
            ]}
          />
        </SectionField>
        <SectionField label="Limit ($)">
          <FastCurrencyInput
            formikProps={formikProps}
            field={withNameSpace(nameSpace, 'coverageLimit')}
          />
        </SectionField>
        <SectionField label="Policy Expiry date">
          <FastDatePicker
            field={withNameSpace(nameSpace, 'expiryDate')}
            formikProps={formikProps}
          />
        </SectionField>
        <SectionField label="Description of Coverage">
          <TextArea
            rows={4}
            field={withNameSpace(nameSpace, 'coverageDescription')}
            data-testid="insurance-form-description"
          />
        </SectionField>
      </Section>
    </div>
  );
};

export default InsuranceForm;
