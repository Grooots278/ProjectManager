import { useState, useEffect, useCallback } from 'react';
import {
  getProjects,
  getProjectById,
  createProject,
  updateProject,
  deleteProject,
  addEmployeeToProject,
  removeEmployeeFromProject,
} from '../api/projects';

export function useProjectList(filters = {}) {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetch = useCallback(async () => {
        setLoading(true);
        setError(null);
        try {
      
            const params = {};
            Object.entries(filters).forEach(([key, value]) => {
                if (value !== '' && value !== null && value !== undefined) {
                    params[key] = value;
                }
            });
            const result = await getProjects(params);
            setData(result);
        } catch (err) {
            setError(err.response?.data?.error || err.message);
        } finally {
            setLoading(false);
        }
    }, [filters]);

    useEffect(() => {
        fetch();
    }, [fetch]);

    return { data, loading, error, refetch: fetch };
}

export function useProject(id) {
     const [data, setData] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetch = useCallback(async () => {
        if (!id) return;
        setLoading(true);
        setError(null);
        try {
            const result = await getProjectById(id);
            setData(result);
        } catch (err) {
            setError(err.response?.data?.error || err.message);
        } finally {
            setLoading(false);
        }
    }, [id]);

    useEffect(() => {
        fetch();
    }, [fetch]);

    return { data, loading, error, refetch: fetch };
}


export function useProjectMutations() {
    const [loading, setLoading] = useState(false);

    const create = async (projectData) => {
        setLoading(true);
        try {
            const newId = await createProject(projectData);
            return newId;
        } catch (err) {
            throw err;
        } finally {
            setLoading(false);
        }
    };

    const update = async (id, projectData) => {
        setLoading(true);
        try {
            await updateProject(id, projectData);
        } catch (err) {
            throw err;
        } finally {
            setLoading(false);
        }
    };

    const remove = async (id) => {
        setLoading(true);
        try {
            await deleteProject(id);
        } catch (err) {
            throw err;
        } finally {
            setLoading(false);
        }
    };

    const addEmployee = async (projectId, employeeId) => {
        setLoading(true);
        try {
            await addEmployeeToProject(projectId, employeeId);
        } catch (err) {
            throw err;
        } finally {
            setLoading(false);
        }
    };

    const removeEmployee = async (projectId, employeeId) => {
        setLoading(true);
        try {
            await removeEmployeeFromProject(projectId, employeeId);
        } catch (err) {
            throw err;
        } finally {
            setLoading(false);
        }
    };

    return { create, update, remove, addEmployee, removeEmployee, loading };
}