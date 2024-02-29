import { useFormikContext } from 'formik';
import React, { useEffect } from 'react';
import { Prompt } from 'react-router-dom';

const DEFAULT_PROMPT_MESSAGE = 'You have unsaved changes, are you sure you want to leave?';

export interface IUnsavedChangesPrompt {
  message?: string;
  resetFormUponConfirmation?: boolean;
}

/**
 * Formik-connected <Prompt> to show a confirmation dialog
 * when user tries to navigate away and form has unsaved changes.
 */
export const UnsavedChangesPrompt: React.FC<React.PropsWithChildren<IUnsavedChangesPrompt>> = ({
  message = DEFAULT_PROMPT_MESSAGE,
  resetFormUponConfirmation = true,
}) => {
  const { dirty, submitCount, resetForm } = useFormikContext();

  // if we navigate away from this page successfully, reset the form.
  useEffect(() => {
    return () => {
      if (resetFormUponConfirmation) {
        resetForm();
      }
    };
  }, [resetForm, resetFormUponConfirmation]);

  return <Prompt when={dirty && submitCount === 0} message={message} />;
};
