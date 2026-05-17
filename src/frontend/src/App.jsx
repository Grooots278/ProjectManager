import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { WizardProvider } from './context/WizardContext';
import { ToastProvider } from './context/ToastContext';
import Step1 from './pages/Step1';
import Step2 from './pages/Step2';
import Step3 from './pages/Step3';
import Step4 from './pages/Step4';
import Step5 from './pages/Step5';
import EmployeeListPage from './pages/EmployeeListPage';
import ProjectListPage from './pages/ProjectListPage';
import Navbar from './components/Navbar';

function App() {
  return (
    <BrowserRouter>
      <ToastProvider>
        <WizardProvider>
          <Navbar />
          <Routes>
            {/* Wizard create project */}
            <Route path="/wizard/step1" element={<Step1 />} />
            <Route path="/wizard/step2" element={<Step2 />} />
            <Route path="/wizard/step3" element={<Step3 />} />
            <Route path="/wizard/step4" element={<Step4 />} />
            <Route path="/wizard/step5" element={<Step5 />} />

            {/* Manage */}
            <Route path="/employees" element={<EmployeeListPage />} />
            <Route path="/projects" element={<ProjectListPage />} />

            {/* The default redirect */}
            <Route path="*" element={<Navigate to="/projects" replace />} />
          </Routes>
        </WizardProvider>
      </ToastProvider>
    </BrowserRouter>
  );
}

export default App;