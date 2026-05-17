import { useState, useEffect } from 'react';

export default function EmployeeForm({ initialData = {}, onSubmit, onCancel }){
    const [form, setForm] = useState({
        firstName: initialData.firstName || '',
        lastName: initialData.lastName || '',
        middleName: initialData.middleName || '',
        email: initialData.email || '',
        id: initialData.id || null
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(form);
    };

    return (
        <form onSubmit={handleSubmit}>
            <h3>{form.id ? 'Edit' : 'New'}</h3>
            <div>
                <label>Last name*</label>
                <input name='lastName' value={form.lastName} onChange={handleChange} required />
            </div>
            <div>
                <label>First name*</label>
                <input name='firstName' value={form.firstName} onChange={handleChange} required />
            </div>
            <div>
                <label>Middle name</label>
                <input name='middleName' value={form.middleName} onChange={handleChange} />
            </div>
            <div>
                <label>Email*</label>
                <input name='email' value={form.email} onChange={handleChange} required />
            </div>
            <div style={{ marginTop: 16 }}>
                <button type="submit">Save</button>
                <button type="button" onClick={onCancel}>Cancle</button>
            </div>
        </form>
    )
}