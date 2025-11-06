import { Formik, FormikProps } from 'formik';
import truncate from 'lodash/truncate';

import { DisplayError } from '@/components/common/form';
import FileDragAndDrop from '@/components/common/form/FileDragAndDrop';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { SectionField } from '@/components/common/Section/SectionField';
import { exists, firstOrNull } from '@/utils';

import { ShapeUploadModel } from './models';

export interface IShapeUploadFormProps {
  isLoading: boolean;
  formikRef: React.Ref<FormikProps<ShapeUploadModel>>;
  onUploadFile: (request: ShapeUploadModel) => void;
}

/**
 * Component that provides functionality to upload shapefiles. Can be embedded as a widget.
 */
export const ShapeUploadForm: React.FunctionComponent<IShapeUploadFormProps> = ({
  isLoading,
  formikRef,
  onUploadFile,
}) => {
  return (
    <Formik<ShapeUploadModel>
      innerRef={formikRef}
      enableReinitialize
      initialValues={new ShapeUploadModel()}
      validateOnMount={true}
      onSubmit={async (values: ShapeUploadModel) => await onUploadFile(values)}
    >
      {formikProps => (
        <>
          <LoadingBackdrop show={isLoading} />
          <SectionField
            label="Choose a shapefile to attach for upload"
            labelWidth={{ xs: 12 }}
            className="mb-4"
          >
            <div className="pt-2"></div>
            <FileDragAndDrop
              onSelectFiles={files => {
                if (files.length === 1) {
                  formikProps.setFieldValue('file', firstOrNull(files));
                }
              }}
              validExtensions={['zip']}
              multiple={false}
              keyName={formikProps.values?.file?.name}
            />
          </SectionField>

          {exists(formikProps.values?.file) && (
            <>
              <SectionField label="File" labelWidth={{ xs: 12 }}>
                {truncate(formikProps.values?.file?.name, { length: 100 })}
              </SectionField>
              <div className="pt-4"></div>
              <DisplayError field="file" />
              <div className="pt-1">
                You have attached a shapefile. Do you want to proceed and save?
              </div>
            </>
          )}
        </>
      )}
    </Formik>
  );
};

export default ShapeUploadForm;
