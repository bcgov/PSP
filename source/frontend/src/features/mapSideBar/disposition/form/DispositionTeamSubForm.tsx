import { FieldArray, useFormikContext } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LinkButton, RemoveButton } from '@/components/common/buttons';
import { Select } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { PrimaryContactSelector } from '@/components/common/form/PrimaryContactSelector/PrimaryContactSelector';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';

import {
  DispositionTeamSubFormModel,
  WithDispositionTeam,
} from '../models/DispositionTeamSubFormModel';

interface IDispositionTeamSubForm {}

const DispositionTeamSubForm: React.FunctionComponent<
  React.PropsWithChildren<IDispositionTeamSubForm>
> = () => {
  const { values, setFieldTouched } = useFormikContext<WithDispositionTeam>();
  const { getOptionsByType } = useLookupCodeHelpers();
  const { setModalContent, setDisplayModal } = useModalContext();

  const teamProfileTypes = getOptionsByType(API.DISPOSITION_TEAM_PROFILE_TYPES);

  return (
    <FieldArray
      name="team"
      render={arrayHelpers => (
        <>
          {values.team.map((teamMember, index) => (
            <React.Fragment key={`disp-team-${index}`}>
              <Row className="py-3" data-testid={`teamMemberRow[${index}]`}>
                <Col xs="auto" xl="5">
                  <Select
                    data-testid="select-profile"
                    placeholder="Select profile..."
                    field={`team.${index}.teamProfileTypeCode`}
                    options={teamProfileTypes}
                    value={teamMember.teamProfileTypeCode}
                    onChange={() => {
                      setFieldTouched(`team.${index}.contact`);
                    }}
                  />
                </Col>
                <Col xs="auto" xl="5" className="pl-0" data-testid="contact-input">
                  <ContactInputContainer
                    field={`team.${index}.contact`}
                    View={ContactInputView}
                    displayErrorAsTooltip={false}
                  ></ContactInputContainer>
                </Col>
                <Col xs="auto" xl="2" className="pl-0 mt-2">
                  <RemoveButton
                    dataTestId={`team.${index}.remove-button`}
                    onRemove={() => {
                      setModalContent({
                        ...getDeleteModalProps(),
                        title: 'Remove Team Member',
                        message: 'Do you wish to remove this team member?',
                        okButtonText: 'Remove',
                        handleOk: async () => {
                          arrayHelpers.remove(index);
                          setDisplayModal(false);
                        },
                        handleCancel: () => {
                          setDisplayModal(false);
                        },
                      });
                      setDisplayModal(true);
                    }}
                  />
                </Col>
              </Row>
              {teamMember.contact?.organizationId && !teamMember.contact?.personId && (
                <SectionField label="Primary contact" labelWidth="5" noGutters>
                  <PrimaryContactSelector
                    field={`team.${index}.primaryContactId`}
                    contactInfo={teamMember?.contact}
                  ></PrimaryContactSelector>
                </SectionField>
              )}
            </React.Fragment>
          ))}
          <LinkButton
            data-testid="add-team-member"
            onClick={() => {
              const member = new DispositionTeamSubFormModel();
              arrayHelpers.push(member);
            }}
          >
            + Add another team member
          </LinkButton>
        </>
      )}
    />
  );
};

export default DispositionTeamSubForm;