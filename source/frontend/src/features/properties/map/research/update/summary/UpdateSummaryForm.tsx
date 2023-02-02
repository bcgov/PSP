import { Input, Select, TextArea } from 'components/common/form';
import { ContactInput } from 'components/common/form/ContactInput';
import { RadioGroup } from 'components/common/form/RadioGroup';
import { InlineFastDatePicker } from 'components/common/form/styles';
import { ContactManagerModal } from 'components/contact/ContactManagerModal';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FormikProps, useFormikContext } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IContactSearchResult } from 'interfaces';
import Multiselect from 'multiselect-react-dropdown';
import * as React from 'react';
import { FaTimes } from 'react-icons/fa';
import styled from 'styled-components';

import { ResearchFileNameGuide } from '../../common/ResearchFileNameGuide';
import { ResearchFilePurposeFormModel, UpdateResearchSummaryFormModel } from './models';

interface MultiSelectOption {
  id: string;
  text: string;
}

export interface IUpdateSummaryFormProps {
  formikProps: FormikProps<UpdateResearchSummaryFormModel>;
}

const UpdateSummaryForm: React.FunctionComponent<
  React.PropsWithChildren<IUpdateSummaryFormProps>
> = props => {
  const { values } = useFormikContext<UpdateResearchSummaryFormModel>();
  const { getOptionsByType, getByType } = useLookupCodeHelpers();
  const requestSourceTypeOptions = getOptionsByType(API.REQUEST_SOURCE_TYPES);

  const researchPurposeOptions = getByType(API.RESEARCH_PURPOSE_TYPES);
  const researchStatusOptions = getOptionsByType(API.RESEARCH_FILE_STATUS_TYPES);

  const purposeFilterOptions: MultiSelectOption[] = researchPurposeOptions.map<MultiSelectOption>(
    x => {
      return { id: x.id as string, text: x.name };
    },
  );

  const initialPurposeList = purposeFilterOptions.filter(x =>
    values.researchFilePurposes?.map(x => x.researchPurposeTypeCode).includes(x.id),
  );

  const [showContactManager, setShowContactManager] = React.useState(false);

  const initialContacts: IContactSearchResult[] = [];
  if (values.requestor !== undefined) {
    initialContacts.push(values.requestor);
  }

  const [selectedContacts, setSelectedContacts] =
    React.useState<IContactSearchResult[]>(initialContacts);

  const [selectedPurposes, setSelectedPurposes] =
    React.useState<MultiSelectOption[]>(initialPurposeList);

  const multiselectProgramRef = React.createRef<Multiselect>();

  function onSelectedPurposeChange(selectedList: MultiSelectOption[]) {
    setSelectedPurposes(selectedList);
    var mapped = selectedList.map<ResearchFilePurposeFormModel>(x => {
      var purposeType = new ResearchFilePurposeFormModel();
      purposeType.researchPurposeTypeCode = x.id;
      purposeType.researchPurposeTypeDescription = x.text;
      return purposeType;
    });
    props.formikProps.setFieldValue('researchFilePurposes', mapped);
  }

  function handleRequesterSelected() {
    var selectedContact = selectedContacts[0];
    props.formikProps.setFieldValue('requestor', selectedContact);
    setShowContactManager(false);
  }

  return (
    <StyledSummarySection>
      <Section header="Research File Information">
        <SectionField label="Status">
          <Select
            field="statusTypeCode"
            data-testid="researchFileStatus"
            required
            options={researchStatusOptions}
            placeholder={values.statusTypeCode ? undefined : 'Please Select'}
          />
        </SectionField>
        <SectionField label="R-file name">
          <Input field="name" />
        </SectionField>
        <ResearchFileNameGuide />
      </Section>
      <Section header="Roads">
        <SectionField label="Road name">
          <Input field="roadName" />
        </SectionField>
        <SectionField label="Road alias">
          <Input field="roadAlias" />
        </SectionField>
      </Section>

      <Section header="Research Request">
        <SectionField label="Research purpose">
          <Multiselect
            id="purpose-selector"
            ref={multiselectProgramRef}
            options={purposeFilterOptions}
            onSelect={onSelectedPurposeChange}
            onRemove={onSelectedPurposeChange}
            selectedValues={selectedPurposes}
            displayValue="text"
            placeholder="Select Research Purpose"
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
        <SectionField label="Request date">
          <InlineFastDatePicker formikProps={props.formikProps} field="requestDate" />
        </SectionField>
        <SectionField label="Source of request">
          <Select
            field="requestSourceTypeCode"
            data-testid="requestSource"
            required
            options={requestSourceTypeOptions}
            placeholder={values.requestSourceTypeCode ? undefined : 'Please Select'}
          />
        </SectionField>
        <SectionField label="Requester">
          <ContactInput
            field="requestor"
            setShowContactManager={setShowContactManager}
            onClear={() => {
              props.formikProps.setFieldValue('requestor', undefined);
              setSelectedContacts([]);
            }}
          />
        </SectionField>
        {values.requestor?.id.startsWith('P') && (
          <SectionField label="Organization" className="pb-4">
            {values.requestor.organizationName ?? 'none'}
          </SectionField>
        )}
        <SectionField label="Description of request" />
        <TextArea field="requestDescription" required />
      </Section>

      <Section header="Result">
        <SectionField label="Research completed on">
          <InlineFastDatePicker formikProps={props.formikProps} field="researchCompletionDate" />
        </SectionField>
        <SectionField label="Result of request" />
        <TextArea field="researchResult" required />
      </Section>

      <Section header="Expropriation">
        <SectionField label="Expropriation?">
          <RadioGroup
            field="isExpropriation"
            flexDirection="column"
            radioValues={[
              {
                radioValue: 'false',
                radioLabel: 'No',
              },
              {
                radioValue: 'true',
                radioLabel: 'Yes',
              },
            ]}
          />
        </SectionField>
        <SectionField label="Expropriation notes" />
        <TextArea field="expropriationNotes" required />
      </Section>
      <ContactManagerModal
        display={showContactManager}
        setDisplay={setShowContactManager}
        setSelectedRows={setSelectedContacts}
        selectedRows={selectedContacts}
        handleModalOk={handleRequesterSelected}
        isSingleSelect
      ></ContactManagerModal>
    </StyledSummarySection>
  );
};

export default UpdateSummaryForm;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;
