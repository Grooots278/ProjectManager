import ReactDOM from 'react-dom';

export default function Modal({ children, onClose }){
    return ReactDOM.createPortal(
        <div style={{
            position: 'fixed', top: 0, left: 0, width: '100%', height: '100%',
            background: 'rgba(0,0,0,0.5)', display: 'flex', justifyContent: 'center', alignItems: 'center', zIndex: 1000
        }}>
            <div style={{ background: 'white', padding: 24, borderRadius: 8, minWidth: 400, position: 'relative' }}>
                <button onClick={onClose} style={{ position: 'absolute', top: 8, right: 8 }}>x</button>
                {children}
            </div>
        </div>  ,
        document.body
    );
}