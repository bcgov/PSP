import { kml } from '@tmcw/togeojson';
import { DOMParser } from '@xmldom/xmldom';
import { FeatureCollection } from 'geojson';
import JSZip from 'jszip';

import { exists } from '@/utils';

export class KmzHelper {
  public static async isKmz(buffer: ArrayBuffer): Promise<boolean> {
    try {
      await KmzHelper.validateKmz(buffer);
      return true;
    } catch {
      return false;
    }
  }

  public static async isKml(buffer: ArrayBuffer): Promise<boolean> {
    try {
      await KmzHelper.validateKml(buffer);
      return true;
    } catch {
      return false;
    }
  }

  /**
   * Finds a .kml entry in the ZIP file and validates it can be parsed as XML and contains a <kml> root element.
   * @param buffer The ArrayBuffer of the KMZ file to validate.
   */
  public static async validateKmz(buffer: ArrayBuffer): Promise<void> {
    const zip = await JSZip.loadAsync(buffer);
    const files = zip.files;

    const kmlEntry = Object.values(files).find(f => f.name.toLowerCase().endsWith('.kml'));
    if (!exists(kmlEntry)) {
      throw new Error('Invalid KMZ: no .kml file found in ZIP.');
    }

    const kmlText = await kmlEntry.async('text');
    KmzHelper.parseAndValidateKmlText(kmlText);
  }

  /**
   * Validates a KML file represented as an ArrayBuffer.
   * @param buffer The ArrayBuffer of the KML file to validate.
   */
  public static async validateKml(buffer: ArrayBuffer): Promise<void> {
    const kmlText = new TextDecoder().decode(buffer);
    KmzHelper.parseAndValidateKmlText(kmlText);
  }

  /**
   * Converts a KML or KMZ file represented as an ArrayBuffer to a GeoJSON FeatureCollection.
   * @param buffer The ArrayBuffer of the KML or KMZ file to convert.
   * @returns A Promise resolving to the GeoJSON FeatureCollection.
   */
  public static async toGeoJson(buffer: ArrayBuffer): Promise<FeatureCollection> {
    if (await KmzHelper.isKml(buffer)) {
      return KmzHelper.kmlToGeoJson(new TextDecoder().decode(buffer));
    }
    if (await KmzHelper.isKmz(buffer)) {
      return KmzHelper.kmzToGeoJson(buffer);
    }
    throw new Error('Failed to parse file. Please ensure the file is a valid KML or KMZ file.');
  }

  private static parseAndValidateKmlText(kmlText: string): void {
    const doc = new DOMParser().parseFromString(kmlText, 'text/xml');

    if (doc.getElementsByTagName('parsererror').length > 0) {
      throw new Error('Invalid KML: file could not be parsed as XML.');
    }
    if (doc.getElementsByTagName('kml').length === 0) {
      throw new Error('Invalid KML: file does not contain a <kml> root element.');
    }
  }

  private static kmlToGeoJson(kmlText: string): FeatureCollection {
    try {
      const doc = new DOMParser().parseFromString(kmlText, 'text/xml');
      return kml(doc) as FeatureCollection;
    } catch {
      throw new Error('Failed to parse KML. Please ensure the file is a valid KML file.');
    }
  }

  private static async kmzToGeoJson(buffer: ArrayBuffer): Promise<FeatureCollection> {
    try {
      const zip = await JSZip.loadAsync(buffer);
      const kmlEntry = Object.values(zip.files).find(f => f.name.toLowerCase().endsWith('.kml'));
      const kmlText = await kmlEntry.async('text');
      return KmzHelper.kmlToGeoJson(kmlText);
    } catch {
      throw new Error('Failed to parse KMZ. Please ensure the file is a valid KMZ file.');
    }
  }
}
