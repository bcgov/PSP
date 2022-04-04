import { SelectProperty } from 'components/common/mapping/SelectProperty';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { getIn, useFormikContext } from 'formik';
import * as React from 'react';
import { Col, Form as BsForm, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

import { IPropertySelectorModel } from './PropertySelectorFormView';

interface IPropertySelectorSubFormProps {
  nameSpace?: string;
  onClickDraftMarker: () => void;
  onClickAway: () => void;
}

export const PropertySelectorSubForm: React.FunctionComponent<IPropertySelectorSubFormProps> = ({
  nameSpace,
  onClickDraftMarker,
  onClickAway,
}) => {
  const { values } = useFormikContext<IPropertySelectorModel>();
  const pid = getIn(values, withNameSpace(nameSpace, 'pid'));
  const planNumber = getIn(values, withNameSpace(nameSpace, 'planNumber'));
  const address = getIn(values, withNameSpace(nameSpace, 'address'));
  const legalDescription = getIn(values, withNameSpace(nameSpace, 'legalDescription'));
  return (
    <StyledFormRow>
      <Col md={4}>
        <SelectProperty onClickAway={onClickAway} onClick={onClickDraftMarker} />
      </Col>
      <Col md={8}>
        <Row>
          <GroupHeader>Selected property attributes</GroupHeader>
        </Row>
        <SectionField label="PID">{pid}</SectionField>
        <SectionField label="Plan #">{planNumber}</SectionField>
        <SectionField label="Address">{address}</SectionField>
        <Row>
          <Col md={12}>
            <BsForm.Label>Legal Description:</BsForm.Label>
          </Col>
          <Col md={12}>{legalDescription}</Col>
        </Row>
      </Col>
    </StyledFormRow>
  );
};

export const StyledFormRow = styled(Row)`
  &&& {
    input,
    select,
    textarea {
      background: none;
      border: none;
      resize: none;
      height: fit-content;
      padding: 0;
    }
    .form-label {
      font-weight: bold;
      color: ${props => props.theme.css.textColor};
    }
  }
`;

const GroupHeader = styled(Col)`
  color: ${props => props.theme.css.primaryColor};
  font-family: 'BcSans-Bold';
  margin-bottom: 1rem;
`;

export default PropertySelectorSubForm;
