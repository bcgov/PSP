import { FastDatePicker, TextArea } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import { FormSection, InlineInput } from 'components/common/form/styles';
import { getIn, useFormikContext } from 'formik';
import ITypeCode from 'interfaces/ITypeCode';
import { Col, Row } from 'react-bootstrap';
import { withNameSpace } from 'utils/formUtils';

import { FormInsurance } from './models';
import { SubTitle } from './styles';

interface IInsuranceRowProps {
  nameSpace: string;
}

const InsuranceForm: React.FunctionComponent<IInsuranceRowProps> = ({ nameSpace }) => {
  const formikProps = useFormikContext<FormInsurance>();
  const insuranceType: ITypeCode<string> = getIn(
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
            <InlineInput field={withNameSpace(nameSpace, 'otherInsuranceType')} />
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
          <InlineInput
            type="number"
            field={withNameSpace(nameSpace, 'coverageLimit')}
            data-testid="insurance-form-limit"
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
