export default interface Api_TypeCode<T> {
  id?: T;
  code?: string;
  description?: string;
  isDisabled?: boolean;
  displayOrder?: number;
}
