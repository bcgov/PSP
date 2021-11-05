export default interface ITypeCode<T> {
  id: T;
  description: string;
  isDisabled: boolean;
  displayOrder?: number;
}
