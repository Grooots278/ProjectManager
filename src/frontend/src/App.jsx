import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { WizardProvider } from './context/WizardContext';
import Step1 from './pages/Step1';
import Step2 from './pages/Step2';
import Step3 from './pages/Step3';
import Step4 from './pages/Step4';
import Step5 from './pages/Step5';

function App(){
  return(
    <BrowserRouter>
      <WizardProvider>
        <Routes>
          <Route path='/wizard/step1' element={<Step1 />} />
          <Route path="/wizard/step2" element={<Step2 />} />
          <Route path="/wizard/step3" element={<Step3 />} />
          <Route path="/wizard/step4" element={<Step4 />} />
          <Route path="/wizard/step5" element={<Step5 />} />
          <Route path="*" element={<Navigate to="/wizard/step1" replace />} />
        </Routes>
      </WizardProvider>
    </BrowserRouter>
  );
}

export default App;
