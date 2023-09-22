import { EditButton } from '@/components/common/EditButton';
import { Section } from '@/components/common/Section/Section';
import { StyledEditWrapper } from '@/components/common/Section/SectionStyles';
import { Claims } from '@/constants/index';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_Property } from '@/models/api/Property';
import { Api_PropertyLease } from '@/models/api/PropertyLease';

export interface IManagementSummaryViewProps {
  isLoading: boolean;
  property: Api_Property;
  propertyLeases: Api_PropertyLease[];
  setEditMode: (isEditing: boolean) => void;
}

export const ManagementSummaryView: React.FunctionComponent<IManagementSummaryViewProps> = ({
  isLoading,
  property,
  propertyLeases,
  setEditMode,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <Section header="Summary">
      <StyledEditWrapper className="mr-3 my-1">
        {/** TODO: Use MANAGEMENT CLAIMS when available */}
        {setEditMode !== undefined && hasClaim(Claims.PROPERTY_EDIT) && (
          <EditButton
            title="Edit property management information"
            onClick={() => setEditMode(true)}
          />
        )}
      </StyledEditWrapper>
    </Section>
  );
};
