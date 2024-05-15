import { getIn, useFormikContext } from 'formik';
import { Col, Row } from 'react-bootstrap';

import { FastCurrencyInput, FastDatePicker, Input, TextArea } from '@/components/common/form';
import { FormSection } from '@/components/common/form/styles';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { withNameSpace } from '@/utils/formUtils';

import { FormInsurance } from './models';
import { SubTitle } from './styles';

interface IInsuranceRowProps {
  nameSpace: string;
}

const InsuranceForm: React.FunctionComponent<React.PropsWithChildren<IInsuranceRowProps>> = ({
  nameSpace,
}) => {
  const formikProps = useFormikContext<FormInsurance>();
  const insuranceType: ApiGen_Base_CodeType<string> = getIn(
    formikProps.values,
    `${withNameSpace(nameSpace, 'insuranceType')}`,
  );

  return (
    <FormSection className="mb-3" data-testid="insurance-form">
      <SubTitle data-testid="insurance-form-title">{insuranceType.description}</SubTitle>
      {insuranceType.id === 'OTHER' && (
        <SectionField label="Insurance type" labelWidth="3">
          <Input field={withNameSpace(nameSpace, 'otherInsuranceType')} />
        </SectionField>
      )}
      <SectionField label="Insurance in place" labelWidth="3">
        <YesNoSelect field={withNameSpace(nameSpace, 'isInsuranceInPlaceSelect')} />
      </SectionField>
      <Row>
        <Col>
          <div>
            <strong>Limit ($):</strong>
          </div>
          <FastCurrencyInput
            formikProps={formikProps}
            field={withNameSpace(nameSpace, 'coverageLimit')}
          />
          <div>
            <strong>Policy expiry date:</strong>
          </div>
          <FastDatePicker
            field={withNameSpace(nameSpace, 'expiryDate')}
            formikProps={formikProps}
          />
        </Col>
        <Col md={8}>
          <div>
            <strong>Description of coverage:</strong>
          </div>
          <TextArea
            rows={4}
            field={withNameSpace(nameSpace, 'coverageDescription')}
            data-testid="insurance-form-description"
          />
        </Col>
      </Row>
    </FormSection>
  );
};

export default InsuranceForm;
