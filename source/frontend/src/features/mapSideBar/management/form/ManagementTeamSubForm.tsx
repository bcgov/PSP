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
import { isValidId } from '@/utils/utils';

import {
  ManagementTeamSubFormModel,
  WithManagementTeam,
} from '../models/ManagementTeamSubFormModel';

export interface IManagementTeamSubFormProps {
  canEditDetails: boolean;
}

const ManagementTeamSubForm: React.FunctionComponent<IManagementTeamSubFormProps> = ({
  canEditDetails,
}) => {
  const { values, setFieldTouched, errors } = useFormikContext<WithManagementTeam>();
  const { getOptionsByType } = useLookupCodeHelpers();
  const { setModalContent, setDisplayModal } = useModalContext();

  const teamProfileTypes = getOptionsByType(API.MANAGEMENT_TEAM_PROFILE_TYPES);

  return (
    <FieldArray
      name="team"
      render={arrayHelpers => (
        <>
          {values.team.map((teamMember, index) => (
            <React.Fragment key={`management-team-${teamMember?.id ?? index}`}>
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
                    disabled={!canEditDetails}
                  />
                </Col>

                <Col xs="auto" xl="5" className="pl-0" data-testid="contact-input">
                  <ContactInputContainer
                    field={`team.${index}.contact`}
                    View={ContactInputView}
                    displayErrorAsTooltip={false}
                    canEditDetails={canEditDetails}
                  ></ContactInputContainer>
                </Col>

                <Col xs="auto" xl="2" className="pl-0 mt-2">
                  {canEditDetails && (
                    <RemoveButton
                      data-testId={`team.${index}.remove-button`}
                      onRemove={() => {
                        setModalContent({
                          ...getDeleteModalProps(),
                          title: 'Remove Team Member',
                          message: 'Do you wish to remove this team member?',
                          okButtonText: 'Yes',
                          cancelButtonText: 'No',
                          handleOk: () => {
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
                  )}
                </Col>
              </Row>

              {isValidId(teamMember.contact?.organizationId) &&
                !isValidId(teamMember.contact?.personId) && (
                  <SectionField label="Primary contact" labelWidth={{ xs: 6 }} noGutters>
                    <PrimaryContactSelector
                      field={`team.${index}.primaryContactId`}
                      contactInfo={teamMember?.contact}
                      canEditDetails={canEditDetails}
                    ></PrimaryContactSelector>
                  </SectionField>
                )}
            </React.Fragment>
          ))}

          {errors?.team && typeof errors?.team === 'string' && (
            <div className="invalid-feedback" data-testid="team-profile-dup-error">
              {errors.team.toString()}
            </div>
          )}

          {canEditDetails && (
            <LinkButton
              data-testid="add-team-member"
              onClick={() => {
                const member = new ManagementTeamSubFormModel();
                arrayHelpers.push(member);
              }}
            >
              + Add another team member
            </LinkButton>
          )}
        </>
      )}
    />
  );
};

export default ManagementTeamSubForm;
