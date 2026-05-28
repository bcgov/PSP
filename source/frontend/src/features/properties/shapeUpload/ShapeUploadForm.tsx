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
  propertyIdentifier?: string;
  onUploadFile: (request: ShapeUploadModel) => Promise<void>;
}

/**
 * Component that provides functionality to upload boundary files (Shapefile, KML, KMZ).
 * Can be embedded as a widget.
 */
export const ShapeUploadForm: React.FunctionComponent<IShapeUploadFormProps> = ({
  isLoading,
  formikRef,
  propertyIdentifier,
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
            label="Upload a boundary file"
            tooltip="Accepted formats: Shapefile (.zip), KML (.kml), or KMZ (.kmz)"
            labelWidth={{ xs: 12 }}
            className="mb-4"
          >
            <div className="pt-2  pb-1 text-muted">
              Accepted formats: Shapefile (.zip), KML (.kml), or KMZ (.kmz)
            </div>
            <FileDragAndDrop
              onSelectFiles={files => {
                if (files.length === 1) {
                  formikProps.setFieldValue('file', firstOrNull(files));
                }
              }}
              validExtensions={['zip', 'kml', 'kmz']}
              multiple={false}
              keyName={formikProps.values?.file?.name}
            />
          </SectionField>
          <DisplayError field="file" className="pt-4" />

          {exists(formikProps.values?.file) && (
            <>
              <SectionField label="File" labelWidth={{ xs: 1 }}>
                {truncate(formikProps.values?.file?.name, { length: 100 })}
              </SectionField>
              <div className="pt-1">
                {exists(propertyIdentifier)
                  ? `You have attached a boundary file for property: ${propertyIdentifier}. Do you want to proceed and save?`
                  : 'You have attached a boundary file. Do you want to proceed and save?'}
              </div>
            </>
          )}
        </>
      )}
    </Formik>
  );
};

export default ShapeUploadForm;
