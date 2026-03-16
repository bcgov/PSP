import JSZip from 'jszip';

// Maximum ratio of uncompressed to compressed data (3:1 is typical for legitimate archives)
const MAX_COMPRESSION_RATIO = 3;
// Maximum total uncompressed size: 50MB
const MAX_UNCOMPRESSED_BYTES = 50 * 1024 * 1024;
// Maximum number of entries in the archive
const MAX_FILE_ENTRIES = 100;

/**
 * Loads a ZIP archive from an ArrayBuffer, rejecting it if it shows signs of being a zip bomb:
 * - More than MAX_FILE_ENTRIES entries
 * - Total uncompressed size exceeds MAX_UNCOMPRESSED_BYTES
 * - Compression ratio exceeds MAX_COMPRESSION_RATIO
 *
 * @throws Error if the archive fails any of the zip bomb checks.
 * @see https://en.wikipedia.org/wiki/Zip_bomb for more information.
 */
export async function loadZipSafely(buffer: ArrayBuffer): Promise<JSZip> {
  const zip = await JSZip.loadAsync(buffer);
  const entries = Object.values(zip.files).filter(f => !f.dir);

  if (entries.length > MAX_FILE_ENTRIES) {
    throw new Error(
      `Zip archive rejected: contains ${entries.length} entries (max ${MAX_FILE_ENTRIES}).`,
    );
  }

  const compressedSize = buffer.byteLength;
  let totalUncompressedSize = 0;

  for (const entry of entries) {
    // JSZip exposes _data.uncompressedSize after loading
    const uncompressedSize = (entry as any)._data?.uncompressedSize ?? 0;
    totalUncompressedSize += uncompressedSize;

    if (totalUncompressedSize > MAX_UNCOMPRESSED_BYTES) {
      throw new Error(
        `Zip archive rejected: uncompressed size exceeds the ${
          MAX_UNCOMPRESSED_BYTES / 1024 / 1024
        }MB limit.`,
      );
    }
  }

  const compressionRatio = compressedSize > 0 ? totalUncompressedSize / compressedSize : 0;
  if (compressedSize > 0 && compressionRatio > MAX_COMPRESSION_RATIO) {
    throw new Error(
      `Zip archive rejected: compression ratio ${compressionRatio.toFixed(
        1,
      )} exceeds the maximum allowed ratio of ${MAX_COMPRESSION_RATIO}.`,
    );
  }

  return zip;
}
