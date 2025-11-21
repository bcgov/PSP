import { FeatureCollection, MultiPolygon, Polygon } from 'geojson';
import shp from 'shpjs';

import { exists, firstOrNull } from '@/utils';

export class ShapeUploadModel {
  public file: File | null;

  constructor(file: File | null = null) {
    this.file = file;
  }

  public async toGeoJson(): Promise<FeatureCollection> {
    if (!exists(this.file)) {
      throw new Error('No file provided');
    }
    try {
      const arrayBuffer = await this.file.arrayBuffer();
      const geojson = await shp(arrayBuffer);
      return Array.isArray(geojson) ? firstOrNull(geojson) : geojson;
    } catch (error) {
      throw new Error('Failed to parse shapefile. Please ensure the file is a valid shapefile.');
    }
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
