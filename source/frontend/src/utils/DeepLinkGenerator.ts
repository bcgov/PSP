import { generatePath } from 'react-router-dom';

export const sidebarBasePath = '/mapview/sidebar';

/**
 * Pure, side-effect-free builders for sidebar deep-link paths. Every method returns the
 * path string and never touches browser history, so they are safe to call from anywhere
 * (utilities, services, render, tests) without triggering a route change.
 */
export class DeepLinkGenerator {
  static readonly sidebarBasePath = sidebarBasePath;

  static newFile(fileType: string): string {
    return generatePath(`${sidebarBasePath}/:fileType/new`, { fileType });
  }

  static showFile(fileType: string, fileId: number): string {
    return generatePath(`${sidebarBasePath}/:fileType/:fileId`, { fileType, fileId });
  }

  static showDetails(fileType: string, fileId: number, detailType: string): string {
    return generatePath(`${sidebarBasePath}/:fileType/:fileId/:detailType`, {
      fileType,
      fileId,
      detailType,
    });
  }

  static showDetail(
    fileType: string,
    fileId: number,
    detailType: string,
    detailId: number,
  ): string {
    return generatePath(`${sidebarBasePath}/:fileType/:fileId/:detailType/:detailId`, {
      fileType,
      fileId,
      detailType,
      detailId,
    });
  }

  static editDetails(fileType: string, fileId: number, detailType: string): string {
    return generatePath(`${sidebarBasePath}/:fileType/:fileId/:detailType/edit`, {
      fileType,
      fileId,
      detailType,
    });
  }

  static editDetail(
    fileType: string,
    fileId: number,
    detailType: string,
    detailId: number,
  ): string {
    return generatePath(`${sidebarBasePath}/:fileType/:fileId/:detailType/:detailId/edit`, {
      fileType,
      fileId,
      detailType,
      detailId,
    });
  }

  static addDetail(fileType: string, fileId: number, detailType: string): string {
    return generatePath(`${sidebarBasePath}/:fileType/:fileId/:detailType/new`, {
      fileType,
      fileId,
      detailType,
    });
  }

  static editProperties(fileType: string, fileId: number): string {
    return generatePath(`${sidebarBasePath}/:fileType/:fileId/property/selector`, {
      fileType,
      fileId,
    });
  }

  static showFilePropertyId(fileType: string, fileId: number, menuIndex: number): string {
    return generatePath(`${sidebarBasePath}/:fileType/:fileId/property/:menuIndex`, {
      fileType,
      fileId,
      menuIndex,
    });
  }

  static showFilePropertyDetail(
    fileType: string,
    fileId: number,
    filePropertyId: number,
    detailType: string,
    detailSubType?: string,
    detailId?: number,
  ): string {
    return generatePath(
      `${sidebarBasePath}/:fileType/:fileId/property/:filePropertyId/:detailType${
        detailSubType ? '/' + detailSubType : ''
      }/:detailId?`,
      {
        fileType,
        fileId,
        filePropertyId,
        detailType,
        detailId,
      },
    );
  }

  static addFilePropertyDetail(
    fileType: string,
    fileId: number,
    filePropertyId: number,
    detailType: string,
    detailSubType?: string,
  ): string {
    return generatePath(
      `${sidebarBasePath}/:fileType/:fileId/property/:filePropertyId/:detailType${
        detailSubType ? '/' + detailSubType : ''
      }/new`,
      {
        fileType,
        fileId,
        filePropertyId,
        detailType,
      },
    );
  }

  static showPropertyByPid(pid: string): string {
    return generatePath(`${sidebarBasePath}/non-inventory-property/pid/:pid`, { pid });
  }

  static showPropertyDetail(
    propertyId: number,
    detailType: string,
    detailSubType?: string,
    detailId?: number,
  ): string {
    return generatePath(
      `${sidebarBasePath}/property/:propertyId/:detailType${
        detailSubType ? '/' + detailSubType : ''
      }/:detailId?`,
      {
        propertyId,
        detailType,
        detailId,
      },
    );
  }

  static addPropertyDetail(propertyId: number, detailType: string, detailSubType?: string): string {
    return generatePath(
      `${sidebarBasePath}/property/:propertyId/:detailType${
        detailSubType ? '/' + detailSubType : ''
      }/new`,
      {
        propertyId,
        detailType,
      },
    );
  }
}

export default DeepLinkGenerator;
