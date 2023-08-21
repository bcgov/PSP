import axios, { AxiosError } from 'axios';
import { Formik, FormikProps } from 'formik';
import React from 'react';

import { Select } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { IApiError } from '@/interfaces/IApiError';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { prettyFormatUTCDate } from '@/utils';

import { StyledSectionCentered } from '../detail/styles';
import { AcquisitionChecklistFormModel } from './models';

export interface IUpdateAcquisitionChecklistFormProps {
  formikRef: React.Ref<FormikProps<AcquisitionChecklistFormModel>>;
  initialValues: AcquisitionChecklistFormModel;
  onSave: (apiAcquisitionFile: Api_AcquisitionFile) => Promise<Api_AcquisitionFile | undefined>;
  onSuccess: (apiAcquisitionFile: Api_AcquisitionFile) => Promise<void>;
  onError: (e: AxiosError<IApiError>) => void;
}

export const UpdateAcquisitionChecklistForm: React.FC<IUpdateAcquisitionChecklistFormProps> = ({
  formikRef,
  initialValues,
  onSave,
  onSuccess,
  onError,
}) => {
  const { getByType, getOptionsByType } = useLookupCodeHelpers();
  const sectionTypes = getByType(API.ACQUISITION_CHECKLIST_SECTION_TYPES);
  const statusTypes = getOptionsByType(API.ACQUISITION_CHECKLIST_ITEM_STATUS_TYPES);

  const lastUpdated = initialValues.lastModifiedBy();

  return (
    <Formik<AcquisitionChecklistFormModel>
      enableReinitialize
      innerRef={formikRef}
      initialValues={initialValues}
      onSubmit={async (values, formikHelpers) => {
        try {
          const updatedFile = await onSave(values.toApi());
          if (!!updatedFile?.id) {
            formikHelpers.resetForm({
              values: AcquisitionChecklistFormModel.fromApi(updatedFile, sectionTypes),
            });
            await onSuccess(updatedFile);
          }
        } catch (e) {
          if (axios.isAxiosError(e)) {
            const axiosError = e as AxiosError<IApiError>;
            onError && onError(axiosError);
          }
        } finally {
          formikHelpers.setSubmitting(false);
        }
      }}
    >
      {formikProps => (
        <StyledSummarySection>
          {lastUpdated && (
            <StyledSectionCentered>
              <em>
                {`This checklist was last updated ${prettyFormatUTCDate(
                  lastUpdated.appLastUpdateTimestamp,
                )} by `}
                <UserNameTooltip
                  userName={lastUpdated.appLastUpdateUserid}
                  userGuid={lastUpdated.appLastUpdateUserGuid}
                />
              </em>
            </StyledSectionCentered>
          )}

          {formikProps.values.checklistSections.map((section, i) => (
            <Section key={section.id ?? `acq-checklist-section-${i}`} header={section.name}>
              {section.items.map((checklistItem, j) => (
                <SectionField
                  key={checklistItem.itemType?.code ?? `acq-checklist-item-${j}`}
                  label={checklistItem.itemType?.description ?? ''}
                  tooltip={checklistItem.itemType?.hint}
                  labelWidth="7"
                >
                  <Select
                    field={`checklistSections[${i}].items[${j}].statusType`}
                    options={statusTypes}
                  />
                </SectionField>
              ))}
            </Section>
          ))}
        </StyledSummarySection>
      )}
    </Formik>
  );
};
