import { FeatureCollection } from 'geojson';
import JSZip from 'jszip';
import shp from 'shpjs';

import { KmzHelper, ShapefileHelper } from './helpers';
import { ShapeUploadModel, UploadResponseModel } from './models';

vi.mock('shpjs');

// ── Helpers ───────────────────────────────────────────────────────────────────

/**
 * Builds a valid shapefile ZIP buffer containing the required .shp, .shx, and .dbf entries.
 * The .shp entry is written with the correct magic number (9994, big-endian).
 */
async function makeFakeShapefileBuffer(baseName = 'test'): Promise<ArrayBuffer> {
  const zip = new JSZip();

  // .shp with valid magic number (9994 big-endian = 0x0000270A)
  const shpHeader = new ArrayBuffer(8);
  new DataView(shpHeader).setInt32(0, 9994, false);
  zip.file(`${baseName}.shp`, shpHeader);
  zip.file(`${baseName}.shx`, new ArrayBuffer(8));
  zip.file(`${baseName}.dbf`, new ArrayBuffer(8));

  const blob = await zip.generateAsync({ type: 'arraybuffer' });
  return blob;
}

/**
 * Builds a ZIP buffer that contains a .shp with an invalid magic number.
 */
async function makeBadMagicShapefileBuffer(): Promise<ArrayBuffer> {
  const zip = new JSZip();
  const shpHeader = new ArrayBuffer(8);
  new DataView(shpHeader).setInt32(0, 1234, false); // wrong magic
  zip.file('test.shp', shpHeader);
  zip.file('test.shx', new ArrayBuffer(8));
  zip.file('test.dbf', new ArrayBuffer(8));
  return zip.generateAsync({ type: 'arraybuffer' });
}

const VALID_KML = `<?xml version="1.0" encoding="UTF-8"?>
<kml xmlns="http://www.opengis.net/kml/2.2">
  <Placemark>
    <name>Test</name>
    <Point><coordinates>-123.0,49.0,0</coordinates></Point>
  </Placemark>
</kml>`;

const INVALID_KML_NO_ROOT = `<?xml version="1.0"?><root><data/></root>`;

/**
 * Builds a valid KMZ buffer (ZIP containing a .kml entry).
 */
async function makeFakeKmzBuffer(kmlContent = VALID_KML): Promise<ArrayBuffer> {
  const zip = new JSZip();
  zip.file('doc.kml', kmlContent);
  return zip.generateAsync({ type: 'arraybuffer' });
}

/**
 * Encodes a KML string to an ArrayBuffer (as if the user uploaded a plain .kml file).
 */
function makeKmlBuffer(kmlContent = VALID_KML): ArrayBuffer {
  return new TextEncoder().encode(kmlContent).buffer;
}

const mockGeoJson: FeatureCollection = {
  type: 'FeatureCollection',
  features: [],
};

// ── ShapefileHelper ───────────────────────────────────────────────────────────

describe('ShapefileHelper', () => {
  describe('isShapefile', () => {
    it('returns true for a valid shapefile ZIP', async () => {
      const buffer = await makeFakeShapefileBuffer();
      expect(await ShapefileHelper.isShapefile(buffer)).toBe(true);
    });

    it('returns false when ZIP has no .shp entry', async () => {
      const zip = new JSZip();
      zip.file('test.dbf', new ArrayBuffer(8));
      zip.file('test.shx', new ArrayBuffer(8));
      const buffer = await zip.generateAsync({ type: 'arraybuffer' });
      expect(await ShapefileHelper.isShapefile(buffer)).toBe(false);
    });

    it('returns false when .shp has wrong magic number', async () => {
      const buffer = await makeBadMagicShapefileBuffer();
      expect(await ShapefileHelper.isShapefile(buffer)).toBe(false);
    });

    it('returns false when .shx is missing', async () => {
      const zip = new JSZip();
      const shpHeader = new ArrayBuffer(8);
      new DataView(shpHeader).setInt32(0, 9994, false);
      zip.file('test.shp', shpHeader);
      zip.file('test.dbf', new ArrayBuffer(8));
      const buffer = await zip.generateAsync({ type: 'arraybuffer' });
      expect(await ShapefileHelper.isShapefile(buffer)).toBe(false);
    });

    it('returns false when .dbf is missing', async () => {
      const zip = new JSZip();
      const shpHeader = new ArrayBuffer(8);
      new DataView(shpHeader).setInt32(0, 9994, false);
      zip.file('test.shp', shpHeader);
      zip.file('test.shx', new ArrayBuffer(8));
      const buffer = await zip.generateAsync({ type: 'arraybuffer' });
      expect(await ShapefileHelper.isShapefile(buffer)).toBe(false);
    });

    it('returns false for a KMZ buffer', async () => {
      const buffer = await makeFakeKmzBuffer();
      expect(await ShapefileHelper.isShapefile(buffer)).toBe(false);
    });
  });

  describe('validate', () => {
    it('resolves for a valid shapefile buffer', async () => {
      const buffer = await makeFakeShapefileBuffer();
      await expect(ShapefileHelper.validate(buffer)).resolves.toBeUndefined();
    });

    it('throws when .shp is missing', async () => {
      const zip = new JSZip();
      zip.file('test.dbf', new ArrayBuffer(8));
      const buffer = await zip.generateAsync({ type: 'arraybuffer' });
      await expect(ShapefileHelper.validate(buffer)).rejects.toThrow(
        'Invalid shapefile: no .shp file found in ZIP.',
      );
    });

    it('throws when magic number is wrong', async () => {
      const buffer = await makeBadMagicShapefileBuffer();
      await expect(ShapefileHelper.validate(buffer)).rejects.toThrow(
        'Invalid shapefile: unexpected .shp header',
      );
    });

    it('throws when .shx companion is missing', async () => {
      const zip = new JSZip();
      const shpHeader = new ArrayBuffer(8);
      new DataView(shpHeader).setInt32(0, 9994, false);
      zip.file('test.shp', shpHeader);
      zip.file('test.dbf', new ArrayBuffer(8));
      const buffer = await zip.generateAsync({ type: 'arraybuffer' });
      await expect(ShapefileHelper.validate(buffer)).rejects.toThrow(
        'Invalid shapefile: missing required .shx file.',
      );
    });

    it('throws when .dbf companion is missing', async () => {
      const zip = new JSZip();
      const shpHeader = new ArrayBuffer(8);
      new DataView(shpHeader).setInt32(0, 9994, false);
      zip.file('test.shp', shpHeader);
      zip.file('test.shx', new ArrayBuffer(8));
      const buffer = await zip.generateAsync({ type: 'arraybuffer' });
      await expect(ShapefileHelper.validate(buffer)).rejects.toThrow(
        'Invalid shapefile: missing required .dbf file.',
      );
    });

    it('validates companions case-insensitively', async () => {
      const zip = new JSZip();
      const shpHeader = new ArrayBuffer(8);
      new DataView(shpHeader).setInt32(0, 9994, false);
      zip.file('TEST.SHP', shpHeader);
      zip.file('TEST.SHX', new ArrayBuffer(8));
      zip.file('TEST.DBF', new ArrayBuffer(8));
      const buffer = await zip.generateAsync({ type: 'arraybuffer' });
      await expect(ShapefileHelper.validate(buffer)).resolves.toBeUndefined();
    });
  });

  describe('toGeoJson', () => {
    beforeEach(() => {
      vi.mocked(shp).mockReset();
    });

    it('returns parsed GeoJSON for a valid shapefile buffer', async () => {
      vi.mocked(shp).mockResolvedValue(mockGeoJson);
      const buffer = await makeFakeShapefileBuffer();
      const result = await ShapefileHelper.toGeoJson(buffer);
      expect(result).toEqual(mockGeoJson);
    });

    it('unwraps first element when shpjs returns an array', async () => {
      const secondGeoJson: FeatureCollection = { type: 'FeatureCollection', features: [] };
      vi.mocked(shp).mockResolvedValue([mockGeoJson, secondGeoJson]);
      const buffer = await makeFakeShapefileBuffer();
      const result = await ShapefileHelper.toGeoJson(buffer);
      expect(result).toEqual(mockGeoJson);
    });

    it('throws a friendly error when shpjs fails', async () => {
      vi.mocked(shp).mockRejectedValue(new Error('parse error'));
      const buffer = await makeFakeShapefileBuffer();
      await expect(ShapefileHelper.toGeoJson(buffer)).rejects.toThrow('Failed to parse shapefile.');
    });
  });
});

// ── KmzHelper ─────────────────────────────────────────────────────────────────

describe('KmzHelper', () => {
  describe('isKmz', () => {
    it('returns true for a valid KMZ buffer', async () => {
      const buffer = await makeFakeKmzBuffer();
      expect(await KmzHelper.isKmz(buffer)).toBe(true);
    });

    it('returns false when ZIP has no .kml entry', async () => {
      const zip = new JSZip();
      zip.file('other.txt', 'data');
      const buffer = await zip.generateAsync({ type: 'arraybuffer' });
      expect(await KmzHelper.isKmz(buffer)).toBe(false);
    });

    it('returns false when .kml content is not valid XML', async () => {
      const buffer = await makeFakeKmzBuffer('not xml');
      expect(await KmzHelper.isKmz(buffer)).toBe(false);
    });

    it('returns false when .kml has no <kml> root element', async () => {
      const buffer = await makeFakeKmzBuffer(INVALID_KML_NO_ROOT);
      expect(await KmzHelper.isKmz(buffer)).toBe(false);
    });

    it('returns false for a plain shapefile ZIP', async () => {
      const buffer = await makeFakeShapefileBuffer();
      expect(await KmzHelper.isKmz(buffer)).toBe(false);
    });
  });

  describe('isKml', () => {
    it('returns true for a valid KML buffer', async () => {
      const buffer = makeKmlBuffer();
      expect(await KmzHelper.isKml(buffer)).toBe(true);
    });

    it('returns false for KML content without <kml> root', async () => {
      const buffer = makeKmlBuffer(INVALID_KML_NO_ROOT);
      expect(await KmzHelper.isKml(buffer)).toBe(false);
    });

    it('returns false for a non-XML buffer', async () => {
      const buffer = new TextEncoder().encode('not xml at all').buffer;
      expect(await KmzHelper.isKml(buffer)).toBe(false);
    });

    it('returns false for a KMZ (ZIP) buffer', async () => {
      const buffer = await makeFakeKmzBuffer();
      // A KMZ is a ZIP, so decoding as text won't produce valid KML XML
      expect(await KmzHelper.isKml(buffer)).toBe(false);
    });
  });

  describe('validate (KMZ)', () => {
    it('resolves for a valid KMZ buffer', async () => {
      const buffer = await makeFakeKmzBuffer();
      await expect(KmzHelper.validateKmz(buffer)).resolves.toBeUndefined();
    });

    it('throws when ZIP has no .kml entry', async () => {
      const zip = new JSZip();
      zip.file('other.txt', 'data');
      const buffer = await zip.generateAsync({ type: 'arraybuffer' });
      await expect(KmzHelper.validateKmz(buffer)).rejects.toThrow(
        'Invalid KMZ: no .kml file found in ZIP.',
      );
    });

    it('throws when .kml XML has a parse error', async () => {
      const buffer = await makeFakeKmzBuffer('not xml');
      await expect(KmzHelper.validateKmz(buffer)).rejects.toThrow(
        'Invalid KML: file could not be parsed as XML.',
      );
    });

    it('throws when .kml has no <kml> root', async () => {
      const buffer = await makeFakeKmzBuffer(INVALID_KML_NO_ROOT);
      await expect(KmzHelper.validateKmz(buffer)).rejects.toThrow(
        'Invalid KML: file does not contain a <kml> root element.',
      );
    });
  });

  describe('validateKml', () => {
    it('resolves for a valid KML buffer', async () => {
      const buffer = makeKmlBuffer();
      await expect(KmzHelper.validateKml(buffer)).resolves.toBeUndefined();
    });

    it('throws when KML has no <kml> root element', async () => {
      const buffer = makeKmlBuffer(INVALID_KML_NO_ROOT);
      await expect(KmzHelper.validateKml(buffer)).rejects.toThrow(
        'Invalid KML: file does not contain a <kml> root element.',
      );
    });

    it('throws when content is not valid XML', async () => {
      const buffer = new TextEncoder().encode('bad').buffer;
      await expect(KmzHelper.validateKml(buffer)).rejects.toThrow(
        'Invalid KML: file could not be parsed as XML.',
      );
    });
  });

  describe('toGeoJson', () => {
    it('parses a valid KML buffer', async () => {
      const buffer = makeKmlBuffer();
      const result = await KmzHelper.toGeoJson(buffer);
      expect(result.type).toBe('FeatureCollection');
      expect(Array.isArray(result.features)).toBe(true);
    });

    it('parses a valid KMZ buffer', async () => {
      const buffer = await makeFakeKmzBuffer();
      const result = await KmzHelper.toGeoJson(buffer);
      expect(result.type).toBe('FeatureCollection');
    });

    it('KML takes priority over KMZ detection', async () => {
      // A plain KML buffer should resolve via the KML path, not fall through to KMZ
      const buffer = makeKmlBuffer();
      const isKmlSpy = vi.spyOn(KmzHelper, 'isKml').mockResolvedValueOnce(true);
      const isKmzSpy = vi.spyOn(KmzHelper, 'isKmz');
      await KmzHelper.toGeoJson(buffer);
      expect(isKmlSpy).toHaveBeenCalledWith(buffer);
      expect(isKmzSpy).not.toHaveBeenCalled();
      isKmlSpy.mockRestore();
      isKmzSpy.mockRestore();
    });

    it('throws when buffer is neither valid KML nor KMZ', async () => {
      const buffer = new TextEncoder().encode('not a geo file').buffer;
      await expect(KmzHelper.toGeoJson(buffer)).rejects.toThrow(
        'Failed to parse file. Please ensure the file is a valid KML or KMZ file.',
      );
    });
  });
});

// ── ShapeUploadModel ──────────────────────────────────────────────────────────

describe('ShapeUploadModel', () => {
  let model: ShapeUploadModel;

  beforeEach(() => {
    model = new ShapeUploadModel();
    vi.clearAllMocks();
  });

  describe('toGeoJson', () => {
    it('throws when no file is set', async () => {
      await expect(model.toGeoJson()).rejects.toThrow('No file provided');
    });

    it('delegates to ShapefileHelper when isShapefile returns true', async () => {
      const buffer = await makeFakeShapefileBuffer();
      const file = new File([buffer], 'test.zip');
      model.file = file;

      vi.spyOn(ShapefileHelper, 'isShapefile').mockResolvedValue(true);
      const shapefileSpy = vi.spyOn(ShapefileHelper, 'toGeoJson').mockResolvedValue(mockGeoJson);

      const result = await model.toGeoJson();
      expect(shapefileSpy).toHaveBeenCalled();
      expect(result).toEqual(mockGeoJson);
    });

    it('delegates to KmzHelper when isKmz returns true', async () => {
      const buffer = await makeFakeKmzBuffer();
      const file = new File([buffer], 'test.kmz');
      model.file = file;

      vi.spyOn(ShapefileHelper, 'isShapefile').mockResolvedValue(false);
      vi.spyOn(KmzHelper, 'isKml').mockResolvedValue(false);
      vi.spyOn(KmzHelper, 'isKmz').mockResolvedValue(true);
      const kmzSpy = vi.spyOn(KmzHelper, 'toGeoJson').mockResolvedValue(mockGeoJson);

      const result = await model.toGeoJson();
      expect(kmzSpy).toHaveBeenCalled();
      expect(result).toEqual(mockGeoJson);
    });

    it('delegates to KmzHelper when isKml returns true', async () => {
      const buffer = makeKmlBuffer();
      const file = new File([buffer], 'test.kml');
      model.file = file;

      vi.spyOn(ShapefileHelper, 'isShapefile').mockResolvedValue(false);
      vi.spyOn(KmzHelper, 'isKml').mockResolvedValue(true);
      const kmzSpy = vi.spyOn(KmzHelper, 'toGeoJson').mockResolvedValue(mockGeoJson);

      const result = await model.toGeoJson();
      expect(kmzSpy).toHaveBeenCalled();
      expect(result).toEqual(mockGeoJson);
    });

    it('throws when file matches neither shapefile nor KML/KMZ', async () => {
      const file = new File([new ArrayBuffer(16)], 'test.zip');
      model.file = file;

      vi.spyOn(ShapefileHelper, 'isShapefile').mockResolvedValue(false);
      vi.spyOn(KmzHelper, 'isKml').mockResolvedValue(false);
      vi.spyOn(KmzHelper, 'isKmz').mockResolvedValue(false);

      await expect(model.toGeoJson()).rejects.toThrow('Unsupported file format');
    });

    it('reads the file buffer exactly once and passes it to helpers', async () => {
      const buffer = await makeFakeShapefileBuffer();
      const file = new File([buffer], 'test.zip');
      model.file = file;

      const arrayBufferSpy = vi.spyOn(file, 'arrayBuffer');
      vi.spyOn(ShapefileHelper, 'isShapefile').mockResolvedValue(true);
      vi.spyOn(ShapefileHelper, 'toGeoJson').mockResolvedValue(mockGeoJson);

      await model.toGeoJson();
      expect(arrayBufferSpy).toHaveBeenCalledTimes(1);
    });
  });
});

// ── UploadResponseModel ───────────────────────────────────────────────────────

describe('UploadResponseModel', () => {
  it('initialises with correct defaults', () => {
    const model = new UploadResponseModel('test.zip');
    expect(model.fileName).toBe('test.zip');
    expect(model.isSuccess).toBe(false);
    expect(model.errorMessage).toBeNull();
    expect(model.boundary).toBeNull();
  });
});
