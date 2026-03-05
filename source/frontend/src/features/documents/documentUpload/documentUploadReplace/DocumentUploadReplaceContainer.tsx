import { useState } from 'react';

import { exists } from '@/utils/utils';

import { IDocumentUploadReplaceViewProps } from './DocumentUploadReplaceView';

export interface IDocumentUploadReplaceContainerProps {
  onReplaceDocumentFile: (file: File) => void;
  onCancelReplaceFile: () => void;
  View: React.FunctionComponent<IDocumentUploadReplaceViewProps>;
}

const DocumentUploadReplaceContainer: React.FunctionComponent<
  IDocumentUploadReplaceContainerProps
> = ({ onReplaceDocumentFile, onCancelReplaceFile, View }) => {
  const [replacementFile, setReplacementFile] = useState<File | null>(null);

  const handleOnSelectedFile = (files: File[]) => {
    if (exists(files) && files.length) {
      const replacementFile = files[0];
      setReplacementFile(replacementFile);
    }
  };

  return (
    <View
      file={replacementFile}
      onCancelReplace={() => {
        setReplacementFile(null);
        onCancelReplaceFile && onCancelReplaceFile();
      }}
      onConfirmReplace={() => onReplaceDocumentFile(replacementFile)}
      onSelectedReplacementFile={handleOnSelectedFile}
    ></View>
  );
};

export default DocumentUploadReplaceContainer;
