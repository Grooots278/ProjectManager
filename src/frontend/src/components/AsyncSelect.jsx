import { useState, useEffect, useRef } from 'react';

export default function AsyncSelect({ fetchOptions, value, onChange, placeholder }) {
    const [inputValue, setInputValue] = useState(value || '');
    const [options, setOptions] = useState([]);
    const [isOpen, setIsOpen] = useState(false);
    const [loading, setLoading] = useState(false);
    const debounceTimer = useRef(null);
    const latestQueryRef = useRef(''); // to prevent a race
    const blurTimer = useRef(null);
    const wrapperRef = useRef(null);

    // Syncing with an external value
    useEffect(() => {
        setInputValue(value || '');
    }, [value]);

    const handleInputChange = (e) => {
        const val = e.target.value;
        setInputValue(val);
        if (debounceTimer.current) clearTimeout(debounceTimer.current);

        debounceTimer.current = setTimeout(async () => {
            if (val.trim().length > 0) {
                latestQueryRef.current = val; // we remember the current request.
                setLoading(true);
                try {
                    const result = await fetchOptions(val);
                    // we apply the result ONLY if the query is still relevant.
                    if (latestQueryRef.current === val) {
                        setOptions(result);
                        setIsOpen(true);
                    }
                } catch (err) {
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
        setOptions([]);
    };

    // Closing the list with a delay so that the click can be triggered.
    const handleBlur = () => {
        blurTimer.current = setTimeout(() => setIsOpen(false), 200);
    };

    const handleFocus = () => {
        clearTimeout(blurTimer.current);
        if (options.length > 0) setIsOpen(true);
    };

    // Clearing timers during unmounting
    useEffect(() => {
        return () => {
            clearTimeout(debounceTimer.current);
            clearTimeout(blurTimer.current);
        };
    }, []);

    return (
        <div style={{ position: 'relative' }} ref={wrapperRef}>
            <input
                type="text"
                value={inputValue}
                onChange={handleInputChange}
                onBlur={handleBlur}
                onFocus={handleFocus}
                placeholder={placeholder || 'Start typing...'}
            />
            {loading && <span> Loading... </span>}
            {isOpen && options.length > 0 && (
                <ul
                    style={{
                        position: 'absolute',
                        background: 'white',
                        border: '1px solid #ccc',
                        listStyle: 'none',
                        padding: 0,
                        margin: 0,
                        zIndex: 1000,
                    }}
                >
                    {options.map((opt) => (
                        <li
                            key={opt.value}
                            onMouseDown={(e) => {
                                e.preventDefault(); // prevents blur before click
                                handleSelect(opt);
                            }}
                            style={{ padding: '8px', cursor: 'pointer' }}
                        >
                            {opt.label}
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}