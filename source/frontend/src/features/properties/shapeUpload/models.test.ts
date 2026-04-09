import { getMockFile } from '@/utils/test-utils';

import { KmzHelper } from './helpers/KmzHelper';
import { ShapefileHelper } from './helpers/ShapefileHelper';
import { ShapeUploadModel, UploadResponseModel } from './models';
import {
  makeFakeKmzBuffer,
  makeFakeShapefileBuffer,
  makeKmlBuffer,
  mockGeoJson,
} from './models.fixtures';

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
      model.file = getMockFile(buffer, 'test.zip');

      vi.spyOn(ShapefileHelper, 'isShapefile').mockResolvedValue(true);
      const shapefileSpy = vi.spyOn(ShapefileHelper, 'toGeoJson').mockResolvedValue(mockGeoJson);

      const result = await model.toGeoJson();
      expect(shapefileSpy).toHaveBeenCalled();
      expect(result).toEqual(mockGeoJson);
    });

    it('delegates to KmzHelper when isKmz returns true', async () => {
      const buffer = await makeFakeKmzBuffer();
      model.file = getMockFile(buffer, 'test.kmz');

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
      model.file = getMockFile(buffer, 'test.kml');

      vi.spyOn(ShapefileHelper, 'isShapefile').mockResolvedValue(false);
      vi.spyOn(KmzHelper, 'isKml').mockResolvedValue(true);
      const kmzSpy = vi.spyOn(KmzHelper, 'toGeoJson').mockResolvedValue(mockGeoJson);

      const result = await model.toGeoJson();
      expect(kmzSpy).toHaveBeenCalled();
      expect(result).toEqual(mockGeoJson);
    });

    it('throws when file matches neither shapefile nor KML/KMZ', async () => {
      model.file = getMockFile(new ArrayBuffer(16), 'test.zip');

      vi.spyOn(ShapefileHelper, 'isShapefile').mockResolvedValue(false);
      vi.spyOn(KmzHelper, 'isKml').mockResolvedValue(false);
      vi.spyOn(KmzHelper, 'isKmz').mockResolvedValue(false);

      await expect(model.toGeoJson()).rejects.toThrow('Unsupported file format');
    });

    it('reads the file buffer exactly once and passes it to helpers', async () => {
      const buffer = await makeFakeShapefileBuffer();
      const file = getMockFile(buffer, 'test.zip');
      model.file = file;

      vi.spyOn(ShapefileHelper, 'isShapefile').mockResolvedValue(true);
      vi.spyOn(ShapefileHelper, 'toGeoJson').mockResolvedValue(mockGeoJson);

      await model.toGeoJson();
      expect(file.arrayBuffer).toHaveBeenCalledTimes(1);
    });
  });
});

describe('UploadResponseModel', () => {
  it('initialises with correct defaults', () => {
    const model = new UploadResponseModel('test.zip');
    expect(model.fileName).toBe('test.zip');
    expect(model.isSuccess).toBe(false);
    expect(model.errorMessage).toBeNull();
    expect(model.boundary).toBeNull();
  });
});
