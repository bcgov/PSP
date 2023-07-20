import classNames from 'classnames';
import { getIn, useFormikContext } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import Form from 'react-bootstrap/Form';
import { FormControlProps } from 'react-bootstrap/FormControl';
import { FaAddressBook } from 'react-icons/fa';
import { MdClose } from 'react-icons/md';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { Input } from '@/components/common/form';
import { IContactSearchResult } from '@/interfaces';
import { formatNames } from '@/utils/personUtils';

import { LinkButton } from '../buttons';
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

export const ContactInput: React.FC<React.PropsWithChildren<ContactInputProps>> = ({
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
  const contactInfo: IContactSearchResult | undefined = getIn(values, field);
  const errorTooltip = error && touch && displayErrorTooltips ? error : undefined;

  var text = 'Select from contacts';

  if (contactInfo !== undefined) {
    if (contactInfo.personId !== undefined) {
      text = formatNames([contactInfo.firstName, contactInfo.middleNames, contactInfo.surname]);
    } else if (contactInfo.organizationId !== undefined) {
      text = contactInfo.organizationName || '';
    }
  }

  return (
    <Form.Group
      controlId={`input-${field}`}
      className={classNames(!!required ? 'required' : '', 'input')}
    >
      {!!label && <Form.Label>{label}</Form.Label>}

      <TooltipWrapper toolTipId={`${field}-error-tooltip}`} toolTip={errorTooltip}>
        <Row>
          <Col>
            <StyledDiv className={!!error ? 'is-invalid' : ''}>
              {text}
              <StyledRemoveLinkButton
                onClick={() => {
                  onClear();
                }}
                disabled={contactInfo === undefined}
              >
                <MdClose size="2rem" title="remove" />
              </StyledRemoveLinkButton>
            </StyledDiv>
            <Input
              field={field + '.id'}
              placeholder="Select from Contacts"
              className="d-none"
            ></Input>
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
      {!displayErrorTooltips && <DisplayError field={field} errorPrompt={true} />}
    </Form.Group>
  );
};

const StyledDiv = styled.div`
  position: relative;
  border-radius: 0.3rem;
  padding: 0.6rem;
  padding-right: 2.1rem;
  min-height: 2.5em;
  background-image: none;
  color: ${props => props.theme.css.formControlTextColor};
  border: ${props => props.theme.css.lightVariantColor} solid 0.1rem;
  &.is-invalid {
    border: ${props => props.theme.css.dangerColor} solid 0.1rem;
  }
`;

export const StyledRemoveLinkButton = styled(LinkButton)`
  &&.btn {
    position: absolute;
    top: calc(50% - 1.4rem);
    right: 0.4rem;
    color: ${props => props.theme.css.primaryBorderColor};
    text-decoration: none;
    line-height: unset;
    .text {
      display: none;
    }
    &:hover,
    &:active,
    &:focus {
      color: ${props => props.theme.css.dangerColor};
      text-decoration: none;
      opacity: unset;
    }
  }
`;
