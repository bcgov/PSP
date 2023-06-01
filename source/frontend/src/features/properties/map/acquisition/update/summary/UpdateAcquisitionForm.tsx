import {
  FastDatePicker,
  Input,
  ProjectSelector,
  Select,
  SelectOption,
} from 'components/common/form';
import { ContactInputContainer } from 'components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from 'components/common/form/ContactInput/ContactInputView';
import { UserRegionSelectContainer } from 'components/common/form/UserRegionSelect/UserRegionSelectContainer';
import { StyledSectionParagraph } from 'components/common/styles';
import TooltipIcon from 'components/common/TooltipIcon';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import { useProjectProvider } from 'hooks/repositories/useProjectProvider';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import { IAutocompletePrediction } from 'interfaces';
import { Api_Product } from 'models/api/Project';
import React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import UpdateAcquisitionOwnersSubForm from '../../common/update/acquisitionOwners/UpdateAcquisitionOwnersSubForm';
import { UpdateAcquisitionTeamSubForm } from '../../common/update/acquisitionTeam/UpdateAcquisitionTeamSubForm';
import { UpdateAcquisitionSummaryFormModel } from './models';
import StatusToolTip from './StatusToolTip';

export interface IUpdateAcquisitionFormProps {
  formikRef: React.Ref<FormikProps<UpdateAcquisitionSummaryFormModel>>;
  /** Initial values of the form */
  initialValues: UpdateAcquisitionSummaryFormModel;
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (
    values: UpdateAcquisitionSummaryFormModel,
    formikHelpers: FormikHelpers<UpdateAcquisitionSummaryFormModel>,
  ) => void | Promise<any>;
}

export const UpdateAcquisitionForm: React.FC<IUpdateAcquisitionFormProps> = props => {
  const { formikRef, initialValues, validationSchema, onSubmit } = props;

  return (
    <Formik<UpdateAcquisitionSummaryFormModel>
      enableReinitialize
      innerRef={formikRef}
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={onSubmit}
    >
      {formikProps => {
        return (
          <>
            <AcquisitionDetailSubForm formikProps={formikProps}></AcquisitionDetailSubForm>
            <Prompt
              when={formikProps.dirty && formikProps.submitCount === 0}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
          </>
        );
      }}
    </Formik>
  );
};

export default UpdateAcquisitionForm;

const AcquisitionDetailSubForm: React.FC<{
  formikProps: FormikProps<UpdateAcquisitionSummaryFormModel>;
}> = ({ formikProps }) => {
  const {
    setFieldValue,
    initialValues,
    values: { fileStatusTypeCode },
  } = formikProps;

  const [projectProducts, setProjectProducts] = React.useState<Api_Product[] | undefined>(
    undefined,
  );
  const { retrieveProjectProducts } = useProjectProvider();
  const { getOptionsByType } = useLookupCodeHelpers();
  const regionTypes = getOptionsByType(API.REGION_TYPES);
  const acquisitionTypes = getOptionsByType(API.ACQUISITION_TYPES);
  const acquisitionPhysFileTypes = getOptionsByType(API.ACQUISITION_PHYSICAL_FILE_STATUS_TYPES);
  const fileStatusTypeCodes = getOptionsByType(API.ACQUISITION_FILE_STATUS_TYPES);
  const acquisitionFundingTypes = getOptionsByType(API.ACQUISITION_FUNDING_TYPES);

  const onMinistryProjectSelected = React.useCallback(
    async (param: IAutocompletePrediction[]) => {
      if (param.length > 0) {
        if (param[0].id !== undefined) {
          const result = await retrieveProjectProducts(param[0].id);
          if (result !== undefined) {
            setProjectProducts(result);
          }
        }
      } else {
        setProjectProducts(undefined);
      }
    },
    [retrieveProjectProducts],
  );

  React.useEffect(() => {
    if (initialValues.project !== undefined) {
      onMinistryProjectSelected([initialValues.project]);
    }
  }, [initialValues, onMinistryProjectSelected]);

  // clear the associated 'Completion Date' field if the corresponding File Status has its value changed from COMPLETE to something else.
  React.useEffect(() => {
    if (!!fileStatusTypeCode && fileStatusTypeCode !== 'COMPLT') {
      setFieldValue('completionDate', '');
    }
  }, [fileStatusTypeCode, setFieldValue]);

  return (
    <Container>
      <Section>
        <SectionField
          label="Status"
          tooltip={
            <TooltipIcon
              className="tooltip-light"
              toolTipId="status-field-tooltip"
              toolTip={<StatusToolTip />}
              placement="auto"
            />
          }
        >
          <Select
            field="fileStatusTypeCode"
            options={fileStatusTypeCodes}
            placeholder="Select..."
            required
          />
        </SectionField>
      </Section>

      <Section header="Project">
        <SectionField label="Ministry project">
          <ProjectSelector
            field="project"
            onChange={(vals: IAutocompletePrediction[]) => {
              onMinistryProjectSelected(vals);
              if (vals.length === 0) {
                formikProps.setFieldValue('product', '');
              }
            }}
          />
        </SectionField>
        {projectProducts !== undefined && (
          <SectionField label="Product">
            <Select
              field="product"
              options={projectProducts.map<SelectOption>(x => {
                return { label: x.code + ' ' + x.description || '', value: x.id || 0 };
              })}
              placeholder="Select..."
            />
          </SectionField>
        )}
        <SectionField label="Funding">
          <Select
            field="fundingTypeCode"
            options={acquisitionFundingTypes}
            placeholder="Select..."
            onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
              const selectedValue = [].slice
                .call(e.target.selectedOptions)
                .map((option: HTMLOptionElement & number) => option.value)[0];
              if (!!selectedValue && selectedValue !== 'OTHER') {
                formikProps.setFieldValue('fundingTypeOtherDescription', '');
              }
            }}
          />
        </SectionField>
        {formikProps.values?.fundingTypeCode === 'OTHER' && (
          <SectionField label="Other funding">
            <Input field="fundingTypeOtherDescription" />
          </SectionField>
        )}
      </Section>

      <Section header="Schedule">
        <SectionField label="Assigned date">
          <FastDatePicker field="assignedDate" formikProps={formikProps} />
        </SectionField>
        <SectionField
          label="Delivery date"
          tooltip="Date for delivery of the property to the project"
        >
          <FastDatePicker field="deliveryDate" formikProps={formikProps} />
        </SectionField>
        <SectionField
          label="Acquisition completed date"
          tooltip={`This will be enabled when the file status is set to "Completed"`}
          required={formikProps.values?.fileStatusTypeCode === 'COMPLT'}
        >
          <FastDatePicker
            field="completionDate"
            formikProps={formikProps}
            disabled={formikProps.values?.fileStatusTypeCode !== 'COMPLT'}
          />
        </SectionField>
      </Section>

      <Section header="Acquisition Details">
        <SectionField label="Acquisition file name" required>
          <Input field="fileName" />
        </SectionField>
        <SectionField
          label="Historical file number"
          tooltip="Older file that this file represents (ex: those from the legacy system or other non-digital files.)"
        >
          <Input field="legacyFileNumber" />
        </SectionField>
        <SectionField label="Physical file status">
          <Select
            field="acquisitionPhysFileStatusType"
            options={acquisitionPhysFileTypes}
            placeholder="Select..."
          />
        </SectionField>
        <SectionField label="Acquisition type" required>
          <Select
            field="acquisitionType"
            options={acquisitionTypes}
            placeholder="Select..."
            required
          />
        </SectionField>
        <SectionField label="Ministry region" required>
          <UserRegionSelectContainer
            field="region"
            options={regionTypes}
            placeholder="Select region..."
            required
          />
        </SectionField>
      </Section>

      <Section header="Acquisition Team">
        <UpdateAcquisitionTeamSubForm />
        {formikProps.errors?.team && typeof formikProps.errors?.team === 'string' && (
          <div className="invalid-feedback" data-testid="team-profile-dup-error">
            {formikProps.errors.team.toString()}
          </div>
        )}
      </Section>

      <Section header="Owners">
        <StyledSectionParagraph>
          Each property in this file should be owned by the owner(s) in this section
        </StyledSectionParagraph>
        <UpdateAcquisitionOwnersSubForm />
        <SectionField label="Owner's Solicitor" className="mt-4">
          <ContactInputContainer
            field="ownerSolicitor.contact"
            View={ContactInputView}
          ></ContactInputContainer>
        </SectionField>
      </Section>
    </Container>
  );
};

const Container = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};

  .react-datepicker-wrapper {
    max-width: 14rem;
  }

  [name='region'] {
    max-width: 25rem;
  }
`;
