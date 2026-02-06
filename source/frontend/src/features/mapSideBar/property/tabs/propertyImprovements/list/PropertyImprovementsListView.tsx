import { FaEdit, FaPlus, FaTrash } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import { RemoveIconButton } from '@/components/common/buttons/RemoveButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import Claims from '@/constants/claims';
import PropertyImprovementDetails from '@/features/mapSideBar/shared/improvements/PropertyImprovementDetails/PropertyImprovementDetails';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';

export interface IPropertyImprovementsListViewProps {
  loading: boolean;
  propertyImprovements: ApiGen_Concepts_PropertyImprovement[];
  onDelete: (improvementId: number) => void;
}

export const PropertyImprovementsListView: React.FunctionComponent<
  IPropertyImprovementsListViewProps
> = ({ loading, propertyImprovements, onDelete }) => {
  const history = useHistory();
  const match = useRouteMatch();
  const { setModalContent, setDisplayModal } = useModalContext();

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />

      <Section
        header={
          <SectionListHeader
            claims={[Claims.PROJECT_VIEW]}
            title="Property Improvements"
            addButtonText="Add Improvement"
            addButtonIcon={<FaPlus size={'2rem'} />}
            onButtonAction={() => {
              history.push(`${match.url}/add`);
            }}
          />
        }
      >
        {propertyImprovements?.map((improvement, index) => (
          <StyledBorder key={improvement.id}>
            <Section
              header={
                <StyledHeaderContainer>
                  <div>{`Improvement ${++index}`}</div>
                  <StyledButtonContainer>
                    <EditButton
                      title="Edit Improvement"
                      data-testId={`improvements[${index}].edit-btn`}
                      onClick={() => history.push(`${match.url}/${improvement.id}/update`)}
                      icon={<FaEdit size={'2rem'} />}
                    />
                    <RemoveIconButton
                      title="Delete Improvement"
                      data-testId={`improvements[${index}].delete-btn`}
                      icon={<FaTrash size={'1.75rem'} />}
                      onRemove={() => {
                        setModalContent({
                          ...getDeleteModalProps(),
                          variant: 'error',
                          title: 'Delete Improvement',
                          message: `You have selected to delete this Improvement.
                                                          Do you want to proceed?`,
                          okButtonText: 'Yes',
                          cancelButtonText: 'No',
                          handleOk: async () => {
                            improvement.id && onDelete(improvement.id);
                            setDisplayModal(false);
                          },
                          handleCancel: () => {
                            setDisplayModal(false);
                          },
                        });
                        setDisplayModal(true);
                      }}
                    />
                  </StyledButtonContainer>
                </StyledHeaderContainer>
              }
              isCollapsable
              initiallyExpanded
            >
              <PropertyImprovementDetails
                propertyImprovement={improvement}
                propertyImprovementIndex={index}
              ></PropertyImprovementDetails>
            </Section>
          </StyledBorder>
        ))}

        {(!propertyImprovements || propertyImprovements.length === 0) && (
          <p>
            There are no commercial, residential, or other improvements indicated with this
            Property.
          </p>
        )}
      </Section>
    </StyledSummarySection>
  );
};

export default PropertyImprovementsListView;

const StyledBorder = styled.div`
  border: solid 0.2rem ${props => props.theme.css.headerBorderColor};
  margin-bottom: 1.5rem;
  border-radius: 0.5rem;
`;

const StyledHeaderContainer = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: end;
`;

export const StyledButtonContainer = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  align-items: center;
  margin-bottom: 0.5rem;
  align-items: center;
`;
