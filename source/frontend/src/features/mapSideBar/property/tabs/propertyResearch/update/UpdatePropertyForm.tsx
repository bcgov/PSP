import { FormikProps, useFormikContext } from 'formik';
import Multiselect from 'multiselect-react-dropdown';
import { createRef, useState } from 'react';
import { FaTimes } from 'react-icons/fa';

import { Input, Select, SelectOption, TextArea } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';

import { PropertyResearchFilePurposeFormModel, UpdatePropertyFormModel } from './models';

interface MultiSelectOption {
  id: string;
  text: string;
}

export interface IUpdatePropertyFormProps {
  formikProps: FormikProps<UpdatePropertyFormModel>;
}

const UpdatePropertyForm: React.FunctionComponent<
  React.PropsWithChildren<IUpdatePropertyFormProps>
> = props => {
  const { values } = useFormikContext<UpdatePropertyFormModel>();
  const { getByType } = useLookupCodeHelpers();

  const opinionOptions: SelectOption[] = [
    { label: 'Unknown', value: 'unknown' },
    { label: 'Yes', value: 'yes' },
    { label: 'No', value: 'no' },
  ];

  const propertyResearchPurposeOptions = getByType(API.PROPERTY_RESEARCH_PURPOSE_TYPES);

  const purposeFilterOptions: MultiSelectOption[] =
    propertyResearchPurposeOptions.map<MultiSelectOption>(x => {
      return { id: x.id as string, text: x.name };
    });

  const initialPurposeList = purposeFilterOptions.filter(x =>
    values.purposeTypes?.map(x => x.propertyPurposeTypeCode).includes(x.id),
  );

  const [selectedPurposes, setSelectedPurposes] = useState<MultiSelectOption[]>(initialPurposeList);

  const multiselectProgramRef = createRef<Multiselect>();

  function onSelectedPurposeChange(selectedList: MultiSelectOption[]) {
    setSelectedPurposes(selectedList);
    const mapped = selectedList.map<PropertyResearchFilePurposeFormModel>(x => {
      const purposeType = new PropertyResearchFilePurposeFormModel();
      purposeType.propertyPurposeTypeCode = x.id;
      purposeType.propertyPurposeTypeDescription = x.text;
      return purposeType;
    });
    props.formikProps.setFieldValue('purposeTypes', mapped);
  }

  return (
    <StyledSummarySection>
      <Section header="Property of Interest">
        <SectionField label="Descriptive name">
          <Input field="propertyName" />
        </SectionField>
        <SectionField label="Purpose">
          <Multiselect
            id="purpose-selector"
            ref={multiselectProgramRef}
            options={purposeFilterOptions}
            onSelect={onSelectedPurposeChange}
            onRemove={onSelectedPurposeChange}
            selectedValues={selectedPurposes}
            displayValue="text"
            placeholder="Select Property Purpose"
            customCloseIcon={<FaTimes size="18px" className="ml-3" />}
            hidePlaceholder={true}
            style={{
              chips: {
                background: '#F2F2F2',
                borderRadius: '4px',
                color: 'black',
                fontSize: '16px',
                marginRight: '1em',
              },
              multiselectContainer: {
                width: 'auto',
                color: 'black',
                paddingBottom: '12px',
              },
              searchBox: {
                background: 'white',
                border: '1px solid #606060',
              },
            }}
          />
        </SectionField>
        <SectionField label="Legal opinion req'd?">
          <Select field="isLegalOpinionRequired" options={opinionOptions} />
        </SectionField>
        <SectionField label="Legal opinion obtained?">
          <Select field="isLegalOpinionObtained" options={opinionOptions} />
        </SectionField>
        <SectionField label="Document reference">
          <Input field="documentReference" />
        </SectionField>
      </Section>

      <Section header="Research Summary">
        <SectionField label="Summary notes" />
        <TextArea field="researchSummary" />
      </Section>
    </StyledSummarySection>
  );
};

export default UpdatePropertyForm;
