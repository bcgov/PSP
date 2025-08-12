import { useCallback } from 'react';
import { generatePath, useHistory } from 'react-router-dom';

export const sidebarBasePath = '/mapview/sidebar';

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
    replace: boolean,
  ) => void;
  showPropertyByPid: (pid: string) => void;
}

export type IPathGenerator = () => IPathGeneratorMethods;

const usePathGenerator: IPathGenerator = () => {
  const history = useHistory();

  const newFile = useCallback(
    (fileType: string) => {
      const a = `${sidebarBasePath}/:fileType/new`;
      const path = generatePath(a, {
        fileType,
      });

      history.push(path);
    },
    [history],
  );

  const showFile = useCallback(
    (fileType: string, fileId: number) => {
      const a = `${sidebarBasePath}/:fileType/:fileId`;
      const path = generatePath(a, {
        fileType,
        fileId,
      });

      history.push(path);
    },
    [history],
  );

  const showDetails = useCallback(
    (fileType: string, fileId: number, detailType: string, replace: boolean) => {
      const a = `${sidebarBasePath}/:fileType/:fileId/:detailType`;
      const path = generatePath(a, {
        fileType,
        fileId,
        detailType,
      });

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
      const a = `${sidebarBasePath}/:fileType/:fileId/:detailType/:detailId`;
      const path = generatePath(a, {
        fileType,
        fileId,
        detailType,
        detailId,
      });

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
      const a = `${sidebarBasePath}/:fileType/:fileId/:detailType/edit`;
      const path = generatePath(a, {
        fileType,
        fileId,
        detailType,
      });

      history.push(path);
    },
    [history],
  );

  const editDetail = useCallback(
    (fileType: string, fileId: number, detailType: string, detailId: number) => {
      const a = `${sidebarBasePath}/:fileType/:fileId/:detailType/:detailId/edit`;
      const path = generatePath(a, {
        fileType,
        fileId,
        detailType,
        detailId,
      });

      history.push(path);
    },
    [history],
  );

  const addDetail = useCallback(
    (fileType: string, fileId: number, detailType: string) => {
      const a = `${sidebarBasePath}/:fileType/:fileId/:detailType/new`;
      const path = generatePath(a, {
        fileType,
        fileId,
        detailType,
      });

      history.push(path);
    },
    [history],
  );

  const editProperties = useCallback(
    (fileType: string, fileId: number) => {
      const a = `${sidebarBasePath}/:fileType/:fileId/property/selector`;
      const path = generatePath(a, {
        fileType,
        fileId,
      });

      history.push(path);
    },
    [history],
  );

  const showFilePropertyId = useCallback(
    (fileType: string, fileId: number, menuIndex: number) => {
      const a = `${sidebarBasePath}/:fileType/:fileId/property/:menuIndex`;
      const path = generatePath(a, {
        fileType,
        fileId,
        menuIndex,
      });

      history.push(path);
    },
    [history],
  );

  const showFilePropertyDetail = useCallback(
    (
      fileType: string,
      fileId: number,
      filePropertyId: number,
      detailType: string,
      replace: boolean,
    ) => {
      const a = `${sidebarBasePath}/:fileType/:fileId/file_property/:filePropertyId/:detailType`;
      const path = generatePath(a, {
        fileType,
        fileId,
        filePropertyId,
        detailType,
      });

      if (replace) {
        history.replace(path);
      } else {
        history.push(path);
      }
    },
    [history],
  );

  const showPropertyByPid = useCallback(
    (pid: string) => {
      const a = `${sidebarBasePath}/non-inventory-property/pid/:pid`;
      const path = generatePath(a, {
        pid,
      });

      history.push(path);
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
    showPropertyByPid,
  };
};

export default usePathGenerator;
