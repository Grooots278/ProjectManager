import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { useEmployeeList, useEmployeeMutations } from '../hooks/useEmployees';
import { useToast } from '../context/ToastContext';
import ConfirmDialog from '../components/ConfirmDialog';
import Modal from '../components/Modal';
import EmployeeForm from '../components/EmployeeForm';

export default function EmployeeListPage(){
    const [search, setSearch] = useState('');
    const [debouncedSearch, setDebouncedSearch] = useState('');
    const [editingEmployee, setEditingEmployee] = useState(null); // employee for editing
    const [deleteTarget, setDeleteTarget] = useState(null); //id to delete
    const { addToast } = useToast();
    const { data: employees, loading, error, refetch } = useEmployeeList({ query: debouncedSearch });
    const { save, remove } = useEmployeeMutations();

    //Debounce
    useEffect(() => {
        const timer = setTimeout(() => setDebouncedSearch(search), 300);
    }, [search]);

    const handleSave = async (formData) => {
        try{
            await save(formData);
            addToast('The employee has been saved');
            setEditingEmployee(null);
            refetch();
        } catch (err){
            addToast("Error: " + err.message, "error");
        }
    };

    const handleDelete = async () => {
        if (!deleteTarget) return;
        try{
            await remove(deleteTarget);
            addToast('The employee has been deleted');
            setDeleteTarget(null);
            refetch();
        } catch (err){
            addToast('Error when deleting', 'error');
        }
    };

    return (
        <div>
            <h2>Employees</h2>
            <div style={{ marginBottom: 16 }}>
                <input 
                    type='text'
                    placeholder='Search by name'
                    value={search}
                    onChange={e => setSearch(e.target.value)} />
                <button onClick={() => setEditingEmployee({})}>Add employee</button>
            </div>

            {loading && <p>Loading...</p>}
            {error && <p style={{ color: 'red' }}>Error: {error}</p>}

            <table border="1" cellPadding="8" style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {employees.map(emp => (
                        <tr key={emp.id}>
                            <td>{emp.lastName} {emp.firstName} {emp.middleName || ''}</td>
                            <td>{emp.email}</td>
                            <td>
                                <button onClick={() => setEditingEmployee(emp)}>✏️</button>
                                <button onClick={() => setDeleteTarget(emp.id)}>🗑️</button>
                            </td>
                        </tr>
                    ))}
                    {employees.length === 0 && !loading && <tr><td colSpan="3">No data available</td></tr>}
                </tbody>
            </table>

            {/* Modal for the form */}
            {editingEmployee && (
                <Modal onClose={() => setEditingEmployee(null)}>
                    <EmployeeForm
                        initialData={editingEmployee}
                        onSubmit={handleSave}
                        onCancel={() => setEditingEmployee(null)}
                    />
                </Modal>
            )}

            {/* Deletion Confirmation dialog */}
            {deleteTarget && (
                <ConfirmDialog
                    message="Delete employee?"
                    onConfirm={handleDelete}
                    onCancel={() => setDeleteTarget(null)}
                />
            )}
        </div>
    );
}