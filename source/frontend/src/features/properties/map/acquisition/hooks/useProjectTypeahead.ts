import { useApiProjects } from 'hooks/pims-api/useApiProjects';
import { IAutocompletePrediction } from 'interfaces';
import { useState } from 'react';
import { toast } from 'react-toastify';

interface IProjectTypeahead {
  handleTypeaheadSearch: (query: string) => Promise<void>;
  isTypeaheadLoading: boolean;
  matchedProjects: IAutocompletePrediction[];
}

export const useProjectTypeahead = (): IProjectTypeahead => {
  // project type-ahead state
  const { getProjectPredictions } = useApiProjects();
  const [isTypeaheadLoading, setIsTypeaheadLoading] = useState(false);
  const [matchedProjects, setMatchedProjects] = useState<IAutocompletePrediction[]>([]);

  // fetch autocomplete suggestions from server
  const handleTypeaheadSearch = async (query: string) => {
    try {
      setIsTypeaheadLoading(true);
      const { data } = await getProjectPredictions(query);
      console.log(data);
      setMatchedProjects(
        data.map<IAutocompletePrediction>(x => {
          return { id: x.id || 0, text: x.description?.toString() || '' };
        }),
      );
      setIsTypeaheadLoading(false);
    } catch (e) {
      console.error(e);
      setMatchedProjects([]);
      toast.error('Failed to get autocomplete results for supplied search', {
        autoClose: 7000,
      });
    } finally {
      setIsTypeaheadLoading(false);
    }
  };

  return { handleTypeaheadSearch, isTypeaheadLoading, matchedProjects };
};
