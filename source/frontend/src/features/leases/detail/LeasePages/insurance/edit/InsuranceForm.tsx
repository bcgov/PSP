import { getIn, useFormikContext } from 'formik';

import { FastCurrencyInput, FastDatePicker, Input, TextArea } from '@/components/common/form';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { withNameSpace } from '@/utils/formUtils';

import { FormInsurance } from './models';

interface IInsuranceRowProps {
  nameSpace: string;
}

const InsuranceForm: React.FunctionComponent<IInsuranceRowProps> = ({ nameSpace }) => {
  const formikProps = useFormikContext<FormInsurance>();
  const insuranceType: ApiGen_Base_CodeType<string> = getIn(
    formikProps.values,
    `${withNameSpace(nameSpace, 'insuranceType')}`,
  );

  return (
    <Section header={insuranceType.description} data-testid="insurance-form">
      {insuranceType.id === 'OTHER' && (
        <SectionField label="Other insurance type">
          <Input
            field={withNameSpace(nameSpace, 'otherInsuranceType')}
            placeholder="Description of the other insurance coverage"
          />
        </SectionField>
      )}
      <SectionField label="Insurance in place">
        <YesNoSelect field={withNameSpace(nameSpace, 'isInsuranceInPlaceSelect')} />
      </SectionField>
      <SectionField label="Limit ($)" contentWidth={{ xs: 3 }}>
        <FastCurrencyInput
          formikProps={formikProps}
          field={withNameSpace(nameSpace, 'coverageLimit')}
        />
      </SectionField>
      <SectionField label="Policy expiry">
        <FastDatePicker field={withNameSpace(nameSpace, 'expiryDate')} formikProps={formikProps} />
      </SectionField>
      <SectionField label="Description of coverage" contentWidth={{ xs: 12 }}>
        <TextArea
          rows={4}
          field={withNameSpace(nameSpace, 'coverageDescription')}
          data-testid="insurance-form-description"
        />
      </SectionField>
    </Section>
  );
};

export default InsuranceForm;
