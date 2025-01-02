import { FieldArray, useFormikContext } from 'formik';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LinkButton, RemoveButton } from '@/components/common/buttons';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { ModalSize } from '@/components/common/GenericModal';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import { useModalContext } from '@/hooks/useModalContext';

import { ProjectForm, ProjectTeamForm } from '../models';

const AddProjectTeamSubForm: React.FunctionComponent = () => {
  const { values } = useFormikContext<ProjectForm>();
  const { setModalContent, setDisplayModal } = useModalContext();

  return (
    <FieldArray
      name="projectTeam"
      render={arrayHelpers => (
        <>
          {values.projectTeam.map((teamMember, index) => (
            <React.Fragment key={`project-team-${index}`}>
              <Row className="py-3" data-testid={`teamMemberRow-${teamMember?.contact?.personId}`}>
                <Col xs="auto" xl="6" data-testid="contact-input">
                  <ContactInputContainer
                    field={`projectTeam.${index}.contact`}
                    View={ContactInputView}
                    displayErrorAsTooltip={false}
                    restrictContactType={RestrictContactType.ONLY_INDIVIDUALS}
                    placeholder="Select from Management Team..."
                  ></ContactInputContainer>
                </Col>
                <Col xs="auto" xl="2" className="pl-0 mt-2">
                  <RemoveButton
                    data-testId={`team.${index}.remove-button`}
                    onRemove={() => {
                      setModalContent({
                        modalSize: ModalSize.LARGE,
                        variant: 'info',
                        message: 'Are you sure you want to remove this row?',
                        title: 'Remove Team Member',
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
                </Col>
              </Row>
            </React.Fragment>
          ))}
          <LinkButton
            data-testid="add-team-member"
            onClick={() => {
              const teamMember = new ProjectTeamForm(values.id);
              arrayHelpers.push(teamMember);
            }}
          >
            + Add another management team member
          </LinkButton>
        </>
      )}
    />
  );
};

export default AddProjectTeamSubForm;
