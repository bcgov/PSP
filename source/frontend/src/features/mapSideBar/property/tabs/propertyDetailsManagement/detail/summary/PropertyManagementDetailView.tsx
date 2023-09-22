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

<<<<<<<< HEAD:source/frontend/src/features/mapSideBar/property/tabs/propertyDetailsManagement/detail/summary/PropertyManagementDetailView.tsx
export interface IPropertyManagementDetailViewProps {
========
import { EditForms } from '../../../../PropertyViewSelector';

export interface IManagementSummaryViewProps {
>>>>>>>> 016b273dd (WIP):source/frontend/src/features/mapSideBar/property/tabs/propertyDetailsManagement/detail/summary/ManagementSummaryView.tsx
  isLoading: boolean;
  propertyManagement: Api_PropertyManagement;
  setEditManagementState: (state: EditManagementState | null) => void;
}

<<<<<<<< HEAD:source/frontend/src/features/mapSideBar/property/tabs/propertyDetailsManagement/detail/summary/PropertyManagementDetailView.tsx
export const PropertyManagementDetailView: React.FC<IPropertyManagementDetailViewProps> = ({
========
export const ManagementSummaryView: React.FC<IManagementSummaryViewProps> = ({
>>>>>>>> 016b273dd (WIP):source/frontend/src/features/mapSideBar/property/tabs/propertyDetailsManagement/detail/summary/ManagementSummaryView.tsx
  isLoading,
  propertyManagement,
  setEditManagementState,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <Section header="Summary" isCollapsable initiallyExpanded>
      <StyledEditWrapper className="mr-3 my-1">
        {setEditManagementState !== undefined && hasClaim(Claims.MANAGEMENT_EDIT) && (
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
