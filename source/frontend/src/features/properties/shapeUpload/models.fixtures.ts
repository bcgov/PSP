import { FeatureCollection } from 'geojson';
import JSZip from 'jszip';

export const VALID_KML = `<?xml version="1.0" encoding="UTF-8"?>
<kml xmlns="http://www.opengis.net/kml/2.2">
  <Placemark>
    <n>Test</n>
    <Point><coordinates>-123.0,49.0,0</coordinates></Point>
  </Placemark>
</kml>`;

export const INVALID_KML_NO_ROOT = `<?xml version="1.0"?><root><data/></root>`;

export const mockGeoJson: FeatureCollection = {
  type: 'FeatureCollection',
  features: [],
};

/**
 * Builds a valid shapefile ZIP buffer containing the required .shp, .shx, and .dbf entries.
 * The .shp entry is written with the correct magic number (9994, big-endian).
 */
export async function makeFakeShapefileBuffer(baseName = 'test'): Promise<ArrayBuffer> {
  const zip = new JSZip();
  const shpHeader = new ArrayBuffer(8);
  new DataView(shpHeader).setInt32(0, 9994, false);
  zip.file(`${baseName}.shp`, shpHeader);
  zip.file(`${baseName}.shx`, new ArrayBuffer(8));
  zip.file(`${baseName}.dbf`, new ArrayBuffer(8));
  return zip.generateAsync({ type: 'arraybuffer' });
}

/**
 * Builds a ZIP buffer that contains a .shp with an invalid magic number.
 */
export async function makeBadMagicShapefileBuffer(): Promise<ArrayBuffer> {
  const zip = new JSZip();
  const shpHeader = new ArrayBuffer(8);
  new DataView(shpHeader).setInt32(0, 1234, false); // wrong magic
  zip.file('test.shp', shpHeader);
  zip.file('test.shx', new ArrayBuffer(8));
  zip.file('test.dbf', new ArrayBuffer(8));
  return zip.generateAsync({ type: 'arraybuffer' });
}

/**
 * Builds a valid KMZ buffer (ZIP containing a .kml entry).
 */
export async function makeFakeKmzBuffer(kmlContent = VALID_KML): Promise<ArrayBuffer> {
  const zip = new JSZip();
  zip.file('doc.kml', kmlContent);
  return zip.generateAsync({ type: 'arraybuffer' });
}

/**
 * Encodes a KML string to an ArrayBuffer (as if the user uploaded a plain .kml file).
 */
export function makeKmlBuffer(kmlContent = VALID_KML): ArrayBuffer {
  return new TextEncoder().encode(kmlContent).buffer;
}
