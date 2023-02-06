import {
  AsyncTypeahead,
  FastDatePicker,
  Input,
  Select,
  SelectOption,
} from 'components/common/form';
import { UserRegionSelectContainer } from 'components/common/form/UserRegionSelect/UserRegionSelectContainer';
import TooltipIcon from 'components/common/TooltipIcon';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import { useProjectProvider } from 'hooks/repositories/useProjectProvider';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { useProjectTypeahead } from 'hooks/useProjectTypeahead';
import { IAutocompletePrediction } from 'interfaces';
import { Api_Product } from 'models/api/Project';
import React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { UpdateAcquisitionTeamSubForm } from '../../common/update/acquisitionTeam/UpdateAcquisitionTeamSubForm';
import { UpdateAcquisitionSummaryFormModel } from './models';
import StatusToolTip from './StatusToolTip';

export interface IUpdateAcquisitionFormProps {
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

export const UpdateAcquisitionForm = React.forwardRef<
  FormikProps<UpdateAcquisitionSummaryFormModel>,
  IUpdateAcquisitionFormProps
>((props, formikRef) => {
  const { initialValues, validationSchema, onSubmit } = props;

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

  const { handleTypeaheadSearch, isTypeaheadLoading, matchedProjects } = useProjectTypeahead();

  const onMinistrySelected = React.useCallback(
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
      onMinistrySelected([initialValues.project]);
    }
  }, [initialValues, onMinistrySelected]);

  return (
    <Formik<UpdateAcquisitionSummaryFormModel>
      enableReinitialize
      innerRef={formikRef}
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={onSubmit}
    >
      {formikProps => (
        <>
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
                <AsyncTypeahead
                  field="project"
                  labelKey="text"
                  isLoading={isTypeaheadLoading}
                  options={matchedProjects}
                  onSearch={handleTypeaheadSearch}
                  onChange={(vals: IAutocompletePrediction[]) => {
                    onMinistrySelected(vals);
                    if (vals.length === 0) {
                      formikProps.setFieldValue('product', 0);
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
            </Section>

            <Section header="Acquisition Details">
              <SectionField label="Acquisition file name">
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
              <SectionField label="Acquisition type">
                <Select
                  field="acquisitionType"
                  options={acquisitionTypes}
                  placeholder="Select..."
                  required
                />
              </SectionField>
              <SectionField label="Ministry region">
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
            </Section>
          </Container>

          <Prompt
            when={formikProps.dirty && formikProps.submitCount === 0}
            message="You have made changes on this form. Do you wish to leave without saving?"
          />
        </>
      )}
    </Formik>
  );
});

export default UpdateAcquisitionForm;

const Container = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};

  .react-datepicker-wrapper {
    max-width: 14rem;
  }

  [name='region'] {
    max-width: 25rem;
  }
`;
