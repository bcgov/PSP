import { LinkButton } from 'components/common/buttons';
import { Button } from 'components/common/buttons/Button';
import { Select } from 'components/common/form';
import { ContactInput } from 'components/common/form/ContactInput';
import { ContactManagerModal } from 'components/contact/ContactManagerModal';
import * as API from 'constants/API';
import { FieldArray, FormikProps, useFormikContext } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IContactSearchResult } from 'interfaces/IContactSearchResult';
import React from 'react';
import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdClose } from 'react-icons/md';

import { AcquisitionForm, AcquisitionTeamForm } from './models';

interface AddAcquisitionTeamFormProp {
  formikProps: FormikProps<AcquisitionForm>;
}
export const AddAcquisitionTeamForm: React.FunctionComponent<AddAcquisitionTeamFormProp> = ({
  formikProps,
}) => {
  const { values } = useFormikContext<AcquisitionForm>();
  const [contactIndex, setContactIndex] = useState<number>(-1);
  const [showContactManager, setShowContactManager] = useState<boolean>(false);
  const [selectedContact, setSelectedContact] = useState<IContactSearchResult[]>([]);
  const { getOptionsByType } = useLookupCodeHelpers();
  const personProfileTypes = getOptionsByType(API.ACQUISITION_FILE_PERSON_PROFILE_TYPES);
  const handleContactManagerOk = () => {
    formikProps.setFieldValue(`team[${contactIndex}].contact`, selectedContact[0]);
    setShowContactManager(false);
  };

  return (
    <>
      <FieldArray
        name="team"
        render={arrayHelpers => (
          <>
            {values.team.map((person, index) => (
              <Row key={`team-parent-${index}`} className="py-3">
                <Col xs="auto" xl="5">
                  <Select
                    data-testid="select-profile"
                    placeholder="Select profile..."
                    field={`team[${index}].contactTypeCode`}
                    options={personProfileTypes}
                    value={person.contactTypeCode}
                  />
                </Col>
                <Col xs="auto" xl="5">
                  <ContactInput
                    data-testid="contact-input"
                    setShowContactManager={() => {
                      setContactIndex(index);
                      setShowContactManager(true);
                    }}
                    field={`team[${index}].contact`}
                    onClear={() => formikProps.setFieldValue(`team[${index}].contact`, undefined)}
                  ></ContactInput>
                </Col>
                <Col xs="auto" xl="2" className="pl-0 align-self-center mb-3">
                  <Button
                    data-testid="remove-team-member"
                    icon={<MdClose size={20} />}
                    variant="secondary"
                    className="px-2"
                    onClick={() => {
                      arrayHelpers.remove(index);
                    }}
                  ></Button>
                </Col>
              </Row>
            ))}
            <LinkButton
              data-testid="add-team-member"
              onClick={() => {
                const person: AcquisitionTeamForm = {
                  contactTypeCode: '',
                };
                arrayHelpers.push(person);
              }}
            >
              + Add another team member
            </LinkButton>
          </>
        )}
      />
      <ContactManagerModal
        selectedRows={selectedContact}
        setSelectedRows={setSelectedContact}
        display={showContactManager}
        setDisplay={setShowContactManager}
        isSingleSelect
        handleModalOk={handleContactManagerOk}
        handleModalCancel={() => {
          setShowContactManager(false);
        }}
        showActiveSelector={true}
        showOnlyIndividuals={true}
      ></ContactManagerModal>
    </>
  );
};
