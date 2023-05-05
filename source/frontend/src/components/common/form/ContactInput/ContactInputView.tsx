import { Button } from 'components/common/buttons';
import TooltipWrapper from 'components/common/TooltipWrapper';
import {
  ContactManagerModal,
  IContactManagerModalProps,
} from 'components/contact/ContactManagerModal';
import { StyledDiv } from 'features/properties/map/acquisition/common/update/acquisitionOwners/UpdateAcquisitionOwnersSubForm';
import { getIn, useFormikContext } from 'formik';
import { IContactSearchResult } from 'interfaces';
import * as React from 'react';
import { Col, FormControlProps, Row } from 'react-bootstrap';
import { Form } from 'react-bootstrap';
import { FaAddressBook } from 'react-icons/fa';
import { MdClose } from 'react-icons/md';
import { formatNames } from 'utils/personUtils';

import { StyledRemoveLinkButton } from '../ContactInput';
import { DisplayError } from '../DisplayError';
import { Input } from '../Input';

export type RequiredAttributes = {
  field: string;
  setShowContactManager: React.Dispatch<React.SetStateAction<boolean>>;
  onClear: Function;
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

  var text = 'Select from contacts';

  if (contactInfo !== undefined) {
    if (contactInfo?.personId !== undefined) {
      text = formatNames([contactInfo.firstName, contactInfo.middleNames, contactInfo.surname]);
    } else if (contactInfo?.organizationId !== undefined) {
      text = contactInfo.organizationName || '';
    }
  }

  return (
    <>
      <Form.Group controlId={`input-${field}`}>
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
                  title="remove"
                >
                  <MdClose size="2rem" />
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
        isSingleSelect
        handleModalOk={contactManagerProps?.handleModalOk}
        handleModalCancel={contactManagerProps?.handleModalCancel}
        showActiveSelector={true}
        showOnlyIndividuals={true}
      ></ContactManagerModal>
    </>
  );
};

export default ContactInputView;
