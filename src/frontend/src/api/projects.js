import apiClient from './client';

export const getProjects = async (params = {}) => {
    const response = await apiClient.get('/projects', { params });
    return response.data;
};

export const getProjectById = async (id) => {
    const response = await apiClient.get(`/projects/${id}`);
    return response.data;
};

export const createProject = async (projectData) => {
    const response = await apiClient.post('/projects', projectData);
    return response.data;
};

export const updateProject = async (id, projectData) => {
    await apiClient.put(`/projects/${id}`, projectData);
};

export const deleteProject = async (id) => {
    await apiClient.delete(`/projects/${id}`);
};

export const addEmployeeToProject = async (projectId, employeeId) => {
    await apiClient.post(`/projects/${projectId}/employees/${employeeId}`);
};

export const removeEmployeeFromProject = async (projectId, employeeId) => {
    await apiClient.delete(`/projects/${projectId}/employees/${employeeId}`);
};