import { FieldArray, useFormikContext } from 'formik';
import React from 'react';
import { useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { LinkButton, RemoveButton } from '@/components/common/buttons';
import { Select } from '@/components/common/form';
import { ContactInput } from '@/components/common/form/ContactInput';
import { ContactManagerModal } from '@/components/contact/ContactManagerModal';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { IContactSearchResult } from '@/interfaces/IContactSearchResult';

import { AcquisitionFormModal } from '../../modals/AcquisitionFormModal';
import { AcquisitionTeamFormModel, WithAcquisitionTeam } from '../../models';

interface IUpdateAcquisitionTeamSubFormProps {}

export const UpdateAcquisitionTeamSubForm: React.FunctionComponent<
  React.PropsWithChildren<IUpdateAcquisitionTeamSubFormProps>
> = () => {
  const { values, setFieldValue } = useFormikContext<WithAcquisitionTeam>();
  const [contactIndex, setContactIndex] = useState<number>(-1);
  const [showContactManager, setShowContactManager] = useState<boolean>(false);
  const [showRemoveMemberModal, setShowRemoveMemberModal] = useState<boolean>(false);
  const [removeIndex, setRemoveIndex] = useState<number>(-1);
  const [selectedContact, setSelectedContact] = useState<IContactSearchResult[]>([]);
  const { getOptionsByType } = useLookupCodeHelpers();
  const personProfileTypes = getOptionsByType(API.ACQUISITION_FILE_PERSON_PROFILE_TYPES);

  const handleContactManagerOk = () => {
    setFieldValue(`team[${contactIndex}].contact`, selectedContact[0]);
    setShowContactManager(false);
    setSelectedContact([]);
  };

  return (
    <>
      <FieldArray
        name="team"
        render={arrayHelpers => (
          <>
            {values.team.map((person, index) => (
              <Row
                key={`team-parent-${index}`}
                className="py-3"
                data-testid={`teamMemberRow[${index}]`}
              >
                <Col xs="auto" xl="5">
                  <Select
                    data-testid="select-profile"
                    placeholder="Select profile..."
                    field={`team[${index}].contactTypeCode`}
                    options={personProfileTypes}
                    value={person.contactTypeCode}
                  />
                </Col>
                <Col xs="auto" xl="5" className="pl-0" data-testid="contact-input">
                  <ContactInput
                    setShowContactManager={() => {
                      setContactIndex(index);
                      setShowContactManager(true);
                    }}
                    field={`team[${index}].contact`}
                    onClear={() => setFieldValue(`team[${index}].contact`, undefined)}
                  ></ContactInput>
                </Col>
                <Col xs="auto" xl="2" className="pl-0 mt-2">
                  <RemoveButton
                    onRemove={() => {
                      setRemoveIndex(index);
                      setShowRemoveMemberModal(true);
                    }}
                  />
                </Col>
              </Row>
            ))}
            <LinkButton
              data-testid="add-team-member"
              onClick={() => {
                const person = new AcquisitionTeamFormModel('');
                arrayHelpers.push(person);
              }}
            >
              + Add another team member
            </LinkButton>

            <AcquisitionFormModal
              message="Are you sure you want to remove this row?"
              title="Remove Team Member"
              display={showRemoveMemberModal}
              handleOk={() => {
                setShowRemoveMemberModal(false);
                arrayHelpers.remove(removeIndex);
                setRemoveIndex(-1);
              }}
              handleCancel={() => {
                setShowRemoveMemberModal(false);
                setRemoveIndex(-1);
              }}
            ></AcquisitionFormModal>
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
          setSelectedContact([]);
        }}
        showActiveSelector={true}
        restrictContactType={RestrictContactType.ONLY_INDIVIDUALS}
      ></ContactManagerModal>
    </>
  );
};
