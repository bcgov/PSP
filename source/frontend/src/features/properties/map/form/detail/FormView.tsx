import * as React from 'react';
import { MdClose } from 'react-icons/md';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { ActivityTrayPage, CloseButton, TrayHeader } from '@/components/common/styles';
import { Api_FormDocumentFile } from '@/models/api/FormDocument';

export interface IFormViewProps {
  loading: boolean;
  formFile?: Api_FormDocumentFile;
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
    <ActivityTrayPage>
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
    </ActivityTrayPage>
  );
};

export default FormView;
