import React, { useState } from 'react';

import { IToggleSaveInputViewProps } from './ToggleSaveInputView';

export interface IToggleSaveInputContainerProps {
  onSave: (value: string) => Promise<string>;
  initialValue?: string;
  asCurrency?: boolean;
  View: React.FC<IToggleSaveInputViewProps>;
}

/**
 * Non-formik component that allows a single input to be updated/saved.
 */
export const ToggleSaveInputContainer: React.FC<IToggleSaveInputContainerProps> = ({
  onSave,
  initialValue,
  asCurrency,
  View,
}) => {
  const [isEditing, setIsEditing] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const [value, setValue] = useState(initialValue || '');

  const onClick = async () => {
    try {
      setIsSaving(true);
      const updatedValue = await onSave(value);
      setValue(updatedValue);
      setIsEditing(false); // only set to false if save is successful
    } catch (e) {
      //ignore any errors, just set isSaving to false
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <View
      {...{
        onClick,
        initialValue,
        asCurrency,
        isEditing,
        isSaving,
        value,
        setValue,
        setIsEditing,
      }}
    />
  );
};
