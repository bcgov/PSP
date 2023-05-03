import EditButton from 'components/common/EditButton';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import Claims from 'constants/claims';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { StyledEditWrapper, StyledSummarySection } from 'features/mapSideBar/tabs/SectionStyles';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_Compensation } from 'models/api/Compensation';

export interface CompensationRequisitionDetailViewProps {
  compensation: Api_Compensation;
  clientConstant: string;
  gstConstant: number | undefined;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
}

export const CompensationRequisitionDetailView: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailViewProps>
> = ({ compensation, loading, setEditMode }) => {
  const { hasClaim } = useKeycloakWrapper();

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />
      <StyledEditWrapper>
        {setEditMode !== undefined && hasClaim(Claims.COMPENSATION_REQUISITION_EDIT) && (
          <EditButton
            title="Edit compensation requisition"
            onClick={() => {
              setEditMode(true);
            }}
          />
        )}
      </StyledEditWrapper>
      <Section header="Requisition Details">
        <SectionField label="Status" labelWidth={'4'}>
          {compensation.isDraft ? 'Draft' : 'Final'}
        </SectionField>
      </Section>
    </StyledSummarySection>
  );
};

export default CompensationRequisitionDetailView;
