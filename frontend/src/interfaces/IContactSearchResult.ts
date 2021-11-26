export interface IContactSearchResult {
  id: number;
  personId?: number;
  organizationId?: number;
  isDisabled: boolean;
  summary: string;
  surname: string;
  firstName: string;
  organizationName: string;
  email: string;
  mailingAddress: string;
  municipalityName: string;
  provinceState: string;
  provinceStateId: number;
}
