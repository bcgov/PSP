import { useCallback } from 'react';
import { useHistory } from 'react-router-dom';

import DeepLinkGenerator, { sidebarBasePath } from '@/utils/DeepLinkGenerator';

// Re-exported for backwards compatibility with existing imports.
export { sidebarBasePath };

export interface IPathGeneratorMethods {
  newFile: (fileType: string) => void;
  showFile: (fileType: string, fileId: number) => void;
  showDetails: (fileType: string, fileId: number, detailType: string, replace: boolean) => void;
  showDetail: (
    fileType: string,
    fileId: number,
    detailType: string,
    detailId: number,
    replace: boolean,
  ) => void;
  editDetails: (fileType: string, fileId: number, detailType: string) => void;
  editDetail: (fileType: string, fileId: number, detailType: string, detailId: number) => void;
  addDetail: (fileType: string, fileId: number, detailType: string) => void;
  editProperties: (fileType: string, fileId: number) => void;
  showFilePropertyId: (fileType: string, fileId: number, filePropertyId: number) => void;
  showFilePropertyDetail: (
    fileType: string,
    fileId: number,
    filePropertyId: number,
    detailType: string,
    detailSubType?: string,
    detailId?: number,
    replace?: boolean,
  ) => void;
  addFilePropertyDetail: (
    fileType: string,
    fileId: number,
    filePropertyId: number,
    detailType: string,
    detailSubType?: string,
    replace?: boolean,
  ) => void;
  showPropertyByPid: (pid: string) => void;
  showPropertyDetail: (
    propertyId: number,
    detailType: string,
    detailSubType?: string,
    detailId?: number,
  ) => void;
  addPropertyDetail: (propertyId: number, detailType: string, detailSubType?: string) => void;
}

export type IPathGenerator = () => IPathGeneratorMethods;

const usePathGenerator: IPathGenerator = () => {
  const history = useHistory();

  const newFile = useCallback(
    (fileType: string) => {
      history.push(DeepLinkGenerator.newFile(fileType));
    },
    [history],
  );

  const showFile = useCallback(
    (fileType: string, fileId: number) => {
      history.push(DeepLinkGenerator.showFile(fileType, fileId));
    },
    [history],
  );

  const showDetails = useCallback(
    (fileType: string, fileId: number, detailType: string, replace: boolean) => {
      const path = DeepLinkGenerator.showDetails(fileType, fileId, detailType);
      if (replace) {
        history.replace(path);
      } else {
        history.push(path);
      }
    },
    [history],
  );

  const showDetail = useCallback(
    (fileType: string, fileId: number, detailType: string, detailId: number, replace: boolean) => {
      const path = DeepLinkGenerator.showDetail(fileType, fileId, detailType, detailId);
      if (replace) {
        history.replace(path);
      } else {
        history.push(path);
      }
    },
    [history],
  );

  const editDetails = useCallback(
    (fileType: string, fileId: number, detailType: string) => {
      history.push(DeepLinkGenerator.editDetails(fileType, fileId, detailType));
    },
    [history],
  );

  const editDetail = useCallback(
    (fileType: string, fileId: number, detailType: string, detailId: number) => {
      history.push(DeepLinkGenerator.editDetail(fileType, fileId, detailType, detailId));
    },
    [history],
  );

  const addDetail = useCallback(
    (fileType: string, fileId: number, detailType: string) => {
      history.push(DeepLinkGenerator.addDetail(fileType, fileId, detailType));
    },
    [history],
  );

  const editProperties = useCallback(
    (fileType: string, fileId: number) => {
      history.push(DeepLinkGenerator.editProperties(fileType, fileId));
    },
    [history],
  );

  const showFilePropertyId = useCallback(
    (fileType: string, fileId: number, menuIndex: number) => {
      history.push(DeepLinkGenerator.showFilePropertyId(fileType, fileId, menuIndex));
    },
    [history],
  );

  const showFilePropertyDetail = useCallback(
    (
      fileType: string,
      fileId: number,
      filePropertyId: number,
      detailType: string,
      detailSubType?: string,
      detailId?: number,
      replace?: boolean,
    ) => {
      const path = DeepLinkGenerator.showFilePropertyDetail(
        fileType,
        fileId,
        filePropertyId,
        detailType,
        detailSubType,
        detailId,
      );
      if (replace === true) {
        history.replace(path);
      } else {
        history.push(path);
      }
    },
    [history],
  );

  const addFilePropertyDetail = useCallback(
    (
      fileType: string,
      fileId: number,
      filePropertyId: number,
      detailType: string,
      detailSubType?: string,
      replace?: boolean,
    ) => {
      const path = DeepLinkGenerator.addFilePropertyDetail(
        fileType,
        fileId,
        filePropertyId,
        detailType,
        detailSubType,
      );
      if (replace === true) {
        history.replace(path);
      } else {
        history.push(path);
      }
    },
    [history],
  );

  const showPropertyByPid = useCallback(
    (pid: string) => {
      history.push(DeepLinkGenerator.showPropertyByPid(pid));
    },
    [history],
  );

  const showPropertyDetail = useCallback(
    (propertyId: number, detailType: string, detailSubType?: string, detailId?: number) => {
      history.push(
        DeepLinkGenerator.showPropertyDetail(propertyId, detailType, detailSubType, detailId),
      );
    },
    [history],
  );

  const addPropertyDetail = useCallback(
    (propertyId: number, detailType: string, detailSubType?: string) => {
      history.push(DeepLinkGenerator.addPropertyDetail(propertyId, detailType, detailSubType));
    },
    [history],
  );

  return {
    newFile,
    showFile,
    showDetail,
    showDetails,
    editDetail,
    editDetails,
    addDetail,
    editProperties,
    showFilePropertyId,
    showFilePropertyDetail,
    addFilePropertyDetail,
    showPropertyByPid,
    showPropertyDetail,
    addPropertyDetail,
  };
};

export default usePathGenerator;
