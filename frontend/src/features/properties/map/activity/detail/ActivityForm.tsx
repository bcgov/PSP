import { UnsavedChangesPrompt } from 'components/common/form/UnsavedChangesPrompt';
import { Section } from 'features/mapSideBar/tabs/Section';
import SidebarFooter from 'features/properties/map/shared/SidebarFooter';
import { Formik, validateYupSchema, yupToFormErrors } from 'formik';
import { getCancelModalProps, useModalContext } from 'hooks/useModalContext';
import { Api_Activity } from 'models/api/Activity';
import * as React from 'react';
import { toTypeCode } from 'utils/formUtils';

import { Activity, ActivityFile } from './ActivityContainer';
import { ActivityView } from './ActivityView';
import { IActivityFormContent } from './content/models';
import { ActivityModel } from './models';

export interface IActivityFormProps {
  activity: Activity;
  file: ActivityFile;
  editMode: boolean;
  setEditMode: (editMode: boolean) => void;
  onSave: (activity: Api_Activity) => Promise<Api_Activity | undefined>;
  onEditRelatedProperties: () => void;
  formContent?: IActivityFormContent;
}

export const ActivityForm = ({
  activity,
  file,
  editMode,
  setEditMode,
  onSave,
  onEditRelatedProperties,
  formContent,
}: IActivityFormProps) => {
  const { setModalProps, setDisplayModal } = useModalContext();

  const cancelFunc = (resetForm: () => void, dirty: boolean) => {
    const onCancel = () => {
      resetForm();
      setEditMode(false);
    };
    if (!dirty) {
      onCancel();
    } else {
      setModalProps({
        ...getCancelModalProps(),
        handleOk: () => {
          onCancel();
          setDisplayModal(false);
        },
        display: true,
      });
    }
  };
  const EditForm = formContent?.EditForm;
  const ViewForm = formContent?.ViewForm;
  const initialValues = ActivityModel.fromApi(activity, file.fileType);
  if (
    !!initialValues.activityData?.version &&
    formContent !== undefined &&
    initialValues.activityData?.version !== formContent?.version
  ) {
    throw Error(
      'Unable to display activity form data. Form data was saved in an older version that is not compatible with this version of the application',
    );
  }
  initialValues.activityData = { ...formContent?.initialValues, ...initialValues.activityData };

  return (
    <Formik<ActivityModel>
      initialValues={initialValues}
      validate={(values: ActivityModel) => {
        let functionErrors = {};
        try {
          functionErrors = formContent?.validationFunction
            ? formContent?.validationFunction(values)
            : {};
          validateYupSchema(values, formContent?.validationSchema, true);
          return functionErrors;
        } catch (err) {
          return { ...functionErrors, ...yupToFormErrors(err) };
        }
      }}
      onSubmit={async (values, formikHelpers) => {
        const updatedActivity = await onSave(values.toApi());
        if (!!updatedActivity) {
          formikHelpers.resetForm({
            values: ActivityModel.fromApi(updatedActivity, file.fileType),
          });
          setEditMode(false);
        }
      }}
    >
      {({ submitForm, resetForm, dirty, isSubmitting, values }) => (
        <>
          <UnsavedChangesPrompt />
          <ActivityView
            activity={activity}
            file={file}
            editMode={editMode}
            setEditMode={setEditMode}
            onEditRelatedProperties={onEditRelatedProperties}
          >
            <Section
              header={formContent?.header ?? ''}
              initiallyExpanded={editMode}
              isCollapsable
              title={formContent?.header?.toLocaleLowerCase() ?? ''}
            >
              {editMode && EditForm && <EditForm />}
              {!editMode && ViewForm && <ViewForm />}
            </Section>
          </ActivityView>
          {editMode && (
            <SidebarFooter
              onSave={() => submitForm()}
              isOkDisabled={isSubmitting || !dirty}
              onCancel={() => cancelFunc(resetForm, dirty)}
            />
          )}
        </>
      )}
    </Formik>
  );
};

export default ActivityForm;
