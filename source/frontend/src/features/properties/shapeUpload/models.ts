import { FeatureCollection, MultiPolygon, Polygon } from 'geojson';
import JSZip from 'jszip';
import shp from 'shpjs';

import { exists, firstOrNull } from '@/utils';

export class ShapeUploadModel {
  public file: File | null;

  constructor(file: File | null = null) {
    this.file = file;
  }

  public async isShapefile(): Promise<boolean> {
    try {
      await this.validateShapefile();
      return true;
    } catch {
      return false;
    }
  }

  public async validateShapefile(buffer?: ArrayBuffer): Promise<void> {
    if (!exists(this.file) && !exists(buffer)) {
      throw new Error('No file or buffer provided');
    }

    const arrayBuffer = buffer ?? (await this.file.arrayBuffer());
    const zip = await JSZip.loadAsync(arrayBuffer);
    const files = zip.files;

    // Find a .shp entry in the ZIP file and validate it has the correct file header and accompanying .shx and .dbf files.
    const shpEntry = Object.values(files).find(f => f.name.toLowerCase().endsWith('.shp'));
    if (!exists(shpEntry)) {
      throw new Error('Invalid shapefile: no .shp file found in ZIP.');
    }

    // Validate the .shp magic number (big-endian 9994 = 0x0000270A) - https://en.wikipedia.org/wiki/Shapefile
    const shpBuffer = await shpEntry.async('arraybuffer');
    const magicNumber = new DataView(shpBuffer).getInt32(0, false);
    if (magicNumber !== 9994) {
      throw new Error(
        `Invalid shapefile: unexpected .shp header (got ${magicNumber}, expected 9994).`,
      );
    }

    const baseName = shpEntry.name.toLowerCase().replace(/\.shp$/, '');
    const allNames = Object.keys(files).map(f => f.toLowerCase());

    if (!allNames.includes(`${baseName}.shx`)) {
      throw new Error('Invalid shapefile: missing required .shx file.');
    }
    if (!allNames.includes(`${baseName}.dbf`)) {
      throw new Error('Invalid shapefile: missing required .dbf file.');
    }
  }

  public async toGeoJson(): Promise<FeatureCollection> {
    if (!exists(this.file)) {
      throw new Error('No file provided');
    }

    const arrayBuffer = await this.file.arrayBuffer();
    await this.validateShapefile(arrayBuffer);

    try {
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
