import React, { useCallback, useEffect, useState } from 'react';

import { Button } from '@/components/common/buttons/Button';
import { Section } from '@/components/common/Section/Section';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';

interface IDateTestContainerProps {
  something?: any;
}

const DateTestContainer: React.FC<React.PropsWithChildren<IDateTestContainerProps>> = () => {
  const [timeData, setTimeData] = useState(null);
  const compReqRepository = useCompensationRequisitionRepository();
  const getTimeData = compReqRepository.getCompensationRequisitionAtTime.execute;

  const compReqId = 1;
  const time = '2012-12-31T22:00:00.000Z';

  const fetchData = useCallback(async () => {
    const a = await getTimeData(compReqId, time);
    setTimeData(a);
  }, [getTimeData]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return (
    <Section header="Date Test Container">
      <div>
        <Button onClick={() => fetchData()}>Fetch Data</Button>
      </div>
      <div>{JSON.stringify(timeData)}</div>
    </Section>
  );
};

export default DateTestContainer;
