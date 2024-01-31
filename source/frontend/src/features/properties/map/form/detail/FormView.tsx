import * as React from 'react';
import { MdClose } from 'react-icons/md';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { CloseButton, TrayContent, TrayHeader } from '@/components/common/styles';
import { ApiGen_Concepts_FormDocumentFile } from '@/models/api/generated/ApiGen_Concepts_FormDocumentFile';

export interface IFormViewProps {
  loading: boolean;
  formFile?: ApiGen_Concepts_FormDocumentFile;
  onClose: () => void;
}

export const FormView: React.FunctionComponent<IFormViewProps> = ({
  loading,
  formFile,
  onClose,
}) => {
  if (loading) {
    return <LoadingBackdrop show={loading} parentScreen={true} />;
  }
  return (
    <TrayContent>
      <TrayHeader>
        {loading ? '' : formFile?.formDocumentType?.description ?? ''}
        <CloseButton
          id="close-tray"
          icon={<MdClose size={20} />}
          title="close"
          onClick={() => {
            onClose();
          }}
        ></CloseButton>
      </TrayHeader>
      {/*No requirements for form display yet, but if similar to activity form, look at ActivityForm.tsx*/}
    </TrayContent>
  );
};

export default FormView;
