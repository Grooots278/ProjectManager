// Step 1: Name, Dates, Priority
import { useNavigate } from 'react-router-dom';
import { useWizard } from '../context/WizardContext';

export default function Step1(){
    const { state, dispatch } = useWizard();
    const navigate = useNavigate();

    const handleSubmit = (e) => {
        e.preventDefault();

        //validation
        if (!state.projectName || !state.startDate || !state.endDate){
            alert('Fill in the required fields');
            return;
        }
        if (new Date(state.endDate) <= new Date(state.startDate)){
            alert('The end date must be later than the start date');
            return;
        }
        navigate('/wizard/step2');
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Step 1: Basic Information</h2>
            <div>
                <label>Name project*</label>
                <input 
                type='text'
                value={state.projectName}
                onChange={e => dispatch({ type: 'SET_FIELD', field: 'projectName', value: e.target.value})}
                required />
            </div>
            <div>
                <label>Date start*</label>
                <input 
                type='date'
                value={state.startDate}
                onChange={e => dispatch({ type: 'SET_FIELD', field: 'startDate', value: e.target.value })}
                required />
            </div>
            <div>
                <label>Date end*</label>
                <input 
                type='date'
                value={state.endDate}
                onChange={e => dispatch({ type: 'SET_FIELD', field: 'endDate', value: e.target.value })}
                required />
            </div>
            <div>
                <label>Priority (1-5)</label>
                <input 
                type='number'
                min="1"
                max="5"
                value={state.priority}
                onChange={e => dispatch({ type: 'SET_FIELD', field: 'priority', value: e.target.value })} />
            </div>
            <button type='submit'>Next</button>
        </form>
    )
}