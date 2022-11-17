export const MAX_RETRIES = 2;

export const fetchWithRetryTimeout = async (
  options: Partial<Request> & { maxRetries?: number; timeout?: number; onError?: () => void },
  retryCount = 0,
): Promise<Response> => {
  const controller = new AbortController();
  const { maxRetries = MAX_RETRIES, url, ...remainingOptions } = options;
  try {
    const promise = fetch(url ?? '', { ...remainingOptions, signal: controller.signal });
    const timeout = setTimeout(() => {
      controller.abort();
    }, options.timeout ?? 5000);
    return await promise.finally(() => clearTimeout(timeout));
  } catch (error) {
    // if the retryCount has not been exceeded, call again
    if (retryCount < maxRetries) {
      return fetchWithRetryTimeout(options, retryCount + 1);
    } else {
      !!options?.onError && options.onError();
    }
    // max retries exceeded
    throw error;
  }
};
