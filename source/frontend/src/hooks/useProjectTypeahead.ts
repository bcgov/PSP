import { useState } from 'react';
import { toast } from 'react-toastify';

import { useApiProjects } from '@/hooks/pims-api/useApiProjects';
import useIsMounted from '@/hooks/util/useIsMounted';
import { IAutocompletePrediction } from '@/interfaces';

interface IProjectTypeahead {
  handleTypeaheadSearch: (query: string) => Promise<void>;
  isTypeaheadLoading: boolean;
  matchedProjects: IAutocompletePrediction[];
}

export const useProjectTypeahead = (): IProjectTypeahead => {
  // project type-ahead state
  const { searchProject } = useApiProjects();
  const [isTypeaheadLoading, setIsTypeaheadLoading] = useState(false);
  const [matchedProjects, setMatchedProjects] = useState<IAutocompletePrediction[]>([]);

  const isMounted = useIsMounted();

  // fetch autocomplete suggestions from server
  const handleTypeaheadSearch = async (query: string) => {
    try {
      setIsTypeaheadLoading(true);
      const { data } = await searchProject(query);

      if (isMounted()) {
        setMatchedProjects(
          data.map<IAutocompletePrediction>(x => {
            let projectText = `${x.code || ''}  ${x.description || ''}`;
            return { id: x.id || 0, text: projectText.trim() || '' };
          }),
        );
      }
    } catch (e) {
      if (isMounted()) {
        setMatchedProjects([]);
      }
      toast.error('Failed to get autocomplete results for supplied search', {
        autoClose: 7000,
      });
    } finally {
      if (isMounted()) {
        setIsTypeaheadLoading(false);
      }
    }
  };

  return { handleTypeaheadSearch, isTypeaheadLoading, matchedProjects };
};
