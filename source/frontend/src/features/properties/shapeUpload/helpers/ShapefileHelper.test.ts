import { FeatureCollection } from 'geojson';
import JSZip from 'jszip';
import shp from 'shpjs';

import {
  makeBadMagicShapefileBuffer,
  makeFakeKmzBuffer,
  makeFakeShapefileBuffer,
  mockGeoJson,
} from '../models.fixtures';
import { ShapefileHelper } from './ShapefileHelper';

vi.mock('shpjs');

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
