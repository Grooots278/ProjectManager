import apiClient from "./client";

export const searchEmployees = async (query = '') => {
    const response = await apiClient.get('/employees', {
        params: { nameFilter: query, sortBy: 'name'},
    });
    return response.data;
};

export const getEmployeeById = async (id) => {
    const response = await apiClient.get(`/employees/${id}`);
    return response.data;
};