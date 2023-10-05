import { EditButton } from '@/components/common/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { MultiselectTextList } from '@/components/common/MultiselectTextList';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper } from '@/components/common/Section/SectionStyles';
import { Claims } from '@/constants/index';
import {
  EditManagementState,
  PropertyEditForms,
} from '@/features/mapSideBar/property/PropertyViewSelector';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_PropertyManagement } from '@/models/api/Property';
import { formatApiPropertyManagementLease } from '@/utils';
import { booleanToYesNoUnknownString } from '@/utils/formUtils';

export interface IPropertyManagementDetailViewProps {
  isLoading: boolean;
  propertyManagement: Api_PropertyManagement | null;
  setEditManagementState: (state: EditManagementState | null) => void;
}

export const PropertyManagementDetailView: React.FC<IPropertyManagementDetailViewProps> = ({
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
      <LoadingBackdrop show={isLoading} />
      <SectionField label="Property purpose">
        <MultiselectTextList
          values={
            propertyManagement?.managementPurposes?.map(mp => mp.propertyPurposeTypeCode) ?? []
          }
        />
      </SectionField>
      <SectionField label="Lease/Licensed">
        {formatApiPropertyManagementLease(propertyManagement)}
      </SectionField>
      <SectionField label="Utilities payable">
        {booleanToYesNoUnknownString(propertyManagement?.isUtilitiesPayable)}
      </SectionField>
      <SectionField label="Taxes payable">
        {booleanToYesNoUnknownString(propertyManagement?.isTaxesPayable)}
      </SectionField>
      <SectionField
        label="Additional details"
        contentWidth="12"
        tooltip="Describe the purpose of the property for the Ministry."
      >
        {propertyManagement?.additionalDetails}
      </SectionField>
    </Section>
  );
};
