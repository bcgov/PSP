import { FeatureCollection } from 'geojson';
import JSZip from 'jszip';
import shp from 'shpjs';

import { exists, firstOrNull } from '@/utils';

export class ShapefileHelper {
  /**
   * Checks if the provided ArrayBuffer represents a valid shapefile.
   * @param buffer The ArrayBuffer of the shapefile to validate.
   * @returns true if the buffer is a valid shapefile (contains a .shp file with correct header and accompanying .shx and .dbf files), false otherwise.
   */
  public static async isShapefile(buffer: ArrayBuffer): Promise<boolean> {
    try {
      await ShapefileHelper.validate(buffer);
      return true;
    } catch {
      return false;
    }
  }

  /**
   * Find a .shp entry in the ZIP file and validate it has the correct file header and accompanying .shx and .dbf files.
   * @param buffer The ArrayBuffer of the shapefile to validate.
   */
  public static async validate(buffer: ArrayBuffer): Promise<void> {
    const zip = await JSZip.loadAsync(buffer);
    const files = zip.files;

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

  public static async toGeoJson(buffer: ArrayBuffer): Promise<FeatureCollection> {
    try {
      const geojson = await shp(buffer);
      return Array.isArray(geojson) ? firstOrNull(geojson) : geojson;
    } catch {
      throw new Error('Failed to parse shapefile. Please ensure the file is a valid shapefile.');
    }
  }
}
