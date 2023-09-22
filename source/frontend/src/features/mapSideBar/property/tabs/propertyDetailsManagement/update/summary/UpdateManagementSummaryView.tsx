import { EditButton } from '@/components/common/EditButton';
import { Section } from '@/components/common/Section/Section';
import { StyledEditWrapper } from '@/components/common/Section/SectionStyles';
import { Claims } from '@/constants/index';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_Property } from '@/models/api/Property';
import { Api_PropertyLease } from '@/models/api/PropertyLease';

import { EditForms } from '../../../../PropertyViewSelector';

export interface IUpdateManagementSummaryViewProps {
  isLoading: boolean;
  property: Api_Property;
  propertyLeases: Api_PropertyLease[];
  setEditFormId: (formId: EditForms | null) => void;
}

export const UpdateManagementSummaryView: React.FC<IUpdateManagementSummaryViewProps> = ({
  isLoading,
  property,
  propertyLeases,
  setEditFormId,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <Section header="Summary">
      <StyledEditWrapper className="mr-3 my-1">
        {/** TODO: Use MANAGEMENT CLAIMS when available */}
        {setEditFormId !== undefined && hasClaim(Claims.PROPERTY_EDIT) && (
          <EditButton
            title="Edit property management information"
            onClick={() => setEditFormId(EditForms.UpdateManagementSummaryContainer)}
          />
        )}
      </StyledEditWrapper>
    </Section>
  );
};
