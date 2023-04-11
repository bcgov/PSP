import { UnsavedChangesPrompt } from 'components/common/form/UnsavedChangesPrompt';
import { Claims } from 'constants/claims';
import { Section } from 'features/mapSideBar/tabs/Section';
import SidebarFooter from 'features/properties/map/shared/SidebarFooter';
import { Formik, validateYupSchema, yupToFormErrors } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { getCancelModalProps, useModalContext } from 'hooks/useModalContext';
import { Api_Activity } from 'models/api/Activity';
import * as React from 'react';
import * as Yup from 'yup';

import { IFormContent } from '../../shared/content/models';
import { Activity, ActivityFile } from './ActivityContainer';
import { ActivityView } from './ActivityView';
import { ActivityModel } from './models';

export interface IActivityFormProps {
  activity: Activity;
  file: ActivityFile;
  editMode: boolean;
  isEditable: boolean;
  setEditMode: (editMode: boolean) => void;
  onSave: (activity: Api_Activity) => Promise<Api_Activity | undefined>;
  onEditRelatedProperties: () => void;
  formContent?: IFormContent;
}

export const ActivityForm = ({
  activity,
  file,
  editMode,
  isEditable,
  setEditMode,
  onSave,
  onEditRelatedProperties,
  formContent,
}: IActivityFormProps) => {
  const { setModalContent, setDisplayModal } = useModalContext();
  const { hasClaim } = useKeycloakWrapper();
  const cancelFunc = (resetForm: () => void, dirty: boolean) => {
    const onCancel = () => {
      resetForm();
      setEditMode(false);
    };
    if (!dirty) {
      onCancel();
    } else {
      setModalContent({
        ...getCancelModalProps(),
        handleOk: () => {
          onCancel();
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
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
          if (values.activityStatusTypeCode?.id !== 'CANCELLED') {
            validateYupSchema(values, activityYupSchema, true);
            validateYupSchema(values, formContent?.validationSchema, true);
          }
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
            isEditable={isEditable}
            editMode={editMode}
            onEditRelatedProperties={onEditRelatedProperties}
          >
            {(EditForm || ViewForm) && (
              <Section
                header={formContent?.header ?? ''}
                initiallyExpanded={editMode}
                isCollapsable
                title={formContent?.header?.toLocaleLowerCase() ?? ''}
              >
                {editMode && EditForm && <EditForm />}
                {!editMode && ViewForm && <ViewForm />}
              </Section>
            )}
          </ActivityView>

          <SidebarFooter
            editMode={editMode}
            showEdit={hasClaim(Claims.ACTIVITY_EDIT)}
            onEdit={setEditMode}
            onSave={() => submitForm()}
            isOkDisabled={isSubmitting || !dirty}
            onCancel={() => cancelFunc(resetForm, dirty)}
          />
        </>
      )}
    </Formik>
  );
};

const activityYupSchema = Yup.object().shape({
  description: Yup.string().max(500, 'description must be at most 500 characters'),
});

export default ActivityForm;
