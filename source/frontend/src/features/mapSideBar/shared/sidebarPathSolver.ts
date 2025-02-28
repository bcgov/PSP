import { generatePath, useHistory } from 'react-router-dom';

export const sidebarBasePath = '/mapview/sidebar';

export interface IPathSolverMethods {
  newFile: (fileType: string) => void;
  showFile: (fileType: string, fileId: number) => void;
  showDetail: (fileType: string, fileId: number, detailType: string, replace: boolean) => void;
  editDetails: (fileType: string, fileId: number, detailType: string) => void;
  editDetail: (fileType: string, fileId: number, detailType: string, detailId: number) => void;
  addDetail: (fileType: string, fileId: number, detailType: string) => void;
  editProperties: (fileType: string, fileId: number) => void;
  showFilePropertyIndex: (fileType: string, fileId: number, propertyIndex: number) => void;
  showFilePropertyDetail: (
    fileType: string,
    fileId: number,
    filePropertyId: number,
    detailType: string,
    replace: boolean,
  ) => void;
}

export type IPathSolver = () => IPathSolverMethods;

const usePathSolver: IPathSolver = () => {
  const history = useHistory();

  const newFile = (fileType: string) => {
    const a = `${sidebarBasePath}/new/:fileType`;
    const path = generatePath(a, {
      fileType: fileType,
    });

    history.push(path);
  };

  const showFile = (fileType: string, fileId: number) => {
    const a = `${sidebarBasePath}/:fileType/:fileId`;
    const path = generatePath(a, {
      fileType: fileType,
      fileId: fileId,
    });

    history.push(path);
  };

  const showDetail = (fileType: string, fileId: number, detailType: string, replace: boolean) => {
    const a = `${sidebarBasePath}/:fileType/:fileId/file/:detailType`;
    const path = generatePath(a, {
      fileType: fileType,
      fileId: fileId,
      detailType: detailType,
    });

    if (replace) {
      history.replace(path);
    } else {
      history.push(path);
    }
  };

  const editDetails = (fileType: string, fileId: number, detailType: string) => {
    const a = `${sidebarBasePath}/:fileType/:fileId/edit/:detailType`;
    const path = generatePath(a, {
      fileType: fileType,
      fileId: fileId,
      detailType: detailType,
    });

    history.push(path);
  };

  const editDetail = (fileType: string, fileId: number, detailType: string, detailId: number) => {
    const a = `${sidebarBasePath}/:fileType/:fileId/edit/:detailType/:detailId`;
    const path = generatePath(a, {
      fileType: fileType,
      fileId: fileId,
      detailType: detailType,
      detailId: detailId,
    });

    history.push(path);
  };

  const addDetail = (fileType: string, fileId: number, detailType: string) => {
    const a = `${sidebarBasePath}/:fileType/:fileId/edit/:detailType`;
    const path = generatePath(a, {
      fileType: fileType,
      fileId: fileId,
      detailType: detailType,
    });

    history.push(path);
  };

  const editProperties = (fileType: string, fileId: number) => {
    const a = `${sidebarBasePath}/:fileType/:fileId/property/selector`;
    const path = generatePath(a, {
      fileType: fileType,
      fileId: fileId,
    });

    history.push(path);
  };

  const showFilePropertyIndex = (fileType: string, fileId: number, menuIndex: number) => {
    const a = `${sidebarBasePath}/:fileType/:fileId/property/:menuIndex`;
    const path = generatePath(a, {
      fileType: fileType,
      fileId: fileId,
      menuIndex: menuIndex,
    });

    history.push(path);
  };

  const showFilePropertyDetail = (
    fileType: string,
    fileId: number,
    filePropertyId: number,
    detailType: string,
    replace: boolean,
  ) => {
    const a = `${sidebarBasePath}/:fileType/:fileId/file_property/:filePropertyId/:detailType`;
    const path = generatePath(a, {
      fileType: fileType,
      fileId: fileId,
      filePropertyId: filePropertyId,
      detailType: detailType,
    });

    if (replace) {
      history.replace(path);
    } else {
      history.push(path);
    }
  };

  return {
    newFile,
    showFile,
    showDetail,
    editDetail,
    editDetails,
    addDetail,
    editProperties,
    showFilePropertyIndex,
    showFilePropertyDetail,
  };
};

export default usePathSolver;
