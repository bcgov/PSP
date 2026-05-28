import { FeatureCollection, MultiPolygon, Polygon } from 'geojson';

import { exists } from '@/utils';

import { KmzHelper } from './helpers/KmzHelper';
import { ShapefileHelper } from './helpers/ShapefileHelper';

export class ShapeUploadModel {
  public file: File | null;

  constructor(file: File | null = null) {
    this.file = file;
  }

  public async toGeoJson(): Promise<FeatureCollection> {
    if (!exists(this.file)) {
      throw new Error('No file provided');
    }

    const arrayBuffer = await this.file.arrayBuffer();

    if (await ShapefileHelper.isShapefile(arrayBuffer)) {
      return ShapefileHelper.toGeoJson(arrayBuffer);
    }

    if ((await KmzHelper.isKml(arrayBuffer)) || (await KmzHelper.isKmz(arrayBuffer))) {
      return KmzHelper.toGeoJson(arrayBuffer);
    }

    throw new Error(
      'Unsupported file format. Please upload a valid shapefile (.zip), KMZ, or KML file.',
    );
  }
}

export class UploadResponseModel {
  public readonly fileName: string;
  public isSuccess = false;
  public errorMessage: string | null = null;
  public boundary: Polygon | MultiPolygon | null = null;

  constructor(fileName: string) {
    this.fileName = fileName;
  }
}
