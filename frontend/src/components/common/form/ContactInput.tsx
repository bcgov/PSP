import classNames from 'classnames';
import { Button, Input } from 'components/common/form';
import { getIn, useFormikContext } from 'formik';
import { IContactSearchResult } from 'interfaces';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import Form from 'react-bootstrap/Form';
import { FormControlProps } from 'react-bootstrap/FormControl';
import { FaAddressBook } from 'react-icons/fa';
import { MdClose } from 'react-icons/md';
import styled from 'styled-components';

import TooltipWrapper from '../TooltipWrapper';
import { DisplayError } from './DisplayError';

type RequiredAttributes = {
  field: string;
  setShowContactManager: React.Dispatch<React.SetStateAction<boolean>>;
  onClear: Function;
};

type OptionalAttributes = {
  label?: string;
  required?: boolean;
  displayErrorTooltips?: boolean;
};

export type ContactInputProps = FormControlProps & OptionalAttributes & RequiredAttributes;

export const ContactInput: React.FC<ContactInputProps> = ({
  field,
  setShowContactManager,
  onClear,
  label,
  required,
  displayErrorTooltips,
}) => {
  const { errors, touched, values } = useFormikContext<any>();
  const error = getIn(errors, field);
  const touch = getIn(touched, field);
  const value: IContactSearchResult | undefined = getIn(values, field);
  const errorTooltip = error && touch && displayErrorTooltips ? error : undefined;

  return (
    <Form.Group
      controlId={`input-${field}`}
      className={classNames(!!required ? 'required' : '', 'input')}
    >
      {!!label && <Form.Label>{label}</Form.Label>}

      <TooltipWrapper toolTipId={`${field}-error-tooltip}`} toolTip={errorTooltip}>
        <Row className="align-items-center">
          <Col>
            <StyledDiv className={!!touch && !!error ? 'is-invalid' : ''}>
              {value?.summary}
            </StyledDiv>
            <Input field={field + '.id'} className="d-none"></Input>
          </Col>
          <Col xs="auto" className="pl-0 align-self-center">
            <Button
              icon={<MdClose size={20} />}
              variant="secondary"
              className="px-2"
              onClick={() => {
                onClear();
              }}
              disabled={value === undefined}
            ></Button>
          </Col>
          <Col xs="auto" className="pl-0">
            <Button
              icon={<FaAddressBook size={20} />}
              className="px-2"
              onClick={() => {
                setShowContactManager(true);
              }}
            ></Button>
          </Col>
        </Row>
      </TooltipWrapper>
      {!displayErrorTooltips && <DisplayError field={field} errorPrompt />}
    </Form.Group>
  );
};

const StyledDiv = styled.div`
  border-radius: 0.3rem;
  border: ${props => props.theme.css.dangerColor} solid 0.1rem;
  padding: 12px 6px 6px 12px;
  background-color: ${props => props.theme.css.filterBackgroundColor};
  min-height: 2.5em;
  :not(.is-invalid) {
    background-image: none;
    border-color: #606060;
  }
`;
