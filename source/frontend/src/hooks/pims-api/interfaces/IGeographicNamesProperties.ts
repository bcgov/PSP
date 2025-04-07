export interface IGeographicNamesProperties {
  uri: string;
  name: string;
  language: string;
  status: string;
  isOfficial: number;
  nameAuthority: {
    resourceUrl: string;
    id: string;
    nameAuthority: string;
    webSiteUrl: string;
  };
  tags: any[];
  score: number;
  feature: {
    id: string;
    uuid: string;
    uri: string;
    mapsheets: string;
    names: string;
  };
  changeDate: string;
  decisionDate: string;
  featureCategory: number;
  featureCategoryDescription: string;
  featureCategoryURI: string;
  featureType: string;
  lonAsRecorded: number;
  latAsRecorded: number;
  datumAsRecorded: string;
  position: string;
  ntsMap: string;
}
