import Modal from './Modal';

export default function ConfirmDialog({ message, onConfirm, onCancel }){
    return (
        <Modal onClose={onCancel}>
            <p>{message}</p>
            <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
                <button onClick={onCancel}>Cancel</button>
                <button onClick={onConfirm} style={{ background: '#dc3545', color: 'white' }}>Delete</button>
            </div>
        </Modal>
    );
}