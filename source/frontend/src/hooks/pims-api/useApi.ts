import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios, { defaultEnvelope, LifecycleToasts } from '@/customAxios';
import useDeepCompareMemo from '@/hooks/util/useDeepCompareMemo';

/**
 * Common hook to make requests to the PIMS APi.
 * @returns CustomAxios object setup for the PIMS API.
 */
export const useAxiosApi = (
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

export default useAxiosApi;
