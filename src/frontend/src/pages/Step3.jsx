// Step 3: Select a supervisor from the drop-down list with the search

import { useNavigate } from 'react-router-dom';
import { useWizard } from '../context/WizardContext';
import AsyncSelect from '../components/AsyncSelect';
import { searchEmployees } from '../api/employees';

export default function Step3(){
    const { state, dispatch } = useWizard();
    const navigate = useNavigate();

    const fetchEmployees = async (query) => {
        const employees = await searchEmployees(query);
        return employees.map(emp => ({
            value: emp.id,
            label: `${emp.lastName} ${emp.firstName} ${emp.middleName || ''} (${emp.email})`,
        }));
    };

    const handleManagerChange = (selectId) => {
        dispatch({ type: 'SET_MANAGER', id: selected.value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (!state.managerId){
            alert('Choose a supervisor');
            return;
        }
        navigate('/wizard/step4');
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Step 3: Project Manager</h2>
            <AsyncSelect 
            fetchOptions={fetchEmployees}
            onChange={handleManagerChange}
            placeholder="Employee search..." />
            <button type='submit'>Next</button>
            <button type='button' onClick={() => navigate('/wizard/step2')}>Back</button>
        </form>
    );
}