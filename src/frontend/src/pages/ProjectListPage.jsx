import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useProjectList, useProjectMutations } from '../hooks/useProjects';
import { useToast } from '../context/ToastContext';
import Modal from '../components/Modal';
import ConfirmDialog from '../components/ConfirmDialog';
import ProjectForm from '../components/ProjectForm'; // to edit
import ProjectDetails from '../components/ProjectDetails'; // to look

export default function ProjectListPage(){
    const [filters, setFilters] = useState({ name: '', startFrom: '', startTo: '', priority: '' });
    const [debouncedFilters, setDebouncedFilters] = useState(filters);
    const [editingProject, setEditingProject] = useState(null);
    const [viewingProjectId, setViewingProjectId] = useState(null);
    const [deleteTarget, setDeleteTarget] = useState(null);
    const { addToast } = useToast();
    const { data: projects, loading, error, refetch } = useProjectList(debouncedFilters);
    const { update, remove } = useProjectMutations();

    //Dobounce filters
    useEffect(() => {
        const timer = setTimeout(() => setDebouncedFilters(filters), 400);
        return () => clearTimeout(timer);
    }, [filters]);

    const handleUpdate = async (formData) => {
        try{
            await update(formData.id, formData);
            addToast('The project has been updated');
            setEditingProject(null);
            refetch();
        } catch (err){
            addToast('Error: ' + err.message, "Error");
        }
    };
    
    const handleDelete = async () => {
        if (!deleteTarget) return;
        try{
            await remove(deleteTarget);
            addToast('The project has been deleted');
            setDeleteTarget(null);
            refetch();
        } catch (err){
            addToast('Error when deleting', 'error');
        }
    };

    return (
        <div>
            <h2>Projects</h2>
            <div style={{ display: 'flex', gap: 10, marginBottom: 16, flexWrap: 'wrap' }}>
                <input placeholder="Name..." value={filters.name} onChange={e => setFilters({...filters, name: e.target.value})} />
                <input type="date" placeholder="Start from " value={filters.startFrom} onChange={e => setFilters({...filters, startFrom: e.target.value})} />  
                <input type="date" placeholder="End to " value={filters.startTo} onChange={e => setFilters({...filters, startTo: e.target.value})} />
                <input type="number" placeholder="Priority" value={filters.priority} onChange={e => setFilters({...filters, priority: e.target.value})} />
                <Link to="/wizard/step1"><button>Create new project</button></Link>
            </div>
            {loading && <p>Loading...</p>}
            {error && <p style={{ color: 'red' }}>Error: {error}</p>}

            <table border="1" cellPadding="8" style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Customer</th>
                        <th>Executor</th>
                        <th>Manager</th>
                        <th>Date</th>
                        <th>Priority</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {projects.map(p => (
                        <tr key={p.id}>
                            <td>{p.name}</td>
                            <td>{p.customerCompany}</td>
                            <td>{p.executorCompany}</td>
                            <td>{p.projectManagerFullName}</td>
                            <td>{new Date(p.startDate).toLocaleDateString()} – {new Date(p.endDate).toLocaleDateString()}</td>
                            <td>{p.priority}</td>
                            <td>
                                <button onClick={() => setViewingProjectId(p.id)}>👁️</button>
                                <button onClick={() => setEditingProject(p)}>✏️</button>
                                <button onClick={() => setDeleteTarget(p.id)}>🗑️</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {/* Viewing details */}
            {viewingProjectId && (
                <Modal onClose={() => setViewingProjectId(null)}>   
                    <ProjectDetails projectId={viewingProjectId} onClose={() => setViewingProjectId(null)} />
                </Modal>
            )}

            {/* Edit */}
            {editingProject && (
                <Modal onClose={() => setEditingProject(null)}>
                    <ProjectForm
                        initialData={editingProject}
                        onSubmit={handleUpdate}
                        onCancel={() => setEditingProject(null)}
                    />
                </Modal>
            )}

            {deleteTarget && (
                <ConfirmDialog
                    message="Delete the project?"
                    onConfirm={handleDelete}
                    onCancel={() => setDeleteTarget(null)}
                />
            )}
        </div>
    );
}