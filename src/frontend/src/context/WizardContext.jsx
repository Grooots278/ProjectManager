import { createContext, useContext, useReducer } from "react";

const WizardContext = createContext();

const initialState = {
    projectName : '',
    startDate : '',
    endDate : '',
    priority : 1,

    customerCompany : '',
    executorCompany : '',

    managerId : null,

    selectedEmployeeIds : [],

    documents : [],
};

function wizardReducer(state, action){
    switch (action.type){
        case 'SET_FIELD':
            return {...state, [action.field]: action.value };
        case 'SET_MANAGER':
            return {...state, managerId: action.id };
        case 'TOGGLE_EMPLOYEE':
            const ids = state.selectedEmployeeIds.includes(action.id)
                ? state.selectedEmployeeIds.filter(id => id !== action.id)
                : [...state.selectedEmployeeIds, action.id];
            return {...state, selectedEmployeeIds: ids };
        case 'ADD_DOCUMENTS':
            return {...state, documents: [...state.documents, ...action.files] };
        case 'REMOVE_DOCUMENT':
            return {...state, documents: state.documents.filter((_, i) => i !== action.index) };
        case 'RESET':
            return initialState;
        default:
            return state;
    }
}

export function WizardProvider({ children }){
    const [state, dispatch] = useReducer(wizardReducer, initialState);

    return (
        <WizardContext.Provider value={{ state, dispatch }}>
            {children}
        </WizardContext.Provider>
    );
}

export function useWizard(){
    const context = useContext(WizardContext);
    if (!context){
        throw new Error('useWizard must be used within a WizardProvider');
    }
    return context;
}