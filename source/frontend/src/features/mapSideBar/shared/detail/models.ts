export interface IAgreementFormData {
  project_name: string;
  project_number: string;
  road: string; //this is the product name without the #
  ps_file_no: string;
  completion_date: string;
  deposit_amount: string | null;
  purchase_price: string;
  owners: [
    {
      lastname: string;
      firstname: string;
      organization_name: string;
      incorporation_number: string;
      reg_no: string;
    },
  ];
  agents: [{ workphone: string; mobilephone: string; name: string }];
  properties: [{ pid: string; full_legal: string }];
}
export const defaultAgreementFormData: IAgreementFormData = {
  project_name: 'TEST PROJECT',
  project_number: '005',
  road: 'AUTOPLAN',
  ps_file_no: '1234',
  completion_date: 'Feb 10, 2023',
  deposit_amount: '$1,000',
  purchase_price: '$10,000',
  owners: [
    {
      lastname: 'one',
      firstname: 'owner',
      organization_name: 'test organization',
      incorporation_number: '5678',
      reg_no: '910',
    },
  ],
  agents: [{ workphone: '250-812-4261', mobilephone: '778-921-6221', name: 'test agent' }],
  properties: [
    { pid: '016-272-622', full_legal: 'A fake legal description for testing purposes.' },
  ],
};
