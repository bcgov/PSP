import { FastDatePicker, Select, SelectOption } from 'components/common/form';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikProps } from 'formik';
import { Api_Compensation } from 'models/api/Compensation';
import moment from 'moment';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import SidebarFooter from '../shared/SidebarFooter';
import { CompensationRequisitionYupSchema } from './CompensationRequisitionYupSchema';
import { CompensationRequisitionFormModel } from './models';

export interface CompensationRequisitionFormProps {
  isLoading: boolean;
  formikRef: React.Ref<FormikProps<CompensationRequisitionFormModel>>;
  initialValues: CompensationRequisitionFormModel;
  onSave: (compensation: CompensationRequisitionFormModel) => Promise<Api_Compensation | undefined>;
}

const UpdateCompensationRequisitionForm: React.FC<CompensationRequisitionFormProps> = ({
  isLoading,
  formikRef,
  initialValues,
  onSave,
}) => {
  return (
    <StyledFormWrapper>
      <Formik<CompensationRequisitionFormModel>
        enableReinitialize
        innerRef={formikRef}
        initialValues={initialValues}
        onSubmit={async values => {
          await onSave(values);
        }}
        validationSchema={CompensationRequisitionYupSchema}
      >
        {formikProps => (
          <>
            <LoadingBackdrop show={isLoading}></LoadingBackdrop>
            <Section header="Requisition details">
              <SectionField label="Status" labelWidth="4" contentWidth="4">
                <Select
                  field="status"
                  options={[
                    {
                      label: 'Draft',
                      value: 'draft',
                    },
                    {
                      label: 'Final',
                      value: 'final',
                    },
                  ]}
                  placeholder="Select..."
                />
              </SectionField>
              <SectionField label="Agreement date" labelWidth="4" contentWidth="4">
                <FastDatePicker field="agreementDateTime" formikProps={formikProps} />
              </SectionField>
            </Section>
            <Prompt
              when={formikProps.dirty}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
            {/* <SidebarFooter
              onSave={() => submitForm()}
              isOkDisabled={isSubmitting || !dirty}
              onCancel={() => cancelFunc(resetForm, dirty)}
            /> */}
          </>
        )}
      </Formik>
    </StyledFormWrapper>
  );
};

export default UpdateCompensationRequisitionForm;

export const generateFiscalYearOptions = () => {
  let sysdate = moment();
  let fiscalYearStartDate = moment(`03/31/${moment().year()}`, 'MM-DD-YYYY');
  let pivotYear = sysdate.isAfter(fiscalYearStartDate)
    ? moment().year()
    : moment().subtract(1, 'years').year();

  const options: SelectOption[] = [];

  options.push({ label: `${--pivotYear}/${pivotYear}`, value: `${--pivotYear}/${pivotYear}` });
  options.push({ label: `${pivotYear}/${++pivotYear}`, value: `${pivotYear}/${++pivotYear}` });
  options.push({
    label: `${++pivotYear}/${pivotYear + 2}`,
    value: `${++pivotYear}/${pivotYear + 2}`,
  });

  return options;
};

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
