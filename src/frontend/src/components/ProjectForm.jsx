import { useState, useEffect } from 'react';

export default function ProjectForm({ initialData = {}, onSubmit, onCancel }) {
    const [form, setForm] = useState({
        id: initialData.id || null,
        name: initialData.name || '',
        customerCompany: initialData.customerCompany || '',
        executorCompany: initialData.executorCompany || '',
        startDate: initialData.startDate ? initialData.startDate.split('T')[0] : '',
        endDate: initialData.endDate ? initialData.endDate.split('T')[0] : '',
        priority: initialData.priority || 1,
        projectManagerId: initialData.projectManagerId || '',
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (!form.name || !form.customerCompany || !form.executorCompany || !form.startDate || !form.endDate) {
            alert('Заполните обязательные поля');
            return;
        }
        onSubmit(form);
    };

    return (
        <form onSubmit={handleSubmit}>
            <h3>Edit project</h3>
            <div>
                <label>Name*</label>
                <input name="name" value={form.name} onChange={handleChange} required />
            </div>
            <div>
                <label>Customer-company*</label>
                <input name="customerCompany" value={form.customerCompany} onChange={handleChange} required />
            </div>
            <div>
                <label>Executor-company*</label>
                <input name="executorCompany" value={form.executorCompany} onChange={handleChange} required />
            </div>
            <div>
                <label>Date start*</label>
                <input type="date" name="startDate" value={form.startDate} onChange={handleChange} required />
            </div>
            <div>
                <label>Date end*</label>
                <input type="date" name="endDate" value={form.endDate} onChange={handleChange} required />
            </div>
            <div>
                <label>Priority</label>
                <input type="number" name="priority" min="1" max="5" value={form.priority} onChange={handleChange} />
            </div>
            <div style={{ marginTop: 16 }}>
                <button type="submit">Save</button>
                <button type="button" onClick={onCancel}>Cancel</button>
            </div>
        </form>
    );
}