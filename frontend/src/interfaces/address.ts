export interface IAddress {
  id?: number | undefined;
  line1: string;
  line2?: string;
  administrativeArea: string;
  province?: string;
  provinceId: string;
  postal: string;
}
