export default interface ITypeCode<T> {
  id: T;
  description?: string;
  isDisabled?: boolean;
  displayOrder?: number;
}

export const defaultTypeCode: ITypeCode<string> = {
  id: '',
  description: '',
  isDisabled: false,
};
