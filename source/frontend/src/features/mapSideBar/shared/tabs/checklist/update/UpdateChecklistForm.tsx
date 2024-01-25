import axios, { AxiosError } from 'axios';
import { Formik, FormikProps } from 'formik';
import React from 'react';

import { Select } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';
import { exists, isValidId, prettyFormatUTCDate } from '@/utils';

import { StyledSectionCentered } from '../detail/styles';
import { ChecklistFormModel } from './models';

export interface IUpdateChecklistFormProps {
  formikRef: React.Ref<FormikProps<ChecklistFormModel>>;
  initialValues: ChecklistFormModel;
  sectionTypeName: string;
  statusTypeName: string;
  prefix: string;
  onSave: (
    apiFile: ApiGen_Concepts_FileWithChecklist,
  ) => Promise<ApiGen_Concepts_FileWithChecklist | undefined>;
  onSuccess: (apiFile: ApiGen_Concepts_FileWithChecklist) => Promise<void>;
  onError: (e: AxiosError<IApiError>) => void;
}

export const UpdateChecklistForm: React.FC<IUpdateChecklistFormProps> = ({
  formikRef,
  initialValues,
  onSave,
  onSuccess,
  onError,
  sectionTypeName,
  statusTypeName,
  prefix,
}) => {
  const { getByType, getOptionsByType } = useLookupCodeHelpers();
  const sectionTypes = getByType(sectionTypeName);
  const statusTypes = getOptionsByType(statusTypeName);

  const lastUpdated = initialValues.lastModifiedBy();

  return (
    <Formik<ChecklistFormModel>
      enableReinitialize
      innerRef={formikRef}
      initialValues={initialValues}
      onSubmit={async (values, formikHelpers) => {
        try {
          const updatedFile = await onSave(values.toApi());
          if (exists(updatedFile) && isValidId(updatedFile?.id)) {
            formikHelpers.resetForm({
              values: ChecklistFormModel.fromApi(updatedFile, sectionTypes),
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
            <Section key={section.id ?? `${prefix}-checklist-section-${i}`} header={section.name}>
              {section.items.map((checklistItem, j) => (
                <SectionField
                  key={checklistItem.itemType?.code ?? `${prefix}-checklist-item-${j}`}
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
