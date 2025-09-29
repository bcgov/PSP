import { FieldArray, Formik, FormikHelpers, FormikProps } from 'formik';
import { isEmpty } from 'lodash';
import React, { useRef } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaExternalLinkAlt } from 'react-icons/fa';
import styled from 'styled-components';

import { Form } from '@/components/common/form/Form';
import GenericModal from '@/components/common/GenericModal';
import { StyledLink } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { StyledNoData } from '@/features/documents/commonStyles';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { formatApiAddress } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { DetailAcquisitionFileOwner } from '../../acquisition/models/DetailAcquisitionFileOwner';

export interface INoticeSelectorModalProps {
  fileOwners: ApiGen_Concepts_AcquisitionFileOwner[];
  teamMembers: ApiGen_Concepts_AcquisitionFileTeam[];
  isOpened: boolean;
  onSelectOk: (
    selectedOwners: ApiGen_Concepts_AcquisitionFileOwner[],
    responsibleTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
    signingTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
  ) => void;
  onCancelClick: () => void;
}

interface NoticeSelectorForm {
  selectedOwnerIds: string[];
  responsibleTeamMemberId: string;
  signingTeamMemberId: string;
}

const NoticeSelectorModal: React.FunctionComponent<
  React.PropsWithChildren<INoticeSelectorModalProps>
> = props => {
  const { isOpened, onCancelClick, fileOwners, teamMembers, onSelectOk } = props;

  const formikRef = useRef<FormikProps<NoticeSelectorForm>>(null);

  const initialValues: NoticeSelectorForm = {
    selectedOwnerIds: [],
    responsibleTeamMemberId: '',
    signingTeamMemberId: '',
  };

  const ownerDetailList = fileOwners?.map(o => DetailAcquisitionFileOwner.fromApi(o));

  const handleSubmit = (
    values: NoticeSelectorForm,
    formikHelpers: FormikHelpers<NoticeSelectorForm>,
  ) => {
    const selectedProperties = fileOwners.filter(x =>
      values.selectedOwnerIds.includes(x.id.toString()),
    );
    const selectedResponsibleTeamMember =
      teamMembers.find(x => values.responsibleTeamMemberId === x.id.toString()) ?? null;
    const selectedSigningTeamMember =
      teamMembers.find(x => values.signingTeamMemberId === x.id.toString()) ?? null;

    onSelectOk(selectedProperties, selectedResponsibleTeamMember, selectedSigningTeamMember);
    formikHelpers.resetForm();
    formikHelpers.setSubmitting(false);
  };

  return (
    <Formik<NoticeSelectorForm>
      enableReinitialize
      innerRef={formikRef}
      initialValues={initialValues}
      onSubmit={handleSubmit}
    >
      {formikProps => (
        <StyledModal
          variant="info"
          display={isOpened}
          title="Generate Form 12"
          message={
            <>
              <p>Select who should receive this letter from the following list</p>
              <p className="pt-5">
                <strong>Available recipients in this file:</strong>
              </p>
              <StyledDiv>
                <FieldArray
                  name={withNameSpace('selectedOwnerIds')}
                  render={() => (
                    <Form.Group>
                      {ownerDetailList.map((owner: DetailAcquisitionFileOwner, index: number) => (
                        <Form.Check
                          id={`propertyOwner-${index}`}
                          type="checkbox"
                          name="selectedOwnerIds"
                          key={owner.ownerId}
                        >
                          <Form.Check.Input
                            id={`propertyOwner-${index}`}
                            type="checkbox"
                            name="selectedOwnerIds"
                            value={owner.ownerId}
                            onChange={formikProps.handleChange}
                          />
                          <Form.Check.Label className="w-100" htmlFor={'propertyOwner-' + index}>
                            <Row>
                              <Col xs="auto">{owner.ownerName}</Col>
                              <Col> {formatApiAddress(owner.ownerAddress)}</Col>
                            </Row>
                          </Form.Check.Label>
                        </Form.Check>
                      ))}
                    </Form.Group>
                  )}
                />
                {isEmpty(teamMembers) && (
                  <StyledNoData className="m-4">No Team Members availiable</StyledNoData>
                )}
              </StyledDiv>
              <p className="pt-5">
                <strong>Responsible team member:</strong>
                <TooltipIcon
                  toolTipId={`responsible-member-tooltip`}
                  toolTip={
                    'The team member who should be contacted for questions about the notice.'
                  }
                />
              </p>
              <StyledDiv>
                <FieldArray
                  name={withNameSpace('responsibleTeamMemberId')}
                  render={() => (
                    <Form.Group>
                      {teamMembers.map(
                        (teamMember: ApiGen_Concepts_AcquisitionFileTeam, index: number) => (
                          <Form.Check
                            id={`responsibleMember-${index}`}
                            type="radio"
                            name="responsibleTeamMemberId"
                            key={teamMember.id}
                          >
                            <Form.Check.Input
                              id={`responsibleTeamMember-${index}`}
                              type="radio"
                              name="responsibleTeamMemberId"
                              value={teamMember.id}
                              onChange={formikProps.handleChange}
                            />
                            <Form.Check.Label className="w-100" htmlFor={'recipient-' + index}>
                              <StyledLinkWrapper>
                                <StyledLink
                                  target="_blank"
                                  rel="noopener noreferrer"
                                  to={`/contact/P${teamMember?.personId}`}
                                >
                                  <span>{formatApiPersonNames(teamMember?.person)}</span>
                                  <FaExternalLinkAlt className="ml-2" size="1rem" />
                                </StyledLink>
                                <StyledMemberRole>
                                  ({teamMember.teamProfileType.description})
                                </StyledMemberRole>
                              </StyledLinkWrapper>
                            </Form.Check.Label>
                          </Form.Check>
                        ),
                      )}
                    </Form.Group>
                  )}
                />
                {isEmpty(teamMembers) && (
                  <StyledNoData className="m-4">No Team Members availiable</StyledNoData>
                )}
              </StyledDiv>
              <p className="pt-5">
                <strong>Signing block team member:</strong>
                <TooltipIcon
                  toolTipId={'signing-member-tooltip'}
                  toolTip={
                    'The team member who should display in the signature block of the notice.'
                  }
                />
              </p>
              <StyledDiv>
                <FieldArray
                  name={withNameSpace('signingTeamMemberId')}
                  render={() => (
                    <Form.Group>
                      {teamMembers.map(
                        (teamMember: ApiGen_Concepts_AcquisitionFileTeam, index: number) => (
                          <Form.Check
                            id={`signingMember-${index}`}
                            type="radio"
                            name="signingTeamMemberId"
                            key={teamMember.id}
                          >
                            <Form.Check.Input
                              id={`signingMember-${index}`}
                              type="radio"
                              name="signingTeamMemberId"
                              value={teamMember.id}
                              onChange={formikProps.handleChange}
                            />
                            <Form.Check.Label className="w-100" htmlFor={'recipient-' + index}>
                              <StyledLinkWrapper>
                                <StyledLink
                                  target="_blank"
                                  rel="noopener noreferrer"
                                  to={`/contact/P${teamMember?.personId}`}
                                >
                                  <span>{formatApiPersonNames(teamMember?.person)}</span>
                                  <FaExternalLinkAlt className="ml-2" size="1rem" />
                                </StyledLink>
                                <StyledMemberRole>
                                  ({teamMember.teamProfileType.description})
                                </StyledMemberRole>
                              </StyledLinkWrapper>
                            </Form.Check.Label>
                          </Form.Check>
                        ),
                      )}
                    </Form.Group>
                  )}
                />
                {isEmpty(teamMembers) && (
                  <StyledNoData className="m-4">No Team Members availiable</StyledNoData>
                )}
              </StyledDiv>
            </>
          }
          okButtonText="Continue"
          cancelButtonText="Cancel"
          handleOk={() => formikProps.submitForm()}
          handleCancel={() => {
            formikProps.resetForm();
            onCancelClick();
          }}
        ></StyledModal>
      )}
    </Formik>
  );
};

export default NoticeSelectorModal;

const StyledModal = styled(GenericModal)`
  min-width: 70rem;

  .modal-body {
    padding-left: 2rem;
    padding-right: 2rem;
  }

  .modal-footer {
    padding-left: 2rem;
    padding-right: 2rem;
  }
`;

const StyledDiv = styled.div`
  border: 0.1rem solid ${props => props.theme.css.borderOutlineColor};
  border-radius: 0.5rem;
  max-height: 180px;
  overflow-y: auto;
  overflow-x: hidden;
  padding: 0.5rem 1.5rem;

  .form-check {
    input {
      margin-top: 0.6rem;
    }
  }

  .form-group {
    label {
      line-height: 1.5rem;
    }
  }
`;

const StyledLinkWrapper = styled.div`
  display: flex;
  flex-direction: row;
`;
const StyledMemberRole = styled.div`
  font-style: italic;
  color: ${props => props.theme.bcTokens.typographyColorSecondary};
`;
