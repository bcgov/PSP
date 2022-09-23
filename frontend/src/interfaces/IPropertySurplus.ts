import ITypeCode from './ITypeCode';

export default interface IPropertySurplus {
  comment?: string;
  date?: string;
  type?: ITypeCode<string>;
}
