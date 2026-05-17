import { useState, useEffect, useRef } from 'react';

export default function AsyncMultiSelect({ fetchOptions, selectedValues, onAdd, onRemove, placeholder }){
    const [inputValue, setInputValue] = useState('');
    const [options, setOptions] = useState([]);
    const [isOpen, setIsOpen] = useState(false);
    const [loading, setLoading] = useState(false);
    const debounceTimer = useRef(null);

    const handleInputChange = (e) => {
        const val = e.target.value;
        setInputValue(val);

        if (debounceTimer.current) clearTimeout(debounceTimer.current);
        debounceTimer.current = setTimeout(async () => {
            if (val.trim().length > 0){
                setLoading(true);
                try{
                    const result = await fetchOptions(val);
                    // Exclude those already selected
                    const filtered = result.filter(opt => !selectedValues.find(sv => sv.value === opt.value));
                    setOptions(filtered);
                    setIsOpen(true);
                } catch(err){
                    console.error(err);
                } finally {
                    setLoading(false);
                }
            }else{
                setOptions([]);
                setIsOpen(false);
            }
        }, 300);
    };

    const handleSelect = (item) => {
        onAdd(item);
        setInputValue('');
        setIsOpen(false);
    };

    useEffect(() => {
        return () => clearTimeout(debounceTimer.current);
    }, []);

    return(
        <div>
            <div>
                <input 
                type='text'
                value={inputValue}
                onChange={handleInputChange}
                placeholder={placeholder || 'Employee search'} />
                {loading && <span>Loading...</span>}
                {isOpen && options.length > 0 && (
                    <ul style={{ position: 'absolute', background: 'white', border: '1px solid #ccc', listStyle: 'none', padding: 0, margin: 0 }}>
                        {options.map((opt) => (
                            <li key={opt.value} onClick={() =>  handleSelect(opt)} style={{ padding: '8px', cursor: 'pointer' }}>
                                {opt.label}
                            </li>
                        ))}
                    </ul>
                )}
            </div>
            <div>
                {selectedValues.map((emp) => (
                    <span key={emp.value} style={{ display: 'inline-block', margin: '2px', padding: '2px 5px', background: '#eee' }}>
                        {emp.label}
                        <button type="button" onClick={() => onRemove(emp)} style={{ marginLeft: '5px', cursor: 'pointer' }}>x</button>
                    </span>
                ))}
            </div>
        </div>
    );
}