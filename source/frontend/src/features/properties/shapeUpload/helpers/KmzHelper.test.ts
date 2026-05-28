import {
  INVALID_KML_NO_ROOT,
  makeFakeKmzBuffer,
  makeFakeShapefileBuffer,
  makeKmlBuffer,
} from '../models.fixtures';
import { KmzHelper } from './KmzHelper';

describe('KmzHelper', () => {
  // @xmldom/xmldom calls console.error internally when it encounters invalid XML,
  // before populating the parsererror element. Suppress it globally for this suite
  // so vitest-fail-on-console does not fail tests that intentionally pass bad XML.
  beforeEach(() => {
    vi.spyOn(console, 'error').mockImplementation(() => {});
    vi.spyOn(console, 'warn').mockImplementation(() => {});
  });

  describe('isKmz', () => {
    it('returns true for a valid KMZ buffer', async () => {
      const buffer = await makeFakeKmzBuffer();
      expect(await KmzHelper.isKmz(buffer)).toBe(true);
    });

    it('returns false when ZIP has no .kml entry', async () => {
      const { default: JSZip } = await import('jszip');
      const zip = new JSZip();
      zip.file('other.txt', 'data');
      const buffer = await zip.generateAsync({ type: 'arraybuffer' });
      expect(await KmzHelper.isKmz(buffer)).toBe(false);
    });

    it('returns false when .kml content is not valid XML', async () => {
      const buffer = await makeFakeKmzBuffer('<<<not xml>>>');
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
      const buffer = new TextEncoder().encode('not xml at all <<<').buffer;
      expect(await KmzHelper.isKml(buffer)).toBe(false);
    });

    it('returns false for a shapefile ZIP buffer', async () => {
      const buffer = await makeFakeShapefileBuffer();
      expect(await KmzHelper.isKml(buffer)).toBe(false);
    });
  });

  describe('validate (KMZ)', () => {
    it('resolves for a valid KMZ buffer', async () => {
      const buffer = await makeFakeKmzBuffer();
      await expect(KmzHelper.validateKmz(buffer)).resolves.toBeUndefined();
    });

    it('throws when ZIP has no .kml entry', async () => {
      const { default: JSZip } = await import('jszip');
      const zip = new JSZip();
      zip.file('other.txt', 'data');
      const buffer = await zip.generateAsync({ type: 'arraybuffer' });
      await expect(KmzHelper.validateKmz(buffer)).rejects.toThrow(
        'Invalid KMZ: no .kml file found in ZIP.',
      );
    });

    it('throws when .kml XML has a parse error', async () => {
      const buffer = await makeFakeKmzBuffer('<<<not xml>>>');
      await expect(KmzHelper.validateKmz(buffer)).rejects.toThrow(
        'Invalid KML: file does not contain a <kml> root element.',
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
      const buffer = new TextEncoder().encode('<<<bad>>>').buffer;
      await expect(KmzHelper.validateKml(buffer)).rejects.toThrow(
        'Invalid KML: file does not contain a <kml> root element.',
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

    it('KMZ takes priority over KML detection', async () => {
      const buffer = await makeFakeKmzBuffer();
      const isKmlSpy = vi.spyOn(KmzHelper, 'isKml');
      const isKmzSpy = vi.spyOn(KmzHelper, 'isKmz').mockResolvedValueOnce(true);
      await KmzHelper.toGeoJson(buffer);
      expect(isKmzSpy).toHaveBeenCalledWith(buffer);
      expect(isKmlSpy).not.toHaveBeenCalled();
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
