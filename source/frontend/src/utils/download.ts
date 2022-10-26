import { AxiosRequestConfig } from 'axios';
import CustomAxios from 'customAxios';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { AnyAction } from 'redux';
import { ThunkAction } from 'redux-thunk';
import { logError, logRequest, logSuccess } from 'store/slices/network/networkSlice';
import { RootState } from 'store/store';

/**
 * Download configuration options interface.
 */
export interface IDownloadConfig extends AxiosRequestConfig {
  fileName: string;
  actionType: string;
}

/**
 * Programmatically triggers a file download with content generated through an API
 * @param filename the file name
 * @param blobData Raw blob data as returned by a file export API
 */
export const downloadFile = (filename: string, blobData: any) => {
  const uri = window.URL.createObjectURL(new Blob([blobData]));
  const link = document.createElement('a');
  link.href = uri;
  link.setAttribute('download', filename ?? new Date().toDateString());
  document.body.appendChild(link);
  link.click();
  // release the object URL after the link has been clicked
  document.body.removeChild(link);
  window.URL.revokeObjectURL(link.href);
};

/**
 * Make an AJAX request to download content from the specified endpoint.
 * @param config - Configuration options to make an AJAX request to download content.
 * @param config.url - The url to the endpoint.
 * @param config.actionType - The action type name to identify the request in the redux store.
 * @param config.method - The HTTP method to use.
 * @param config.headers - The HTTP request headers to include.  By default it will include the JWT bearer token.
 * @param config.fileName - The file name you want to save the download as.  By default it will use the current date.
 */
const download =
  (config: IDownloadConfig): ThunkAction<Promise<void>, RootState, unknown, AnyAction> =>
  dispatch => {
    const options = { ...config, headers: { ...config.headers } };
    dispatch(logRequest(options.actionType));
    dispatch(showLoading());
    return CustomAxios()
      .request<BlobPart>({
        url: options.url,
        headers: options.headers,
        method: options.method ?? 'get',
        responseType: options.responseType ?? 'blob',
        data: options.data,
      })
      .then(response => {
        dispatch(logSuccess({ name: options.actionType }));

        const uri = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = uri;
        link.setAttribute('download', options.fileName ?? new Date().toDateString());
        document.body.appendChild(link);
        link.click();
      })
      .catch(axiosError => {
        dispatch(
          logError({
            name: options.actionType,
            status: axiosError?.response?.status,
            error: axiosError,
          }),
        );
      })
      .finally(() => {
        dispatch(hideLoading());
      });
  };

export default download;
