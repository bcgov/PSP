export interface IAutocompletionRequest {
  /* Required. The text to search for. The search text to complete. Must be at least 1 character. */
  search: string;

  /* Optional. Defaults to 5. The number of autocompleted suggestions to retrieve. The value must be a number between 1 and 100.  */
  top?: number;
}

export interface IAutocompleteResponse {
  predictions: IAutocompletePrediction[];
}

export interface IAutocompletePrediction {
  /* The completed term or phrase */
  text: string;

  /* An entity ID that can be used to retrieve details about this autocomplete suggestion */
  id: number;
}
