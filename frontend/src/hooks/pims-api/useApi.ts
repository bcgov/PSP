import useDeepCompareMemo from 'hooks/useDeepCompareMemo';
import { ENVIRONMENT } from 'constants/environment';
import CustomAxios, { defaultEnvelope, LifecycleToasts } from 'customAxios';

/**
 * Common hook to make requests to the PIMS APi.
 * @returns CustomAxios object setup for the PIMS API.
 */
export const useApi = (
  axiosOptions: {
    lifecycleToasts?: LifecycleToasts;
    selector?: Function;
    envelope?: typeof defaultEnvelope;
    baseURL?: string;
  } = {},
) => {
  return useDeepCompareMemo(
    () => CustomAxios({ ...axiosOptions, baseURL: axiosOptions.baseURL ?? ENVIRONMENT.apiUrl }),
    [axiosOptions],
  );
};

export default useApi;
