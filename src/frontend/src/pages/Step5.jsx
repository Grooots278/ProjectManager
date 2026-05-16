//Step 5: Upload Documents (Drag & Drop)
import { useNavigate } from 'react-router-dom';
import { useWizard } from '../context/WizardContext';
import DragDropUploader from '../components/DragDropUploader';
import { createProject, addEmployeeToProject } from '../api/projects';

export default function Step5(){
    const { state, dispatch } = useWizard();
    const navigate = useNavigate();

    const handleAddFiles = (files) => {
        dispatch({ type: 'ADD_DOCUMENTS', files });
    };

    const handleRemoveFiles = (index) => {
        dispatch({ type: 'REMOVE_DOCUMENT', index });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const projectPayload = {
            name: state.projectName,
            startDate: state.startDate,
            endDate: state.endDate,
            priority: state.priority,
            customerCompany: state.customerCompany,
            executorCompany: state.executorCompany,
            projectManagerId: state.managerId,
        };

        try{
            const projectId = await createProject(projectPayload);
            //Adding employees to the project
            for (const empId of state.selectedEmployeeIds){
                await addEmployeeToProject(projectId, empId);
            }
            alert('The project was successfully created');
            navigate('/');
        } catch (err){
            console.error(error);
            alert('Error when creating a project');
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Step 5: Uploading Documents</h2>
            <DragDropUploader
            files={state.documents}
            onAddFiles={handleAddFiles}
            onRemoveFile={handleRemoveFiles} />
            <button type='submit'>Create a project</button>
            <button type='button' onClick={() =>  navigate('/wizard/step4')}>Back</button>
        </form>
    );
}