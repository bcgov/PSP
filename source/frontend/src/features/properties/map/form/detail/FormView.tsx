import { ActivityTrayPage, CloseButton, TrayHeader } from 'components/common/styles';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Api_FileForm } from 'models/api/Form';
import * as React from 'react';
import { MdClose } from 'react-icons/md';

import { IFormContent } from '../../shared/content/models';

export interface IFormViewProps {
  loading: boolean;
  formFile?: Api_FileForm;
  formContent?: IFormContent;
  onClose: () => void;
}

export const FormView: React.FunctionComponent<IFormViewProps> = ({
  loading,
  formFile,
  formContent,
  onClose,
}) => {
  if (loading) {
    return <LoadingBackdrop show={loading} parentScreen={true} />;
  }
  return (
    <ActivityTrayPage>
      <TrayHeader>
        {loading ? '' : formFile?.formTypeCode?.name ?? ''}
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
