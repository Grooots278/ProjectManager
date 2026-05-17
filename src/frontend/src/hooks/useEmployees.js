import { useState, useEffect, useCallback } from 'react';
import { searchEmployees, getEmployeeById, createEmployee, updateEmployee, deleteEmployee } from '../api/employees';

export function useEmployeeList(filters = {}){
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null); 

    const fetch = useCallback(async () => {
        setLoading(true);
        setError(null);
        try{
            const result = await searchEmployees(filters.query);
            setData(result);
        } catch (err){
            setError(err.message);
        } finally{
            setLoading(false);
        }
    }, [filters.query]);

    useEffect(() => { fetch(); }, [fetch]);

    return { data, loading, error, refetch: fetch };
}

export function useEmployeeMutations(){
    const [loading, setLoading] = useState(false);

    const save = async (employee) => {
        setLoading(true);
        try{
            if (employee.id){
                await updateEmployee(employee.id, employee);
            } else {
                await createEmployee(employee);
            }
            return true;
        } catch (err){
            throw err;
        } finally{
            setLoading(false);
        }
    };

    const remove = async (id) => {
        setLoading(true);
        try{
            await deleteEmployee(id);
        } finally{
            setLoading(false);
        }
    };

    return { save, remove, loading };
}