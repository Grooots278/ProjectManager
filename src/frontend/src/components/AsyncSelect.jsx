import { useState, useEffect, useRef } from 'react';

export default function AsyncSelect({ fetchOptions, value, onChange, placeholder }){
    const [inputValue, setInputValue] = useState('');
    const [options, setOptions] = useState([]);
    const [isOpen, setIsOpen] = useState(false);
    const [loading, setLoading] = useState(false);
    const debounceTimer = useRef(null);

    //Delayed search function
    const handleInputChange = (e) => {
        const val = e.target.value;
        setInputValue(val);
        if (debounceTimer.current) clearTimeout(debounceTimer.current);
        debounceTimer.current = setTimeout(async () => {
            if (val.trim().length > 0){
                setLoading(true);
                try{
                    const result = await fetchOptions(val);
                    setOptions(result);
                    setIsOpen(true);
                } catch(err){
                    console.error(err);
                } finally {
                    setLoading(false);
                }
            } else {
                setOptions([]);
                setIsOpen(false);
            }
        }, 300);
    };

    const handleSelect = (item) => {
        onChange(item);
        setInputValue(item.label);
        setIsOpen(false);
    };

    //Clearing the timer when unmounting
    useEffect(() => {
        return () => clearTimeout(debounceTimer.current);
    }, []);

    return (
        <div style={{ position: 'relative' }}>
            <input 
            type='text'
            value={inputValue}
            onChange={handleInputChange}
            placeholder={placeholder || 'Start typing...'}
            />
            {loading && <span>Loading...</span>}
            {isOpen && options.length > 0 && (
                <ul style={{ position: 'absolute', background: 'white', border: '1px solid #ccc', listStyle: 'none', padding: 0, margin: 0 }}>
                    {options.map((opt) => (
                        <li 
                        key={opt.value}
                        onClick={() => handleSelect(opt)}
                        style={{ padding: '8px', cursor: 'pointer' }}>
                            {opt.label}
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}