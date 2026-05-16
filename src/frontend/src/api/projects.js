import apiClient from "./client";

export const createProject = async (projectData) => {
    const response = await apiClient.post('/projects', projectData);
    return response.data;
};

export const addEmployeeToProject = async (projectId, employeeId) => {
    await apiClient.post(`/projects/${projectId}/employees/${employeeId}`);
};