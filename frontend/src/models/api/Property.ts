import { Api_Address, Api_CodeType } from './Address';
import { Api_ConcurrentVersion } from './ConcurrentVersion';

export interface Api_Coordinate {
  /**
   * @format double
   */
  x?: number;

  /**
   * @format double
   */
  y?: number;
}

export interface Api_Geometry {
  coordinate?: Api_Coordinate;
}

export interface Api_Property extends Api_ConcurrentVersion {
  id?: string;
  pid?: number;
  pin?: number;
  address?: Api_Address;
  district?: Api_CodeType;
  region?: Api_CodeType;
  location?: Api_Geometry;
}
