import JSZip from 'jszip';

import { loadZipSafely } from './ZipSafetyValidator';

async function makeZipWithEntries(count: number, entrySize = 8): Promise<ArrayBuffer> {
  const zip = new JSZip();
  for (let i = 0; i < count; i++) {
    zip.file(`file${i}.txt`, new ArrayBuffer(entrySize));
  }
  return zip.generateAsync({ type: 'arraybuffer' });
}

/**
 * Builds a small real ZIP then patches _data.uncompressedSize on each file entry
 * so the validator sees the desired size without allocating real memory.
 */
async function makeZipWithFakeUncompressedSize(uncompressedSize: number): Promise<ArrayBuffer> {
  const zip = new JSZip();
  zip.file('file.bin', new ArrayBuffer(8));
  const buffer = await zip.generateAsync({ type: 'arraybuffer' });

  // Patch JSZip.loadAsync to return a zip whose entry reports the fake uncompressed size
  const realLoadAsync: typeof JSZip.loadAsync = JSZip.loadAsync.bind(JSZip);

  vi.spyOn(JSZip, 'loadAsync').mockImplementationOnce(async (...args) => {
    const loaded = await realLoadAsync(...args);
    Object.values(loaded.files)
      .filter(f => !f.dir)
      .forEach(f => ((f as any)._data = { uncompressedSize }));
    return loaded;
  });

  return buffer;
}

describe('loadZipSafely', () => {
  it('loads a normal ZIP without throwing', async () => {
    const buffer = await makeZipWithEntries(3);
    await expect(loadZipSafely(buffer)).resolves.toBeDefined();
  });

  it('throws when entry count exceeds the maximum', async () => {
    const buffer = await makeZipWithEntries(101);
    await expect(loadZipSafely(buffer)).rejects.toThrow(
      'Zip archive rejected: contains 101 entries',
    );
  });

  it('accepts an archive exactly at the entry limit', async () => {
    const buffer = await makeZipWithEntries(100);
    await expect(loadZipSafely(buffer)).resolves.toBeDefined();
  });

  it('throws when uncompressed size exceeds the 50MB limit', async () => {
    const buffer = await makeZipWithFakeUncompressedSize(51 * 1024 * 1024);
    await expect(loadZipSafely(buffer)).rejects.toThrow(
      'Zip archive rejected: uncompressed size exceeds the 50MB limit.',
    );
  });

  it('throws when compression ratio exceeds the maximum', async () => {
    const zip = new JSZip();
    // Highly compressible data: a long repeated string compresses far beyond 3:1
    const highlyCompressible = 'A'.repeat(1024 * 1024);
    zip.file('compressible.txt', highlyCompressible);
    const buffer = await zip.generateAsync({ type: 'arraybuffer', compression: 'DEFLATE' });
    await expect(loadZipSafely(buffer)).rejects.toThrow('Zip archive rejected: compression ratio');
  });

  it('ignores directory entries when counting files', async () => {
    const zip = new JSZip();
    zip.folder('subdir');
    zip.file('subdir/file.txt', new ArrayBuffer(8));
    const buffer = await zip.generateAsync({ type: 'arraybuffer' });
    await expect(loadZipSafely(buffer)).resolves.toBeDefined();
  });
});
