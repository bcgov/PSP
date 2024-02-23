import { ENVIRONMENT } from '@/constants/environment';
import CustomAxios, { defaultEnvelope, LifecycleToasts } from '@/customAxios';
import useDeepCompareMemo from '@/hooks/util/useDeepCompareMemo';
import { RootState } from '@/store/store';

/**
 * Common hook to make requests to the PIMS APi.
 * @returns CustomAxios object setup for the PIMS API.
 */
export const useAxiosApi = (
  axiosOptions: {
    lifecycleToasts?: LifecycleToasts;
    // eslint-disable-next-line @typescript-eslint/ban-types
    selector?: (state: RootState) => RootState;
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
