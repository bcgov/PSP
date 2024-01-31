import { getIn, useFormikContext } from 'formik';
import { Col, Row } from 'react-bootstrap';

import { FastCurrencyInput, FastDatePicker, Input, TextArea } from '@/components/common/form';
import { RadioGroup } from '@/components/common/form/RadioGroup';
import { FormSection } from '@/components/common/form/styles';
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
        <Row>
          <Col md="4">
            <div>
              <strong>Insurance Type:</strong>
            </div>
            <Input field={withNameSpace(nameSpace, 'otherInsuranceType')} />
          </Col>
        </Row>
      )}
      <Row className="py-4">
        <Col md="auto">
          <strong>Insurance In Place:</strong>
        </Col>
        <Col>
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
        </Col>
      </Row>
      <Row>
        <Col>
          <div>
            <strong>Limit ($)</strong>
          </div>
          <FastCurrencyInput
            formikProps={formikProps}
            field={withNameSpace(nameSpace, 'coverageLimit')}
          />
          <div>
            <strong>Policy Expiry date:</strong>
          </div>
          <FastDatePicker
            field={withNameSpace(nameSpace, 'expiryDate')}
            formikProps={formikProps}
          />
        </Col>
        <Col md={8}>
          <div>
            <strong>Description of Coverage</strong>
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
