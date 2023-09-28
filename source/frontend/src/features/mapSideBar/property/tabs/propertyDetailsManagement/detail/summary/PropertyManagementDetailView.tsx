import { EditButton } from '@/components/common/EditButton';
import { Section } from '@/components/common/Section/Section';
import { StyledEditWrapper } from '@/components/common/Section/SectionStyles';
import { Claims } from '@/constants/index';
import {
  EditManagementState,
  PropertyEditForms,
} from '@/features/mapSideBar/property/PropertyViewSelector';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_PropertyManagement } from '@/models/api/Property';

export interface IPropertyManagementDetailViewProps {
  isLoading: boolean;
  propertyManagement: Api_PropertyManagement;
  setEditManagementState: (state: EditManagementState | null) => void;
}

export const PropertyManagementDetailView: React.FC<IPropertyManagementDetailViewProps> = ({
  isLoading,
  propertyManagement,
  setEditManagementState,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <Section header="Summary">
      <StyledEditWrapper className="mr-3 my-1">
        {/** TODO: Use MANAGEMENT CLAIMS when available */}
        {setEditManagementState !== undefined && hasClaim(Claims.PROPERTY_EDIT) && (
          <EditButton
            title="Edit property management information"
            onClick={() =>
              setEditManagementState({
                form: PropertyEditForms.UpdateManagementSummaryContainer,
                childId: null,
              })
            }
          />
        )}
      </StyledEditWrapper>
    </Section>
  );
};
