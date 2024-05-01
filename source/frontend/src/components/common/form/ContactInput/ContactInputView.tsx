import { getIn, useFormikContext } from 'formik';
import { Col, Form, FormControlProps, Row } from 'react-bootstrap';
import { FaAddressBook } from 'react-icons/fa';
import { MdClose } from 'react-icons/md';
import styled from 'styled-components';

import { Button, LinkButton } from '@/components/common/buttons';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import {
  ContactManagerModal,
  IContactManagerModalProps,
} from '@/components/contact/ContactManagerModal';
import { formatContactSearchResult } from '@/features/contacts/contactUtils';
import { IContactSearchResult } from '@/interfaces';

import { DisplayError } from '../DisplayError';
import { Input } from '../Input';

export type RequiredAttributes = {
  field: string;
  setShowContactManager: React.Dispatch<React.SetStateAction<boolean>>;
  onClear: () => void;
  contactManagerProps: IContactManagerModalProps;
};

export type OptionalAttributes = {
  label?: string;
  required?: boolean;
  displayErrorTooltips?: boolean;
};

export type IContactInputViewProps = FormControlProps & OptionalAttributes & RequiredAttributes;

const ContactInputView: React.FunctionComponent<IContactInputViewProps> = ({
  label,
  displayErrorTooltips,
  field,
  onClear,
  setShowContactManager,
  contactManagerProps,
}) => {
  const { errors, touched, values } = useFormikContext<any>();
  const error = getIn(errors, field);
  const touch = getIn(touched, field);
  const contactInfo: IContactSearchResult | undefined = getIn(values, field);
  const errorTooltip = error && touch && displayErrorTooltips ? error : undefined;

  let text = 'Select from contacts';

  if (contactInfo !== undefined) {
    text = formatContactSearchResult(contactInfo, 'Select from contacts');
  }

  return (
    <>
      <Form.Group controlId={`input-${field}`}>
        {!!label && <Form.Label>{label}</Form.Label>}

        <TooltipWrapper tooltipId={`${field}-error-tooltip}`} tooltip={errorTooltip}>
          <Row>
            <Col>
              <StyledDiv className={!!error && !!touch ? 'is-invalid' : ''}>
                {text}
                <StyledRemoveLinkButton
                  onClick={() => {
                    onClear();
                  }}
                  disabled={contactInfo === undefined}
                  title="remove"
                >
                  <MdClose size="2rem" />
                </StyledRemoveLinkButton>
              </StyledDiv>
              <Input
                field={field + '.id'}
                placeholder="Select from Contacts"
                className="d-none"
                defaultValue=""
              ></Input>
            </Col>
            <Col xs="auto" className="pl-0">
              <Button
                title="Select Contact"
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
      <ContactManagerModal
        selectedRows={contactManagerProps?.selectedRows}
        setSelectedRows={contactManagerProps?.setSelectedRows}
        display={contactManagerProps?.display}
        setDisplay={setShowContactManager}
        isSingleSelect={contactManagerProps?.isSingleSelect}
        handleModalOk={contactManagerProps?.handleModalOk}
        handleModalCancel={contactManagerProps?.handleModalCancel}
        showActiveSelector={contactManagerProps.showActiveSelector}
        restrictContactType={contactManagerProps.restrictContactType}
      ></ContactManagerModal>
    </>
  );
};

export default ContactInputView;

const StyledDiv = styled.div`
  background: none;
  position: relative;
  border-radius: 0.3rem;
  padding: 0.6rem;
  padding-right: 2.1rem;
  color: ${props => props.theme.css.formControlTextColor};
  border: ${props => props.theme.css.lightVariantColor} solid 0.1rem;
  &.is-invalid {
    border: ${props => props.theme.css.dangerColor} solid 0.1rem;
  }
`;

const StyledRemoveLinkButton = styled(LinkButton)`
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
