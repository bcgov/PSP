import { FormikProps } from 'formik/dist/types';
import { MultiPolygon, Polygon } from 'geojson';
import { useState } from 'react';

import { exists, firstOrNull } from '@/utils';

import { ShapeUploadModel, UploadResponseModel } from './models';
import { IShapeUploadFormProps } from './ShapeUploadForm';
import { IShapeUploadResultViewProps } from './ShapeUploadResultView';

export interface IShapeUploadContainerProps {
  formikRef: React.Ref<FormikProps<ShapeUploadModel>>;
  uploadResult: UploadResponseModel | null;
  propertyIdentifier?: string;
  onUploadFile: (result: UploadResponseModel) => void;
  View: React.FunctionComponent<IShapeUploadFormProps>;
  ResultsView: React.FunctionComponent<IShapeUploadResultViewProps>;
}

/**
 * Component that provides functionality to upload boundary files (Shapefile, KML, KMZ).
 * Can be embedded as a widget.
 */
export const ShapeUploadContainer: React.FunctionComponent<IShapeUploadContainerProps> = ({
  formikRef,
  uploadResult,
  propertyIdentifier,
  onUploadFile,
  View,
  ResultsView,
}) => {
  const [isUploading, setIsUploading] = useState(false);

  const onUploadFileHandler = async (request: ShapeUploadModel) => {
    const response = new UploadResponseModel(request.file?.name ?? 'Unknown');
    try {
      setIsUploading(true);
      // Convert shapefile to GeoJSON
      const shapeFeatures = await request.toGeoJson();
      response.isSuccess = true;
      response.boundary = firstOrNull(shapeFeatures.features)?.geometry as Polygon | MultiPolygon;
      if (exists(response.boundary)) {
        response.boundary.bbox = undefined; // remove bbox as this can cause issues with polygon json parsing on the backend.
      }
    } catch (error) {
      response.isSuccess = false;
      response.errorMessage = (error as Error).message;
    } finally {
      setIsUploading(false);
      onUploadFile(response);
    }
  };

  return exists(uploadResult) ? (
    <ResultsView uploadResult={uploadResult} />
  ) : (
    <View
      formikRef={formikRef}
      isLoading={isUploading}
      propertyIdentifier={propertyIdentifier}
      onUploadFile={onUploadFileHandler}
    />
  );
};

export default ShapeUploadContainer;
