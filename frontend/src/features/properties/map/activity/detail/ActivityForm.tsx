import { UnsavedChangesPrompt } from 'components/common/form/UnsavedChangesPrompt';
import SidebarFooter from 'features/properties/map/shared/SidebarFooter';
import { Formik } from 'formik';
import { getCancelModalProps, useModalContext } from 'hooks/useModalContext';
import { Api_Activity } from 'models/api/Activity';
import * as React from 'react';

import { Activity, ActivityFile } from './ActivityContainer';
import { ActivityView } from './ActivityView';

export interface IActivityFormProps {
  activity: Activity;
  file: ActivityFile;
  editMode: boolean;
  setEditMode: (editMode: boolean) => void;
  onSave: (activity: Api_Activity) => Promise<Api_Activity | undefined>;
}

export const ActivityForm: React.FunctionComponent<IActivityFormProps> = ({
  activity,
  file,
  editMode,
  setEditMode,
  onSave,
}) => {
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
  return (
    <Formik<Api_Activity>
      initialValues={activity ?? { description: '', activityDataJson: '' }}
      onSubmit={async (values, formikHelpers) => {
        const updatedActivity = await onSave(values);
        if (!!updatedActivity) {
          formikHelpers.resetForm({ values: updatedActivity });
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
          />
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
