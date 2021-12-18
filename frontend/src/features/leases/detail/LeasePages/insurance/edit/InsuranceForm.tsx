import { FastDatePicker, TextArea } from 'components/common/form';
import { RadioGroup } from 'components/common/form/RadioGroup';
import { FormSection, InlineInput } from 'components/common/form/styles';
import { getIn, useFormikContext } from 'formik';
import { Col, Row } from 'react-bootstrap';
import { withNameSpace } from 'utils/formUtils';

import { FormInsurance } from './models';
import { SubTitle } from './styles';

interface IInsuranceRowProps {
  nameSpace: string;
}

const InsuranceForm: React.FunctionComponent<IInsuranceRowProps> = ({ nameSpace }) => {
  const formikProps = useFormikContext<FormInsurance>();
  const description = getIn(
    formikProps.values,
    `${withNameSpace(nameSpace, 'insuranceType.description')}`,
  );

  return (
    <FormSection className="mb-3">
      <SubTitle>{description}</SubTitle>
      <Row className="py-2">
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
          <InlineInput type="number" field={withNameSpace(nameSpace, 'coverageLimit')} />
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
          <TextArea rows={4} field={withNameSpace(nameSpace, 'coverageDescription')} />
        </Col>
      </Row>
    </FormSection>
  );
};

export default InsuranceForm;
