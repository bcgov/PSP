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
import { exists, isValidId } from '@/utils';
import { formatNames } from '@/utils/personUtils';

import { LinkButton } from '../buttons';
import TooltipWrapper from '../TooltipWrapper';
import { DisplayError } from './DisplayError';

type RequiredAttributes = {
  field: string;
  setShowContactManager: React.Dispatch<React.SetStateAction<boolean>>;
  onClear: () => void;
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

  let text = 'Select from contacts';

  if (contactInfo !== undefined) {
    if (isValidId(contactInfo.personId)) {
      text = formatNames([contactInfo.firstName, contactInfo.middleNames, contactInfo.surname]);
    } else if (isValidId(contactInfo.organizationId)) {
      text = contactInfo.organizationName || '';
    }
  }

  return (
    <Form.Group
      controlId={`input-${field}`}
      className={classNames(required ? 'required' : '', 'input')}
    >
      {!!label && <Form.Label>{label}</Form.Label>}

      <TooltipWrapper tooltipId={`${field}-error-tooltip}`} tooltip={errorTooltip}>
        <Row>
          <Col>
            <StyledInputLikeDiv
              className={error ? 'is-invalid' : ''}
              $hasContact={exists(contactInfo)}
              onClick={() => {
                if (!exists(contactInfo)) {
                  setShowContactManager(true);
                }
              }}
            >
              {text}
              {exists(contactInfo) && (
                <StyledRemoveLinkButton
                  onClick={() => {
                    onClear();
                  }}
                >
                  <MdClose size="2rem" title="remove" />
                </StyledRemoveLinkButton>
              )}
            </StyledInputLikeDiv>
            <Input
              field={field + '.id'}
              placeholder="Select from Contacts"
              className="d-none"
              defaultValue=""
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

const StyledInputLikeDiv = styled.div<{ $hasContact: boolean }>`
  position: relative;
  border-radius: 0.4rem;
  padding-top: 0.8rem;
  padding-bottom: 0.8rem;
  padding-left: 1.2rem;
  padding-right: 2.8rem;

  background-image: none;
  color: ${props =>
    props.$hasContact
      ? props.theme.bcTokens.typographyColorSecondary
      : props.theme.bcTokens.typographyColorPlaceholder};
  border: ${props => props.theme.css.borderOutlineColor} solid 0.1rem;
  &.is-invalid {
    border: ${props => props.theme.bcTokens.surfaceColorPrimaryDangerButtonDefault} solid 0.1rem;
  }

  cursor: ${props => (props.$hasContact ? 'default' : 'pointer')};
`;

export const StyledRemoveLinkButton = styled(LinkButton)`
  &&.btn {
    position: absolute;
    top: calc(50% - 1.4rem);
    padding-top: 0.8rem;
    padding-bottom: 0.8rem;
    padding-right: 1.2rem;
    height: 1.6rem;
    right: 0rem;

    color: ${props => props.theme.bcTokens.iconsColorDisabled};
    text-decoration: none;
    line-height: unset;
    .text {
      display: none;
    }

    &:hover,
    &:active,
    &:focus {
      color: ${props => props.theme.bcTokens.surfaceColorPrimaryDangerButtonDefault};
      text-decoration: none;
      opacity: unset;
    }
  }
`;
