import { getMockApiCompensationList } from 'mocks/compensations.mock';
import { getMockAcquisitionPayee } from 'mocks/mockAcquisitionPayee.mock';
import { mockCompReqH120s } from 'mocks/mockCompReqH120s.mock';
import { getMockH120Categories } from 'mocks/mockH120Categories.mock';
import moment from 'moment';

import { Api_GenerateCompensation } from './GenerateCompensation';

describe('GenerateCompensation tests', () => {
  it('Can Generate an empty compensation without throwing an error', () => {
    const compensation = new Api_GenerateCompensation(null, null, [], []);
    expect(compensation.generated_date).toBe(moment().format('MMM DD, YYYY'));
  });

  it('Builds an empty list of summary financials using the passed h120 categories and compensation financials', () => {
    const compensation = new Api_GenerateCompensation(
      getMockApiCompensationList()[0],
      {} as any,
      getMockH120Categories(),
      [],
    );
    expect(compensation.summary_financial_activities).toHaveLength(0);
  });

  it('Builds a list of summary financials using the passed h120 categories and compensation financials', () => {
    const compensation = new Api_GenerateCompensation(
      getMockApiCompensationList()[1],
      {} as any,
      getMockH120Categories(),
      [],
    );
    expect(compensation.summary_financial_activities).toHaveLength(2);
    expect(compensation.summary_financial_activities[0]).toEqual({
      file_total: '$0.00',
      h120_category_name: 'Market',
      total: '$7.00',
    });
    expect(compensation.summary_financial_activities[1]).toEqual({
      file_total: '$0.00',
      h120_category_name: 'Land',
      total: '$15.00',
    });
  });

  it('Builds a list of summary financials even if there are only financials on the file not the h120', () => {
    const compensation = new Api_GenerateCompensation(
      getMockApiCompensationList()[0],
      {} as any,
      getMockH120Categories(),
      mockCompReqH120s(),
    );
    expect(compensation.summary_financial_activities).toHaveLength(2);
    expect(compensation.summary_financial_activities[0]).toEqual({
      file_total: '$200.00',
      h120_category_name: 'Land',
      total: '$0.00',
    });
    expect(compensation.summary_financial_activities[1]).toEqual({
      file_total: '$1,000.00',
      h120_category_name: 'Relocation Costs (REL COST)',
      total: '$0.00',
    });
  });

  it('summary financials for file do not include current h120', () => {
    const compensation = new Api_GenerateCompensation(
      getMockApiCompensationList()[1],
      {} as any,
      getMockH120Categories(),
      mockCompReqH120s(),
    );
    expect(compensation.file_financial_total).toBe('$1,100.00');
  });

  it('adds file financial totals', () => {
    const compensation = new Api_GenerateCompensation(
      {} as any,
      {} as any,
      getMockH120Categories(),
      mockCompReqH120s(),
    );
    expect(compensation.file_financial_total).toBe('$1,200.00');
  });

  it('adds h120 financial totals', () => {
    const compensation = new Api_GenerateCompensation(
      getMockApiCompensationList()[1],
      {} as any,
      getMockH120Categories(),
      [],
    );
    expect(compensation.financial_total).toBe('$35.00');
  });

  it('generates with a payee', () => {
    const compensation = new Api_GenerateCompensation(
      getMockApiCompensationList()[1],
      {} as any,
      getMockH120Categories(),
      [],
      getMockAcquisitionPayee(),
    );
    expect(compensation.payee.gst_number).toBe('3262');
  });

  it('can generate a payee with no cheques', () => {
    const compensation = new Api_GenerateCompensation(
      getMockApiCompensationList()[1],
      {} as any,
      getMockH120Categories(),
      [],
      { ...getMockAcquisitionPayee(), cheques: [] },
    );
    expect(compensation.payee.total_amount).toBe('');
  });
});
