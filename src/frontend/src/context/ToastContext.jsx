import { createContext, useContext, useState, useCallback } from 'react';

const ToastContext = createContext(null);

export function ToastProvider({ children }){
    const [toasts, setToasts] = useState([]);

    const addToast = useCallback((message, type = 'success') => {
        const id = Date.now();
        setToasts(prev => [...prev, {id, message, type }]);
        setTimeout(() => setToasts(prev => prev.filter(t => t.id !== id)), 3000);
    }, []);

    return (
        <ToastContext.Provider value={{ addToast }}>
            {children}
            <div style={{ position: 'fixed', top: 20, right: 20, zIndex: 9999 }}>
                {toasts.map(t => {
                    <div key={t.id} style={{
                        padding: '10px 20px',
                        marginBottom: 8,
                        background: t.type === 'success' ? '#28a745' : '#dc3545',
                        color: 'white',
                        borderRadius: 4,
                        boxShadow: '0 2px 5px rgba(0,0,0,0.2)'}}>
                            {t.message}
                    </div>
                })}
            </div>
        </ToastContext.Provider>
    );
}

export const useToast = () => useContext(ToastContext);