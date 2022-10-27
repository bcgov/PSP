export enum AccessRequestStatus {
  Approved = 'APPROVED',
  Initiated = 'INITIATED',
  Denied = 'DENIED',
  MoreInfo = 'MOREINFO',
  Received = 'RECEIVED',
  ReviewComplete = 'REVIEWCOMPLETE',
  UnderReview = 'UNDERREVIEW',
}
export type AccessStatusDisplay =
  | 'Approved'
  | 'Initiated'
  | 'Denied'
  | 'More Info'
  | 'Received'
  | 'Review Complete'
  | 'Under Review';

export const AccessStatusDisplayMapper: { [key in AccessRequestStatus]: AccessStatusDisplay } = {
  APPROVED: 'Approved',
  INITIATED: 'Initiated',
  DENIED: 'Denied',
  MOREINFO: 'More Info',
  RECEIVED: 'Received',
  REVIEWCOMPLETE: 'Review Complete',
  UNDERREVIEW: 'Under Review',
};
