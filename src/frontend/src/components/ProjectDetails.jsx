import { useState } from 'react';
import { useProject, useProjectMutations } from '../hooks/useProjects';
import { searchEmployees, getEmployeeById } from '../api/employees';
import AsyncMultiSelect from './AsyncMultiSelect';
import { useToast } from '../context/ToastContext';

export default function ProjectDetails({ projectId, onClose }) {
    const { data: project, loading, error, refetch } = useProject(projectId);
    const { addEmployee, removeEmployee } = useProjectMutations();
    const { addToast } = useToast();

    const handleAddEmployee = async (selected) => {
        try {
            await addEmployee(projectId, selected.value);
            addToast('Сотрудник добавлен');
            refetch();
        } catch (err) {
            addToast('Ошибка: ' + err.message, 'error');
        }
    };

    const handleRemoveEmployee = async (emp) => {
        try {
            await removeEmployee(projectId, emp.value);
            addToast('Сотрудник удалён');
            refetch();
        } catch (err) {
            addToast('Ошибка: ' + err.message, 'error');
        }
    };

    if (loading) return <p>Loading...</p>;
    if (error) return <p style={{ color: 'red' }}>{error}</p>;
    if (!project) return null;

    const assignedEmployees = (project.employeeIds || []).map((id, idx) => ({
        value: id,
        label: project.employeeEmails?.[idx] || id,
    }));

    const fetchEmployees = async (query) => {
        const all = await searchEmployees(query);
        return all.map(emp => ({
            value: emp.id,
            label: `${emp.lastName} ${emp.firstName} (${emp.email})`,
        }));
    };

    return (
        <div>
            <h3>{project.name}</h3>
            <p><b>Customer:</b> {project.customerCompany}</p>
            <p><b>Executor:</b> {project.executorCompany}</p>
            <p><b>Manager:</b> {project.projectManagerFullName}</p>
            <p><b>Date:</b> {new Date(project.startDate).toLocaleDateString()} – {new Date(project.endDate).toLocaleDateString()}</p>
            <p><b>Priority:</b> {project.priority}</p>

            <h4>Executor</h4>
            <AsyncMultiSelect
                fetchOptions={fetchEmployees}
                selectedValues={assignedEmployees}
                onAdd={handleAddEmployee}
                onRemove={handleRemoveEmployee}
                placeholder="Adding employee..."
            />
            <button onClick={onClose} style={{ marginTop: 10 }}>Close</button>
        </div>
    );
}