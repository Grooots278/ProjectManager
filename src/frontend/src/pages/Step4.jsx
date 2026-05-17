//Step 4: Selecting performers (multiple choice with search)
import { useNavigate } from 'react-router-dom';
import { useWizard } from '../context/WizardContext';
import AsyncMultiSelect from '../components/AsyncMultiSelect';
import { searchEmployees } from '../api/employees';

export default function Step4(){
    const { state, dispatch } = useWizard();
    const navigate = useNavigate();

    const fetchEmployees = async (query) => {
        const employees = await searchEmployees(query);
        return employees.map(emp => ({
            value: emp.id,
            label: `${emp.lastName} ${emp.firstName} ${emp.middleName || ''} (${emp.email})`,
        }));
    };

    const handleAddEmployee = (emp) => {
        dispatch({ type: 'TOGGLE_EMPLOYEE', id: emp.value });
    };

    const handleRemoveEmployee = (emp) => {
        dispatch({ type: 'TOGGLE_EMPLOYEE', id: emp.value });
    };

    //Converting the ID array into objects for the component
    const selectedEmployees = state.selectedEmployeeIds.map(id => {
        return { value: id, label: `ID: ${id}` };
    });

    const handleSubmit = (e) => {
        e.preventDefault();
        if (state.selectedEmployeeIds.length === 0){
            alert('Add at least one artist');
            return;
        }
        navigate('/wizard/step5');
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Step 4: Project Executors</h2>
            <AsyncMultiSelect
            fetchOptions={fetchEmployees}
            selectedValues={selectedEmployees}
            onAdd={handleAddEmployee}
            onRemove={handleRemoveEmployee}
            placeholder="Adding employees..." />
            <button type='submit'>Next</button>
            <button type='button' onClick={() => navigate('/wizard/step3')}>Back</button>
        </form>
    );
}