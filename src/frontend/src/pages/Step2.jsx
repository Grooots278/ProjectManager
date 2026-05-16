// Step 2: companies

import { useNavigate } from "react-router-dom";
import { useWizard } from "../context/WizardContext";

export default function Step2(){
    const { state, dispatch } = useWizard();
    const navigate = useNavigate();

    const handleSubmit = (e) => {
        e.preventDefault();

        //validation
        if (!state.customerCompany || !state.executorCompany){
            alert('Both companies must be filled in');
            return;
        }

        navigate('/wizard/step3');
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Step 2: Companies</h2>
            <div>
                <label>The customer company*</label>
                <input 
                type="text"
                value={state.customerCompany}
                onChange={e => dispatch({ type: 'SET_FIELD', field: 'customerCompany', value: e.target.value })}
                required />
            </div>
            <div>
                <label>The executing company*</label>
                <input 
                type="text"
                value={state.executorCompany}
                onChange={e => dispatch({ type: 'SET_FIELD', field: 'executorCompany', value: e.target.value })}
                required />
            </div>
            <button type="submit">Next</button>
            <button type="button" onClick={() => navigate('/wizard/step1')}>Back</button>
        </form>
    );
}