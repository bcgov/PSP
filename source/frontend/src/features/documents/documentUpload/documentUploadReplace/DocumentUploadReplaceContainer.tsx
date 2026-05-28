import { useState } from 'react';

import { exists } from '@/utils/utils';

import { IDocumentUploadReplaceViewProps } from './DocumentUploadReplaceView';

export interface IDocumentUploadReplaceContainerProps {
  index: number;
  onReplaceDocumentFile: (file: File) => void;
  onCancelReplaceFile: () => void;
  View: React.FunctionComponent<IDocumentUploadReplaceViewProps>;
}

const DocumentUploadReplaceContainer: React.FunctionComponent<
  IDocumentUploadReplaceContainerProps
> = ({ index, onReplaceDocumentFile, onCancelReplaceFile, View }) => {
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
      index={index}
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
