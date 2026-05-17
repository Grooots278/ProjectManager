import { useState, useEffect, useCallback } from 'react';
import apiClient from '../api/client';

export function useEmployeeList(filters = {}) {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetch = useCallback(async () => {
        setLoading(true);
        setError(null);
        try {
            const params = {};
            if (filters.query) params.nameFilter = filters.query;
            const response = await apiClient.get('/employees', { params });
            setData(response.data);
        } catch (err) {
            setError(err.response?.data?.error || err.message);
        } finally {
            setLoading(false);
        }
    }, [filters.query]);

    useEffect(() => {
        fetch();
    }, [fetch]);

    return { data, loading, error, refetch: fetch };
}

export function useEmployeeMutations() {
    const [loading, setLoading] = useState(false);

    const save = async (employeeData) => {
        setLoading(true);
        try {
            if (employeeData.id) {
                await apiClient.put(`/employees/${employeeData.id}`, employeeData);
            } else {
                await apiClient.post('/employees', employeeData);
            }
        } catch (err) {
            throw err;
        } finally {
            setLoading(false);
        }
    };

    const remove = async (id) => {
        setLoading(true);
        try {
            await apiClient.delete(`/employees/${id}`);
        } catch (err) {
            throw err;
        } finally {
            setLoading(false);
        }
    };

    return { save, remove, loading };
}