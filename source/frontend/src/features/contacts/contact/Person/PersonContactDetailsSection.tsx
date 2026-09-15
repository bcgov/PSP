import { AsyncTypeahead, Input } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { usePersonOrganizationTypeahead } from '@/features/contacts/hooks/usePersonOrganizationTypeahead';

export const PersonContactDetailsSection: React.FunctionComponent = () => {
  const { handleTypeaheadSearch, isTypeaheadLoading, matchedOrgs } =
    usePersonOrganizationTypeahead();

  return (
    <Section header="Contact Details">
      <SectionField label="First name" required>
        <Input field="firstName" />
      </SectionField>
      <SectionField label="Middle">
        <Input field="middleNames" />
      </SectionField>
      <SectionField label="Last name" required>
        <Input field="surname" />
      </SectionField>
      <SectionField label="Preferred name">
        <Input field="preferredName" />
      </SectionField>
      <SectionField label="Link to an existing organization">
        <AsyncTypeahead
          field="organization"
          labelKey="text"
          isLoading={isTypeaheadLoading}
          options={matchedOrgs}
          onSearch={handleTypeaheadSearch}
        />
      </SectionField>
    </Section>
  );
};

export default PersonContactDetailsSection;
